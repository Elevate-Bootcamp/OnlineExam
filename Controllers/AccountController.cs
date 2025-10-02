using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace OnlineExam.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            // Add debug info
            var userInfo = new
            {
                IsAuthenticated = User.Identity.IsAuthenticated,
                UserName = User.Identity.Name,
                UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList()
            };

            Console.WriteLine($"[CONTROLLER] User authenticated: {userInfo.IsAuthenticated}");
            Console.WriteLine($"[CONTROLLER] User name: {userInfo.UserName}");

            return Ok(new
            {
                Message = "hello world from controller",
                User = userInfo
            });
        }

        // ✅ ADD THIS ENDPOINT FOR TESTING
        [HttpGet("test")]
        [AllowAnonymous]
        public IActionResult Test()
        {
            return Ok("This endpoint doesn't require auth");
        }
    }
}