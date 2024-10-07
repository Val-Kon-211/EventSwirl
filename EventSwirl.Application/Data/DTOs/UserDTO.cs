namespace EventSwirl.Application.Data.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string? City { get; set; }

        public string? Email { get; set; }
    }
}
