using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RPM.EIARatesService.ViewModels
{
    // Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
    public class EIAResponseVM
    {
        [JsonPropertyName("request")]
        public Request Request { get; set; }

        [JsonPropertyName("series")]
        public List<Series> Series { get; set; }
    }

    public class Request
    {
        [JsonPropertyName("command")]
        public string Command { get; set; }

        [JsonPropertyName("series_id")]
        public string SeriesId { get; set; }
    }

    public class Series
    {
        [JsonPropertyName("series_id")]
        public string SeriesId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("units")]
        public string Units { get; set; }

        [JsonPropertyName("f")]
        public string F { get; set; }

        [JsonPropertyName("unitsshort")]
        public string Unitsshort { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("copyright")]
        public string Copyright { get; set; }

        [JsonPropertyName("source")]
        public string Source { get; set; }

        [JsonPropertyName("iso3166")]
        public string Iso3166 { get; set; }

        [JsonPropertyName("geography")]
        public string Geography { get; set; }

        [JsonPropertyName("start")]
        public string Start { get; set; }

        [JsonPropertyName("end")]
        public string End { get; set; }

        [JsonPropertyName("updated")]
        public string Updated { get; set; }

        [JsonPropertyName("data")]
        public List<List<object>> Data { get; set; }
    }


}
