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
using System.Windows.Input;

namespace RenovationCalculation.ApplictionVewModel
{
    class StackOfAddingWorksViewModel : INotifyPropertyChanged
    {
        public List<TypeOfWorkModel> TypeOfWorks { get; set; }
        public List<WorkerModel> Workers { get; set; }

        private TypeOfWorkModel EnteredTypeOfWork;
        public StackOfAddingWorksViewModel()
        {
            using (WorksDBContext db = new WorksDBContext())
            {
                Workers = db.Workers.ToList();
                TypeOfWorks = db.Works.ToList();
            }
        }

        private string EnteredNewWork;
        public string enteredNewWork
        {
            get { return EnteredNewWork; }
            set
            {
                EnteredNewWork = value;
                OnPropertyChanged("enteredNewWork");
            }
        }
        private WorkerModel SelectedWorker;
        public WorkerModel selectedWorker
        {
            get { return SelectedWorker; }
            set
            {
                SelectedWorker = value;
                OnPropertyChanged("selectedWorker");
            }
        }
        private int EnteredQuantityOfWork;
        public int enteredQuantityOfWork
        {
            get { return EnteredQuantityOfWork; }
            set
            {
                EnteredQuantityOfWork = value;
                OnPropertyChanged("enteredQuantityOfWork");
            }
        }
        private int EnteresCostOfWork;
        public int enteresCostOfWork
        {
            get { return EnteresCostOfWork; }
            set
            {
                EnteresCostOfWork = value;
                OnPropertyChanged("enteresCostOfWork");
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

