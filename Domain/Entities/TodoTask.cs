using Domain.Events;

namespace Domain.Entities
{
    public class TodoTask : Entity
    {
        public Guid Id { get; set; }
        public string Content { get; private set; }
        public string CreatedBy { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public bool IsCompleted { get; private set; }
        public DateTime? UpdatedAt { get; private set; }


        public TodoTask(string content, string createdBy)
        {
            Id = Guid.NewGuid();
            Content = content;
            CreatedBy = createdBy;
            CreatedAt = DateTime.UtcNow;
            IsCompleted = false;
        }

        public void AddTodoTaskEvent(TodoTask TodoItem)
        {
            AddDomainEvent(new TodoTaskCreated(Guid.NewGuid(), TodoItem));
        }

        public void UpdateTodoTaskEvent(TodoTask TodoItem, string content, bool isCompleted)
        {
            Content = content;
            IsCompleted = isCompleted;
            UpdatedAt = DateTime.UtcNow;
            AddDomainEvent(new TodoTaskUpdated(Guid.NewGuid(), TodoItem));
        }

    }
}
