namespace Application.DTOs
{
    public class TodoTasksPaginated
    {
        public List<TodoTaskDto> Tasks { get; set; }
        public int TotalCount { get; set; }
        public bool HasMoreItems { get; set; }

        public TodoTasksPaginated(List<TodoTaskDto> tasks, int totalCount, int pageNumber, int pageSize)
        {
            Tasks = tasks;
            TotalCount = totalCount;
            HasMoreItems = (pageNumber * pageSize) < totalCount;
        }
    }
}
