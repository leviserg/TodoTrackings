namespace Application.DTOs
{
    public class TodoTaskDto
    {
        public Guid Id { get; set; }
        public string? Content { get; set; }
        public string? CreatedBy { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public TodoTaskDto()
        {
            
        }
    }
}
