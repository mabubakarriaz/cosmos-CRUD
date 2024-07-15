using Microsoft.Azure.Cosmos.Table;

namespace cosmos_crud.Models
{
    public class TodoItem : TableEntity
    {
        public string TaskTitle { get; set; }
        public string TaskDescription { get; set; }
        public DateTime CreatedOn { get; set; }

        public TodoItem()
        {
            PartitionKey = "TodoItem";
            RowKey = Guid.NewGuid().ToString();
        }

        public string Id
        {
            get => RowKey;
            set => RowKey = value;
        }
    }
}
