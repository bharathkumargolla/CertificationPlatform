using Certification.Application.Certifications.DTOs;
using Certification.Domain.Entities;
using Mapster;

namespace Certification.Application.Certifications.Mappings;

public sealed class CertificationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CertificationDefinition, CertificationDto>();
    }
}
