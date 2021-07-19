using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MochaCoffee.Data;
using MochaCoffee.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MochaCoffee.Services.Inventory
{
    public class InventoryService : IInventoryService
    {
        private readonly MochaDbContext _db;
        private readonly ILogger<InventoryService> _logger;

        public InventoryService(MochaDbContext dbContext, ILogger<InventoryService> logger)
        {
            _db = dbContext;
            _logger = logger;
        }

        private void CreateSnapshot()
        {
            var now = DateTime.UtcNow;

            var inventories = _db.ProductInventories
                .Include(inv => inv.Product)
                .ToList();

            foreach (var inventory in inventories)
            {
                var snapshot = new ProductInventorySnapshot
                {
                    SnapshotTime = now,
                    Product = inventory.Product,
                    QuantityOnHand = inventory.QuantityOnHand
                };

                _db.Add(snapshot);
            }
        }

        public ProductInventory GetByProductId(int productId)
        {
            return _db.ProductInventories
                .Include(pi => pi.Product)
                .FirstOrDefault(pi => pi.Product.Id == productId);
        }

        public List<ProductInventory> GetCurrentInventory()
        {
            return _db.ProductInventories
                .Include(pi => pi.Product)
                .Where(pi => !pi.Product.IsArchived)
                .ToList();
        }

        public List<ProductInventorySnapshot> GetSnapshotHistory()
        {
            var earliest = DateTime.UtcNow - TimeSpan.FromHours(6);

            return _db.ProductInventorySnapshots
                .Include(snap => snap.Product)
                .Where(snap => snap.SnapshotTime > earliest && !snap.Product.IsArchived)
                .ToList(); 
        }

        public ServiceResponse<ProductInventory> UpdateUnitsAvailable(int id, int adjustment)
        {
            var now = DateTime.UtcNow;

            try
            {
                var inventory = _db.ProductInventories
                    .Include(inv => inv.Product)
                    .First(inv => inv.Product.Id == id);

                inventory.QuantityOnHand += adjustment;

                try
                {
                    CreateSnapshot();
                }
                catch (Exception e)
                {
                    _logger.LogError("Error creating inventory snapshot.");
                    _logger.LogError(e.StackTrace);

                }

                _db.SaveChanges();

                return new ServiceResponse<ProductInventory>
                {
                    Time = now,
                    IsSuccess = true,
                    Message = $"Product {id} inventory adjusted",
                    Data = inventory
                };
            }
            catch (Exception e)
            {
                return new ServiceResponse<ProductInventory>
                {
                    Time = now,
                    IsSuccess = false,
                    Message = $"Error updating ProductInventory QuantityOnHand",
                    Data = null
                };
            }
        }
    }
}
