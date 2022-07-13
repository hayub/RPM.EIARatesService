using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RPM.EIARatesService.Constants
{
    public static class SystemConstants
    {
        #region Settings
        internal static readonly string EIAAPIBaseURL = "https://api.eia.gov/";
        internal static readonly string EIAConnectionStringName = "EIARateDBConnectionString";
        internal static readonly string EIADateFormat = "yyyyMMdd";
        internal static readonly string EIA_API_Key = "ec92aacd6947350dcb894062a4ad2d08";
        #endregion

        #region Errors

        internal static readonly string Error_No_Series = "No series received from EIA API";

        #endregion

    }
}
