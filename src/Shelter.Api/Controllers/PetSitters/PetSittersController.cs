using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shelter.Application.PetSitters.SearchPetSitters;

namespace Shelter.Api.Controllers.PetSitters;

[Authorize]
[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/petsitters")]
public class PetSittersController : ControllerBase
{
    private readonly ISender _sender;

    public PetSittersController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> SearchPetSitters(
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken)
    {
        var query = new SearchPetSitterQuery(startDate, endDate);
        var result = await _sender.Send(query, cancellationToken);

        return Ok(result.Value);
    }
}
