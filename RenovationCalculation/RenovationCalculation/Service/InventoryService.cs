using RenovationCalculation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenovationCalculation.Service
{
    class InventoryService
    {
        private InventoryService() { }
        private static InventoryService _instance;
        public event Action<InventoryModel> InventoryAddedEvent = delegate { };
        public event Action<InventoryModel> InventoryDeletedEvent = delegate { };

        public static InventoryService GetInstance() 
        {
            if (_instance == null)
            {
                _instance = new InventoryService();
            }
            return _instance;
        }

        private List<InventoryModel> _allInventory;

        public List<InventoryModel> GetAllInventory()
        {
            if (_allInventory == null) 
            {
                using (var db = new WorksDBContext())
                {
                    _allInventory = db.Inventory.ToList();
                }
            }
            return _allInventory;
        }
        public void AddInventory(InventoryModel inventory)
        {
            using (var db = new WorksDBContext())
            {
                db.Inventory.Add(inventory);
                db.SaveChanges();
            }
            _allInventory.Add(inventory);
            InventoryAddedEvent(inventory);
        }
            public void DeleteInventory(InventoryModel inventory)
        {
            using (var db = new WorksDBContext())
            {
                db.Inventory.Remove(inventory);
                db.SaveChanges();
            }
           _allInventory.Remove(inventory);
            InventoryDeletedEvent(inventory);
        }
    }
}
