using Microsoft.Extensions.Logging;
using RestSharp;
using RPM.EIARatesService.Constants;
using RPM.EIARatesService.ViewModels;
using System;
using System.Threading.Tasks;

namespace RPM.EIARatesService.ApiClients
{
    public class RatesAPI : IRatesAPI
    {
        private readonly RestClient _restClient;
        private readonly ILogger _logger;
        public RatesAPI(ILogger<RatesAPI> logger, RestClient restClient)
        {
            _logger = logger;
            _restClient = restClient;
            _restClient.Options.BaseUrl = new Uri(SystemConstants.EIAAPIBaseURL);
        }
        public async Task<EIAResponseVM> GetRatesAsync()
        {
            try
            {
                var request = new RestRequest($"series/?api_key={SystemConstants.EIA_API_Key}&series_id=PET.EMD_EPD2D_PTE_NUS_DPG.W");
                _restClient.Options.ThrowOnAnyError = true;
                var content = await _restClient.GetAsync<EIAResponseVM>(request);
                _logger.LogDebug("Response from EIA API:", content);
                return content;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Rates API:{ex.Message}", ex);
                throw;
            }

        }
    }
}
