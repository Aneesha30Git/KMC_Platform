using System.Security.Claims;
using KMC_API.DTOs;
using KMC_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KMC_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistrationsController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;

        public RegistrationsController(IRegistrationService registrationService) =>
            _registrationService = registrationService;

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Register(RegisterForEventRequest request)
        {
            var userId = GetUserId();
            var (success, message, data) = await _registrationService.Register(request.EventId, userId);
            if (!success)
                return BadRequest(new { message });
            return Ok(new { message, data });
        }

        [HttpDelete("{eventId}")]
        [Authorize]
        public async Task<IActionResult> Unregister(int eventId)
        {
            var userId = GetUserId();
            var (success, message) = await _registrationService.Unregister(eventId, userId);
            if (!success)
                return NotFound(new { message });
            return Ok(new { message });
        }

        [HttpGet("my")]
        [Authorize]
        public async Task<IActionResult> GetMyRegistrations()
        {
            var userId = GetUserId();
            return Ok(await _registrationService.GetMyRegistrations(userId));
        }

        [HttpGet("event/{eventId}")]
        [Authorize(Roles = "Organizer")]
        public async Task<IActionResult> GetEventRegistrations(int eventId)
        {
            var organizerId = GetUserId();
            var result = await _registrationService.GetEventRegistrations(eventId, organizerId);
            return Ok(result);
        }

        private int GetUserId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}