using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Retail.Domain.DataTransferObjects;

namespace RetailUi.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public bool GetOrdersError { get; private set; }
        public List<CustomerOrder> Orders { get; private set; } = new List<CustomerOrder>();

        public IndexModel(IConfiguration configuration,
                          IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGet()
        {
            var getOrdersEndpoint = $"{_configuration["RetailApi:EndpointBaseUrl"]}orders";
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync(getOrdersEndpoint);

            if (response.IsSuccessStatusCode)
            {
                Orders = await response.Content.ReadAsAsync<List<CustomerOrder>>();
            }
            else
            {
                GetOrdersError = true;
            }
        }
    }
}
