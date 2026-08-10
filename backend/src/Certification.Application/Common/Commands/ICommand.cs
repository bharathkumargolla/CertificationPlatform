using MediatR;

namespace Certification.Application.Common.Commands;

public interface ICommand<TResponse> : IRequest<TResponse>
{
}
