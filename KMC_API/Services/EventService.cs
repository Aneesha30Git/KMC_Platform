using KMC_API.Data;
using KMC_API.DTOs;
using KMC_API.Models;
using Microsoft.EntityFrameworkCore;

namespace KMC_API.Services
{
    public interface IEventService
    {
        Task<List<EventResponse>> GetAll();
        Task<EventResponse?> GetById(int id);
        Task<EventResponse> Create(CreateEventRequest request, int organizerId);
        Task<EventResponse?> Update(int id, UpdateEventRequest request, int organizerId);
        Task<bool> Delete(int id, int organizerId);
        Task<List<EventResponse>> Search(EventSearchRequest request);
        Task<List<EventResponse>> GetByOrganizer(int organizerId);
    }

    public class EventService : IEventService
    {
        private readonly AppDbContext _db;

        public EventService(AppDbContext db) => _db = db;

        public async Task<List<EventResponse>> GetAll()
        {
            var events = await _db.Events
                .Include(e => e.Organizer)
                .Include(e => e.Registrations)
                .OrderBy(e => e.EventDate)
                .ToListAsync();
            return events.Select(MapToResponse).ToList();
        }

        public async Task<EventResponse?> GetById(int id)
        {
            var ev = await _db.Events
                .Include(e => e.Organizer)
                .Include(e => e.Registrations)
                .FirstOrDefaultAsync(e => e.Id == id);
            return ev == null ? null : MapToResponse(ev);
        }

        public async Task<EventResponse> Create(CreateEventRequest request, int organizerId)
        {
            var ev = new Event
            {
                Title = request.Title,
                Description = request.Description,
                Location = request.Location,
                EventDate = request.EventDate,
                Type = request.Type,
                Capacity = request.Capacity,
                ImageUrl = request.ImageUrl,
                OrganizerId = organizerId
            };

            _db.Events.Add(ev);
            await _db.SaveChangesAsync();
            await _db.Entry(ev).Reference(e => e.Organizer).LoadAsync();
            return MapToResponse(ev);
        }

        public async Task<EventResponse?> Update(int id, UpdateEventRequest request, int organizerId)
        {
            var ev = await _db.Events
                .Include(e => e.Organizer)
                .Include(e => e.Registrations)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (ev == null) return null;
            if (ev.OrganizerId != organizerId) return null;

            ev.Title = request.Title;
            ev.Description = request.Description;
            ev.Location = request.Location;
            ev.EventDate = request.EventDate;
            ev.Type = request.Type;
            ev.Capacity = request.Capacity;
            ev.ImageUrl = request.ImageUrl;

            await _db.SaveChangesAsync();
            return MapToResponse(ev);
        }

        public async Task<bool> Delete(int id, int organizerId)
        {
            var ev = await _db.Events.FindAsync(id);
            if (ev == null) return false;
            if (ev.OrganizerId != organizerId) return false;

            _db.Events.Remove(ev);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<List<EventResponse>> Search(EventSearchRequest request)
        {
            var query = _db.Events
                .Include(e => e.Organizer)
                .Include(e => e.Registrations)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Type))
                query = query.Where(e => e.Type.ToLower() == request.Type.ToLower());

            if (request.FromDate.HasValue)
                query = query.Where(e => e.EventDate >= request.FromDate.Value);

            if (request.ToDate.HasValue)
                query = query.Where(e => e.EventDate <= request.ToDate.Value);

            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                var kw = request.Keyword.ToLower();
                query = query.Where(e =>
                    e.Title.ToLower().Contains(kw) ||
                    e.Description.ToLower().Contains(kw) ||
                    e.Location.ToLower().Contains(kw));
            }

            var results = await query.OrderBy(e => e.EventDate).ToListAsync();
            return results.Select(MapToResponse).ToList();
        }

        public async Task<List<EventResponse>> GetByOrganizer(int organizerId)
        {
            var events = await _db.Events
                .Include(e => e.Organizer)
                .Include(e => e.Registrations)
                .Where(e => e.OrganizerId == organizerId)
                .OrderBy(e => e.EventDate)
                .ToListAsync();
            return events.Select(MapToResponse).ToList();
        }

        private static EventResponse MapToResponse(Event e) => new EventResponse
        {
            Id = e.Id,
            ImageUrl = e.ImageUrl,
            Title = e.Title,
            Description = e.Description,
            Location = e.Location,
            EventDate = e.EventDate,
            Type = e.Type,
            Capacity = e.Capacity,
            RegisteredCount = e.Registrations?.Count ?? 0,
            OrganizerName = e.Organizer?.Name ?? "",
            OrganizerId = e.OrganizerId
        };
    }
}