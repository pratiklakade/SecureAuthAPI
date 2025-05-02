using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIDevelopmentinASP.NETCore.Model
{
    public class User
    {
        [Key] // Primary Key
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [NotMapped]
        public string Password { get; set; } // 🔐 Input ke liye, DB me save nahi hoga

        [Required]
        public string PasswordHash { get; set; }

        public string Role { get; set; } = "User"; // default role

        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
