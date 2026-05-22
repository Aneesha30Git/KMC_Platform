using KMC_API.Data;
using KMC_API.DTOs;
using KMC_API.Models;
using Microsoft.EntityFrameworkCore;

namespace KMC_API.Services
{
    public interface IRegistrationService
    {
        Task<(bool success, string message, RegistrationResponse? data)> Register(int eventId, int userId);
        Task<(bool success, string message)> Unregister(int eventId, int userId);
        Task<List<RegistrationResponse>> GetMyRegistrations(int userId);
        Task<List<RegistrationResponse>> GetEventRegistrations(int eventId, int organizerId);
    }

    public class RegistrationService : IRegistrationService
    {
        private readonly AppDbContext _db;

        public RegistrationService(AppDbContext db) => _db = db;

        public async Task<(bool success, string message, RegistrationResponse? data)> Register(int eventId, int userId)
        {
            var ev = await _db.Events
                .Include(e => e.Registrations)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (ev == null)
                return (false, "Event not found.", null);

            if (ev.Registrations.Count >= ev.Capacity)
                return (false, "Event is fully booked.", null);

            var alreadyRegistered = await _db.Registrations
                .AnyAsync(r => r.EventId == eventId && r.UserId == userId);

            if (alreadyRegistered)
                return (false, "You are already registered for this event.", null);

            var registration = new Registration
            {
                EventId = eventId,
                UserId = userId,
                RegisteredAt = DateTime.UtcNow
            };

            _db.Registrations.Add(registration);
            await _db.SaveChangesAsync();

            await _db.Entry(registration).Reference(r => r.Event).LoadAsync();
            await _db.Entry(registration).Reference(r => r.User).LoadAsync();

            return (true, "Successfully registered!", new RegistrationResponse
            {
                Id = registration.Id,
                EventId = registration.EventId,
                EventTitle = registration.Event?.Title ?? "",
                UserName = registration.User?.Name ?? "",
                RegisteredAt = registration.RegisteredAt
            });
        }

        public async Task<(bool success, string message)> Unregister(int eventId, int userId)
        {
            var registration = await _db.Registrations
                .FirstOrDefaultAsync(r => r.EventId == eventId && r.UserId == userId);

            if (registration == null)
                return (false, "Registration not found.");

            _db.Registrations.Remove(registration);
            await _db.SaveChangesAsync();
            return (true, "Successfully unregistered.");
        }

        public async Task<List<RegistrationResponse>> GetMyRegistrations(int userId)
        {
            return await _db.Registrations
                .Include(r => r.Event)
                .Include(r => r.User)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.RegisteredAt)
                .Select(r => new RegistrationResponse
                {
                    Id = r.Id,
                    EventId = r.EventId,
                    EventTitle = r.Event!.Title,
                    UserName = r.User!.Name,
                    RegisteredAt = r.RegisteredAt
                })
                .ToListAsync();
        }

        public async Task<List<RegistrationResponse>> GetEventRegistrations(int eventId, int organizerId)
        {
            var ev = await _db.Events.FindAsync(eventId);
            if (ev == null || ev.OrganizerId != organizerId)
                return new List<RegistrationResponse>();

            return await _db.Registrations
                .Include(r => r.User)
                .Where(r => r.EventId == eventId)
                .Select(r => new RegistrationResponse
                {
                    Id = r.Id,
                    EventId = r.EventId,
                    EventTitle = ev.Title,
                    UserName = r.User!.Name,
                    RegisteredAt = r.RegisteredAt
                })
                .ToListAsync();
        }
    }
}