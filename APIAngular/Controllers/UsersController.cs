using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIAngular.Models;
using APIAngular.Services;

namespace APIAngular.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly PollContext _context;
		private IUserService _userService;

		public UsersController(PollContext context,IUserService userService)
        {
			_userService = userService;

			_context = context;
        }

		[HttpPost("authenticate")] 
		public IActionResult Authenticate(
			[FromBody]User userParam) {
			var user = _userService.Authenticate(userParam.UserName, userParam.Password);
			if (user == null)
			return BadRequest(new { message = "Username or password is incorrect" });
			return Ok(user);
		}

		// GET: api/Users
		[HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }


		[HttpGet("SearchOnEmail/{email}")]
		public async Task<ActionResult<User>> GetUserbyEmail(string email)
		{
			var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.Equals(email));

			if (user == null)
			{
				return NotFound();
			}

			return user;
		}


		// PUT: api/Users/5
		[HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.UserID)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserID }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserID == id);
        }
    }
}
