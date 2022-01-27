using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace RenovationCalculation.Model
{
    class InventoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PriceOfInventory { get; set; }
        //public override string ToString()
        //{            
        //    return Name+" - "+PriceOfInventory+"$";
        //}
    }
}

