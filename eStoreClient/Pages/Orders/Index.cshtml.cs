﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Text.Json;

namespace eStoreClient.Pages.Orders
{
    public class IndexModel : PageModel
    {
        public IndexModel()
        {
        }

        public IList<Order> Order { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            int? memberId = null;
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("LoggedInUser")))
            {
                return Unauthorized();
            }

            Member loggedMember = JsonSerializer.Deserialize<Member>(HttpContext.Session.GetString("LoggedInUser"));
            if (loggedMember.isAdmin == false)
            {
                memberId = loggedMember.MemberId;
            }
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("https://localhost:44353/api/Order?memberId=" + memberId);
            HttpContent content = response.Content;
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Order = await JsonSerializer.DeserializeAsync<List<Order>>(content.ReadAsStream(), options);
            return Page();
        }
    }
}
