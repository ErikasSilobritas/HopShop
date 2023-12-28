using Dapper;
using Domain.Entities;
using System.Data;

namespace Infrastructure.Repositories
{
    public class ItemRepository 
    {
        private readonly IDbConnection _connection;

        public ItemRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public IEnumerable<Item> GetAllItems()
        {
            return _connection.Query<Item>("SELECT item_id, item_name, item_price, item_quantity FROM items WHERE is_deleted = false");
        }

        public Item GetItemById(int id)
        {
            string sql = $"SELECT item_id, item_name, item_price, item_quantity FROM items WHERE item_id = @ItemId AND is_deleted = false";
            var queryArguments = new
            {
                ItemId = id
            };

            return _connection.QuerySingle<Item>(sql, queryArguments);
        }

        public Item EditItem(Item item)
        {
            string sql = $"UPDATE items SET item_name = @ItemName, item_price = @ItemPrice, item_quantity = @ItemQuantity WHERE item_id = @ItemId " +
                "RETURNING item_id, item_name, item_price, item_quantity";
            var queryArguments = new
            {
                ItemId = item.Id,
                ItemName = item.Name,
                ItemPrice = item.Price,
                ItemQuantity = item.Quantity
            };

            var updatedItem = _connection.QuerySingle<Item>(sql, queryArguments);
            return updatedItem;
        }
        public void AddItem(Item item)
        {
            string sql = $"INSERT INTO items (item_name, item_price, item_quantity) VALUES (@ItemName, @ItemPrice, @ItemQuantity)";
            var queryArguments = new
            {
                ItemName = item.Name,
                ItemPrice = item.Price,
                ItemQuantity = item.Quantity
            };

            _connection.Execute(sql, queryArguments);
        }

        public void DeleteItem (string itemName)
        {
            string sql = $"UPDATE items SET is_deleted = true WHERE item_name = @ItemName";
            var queryArguments = new
            {
                ItemName = itemName
            };

            _connection.Execute(sql, queryArguments);
        }
    }
}
