using Dynamic_Eye.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dynamic_Eye.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly UsersDbContext _context;

        public UsersController(UsersDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers([FromQuery] int? id, [FromQuery] string? username, [FromQuery] string? email)
        {
            IQueryable<User> query = _context.Users;

            if (id.HasValue)
            {
                query = query.Where(u => u.id == id);
            }

            if (!string.IsNullOrEmpty(username))
            {
                query = query.Where(u => u.username == username);
            }

            if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(u => u.email == email);
            }

            var users = await query.ToListAsync();

            if (users.Count == 0)
            {
                return NotFound("User Not Found!");
            }

            return Ok(users);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            bool userExists = _context.Users.Any(u => u.username == user.username || u.email == user.email);

            if (userExists)
            {
                return BadRequest("A user with the given username or email already exists.");
            }

            user.created = DateTime.UtcNow;
            user.updated = DateTime.UtcNow;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User Successfully Created!");
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutUser(int id, User userUpdate)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.id == id);
            if (user == null)
            {
                return NotFound("User Not Found!");
            }

            user.username = userUpdate.username;
            user.email = userUpdate.email;
            user.hash = userUpdate.hash;
            user.updated = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
                return Ok($"User Successfully Updated!");
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Error Updating User!");
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound("User Not Found!");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok("User Successfully Deleted!");
        }

    }
}

