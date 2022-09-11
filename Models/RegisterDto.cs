namespace Movie.Models
{
    public class RegisterDto
    {
        [MaxLength(100)]
        public string UserName { get; set; }
        public string Email { get; set; }
        [MaxLength(50)]
        public string Password { get; set; }
    }
}
