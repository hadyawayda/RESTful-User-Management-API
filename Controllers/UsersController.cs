using Dynamic_Eye.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dynamic_Eye.Controllers
{
    [ApiController]
    [Route("api/users")]
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

        [HttpGet("id/{id}")]
        public async Task<ActionResult<IEnumerable<User>>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound("User Not Found!");
            }

            return Ok(user);
        }

        [HttpGet("username/{username}")]
        public async Task<ActionResult<IEnumerable<User>>> GetUserByUsername(string username)
        {
            var user = await _context.Users.FindAsync(username);

            if (user == null)
            {
                return NotFound("User Not Found!");
            }

            return Ok(user);
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<IEnumerable<User>>> GetUserByEmail(string email)
        {
            var user = await _context.Users.FindAsync(email);

            if (user == null)
            {
                return NotFound("User Not Found!");
            }

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            bool userExists = _context.Users.Any(u => u.username == user.username || u.email == user.email);

            if (userExists)
            {
                return BadRequest("A user with the given username or email already exists.");
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.id)
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
                if (!_context.Users.Any(u => u.id == id))
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

