using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using System;
using BusinessObject.Models;

namespace eStoreClient.Pages.Login
{
    public class IndexModel : PageModel
    {
        public string Msg { get; set; }
        [BindProperty] public string Username { get; set; }
        [BindProperty] public string Password { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            HttpClient client = new HttpClient();
            var json = JsonSerializer.Serialize(new { email = Username, password = Password });

            HttpResponseMessage response = await client.PostAsync("https://localhost:44353/api/Member/authentication",
                new StringContent(json, Encoding.UTF8, "application/json"));
            HttpContent content = response.Content;
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var member = await JsonSerializer.DeserializeAsync<Member>(content.ReadAsStream(), options);
            if (String.IsNullOrEmpty(member.Email) == false)
            {
                HttpContext.Session.SetString("LoggedInUser", JsonSerializer.Serialize(member));
                return RedirectToPage("../Index");
            }
            Msg = "Invalid";
            return Page();
        }
        public IActionResult OnGetLogout()
        {
            HttpContext.Session.Remove("LoggedInUser");
            return RedirectToPage("../Index");
        }
    }
}
