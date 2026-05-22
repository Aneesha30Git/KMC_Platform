using System.Security.Claims;
using KMC_API.DTOs;
using KMC_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KMC_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService) => _eventService = eventService;

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _eventService.GetAll());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var ev = await _eventService.GetById(id);
            return ev == null ? NotFound() : Ok(ev);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] EventSearchRequest request) =>
            Ok(await _eventService.Search(request));

        [HttpGet("my-events")]
        [Authorize(Roles = "Organizer")]
        public async Task<IActionResult> GetMyEvents()
        {
            var organizerId = GetUserId();
            return Ok(await _eventService.GetByOrganizer(organizerId));
        }

        [HttpPost]
        [Authorize(Roles = "Organizer")]
        public async Task<IActionResult> Create(CreateEventRequest request)
        {
            var organizerId = GetUserId();
            var ev = await _eventService.Create(request, organizerId);
            return CreatedAtAction(nameof(GetById), new { id = ev.Id }, ev);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Organizer")]
        public async Task<IActionResult> Update(int id, UpdateEventRequest request)
        {
            var organizerId = GetUserId();
            var ev = await _eventService.Update(id, request, organizerId);
            if (ev == null)
                return NotFound(new { message = "Event not found or you are not the organizer." });
            return Ok(ev);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Organizer")]
        public async Task<IActionResult> Delete(int id)
        {
            var organizerId = GetUserId();
            var deleted = await _eventService.Delete(id, organizerId);
            if (!deleted)
                return NotFound(new { message = "Event not found or you are not the organizer." });
            return NoContent();
        }

        private int GetUserId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}