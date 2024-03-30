using Dynamic_Eye.DTOs;
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
        [Authorize]
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

        [HttpPost("register")]
        public async Task<ActionResult<User>> PostUser([FromBody] UserDto user)
        {
            bool userExists = _context.Users.Any(u => u.username == user.username || u.email == user.email);

            if (userExists)
            {
                return BadRequest("A user with the given username or email already exists.");
            }

            User newUser = new User
            {
                username = user.username,
                email = user.email,
                hash = BCrypt.Net.BCrypt.HashPassword(user.password),
                created = DateTime.UtcNow,
                updated = DateTime.UtcNow
            };

            try
            {
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                user.password = null!;
                return Ok(user);
            }
            catch (Exception exception)
            {
                return StatusCode(500, $"An error occurred while creating the user with the following exception:\n{exception}");
            }
        }

        [HttpPut("update")]
        [Authorize]
        public async Task<IActionResult> PutUser([FromBody] UserDto updatedUser)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.email == updatedUser.email);
            if (user == null)
            {
                return NotFound("User Not Found!");
            }

            user.username = updatedUser.username;
            user.email = updatedUser.email;
            user.hash = BCrypt.Net.BCrypt.HashPassword(updatedUser.password);
            user.updated = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
                return Ok($"User Successfully Updated!");
            }
            catch (DbUpdateConcurrencyException exception)
            {
                return StatusCode(500, $"Error while updating user for the following reason:\n{exception}");
            }
        }

        [HttpDelete("delete")]
        [Authorize]
        public async Task<IActionResult> DeleteUser([FromBody] UserDto usr)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.email == usr.email);

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
