using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using System.Net.Http;
using System.Text.Json;

namespace eStoreClient.Pages.OrderDetails
{
    public class IndexModel : PageModel
    {
        public IndexModel()
        {
        }

        public IList<OrderDetail> OrderDetail { get; set; } = default!;
        public int id { get; set; }

        public async Task OnGetAsync(int id)
        {
            this.id = id;
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("https://localhost:44353/api/OrderDetail/order-detail?orderId=" + id);
            HttpContent content = response.Content;
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            OrderDetail = await JsonSerializer.DeserializeAsync<List<OrderDetail>>(content.ReadAsStream(), options);
        }
    }
}
