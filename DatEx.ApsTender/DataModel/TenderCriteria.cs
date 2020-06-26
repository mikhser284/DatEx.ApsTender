using System;
using Newtonsoft.Json;

namespace DatEx.ApsTender.DataModel
{
    public class TenderCriteria
    {
        [JsonProperty("id")]
        public Int32 Id { get; set; }

        [JsonProperty("name")]
        public String Name { get; set; }

        public override String ToString() => $"{Name} ({Id})";
    }
}
