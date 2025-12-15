using System.ComponentModel;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;

namespace Lab34
{ 
    public static class AiFunctions
    {
        [Description("""
        Returns the current DateTime of a given time zone.
        Expects Windows time zone ID (e.g. "Eastern Standard Time", "W. Europe Standard Time")
        """)]
        public static string? GetCurrentDateTime(
        [Description("Windows timezone ID")]
        string timeZoneId
        )
        {
            try
            {
                var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                var date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);
                return date.ToString("o");
            }
            catch (TimeZoneNotFoundException)
            {
                return null;
            }
            catch (InvalidTimeZoneException)
            {
                return null;
            }
        }

        [Description("Returns most recent information about a given stock symbol as a JSON object.")]
        public static async Task<string?> GetQuoteResponseAsync(
            [Description("Stock symbol  (e.g. AMZN, NVDA)")] string symbol,
            IServiceProvider serviceProvider,
            CancellationToken cancellationToken = default)
        {
            var apiClient = serviceProvider.GetRequiredService<AlphaVantageClient>();
            var quote = await apiClient.GetQuoteResponseAsync(symbol, cancellationToken: cancellationToken);

            //Console.WriteLine(JsonSerializer.Serialize(quote));

            return JsonSerializer.Serialize(quote?.GlobalQuote);
        }

        [Description(
            "Returns the market status for a specified region, including local open/close times and current status, as a JSON object."
        )]
        public static async Task<string?> GetMarketStatusResponseAsync(
            [Description("Market region to retrieve status for (must match API region, e.g., 'United States', 'Canada', 'Mainland China', 'Hong Kong', 'Japan' or 'Global').")]
        string region,
            IServiceProvider serviceProvider,
            [Description("Market type to retrieve status for (must match API type, e.g., 'Cryptocurrency' or 'Forex'. Default is 'Equity').")]
        string type = "Equity",
            CancellationToken cancellationToken = default)
        {

            var apiClient = serviceProvider.GetRequiredService<AlphaVantageClient>();
            var marketStatus = await apiClient.GetMarketStatusResponseAsync(cancellationToken: cancellationToken);

            var regionStatus = marketStatus?.Markets
                .Where(market => string.Equals(market.Region, region, StringComparison.OrdinalIgnoreCase) ||
                ((type == "Cryptocurrency" || type == "Forex") && (market.MarketType == "Forex" || market.MarketType == "CryptoCurrency"))
                ) ?? new List<MarketStatus>();

            //Console.WriteLine(JsonSerializer.Serialize(regionStatus));

            return JsonSerializer.Serialize(regionStatus);
        }

        [Description("""
       Searches for financial symbols (tickers) matching a keyword
       and returns a JSON array of matching symbols with their region, 
       market hours, currency, and match score.
    """)]
        public static async Task<string?> GetSymbolSearchResponseAsync(
            [Description("Search keyword used to look up matching financial symbols (e.g. company name or ticker fragment). Must be an applicable string for URL")]
        string keyword,
            IServiceProvider serviceProvider,
            CancellationToken cancellationToken = default
            )
        {
            var apiClient = serviceProvider.GetRequiredService<AlphaVantageClient>();
            var symbolMatches = await apiClient.GetSymbolSearchResponseAsync(keyword, cancellationToken: cancellationToken);

            if (symbolMatches?.BestMatches.Count == 0)
            {
                return "No matches found for provided keyword";
            }

            //Console.WriteLine(JsonSerializer.Serialize(symbolMatches));

            return JsonSerializer.Serialize(symbolMatches?.BestMatches);
        }

    }
}
