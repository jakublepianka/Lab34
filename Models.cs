using System.Text.Json.Serialization;

namespace Lab34
{
    public class GlobalQuoteResponse
    {
        [JsonPropertyName("Global Quote")]
        public required GlobalQuote GlobalQuote { get; set; }
    }

    public class GlobalQuote
    {
        [JsonPropertyName("01. symbol")]
        public required string Symbol { get; set; }

        [JsonPropertyName("02. open")]
        public required string Open { get; set; }

        [JsonPropertyName("03. high")]
        public required string High { get; set; }

        [JsonPropertyName("04. low")]
        public required string Low { get; set; }

        [JsonPropertyName("05. price")]
        public required string Price { get; set; }

        [JsonPropertyName("06. volume")]
        public required string Volume { get; set; }

        [JsonPropertyName("07. latest trading day")]
        public required DateTime LatestTradingDay { get; set; }

        [JsonPropertyName("08. previous close")]
        public required string PreviousClose { get; set; }

        [JsonPropertyName("09. change")]
        public required string Change { get; set; }

        [JsonPropertyName("10. change percent")]
        public required string ChangePercent { get; set; }
    }

    public class MarketStatusResponse
    {
        [JsonPropertyName("markets")]
        public required List<MarketStatus> Markets { get; set; }
    }

    public class MarketStatus
    {
        [JsonPropertyName("market_type")]
        public required string MarketType { get; set; }

        [JsonPropertyName("region")]
        public required string Region { get; set; }

        [JsonPropertyName("primary_exchanges")]
        public required string PrimaryExchanges { get; set; }

        [JsonPropertyName("local_open")]
        public required string LocalOpen { get; set; }

        [JsonPropertyName("local_close")]
        public required string LocalClose { get; set; }

        [JsonPropertyName("current_status")]
        public required string CurrentStatus { get; set; }

        [JsonPropertyName("notes")]
        public required string Notes { get; set; }
    }

    public class SymbolSearchResponse
    {
        [JsonPropertyName("bestMatches")]
        public required List<SymbolMatch> BestMatches { get; set; }
    }

    public class SymbolMatch
    {
        [JsonPropertyName("1. symbol")]
        public required string Symbol { get; set; }

        [JsonPropertyName("2. name")]
        public required string Name { get; set; }

        [JsonPropertyName("3. type")]
        public required string Type { get; set; }

        [JsonPropertyName("4. region")]
        public required string Region { get; set; }

        [JsonPropertyName("5. marketOpen")]
        public required string MarketOpen { get; set; }

        [JsonPropertyName("6. marketClose")]
        public required string MarketClose { get; set; }

        [JsonPropertyName("7. timezone")]
        public required string TimeZone { get; set; }

        [JsonPropertyName("8. currency")]
        public required string Currency { get; set; }

        [JsonPropertyName("9. matchScore")]
        public required string MatchScore { get; set; }
    }
}
