﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject.Models;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Text.Json;

namespace eStoreClient.Pages.Members
{
    public class IndexModel : PageModel
    {
        public IndexModel()
        {
        }

        public IList<Member> Member { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
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


            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("https://localhost:44353/api/Member");
            HttpContent content = response.Content;
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Member = await JsonSerializer.DeserializeAsync<List<Member>>(content.ReadAsStream(), options);
            return Page();
        }
    }
}
