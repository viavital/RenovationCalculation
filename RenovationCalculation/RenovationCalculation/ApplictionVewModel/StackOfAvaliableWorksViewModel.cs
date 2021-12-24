using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenovationCalculation.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Data.Entity;

namespace RenovationCalculation.ApplictionVewModel
{
    class StackOfAvaliableWorksViewModel : INotifyPropertyChanged
    {
        private TypeOfWorkModel SelectedTypeOfWork;
        public TypeOfWorkModel SelectedWork
        {
            get { return SelectedTypeOfWork; }
            set
            {
                SelectedTypeOfWork = value;
                OnPropertyChanged("SelectedTypeOfWork");
            }
        }

        private WorkerModel SelectedWorker;
        public WorkerModel selectedWorker
        {
            get { return SelectedWorker; }
            set
            {
                SelectedWorker = value;
                OnPropertyChanged("SelectedWorker");
            }
        }

        public List<TypeOfWorkModel> TypeOfWorks { get; set; }
        public List<WorkerModel> Workers { get; set; }

        public StackOfAvaliableWorksViewModel()
        {
            using (WorksDBContext db = new WorksDBContext())
            {
                Workers = db.Workers.ToList();
                TypeOfWorks = db.Works.ToList();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}

