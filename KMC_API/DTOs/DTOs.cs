namespace KMC_API.DTOs
{
    public class RegisterRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = "Public";
    }

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public int UserId { get; set; }
    }

    public class CreateEventRequest
    {
        public string Title { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string Type { get; set; } = string.Empty;
        public int Capacity { get; set; }
    }

    public class UpdateEventRequest
    {
        public string Title { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string Type { get; set; } = string.Empty;
        public int Capacity { get; set; }
    }

    public class EventResponse
    {
        public int Id { get; set; }

        public string? ImageUrl { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string Type { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public int RegisteredCount { get; set; }
        public string OrganizerName { get; set; } = string.Empty;
        public int OrganizerId { get; set; }
    }

    public class RegisterForEventRequest
    {
        public int EventId { get; set; }
    }

    public class RegistrationResponse
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string EventTitle { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public DateTime RegisteredAt { get; set; }
    }

    public class EventSearchRequest
    {
        public string? Type { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? Keyword { get; set; }
    }
}