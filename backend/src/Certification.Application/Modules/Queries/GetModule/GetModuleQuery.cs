using Certification.Application.Common.Queries;
using Certification.Application.Common.Results;
using Certification.Application.Modules.DTOs;

namespace Certification.Application.Modules.Queries.GetModule;

public sealed class GetModuleQuery : IQuery<Result<ModuleDto>>
{
    public Guid Id { get; init; }
}
