using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shelter.Application.Bookings.GetBooking;
using Shelter.Application.Bookings.ReserveBooking;

namespace Shelter.Api.Controllers.Bookings;

[Authorize]
[ApiController]
[Route("api/bookings")]
public class BookingsController : ControllerBase
{
    private readonly ISender _sender;

    public BookingsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("id")]
    public async Task<IActionResult> GetBookings(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetBookingQuery(id);
        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> ReserveBooking(
        ReserveBookingRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ReserveBookingCommand(
            request.PetSitterId,
            request.UserId,
            request.StartDate,
            request.EndDate);

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return CreatedAtAction(nameof(GetBookings), new { Id = result.Value }, result.Value);
    }
}
