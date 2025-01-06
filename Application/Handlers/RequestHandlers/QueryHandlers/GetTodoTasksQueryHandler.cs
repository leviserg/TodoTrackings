using Application.DTOs;
using Application.Helpers;
using Application.Queries;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.RequestHandlers.QueryHandlers
{
    public class GetTodoTasksQueryHandler(IApplicationDbContext context, ILogger<GetTodoTasksQueryHandler> logger) : IRequestHandler<GetTodoTasksQuery, TodoTasksPaginated>
    {

        private readonly IApplicationDbContext _context = context;
        private readonly ILogger<GetTodoTasksQueryHandler> _logger = logger;
        public async Task<TodoTasksPaginated> Handle(GetTodoTasksQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var (pageNumber, pageSize) = PaginationHelper.ValidatePagination(request.PageNumber, request.PageSize);

                var query = _context.TodoTasks.AsQueryable();

                if (!string.IsNullOrEmpty(request.SearchText))
                {
                    query = query.Where(t => t.Content.Contains(request.SearchText));
                }

                if (!string.IsNullOrEmpty(request.SortBy))
                {
                    query = request.SortBy.ToLower() switch
                    {
                        "content" => (request.SortDesc) ? query.OrderByDescending(t => t.Content) : query.OrderBy(t => t.Content),
                        "createdat" => (request.SortDesc) ? query.OrderByDescending(t => t.CreatedAt) : query.OrderBy(t => t.CreatedAt),
                        "updatedat" => (request.SortDesc) ? query.OrderByDescending(t => t.UpdatedAt) : query.OrderBy(t => t.UpdatedAt),
                        "createdby" => (request.SortDesc) ? query.OrderByDescending(t => t.CreatedBy) : query.OrderBy(t => t.CreatedBy),
                        "iscompleted" => (request.SortDesc) ? query.OrderByDescending(t => t.IsCompleted).ThenByDescending(t => t.CreatedAt) : query.OrderBy(t => t.IsCompleted).ThenByDescending(t => t.CreatedAt),
                        _ => query.OrderBy(t => t.IsCompleted).ThenByDescending(t => t.CreatedAt)
                    };
                }
                else
                {
                    query = query.OrderBy(t => t.IsCompleted).ThenByDescending(t => t.CreatedAt);
                }

                var totalCount = await query.CountAsync(cancellationToken);

                var tasks = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(t => TodoTaskMapper.TodoTaskToDto(t))
                    .ToListAsync(cancellationToken);

                return new TodoTasksPaginated(tasks, totalCount, pageNumber, pageSize);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while handling GetTodoTasksQuery: {Message}", e.Message);
                return new TodoTasksPaginated([], 0, request.PageNumber, request.PageSize);
            }

        }
    }
}
