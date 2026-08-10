using Certification.Application.Common.Interfaces;
using MapsterMapper;

namespace Certification.Infrastructure.Mapping;

public sealed class MapsterMapperService : IMapperService
{
    private readonly IMapper _mapper;

    public MapsterMapperService(IMapper mapper)
    {
        _mapper = mapper;
    }

    public TDestination Map<TDestination>(object source) => _mapper.Map<TDestination>(source);

    public TDestination Map<TSource, TDestination>(TSource source) => _mapper.Map<TSource, TDestination>(source);
}
