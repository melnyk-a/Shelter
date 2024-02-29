using MediatR;
using Shelter.Domain.Abstractions;

namespace Shelter.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}