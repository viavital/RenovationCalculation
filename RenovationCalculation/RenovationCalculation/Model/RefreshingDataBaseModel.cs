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
        //v: ось такий підхід не дуже явний, коли ти кидєш свої лісти в якусь штуку і вона докорінно їх змінює.
        //ти маєш працювати з одним джерелом істини. Якщо хтось добавляється в списки роботяг, то моделька має уже бути з цим роботягою без потреби всім кому цікавить робити Refresh
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
