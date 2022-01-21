using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenovationCalculation.Model;

namespace RenovationCalculation.Service
{
    class TotalSumCounterService
    {       
        public int CountTotalSum (ObservableCollection<TypeOfWorkModel> Works)
        {
           List<TypeOfWorkModel> ListOfWorks  = new List<TypeOfWorkModel>(Works);
            int TotalSum = ListOfWorks.Sum(u => u.TotalCostOfWork);
            return TotalSum;
        }
    }
}
