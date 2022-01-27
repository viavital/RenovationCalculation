using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RenovationCalculation.Model;

namespace RenovationCalculation.Service
{
    class TotalSumCounterService
    {       
        public int CountTotalSum (ObservableCollection<TypeOfWorkModel> Works)
        {
           List<TypeOfWorkModel> ListOfWorks  = new List<TypeOfWorkModel>(Works);
            int TotalSum = ListOfWorks.Sum(u => u.TotalCostOfWork);           
            return TotalSum + CountSumOfInventory();
        }
        public int CountSumOfInventory()
        {
            int TotalSum;
            using (WorksDBContext db = new WorksDBContext())
            {
                TotalSum = db.Inventory.Sum(u => u.PriceOfInventory);
            }
            return TotalSum;
        }
    }
}
