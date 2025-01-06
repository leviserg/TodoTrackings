namespace Application.Helpers
{
    public static class PaginationHelper
    {
        private const int DefaultPageSize = 10;
        private const int MaxPageSize = 100;

        public static (int PageNumber, int PageSize) ValidatePagination(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0)
            {
                pageNumber = 1;
            }

            if (pageSize <= 0)
            {
                pageSize = DefaultPageSize;
            }
            else if (pageSize > MaxPageSize)
            {
                pageSize = MaxPageSize;
            }

            return (pageNumber, pageSize);
        }
    }
}
