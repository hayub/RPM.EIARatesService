using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RPM.EIARatesService.ApiClients;
using RPM.EIARatesService.Constants;
using RPM.EIARatesService.Data;
using RPM.EIARatesService.Exceptions;
using RPM.EIARatesService.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RPM.EIARatesService
{
    public class TimedWorker : IHostedService, IDisposable
    {
        private readonly ILogger<TimedWorker> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IConfiguration _configuration;
        private Timer _timer;
        private IRatesAPI _ratesApi;

        public TimedWorker(ILogger<TimedWorker> logger, IServiceScopeFactory serviceScopeFactory, IConfiguration configuration, IRatesAPI ratesApi)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _configuration = configuration;
            _ratesApi = ratesApi;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service is starting.");
            var taskExecutionDelayInMins = Convert.ToInt32(_configuration.GetValue(typeof(int), "TaskExecutionDelayInMinutes"));
            _logger.LogInformation($"Task will execute every {taskExecutionDelayInMins} minute[s]");
            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromMinutes(taskExecutionDelayInMins));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            _logger.LogInformation("Service is stopped.");
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        private async void DoWork(object state)
        {
            try
            {
                _logger.LogInformation($"Task execution started at: {DateTime.Now}");

                var days = Convert.ToInt32(_configuration.GetValue(typeof(int), "RatesDurationInDays"));
                var lastDate = DateTime.Today.AddDays(days * (-1));
                var rateResponse = await _ratesApi.GetRates();

                if (rateResponse == null || rateResponse.Series == null || rateResponse.Series.Count == 0)
                    throw new Exception(SystemConstants.Error_No_Series);

                var rates = rateResponse.Series.First().Data.Select(r => new Rate
                {
                    Date = DateTime.ParseExact(r[0].ToString(), SystemConstants.EIADateFormat, CultureInfo.InvariantCulture),
                    Price = Convert.ToDecimal(r[1].ToString())
                }).Where(a => a.Date >= lastDate);

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var minimumDateForComparison = rates.Min(x => x.Date);
                    var dbContext = scope.ServiceProvider.GetRequiredService<EIADbContext>();
                    var existingRates = dbContext.Rates.Where(a => a.Date >= minimumDateForComparison);
                    var ratesToAdd = rates.Where(x => !existingRates.Any(y => y.Date == x.Date)).ToList();
                    await dbContext.Rates.AddRangeAsync(ratesToAdd);
                    await dbContext.SaveChangesAsync();
                }

                _logger.LogInformation($"Task execution completed at: {DateTime.Now}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred while executing the task: {ex.Message}", ex);
            }
        }
    }
}
