using System;

namespace student.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime? Dob { get; set; }
        public int? Age { get; set; }
        public string Gender { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Stage { get; set; }
        public string Passwordhash { get; set; } = null!;
        public string? Address { get; set; }
        public string? Profileimageurl { get; set; }
        public string? Lastlogged { get; set; }  // ✅ MUST be string (NVARCHAR in DB)
        public string? Phonenumber { get; set; }
        public DateTime? Createdat { get; set; }
        public DateTime? Updatedat { get; set; }
    }
}
