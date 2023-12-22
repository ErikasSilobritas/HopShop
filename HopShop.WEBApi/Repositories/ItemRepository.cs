using Dapper;
using HopShop.WEBApi.DTOs.Requests;
using HopShop.WEBApi.Entities;
using HopShop.WEBApi.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace HopShop.WEBApi.Repositories
{
    public class ItemRepository 
    {
        private readonly IDbConnection _connection;

        public ItemRepository(IDbConnection connection)
        {
            _connection = connection;
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

        public IEnumerable<Item> GetAllItems()
        {          
            return _connection.Query<Item>("SELECT item_id, item_name, item_price, item_quantity FROM items WHERE is_deleted = false");
        }

        public void AddItem(string itemName, decimal itemPrice, int itemQuantity)
        {
            string sql = $"INSERT INTO items (item_name, item_price, item_quantity) VALUES (@ItemName, @ItemPrice, @ItemQuantity)";
            var queryArguments = new
            {
                ItemName = itemName,
                ItemPrice = itemPrice,
                ItemQuantity = itemQuantity
            };
            _connection.Execute(sql, queryArguments);
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

        public decimal? BuyItem(string itemName)
        {
            string sql = $"SELECT item_price FROM items WHERE item_name = @ItemName AND is_deleted = false";
            var queryArguments = new
            {
                ItemName = itemName
            };

            decimal? itemPrice = _connection.QuerySingleOrDefault<decimal?>(sql, queryArguments);
            return itemPrice;
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

        public Item GetItemByName(string itemName)
        {
            throw new NotImplementedException();
        }
    }
}
