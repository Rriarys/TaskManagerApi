namespace TaskManagerApi
{
    public class TaskItem
    {
        public int Id { get; set; } // uniq ID
        public string Title { get; set; } // task name
        public string Description { get; set; }
        public string Status { get; set; } = "In process";
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime? UpdateDate { get; set; }

    }
}
