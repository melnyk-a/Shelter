﻿using MediatR;
using Shelter.Application.Bookings.GetBooking;
using Shelter.Application.Bookings.ReserveBooking;

namespace Shelter.Api.Controllers.Bookings;

public static class BookingsEndpoints
{
    public static IEndpointRouteBuilder MapBookingEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("bookings/{id}", GetBooking)
            .RequireAuthorization("bookings:read")
            .WithName(nameof(GetBooking));

        builder.MapPost("bookings", ReserveBooking)
           .RequireAuthorization();

        return builder;
    }

    public static async Task<IResult> GetBooking(
        Guid id,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new GetBookingQuery(id);
        var result = await sender.Send(query, cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound();
    }

    public static async Task<IResult> ReserveBooking(
        ReserveBookingRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new ReserveBookingCommand(
            request.PetSitterId,
            request.UserId,
            request.StartDate,
            request.EndDate);

        var result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return Results.BadRequest(result.Error);
        }

        return Results.CreatedAtRoute(
            nameof(GetBooking),
            new { Id = result.Value },
            result.Value);
    }
}
