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
            AddDomainEvent(new TodoTaskCreated(Id, Content, CreatedBy));
        }

        public void UpdateTodoTask(string content, bool isCompleted)
        {
            Content = content;
            UpdatedAt = DateTime.UtcNow;
            IsCompleted = isCompleted;
            AddDomainEvent(new TodoTaskUpdated(Id, content, isCompleted));
        }

        public void DeleteTodoTask()
        {
            AddDomainEvent(new TodoTaskDeleted(Id));
        }

    }
}
