namespace Vaccination.Application.Dtos.Vaccination
{
    public record GetFilteredUserVaccinationRequest(
        string? CriteriaSearch,
        string OrderBy,
        string OrderDirection,
        int PageNumber = 1,
        int PageSize = 50
    )
    {
        private const int maxPageSize = 50;
        private int _pageSize = PageSize;

        public int PageSize
        {
            get => _pageSize;
            init => _pageSize = value > maxPageSize ? maxPageSize : value;
        }
    }
}