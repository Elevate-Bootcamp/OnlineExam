using Microsoft.AspNetCore.Identity;
using OnlineExam.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using TechZone.Core.Entities;

namespace OnlineExam.Domain
{
    public class ApplicationUser : IdentityUser
    {
        //[MaxLength(100)]
        //public string FullName { get; set; } = string.Empty;
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        public List<RefreshToken>? RefreshTokens { get; set; }
        public virtual ICollection<VerificationCode> VerificationCodes { get; set; } = new List<VerificationCode>();



    }
}
