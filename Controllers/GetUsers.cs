//using Dynamic_Eye.Models;
//using Microsoft.AspNetCore.Mvc;

//namespace Dynamic_Eye.Controllers
//{
//    [ApiController]
//    [Route("users")]
//    public class GetUsers : ControllerBase
//    {
//        private readonly UsersDbContext _context;

//        public GetUsers(UsersDbContext context)
//        {
//            _context = context;
//        }

//        // method to get all users
//        [HttpGet("id")]
//        public async Task<IActionResult> TestDbConnection()
//        {
//            // Create a new user
//            var newUser = new User
//            {
//                username = "testUser",
//                email = "testuser@example.com",
//                hash = "testHash", // Use a proper hashing mechanism for real passwords
//                created = DateTime.UtcNow,
//                updated = DateTime.UtcNow
//            };

//            // Add the new user to the DbContext
//            _context.Users.Add(newUser);

//            // Save changes to the database
//            await _context.SaveChangesAsync();

//            return Ok("Test user created successfully.");
//        }

//        // implement authentication (in the services directory)

//        // implement testing (maybe in swagger?)

//        // Add API Documentation

//        // Improve Error Handling by returning more specific error messages or codes, especially for operations that can fail due to reasons other than resource not found or bad requests.

//    }
//}
