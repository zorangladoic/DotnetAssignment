using Newtonsoft.Json;

namespace FlowrSpot.Quote.Entities
{
    public class Quote
    {
        public string Id { get; set; } = string.Empty;
        [JsonProperty("quote")]
        public string QuoteMsg { get; set; } = string.Empty;
        public int Length { get; set; }
        public string Author { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = default!;
        public string Sfw { get; set; } = string.Empty;
        public string Permalink { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Background { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
    }

}
