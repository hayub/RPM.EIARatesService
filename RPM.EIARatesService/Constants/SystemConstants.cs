namespace RPM.EIARatesService.Constants
{
    public static class SystemConstants
    {
        #region Settings
        internal static readonly string EIAAPIBaseURL = "https://api.eia.gov/";
        internal static readonly string EIAConnectionStringName = "EIARateDBConnectionString";
        internal static readonly string EIADateFormat = "yyyyMMdd";
        internal static readonly string EIA_API_Key = "ec92aacd6947350dcb894062a4ad2d08";

        //API Key, BaseURL and Date Format can also be moved to AppSettings

        #endregion

        #region Messages

        internal static readonly string Error_No_Series = "No series received from EIA API";

        #endregion

    }
}
