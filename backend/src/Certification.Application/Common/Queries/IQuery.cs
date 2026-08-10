using MediatR;

namespace Certification.Application.Common.Queries;

public interface IQuery<TResponse> : IRequest<TResponse>
{
}
