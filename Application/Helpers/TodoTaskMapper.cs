using Application.DTOs;
using Domain.Entities;

namespace Application.Helpers
{
    public static class TodoTaskMapper
    {
        public static TodoTaskDto? TodoTaskToDto(TodoTask? todoTask)
        {
            if (todoTask == null)
            {
                return null;
            }

            return new TodoTaskDto
            {
                Id = todoTask.Id,
                Content = todoTask.Content,
                CreatedBy = todoTask.CreatedBy,
                IsCompleted = todoTask.IsCompleted,
                CreatedAt = todoTask.CreatedAt,
                UpdatedAt = todoTask.UpdatedAt
            };
        }
    }
}
