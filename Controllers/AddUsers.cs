using Dynamic_Eye.DbContext;
using Microsoft.AspNetCore.Mvc;

namespace Dynamic_Eye.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly UsersDbContext _context;

        public TestController(UsersDbContext context)
        {
            _context = context;
        }

        [HttpGet("TestDbConnection")]
        public async Task<IActionResult> TestDbConnection()
        {
            // Create a new user
            var newUser = new User
            {
                username = "testUser",
                email = "testuser@example.com",
                hash = "testHash", // Use a proper hashing mechanism for real passwords
                created = DateTime.UtcNow,
                updated = DateTime.UtcNow
            };

            // Add the new user to the DbContext
            _context.Users.Add(newUser);

            // Save changes to the database
            await _context.SaveChangesAsync();

            return Ok("Test user created successfully.");
        }
    }

}

