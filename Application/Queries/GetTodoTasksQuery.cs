using Application.DTOs;
using MediatR;

namespace Application.Queries
{
    public class GetTodoTasksQuery : IRequest<TodoTasksPaginated>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? SearchText { get; set; }
        public string? SortBy { get; set; }
        public bool SortDesc { get; set; } = false;

        public GetTodoTasksQuery(int pageNumber, int pageSize, string? searchText, string? sortBy, bool sortDesc)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchText = searchText;
            SortBy = sortBy;
            SortDesc = sortDesc;
        }
    }
}
