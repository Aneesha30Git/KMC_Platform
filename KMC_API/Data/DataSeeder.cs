using KMC_API.Models;

namespace KMC_API.Data
{
    public static class DataSeeder
    {
        public static void Seed(AppDbContext db)
        {
            if (db.Users.Any()) return;

            var organizer1 = new User
            {
                Name = "Perera Events",
                Email = "perera@events.lk",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                Role = "Organizer"
            };
            var organizer2 = new User
            {
                Name = "KMC Admin",
                Email = "admin@kmc.lk",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                Role = "Organizer"
            };
            var publicUser = new User
            {
                Name = "Nimal Silva",
                Email = "nimal@gmail.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                Role = "Public"
            };

            db.Users.AddRange(organizer1, organizer2, publicUser);
            db.SaveChanges();

            var events = new List<Event>
            {
                new Event
                {
                    Title = "Kandy Esala Perahera",
                    Description = "The grand annual Kandy Esala Perahera — a magnificent festival of lights and traditional dance.",
                    Location = "Kandy City Centre",
                    EventDate = DateTime.UtcNow.AddDays(30),
                    Type = "Cultural",
                    Capacity = 500,
                    OrganizerId = organizer2.Id
                },
                new Event
                {
                    Title = "Kandy City Run 2025",
                    Description = "Annual 10K city run through the scenic roads of Kandy. Open to all fitness levels.",
                    Location = "Kandy Lake",
                    EventDate = DateTime.UtcNow.AddDays(15),
                    Type = "Sports",
                    Capacity = 200,
                    OrganizerId = organizer1.Id
                },
                new Event
                {
                    Title = "Sri Lanka Music Night",
                    Description = "A night of live music featuring top Sri Lankan artists.",
                    Location = "Queen's Hotel Kandy",
                    EventDate = DateTime.UtcNow.AddDays(7),
                    Type = "Music",
                    Capacity = 150,
                    OrganizerId = organizer1.Id
                },
                new Event
                {
                    Title = "Kandy Food Festival",
                    Description = "Taste the best of Kandyan cuisine — street food, traditional sweets, and more.",
                    Location = "Victoria Park, Kandy",
                    EventDate = DateTime.UtcNow.AddDays(20),
                    Type = "Food",
                    Capacity = 300,
                    OrganizerId = organizer2.Id
                }
            };

            db.Events.AddRange(events);
            db.SaveChanges();
        }
    }
}