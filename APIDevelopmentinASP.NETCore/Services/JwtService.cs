using APIDevelopmentinASP.NETCore.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APIDevelopmentinASP.NETCore.Services
{

        public class JwtService
        {
            private readonly IConfiguration _configuration;

            public JwtService(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            public string GenerateToken(User user)
            {
                var claims = new[]
                {
                   new Claim(ClaimTypes.Name, user.Username),
                   new Claim(ClaimTypes.Role, user.Role),
                };

                var keyString = _configuration["Jwt:Key"];
                Console.WriteLine($"🔐 JWT Key: {keyString}");
                Console.WriteLine($"🔐 JWT Key Length: {keyString.Length}");

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(2),
                   //expires: DateTime.Now.AddMinutes(Convert.ToDouble(_config["Jwt:DurationInMinutes"])),
                    signingCredentials: creds);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
        }
    
}

