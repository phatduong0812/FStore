using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject.Models;
using System.Net.Http;
using System.Text.Json;

namespace eStoreClient.Pages.Members
{
    public class DetailsModel : PageModel
    {
        public DetailsModel()
        {
        }

        public Member Member { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("https://localhost:44353/api/Member/" + id);
            HttpContent content = response.Content;
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var member = await JsonSerializer.DeserializeAsync<Member>(content.ReadAsStream(), options);

            if (member == null)
            {
                return NotFound();
            }
            Member = member;
            return Page();
        }
    }
}
