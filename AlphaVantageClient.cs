using System.Net.Http.Json;

namespace Lab34
{
    public class AlphaVantageClient
    {
        private readonly HttpClient _httpClient;
        public AlphaVantageClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GlobalQuoteResponse?> GetQuoteResponseAsync(
            string symbol,
            CancellationToken cancellationToken = default
            )
        {
            var response = await _httpClient.GetAsync(
                $"query?function=GLOBAL_QUOTE&symbol={symbol}&apikey=P2XSYQZWZDHFIFMA",
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<GlobalQuoteResponse>(cancellationToken);
        }

        public async Task<MarketStatusResponse?> GetMarketStatusResponseAsync(
            CancellationToken cancellationToken = default
            )
        {
            var response = await _httpClient.GetAsync(
                $"query?function=MARKET_STATUS&apikey=P2XSYQZWZDHFIFMA",
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<MarketStatusResponse>(cancellationToken);
        }

        public async Task<SymbolSearchResponse?> GetSymbolSearchResponseAsync(
            string keyword,
            CancellationToken cancellationToken = default
            )
        {

            var response = await _httpClient.GetAsync(
                 $"query?function=SYMBOL_SEARCH&keywords={keyword}&apikey=P2XSYQZWZDHFIFMA",
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<SymbolSearchResponse>(cancellationToken);

        }

    }
}
