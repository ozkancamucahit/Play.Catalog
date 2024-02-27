using Play.Inventory.Service.DTOs;

namespace Play.Inventory.Service.Clients
{
    public sealed class CatalogClient
    {

        private readonly HttpClient _httpClient;

        public CatalogClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IReadOnlyCollection<CatalogItemDTO>> GetCatalogItemsAsync()
        {
            try
            {
                var items = await _httpClient.GetFromJsonAsync<IReadOnlyCollection<CatalogItemDTO>>("/api/items");

                return items;
            }
            catch (Exception ex)
            {

                return new CatalogItemDTO[0];
            }
        }


    }
}
