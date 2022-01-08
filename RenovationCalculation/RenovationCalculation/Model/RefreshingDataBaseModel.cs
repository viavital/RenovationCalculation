using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenovationCalculation.Model
{
    class RefreshingDataBaseModel
    {
        public void RefreshDataBase(ObservableCollection<WorkerModel> ListOfWorkers, ObservableCollection<TypeOfWorkModel> ListOfworks)
        {
            using (WorksDBContext db = new WorksDBContext())
            {
                ListOfWorkers.Clear();
                ListOfworks.Clear();
                
                foreach (var item in (db.Workers.ToList())) // addrenge is not avaliable
                {
                    ListOfWorkers.Add(item);
                }
                foreach (var item in (db.Works.ToList()))
                {
                    ListOfworks.Add(item);
                }                
            }
        }
        public void RefreshDataBase(ObservableCollection<WorkerModel> ListOfWorkers)
        {
            using (WorksDBContext db = new WorksDBContext())
            {
                ListOfWorkers.Clear();
                foreach (var item in (db.Workers.ToList())) // addrenge is not avaliable
                {
                    ListOfWorkers.Add(item);
                }
            }
        }
    }
}
