using APIDevelopmentinASP.NETCore.Data;
using APIDevelopmentinASP.NETCore.Helpers;
using APIDevelopmentinASP.NETCore.Model;
using APIDevelopmentinASP.NETCore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace APIDevelopmentinASP.NETCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;
        private readonly AppDbContext _context;

        //// 🔁 Temporary In-Memory User (Later you can connect to DB)
        //private static User currentUser = new User
        //{
        //    Username = "admin",
        //    PasswordHash = "1234"
        //};

        public AuthController(AppDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpGet("admin-data")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAdminData()
        {
            return Ok("🛡 Welcome Admin!");
        }

        [HttpGet("user-data")]
        [Authorize(Roles = "User")]
        public IActionResult GetUserData()
        {
            return Ok("👤 Only Users can see this!");
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("shared-data")]
        public IActionResult GetSharedData()
        {
            return Ok("🌐 Both Admin and User can see this!");
        }

        // 🔐 Generate secure refresh token
        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes);
            }
        }

        // 🔐 POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginRequest.Username);

            if (dbUser == null || !PasswordHasher.VerifyPassword(loginRequest.Password, dbUser.PasswordHash))
            {
                return Unauthorized("❌ Invalid credentials");
            }

            var accessToken = _jwtService.GenerateToken(dbUser);
            var refreshToken = GenerateRefreshToken();

            dbUser.RefreshToken = refreshToken;
            dbUser.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(15);
            _context.SaveChanges();

            return Ok(new
            {
                token = accessToken,
                refreshToken = refreshToken
            });
            //// Validate against in-memory user
            //if (user.Username == currentUser.Username && user.Password == currentUser.Password)
            //{
            //    // Create JWT Token
            //    var accessToken = _jwtService.GenerateToken(user);
            //    // Create Refresh Token
            //    var refreshToken = GenerateRefreshToken();

            //    currentUser.RefreshToken = refreshToken;
            //    currentUser.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(15); // Valid for 5 min

            //    return Ok(new
            //    {
            //        token = accessToken,
            //        refreshToken = refreshToken
            //    });
            //}

            //return Unauthorized("❌ Invalid credentials");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            if (await _context.Users.AnyAsync(u => u.Username == registerRequest.Username))
            {
                return BadRequest("⚠️ Username already exists.");
            }

            var newUser = new User
            {
                Username = registerRequest.Username,
                PasswordHash = PasswordHasher.HashPassword(registerRequest.Password),
                Role = registerRequest.Role
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok("✅ User registered successfully.");

            //if (_context.Users.Any(u => u.Username == user.Username))
            //    return BadRequest("User already exists");

            //var newUser = new User
            //{
            //    Username = user.Username,
            //    PasswordHash = PasswordHasher.HashPassword(user.Password),
            //    Role = "User"
            //};

            //_context.Users.Add(newUser);
            //_context.SaveChanges();

            //return Ok("✅ Registered successfully");
        }

        // ♻️ POST: api/auth/refresh
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenModel tokenModel)
        {
            // Step 1: Find user by refresh token
            var user = await _context.Users.FirstOrDefaultAsync(u =>u.RefreshToken == tokenModel.RefreshToken);

            if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return Unauthorized("❌ Invalid or expired refresh token");
            }

            // Step 2: Generate new tokens
            var newAccessToken = _jwtService.GenerateToken(user);
            var newRefreshToken = GenerateRefreshToken();

            // Step 3: Update user's refresh token in DB
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(15);

            await _context.SaveChangesAsync();

            // Step 4: Return new tokens
            return Ok(new
            {
                token = newAccessToken,
                refreshToken = newRefreshToken
            });
            //// Validate Refresh Token Step 1: Check if Refresh Token is correct
            //if (tokenModel.RefreshToken != currentUser.RefreshToken || currentUser.RefreshTokenExpiryTime <= DateTime.Now)
            //{
            //    return Unauthorized("❌ Invalid or expired refresh token");
            //}

            //// Step 2: Check if Refresh Token has expired
            //if (currentUser.RefreshTokenExpiryTime < DateTime.UtcNow)
            //{
            //    return Unauthorized("❌ Refresh token expired. Please login again.");
            //}

            //// Step 3: Generate new tokens
            //var newAccessToken = _jwtService.GenerateToken(currentUser);
            //var newRefreshToken = GenerateRefreshToken();

            //// Step 4: Update the current user with new refresh token and expiry time
            //currentUser.RefreshToken = newRefreshToken;
            //currentUser.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(15);

            //// Step 5: Return new tokens
            //return Ok(new
            //{
            //    token = newAccessToken,
            //    refreshToken = newRefreshToken
            //});
        }

        // 🔒 GET: api/auth/secure-data
        [HttpGet("secure-data")]
        [Authorize] // Token Required
        public IActionResult SecureData()
        {
            return Ok("🔐 You accessed secure data because you're authorized!");
        }
    }
}