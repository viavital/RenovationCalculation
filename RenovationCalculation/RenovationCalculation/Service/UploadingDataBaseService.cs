using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RenovationCalculation.Model
{
    class UploadingDataBaseService
    {
        public event Action RefreshedDatbaseEvent;

        private ObservableCollection<TypeOfWorkModel> TypeOfWorks = new();
        public ObservableCollection<TypeOfWorkModel> typeOfWorks
        {
            get { return TypeOfWorks; }
            set
            {
                TypeOfWorks = value;
            }
        }

        private ObservableCollection<WorkerModel> ListOfWorkers = new();
        public ObservableCollection<WorkerModel> listOfWorkers
        {
            get { return ListOfWorkers; }
            set
            {
                ListOfWorkers = value;
            }
        }

        //v: ось такий підхід не дуже явний, коли ти кидєш свої лісти в якусь штуку і вона докорінно їх змінює.
        //ти маєш працювати з одним джерелом істини. Якщо хтось добавляється в списки роботяг, то моделька має уже бути з цим роботягою без потреби всім кому цікавить робити Refresh
        public void RefreshDataBase()
        {
            using (WorksDBContext db = new WorksDBContext())
            {
                foreach (var item in (db.Workers.ToList())) // addrenge is not avaliable
                {
                    listOfWorkers.Add(item);
                }
                foreach (var item in (db.Works.ToList()))
                {
                    typeOfWorks.Add(item);
                }
                RefreshedDatbaseEvent();
            }
        }
    }
}
