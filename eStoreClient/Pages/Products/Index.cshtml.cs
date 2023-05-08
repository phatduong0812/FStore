using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject.Models;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Text.Json;

namespace eStoreClient.Pages.Products
{
    public class IndexModel : PageModel
    {
        public string CurrentFilter { get; set; }


        public IList<Product> Product { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string searchString)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("LoggedInUser")))
            {
                return Unauthorized();
            }

            Member loggedMember = JsonSerializer.Deserialize<Member>(HttpContext.Session.GetString("LoggedInUser"));
            if (loggedMember.isAdmin == false)
            {
                return Unauthorized();
            }

            CurrentFilter = searchString;
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("https://localhost:44353/api/Product?queryKeyword=" + searchString);
            HttpContent content = response.Content;
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Product = await JsonSerializer.DeserializeAsync<List<Product>>(content.ReadAsStream(), options);
            return Page();
        }
    }
}
