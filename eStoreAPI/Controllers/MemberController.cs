using BusinessObject.Models;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using System;

namespace eStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IConfiguration _configuration;
        public MemberController(IMemberRepository memberRepository, IConfiguration configuration)
        {
            _memberRepository = memberRepository;
            _configuration= configuration;
        }       

        public class LoginInformation
        {
            public string email { get; set; }
            public string password { get; set; }
        }

        [HttpPost("authentication")]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<Member>> Authentication(LoginInformation loginInformation)
        {
            Member member = await _memberRepository.Authentication(loginInformation.email, loginInformation.password);
            if (member == null)
            {
                if (loginInformation.email.Equals(_configuration["Admin:Email"], StringComparison.OrdinalIgnoreCase) && loginInformation.password.Equals(_configuration["Admin:Password"]))
                {
                    return new Member { Email = _configuration["Admin:Email"], Password = _configuration["Admin:Password"], isAdmin = true };
                }

                return NotFound();
            }

            return Ok(member);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Member>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Member>>> GetMembers()
        {
            var list = await _memberRepository.GetAll();
            if (list == null)
            {
                return NotFound();
            }

            return Ok(list);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Member))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Member>> GetMember(int id)
        {
            Member? member = await _memberRepository.Get(id);

            if (member == null)
            {
                return NotFound();
            }

            return Ok(member);
        }
      
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMember(int id, Member member)
        {
            if (id != member.MemberId)
            {
                return BadRequest();
            }

            try
            {
                await _memberRepository.Update(member);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _memberRepository.Get(member.MemberId) == null)
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Member>> PostMember(Member member)
        {
            try
            {
                await _memberRepository.Add(member);
            }
            catch (DbUpdateException)
            {
                if (await _memberRepository.Get(member.MemberId) != null)
                {
                    return Conflict();
                }

                throw;
            }

            return CreatedAtAction("GetMember", new { id = member.MemberId }, member);
        }

        // DELETE: api/Member/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            var member = await _memberRepository.Get(id);
            if (member == null)
            {
                return NotFound();
            }

            await _memberRepository.Delete(id);
            return NoContent();
        }
    }
}
