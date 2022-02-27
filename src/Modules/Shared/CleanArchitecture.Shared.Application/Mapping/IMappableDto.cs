using AutoMapper;

namespace CleanArchitecture.Shared.Application.Mapping
{
    public interface IMappableDto<TSource>
    {
        void MappingConfig(Profile profile) => profile.CreateMap(typeof(TSource), GetType());
    }
}
