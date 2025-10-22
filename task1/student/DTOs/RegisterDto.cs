namespace student.DTOs
{
    public class RegisterDto
    {
        public string Name { get; set; } = string.Empty;
        public DateTime Dob { get; set; }
        public string Role { get; set; } = string.Empty;  // ✅ Add this
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string? Address { get; set; }              // ✅ Add this
        public string? Phonenumber { get; set; }          // ✅ Add this
        public string? Stage { get; set; }                // ✅ Add this
    }
}
