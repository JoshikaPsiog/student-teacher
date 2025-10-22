namespace UserManagementAPI.Models
{
    public class UserResponse
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Designation { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsRegistered { get; set; }
    }
}
