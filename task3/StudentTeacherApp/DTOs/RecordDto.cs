using System.ComponentModel.DataAnnotations;

namespace StudentTeacherApp.DTOs
{
    public class RecordDto
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;
    }
}
