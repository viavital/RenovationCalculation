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
    class UploadingDataBaseService : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

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

        public void AddWorks(TypeOfWorkModel typeOfWorkModel)
        {
            using (WorksDBContext db = new WorksDBContext())
            {
                db.Works.Add(typeOfWorkModel);
                db.SaveChanges();                
            }
            typeOfWorks.Add(typeOfWorkModel);
            OnPropertyChanged("");
        }
        public void AddWorker(WorkerModel workerModel)
        {
            using (WorksDBContext db = new WorksDBContext())
            {
                db.Workers.Add(workerModel);
                db.SaveChanges();
            }
            listOfWorkers.Add(workerModel);
            OnPropertyChanged("");
        }
        public void DeleteWorker(string NameWorkerToRemove)
        {
            WorkerModel WorkerToRemove = listOfWorkers.FirstOrDefault(w => w.Name == NameWorkerToRemove);
            using (WorksDBContext db = new WorksDBContext())
            {                
                db.Workers.Remove(WorkerToRemove);
                db.SaveChanges();
            }
            listOfWorkers.Remove(WorkerToRemove);
            OnPropertyChanged("");
        }

        //v: ось такий підхід не дуже явний, коли ти кидєш свої лісти в якусь штуку і вона докорінно їх змінює.
        //ти маєш працювати з одним джерелом істини. Якщо хтось добавляється в списки роботяг, то моделька має уже бути з цим роботягою без потреби всім кому цікавить робити Refresh
        public void UploadDataBase()
        {
            using (WorksDBContext db = new WorksDBContext())
            {
                foreach (var item in (db.Workers.ToList())) // addrange is not avaliable
                {
                    listOfWorkers.Add(item);
                    OnPropertyChanged("typeOfWorks");
                }
                foreach (var item in (db.Works.ToList()))
                {
                    typeOfWorks.Add(item);
                    OnPropertyChanged("listOfWorkers");
                }
            }
        }
    }
}
