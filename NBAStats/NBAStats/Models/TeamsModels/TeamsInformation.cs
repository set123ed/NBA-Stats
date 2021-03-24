using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NBAStats.Models.TeamsModels
{
    public class TeamsInformation
    {
        [JsonPropertyName("pubDateTime")]
        public string PubDateTime { get; set; }

        [JsonPropertyName("igorPath")]
        public string IgorPath { get; set; }

        [JsonPropertyName("xslt")]
        public string Xslt { get; set; }

        [JsonPropertyName("xsltForceRecompile")]
        public string XsltForceRecompile { get; set; }

        [JsonPropertyName("xsltInCache")]
        public string XsltInCache { get; set; }

        [JsonPropertyName("xsltCompileTimeMillis")]
        public string XsltCompileTimeMillis { get; set; }

        [JsonPropertyName("xsltTransformTimeMillis")]
        public string XsltTransformTimeMillis { get; set; }

        [JsonPropertyName("consolidatedDomKey")]
        public string ConsolidatedDomKey { get; set; }

        [JsonPropertyName("endToEndTimeMillis")]
        public string EndToEndTimeMillis { get; set; }
    }
}
