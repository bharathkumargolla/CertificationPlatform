using Certification.Application.Questions.DTOs;
using Certification.Domain.Entities;
using Mapster;

namespace Certification.Application.Questions.Mappings;

public sealed class QuestionMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<QuestionOption, QuestionOptionDto>();

        config.NewConfig<Question, QuestionDto>()
            .Map(dest => dest.Options, src => src.QuestionOptions);
    }
}
