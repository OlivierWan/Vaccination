using AutoMapper;
using Vaccination.Domain.Shared;

namespace Vaccination.Application.Mapper
{
    public class PagedListConverter<TSource, TDestination> : ITypeConverter<PagedList<TSource>, PagedList<TDestination>>
    {
        private readonly IMapper _mapper;

        public PagedListConverter(IMapper mapper)
        {
            _mapper = mapper;
        }

        public PagedList<TDestination> Convert(PagedList<TSource> source, PagedList<TDestination> destination, ResolutionContext context)
        {
            IEnumerable<TDestination> mappedItems = _mapper.Map<IEnumerable<TSource>, IEnumerable<TDestination>>(source);
            return new PagedList<TDestination>(mappedItems.ToList(), source.TotalCount, source.CurrentPage, source.PageSize);
        }
    }
}