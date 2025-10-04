using Microsoft.AspNetCore.Identity;
using OnlineExam.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace OnlineExam.Domain
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        public List<RefreshToken>? RefreshTokens { get; set; }


    }
}
