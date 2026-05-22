namespace KMC_API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "Public";

        public ICollection<Event> Events { get; set; } = new List<Event>();
        public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
    }

    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string Type { get; set; } = string.Empty;
        public int Capacity { get; set; }

        public string? ImageUrl { get; set; }

        public int OrganizerId { get; set; }
        public User? Organizer { get; set; }

        public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
    }

    public class Registration
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public Event? Event { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
    }
}