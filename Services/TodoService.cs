using cosmos_crud.Models;
using Microsoft.Azure.Cosmos.Table;

namespace cosmos_crud.Services
{
    public class TodoService
    {
        private readonly CloudTable _table;

        public TodoService(string storageConnectionString, string tableName)
        {
            var storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
            _table = tableClient.GetTableReference(tableName);
            _table.CreateIfNotExists();
        }

        public async Task<TodoItem> CreateTodoItemAsync(TodoItem todoItem)
        {
            var insertOperation = TableOperation.Insert(todoItem);
            var result = await _table.ExecuteAsync(insertOperation);
            return result.Result as TodoItem;
        }

        public async Task<TodoItem> GetTodoItemAsync(string id)
        {
            var retrieveOperation = TableOperation.Retrieve<TodoItem>("TodoItem", id);
            var result = await _table.ExecuteAsync(retrieveOperation);
            return result.Result as TodoItem;
        }

        public async Task<IEnumerable<TodoItem>> GetAllTodoItemsAsync()
        {
            var query = new TableQuery<TodoItem>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "TodoItem"));
            var result = await _table.ExecuteQuerySegmentedAsync(query, null);
            return result.Results;
        }

        public async Task<TodoItem> UpdateTodoItemAsync(TodoItem todoItem)
        {
            var replaceOperation = TableOperation.Replace(todoItem);
            var result = await _table.ExecuteAsync(replaceOperation);
            return result.Result as TodoItem;
        }

        public async Task DeleteTodoItemAsync(string id)
        {
            var todoItem = await GetTodoItemAsync(id);
            var deleteOperation = TableOperation.Delete(todoItem);
            await _table.ExecuteAsync(deleteOperation);
        }
    }
}
