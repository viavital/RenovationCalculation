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
        private string SelectedWorker;
        public string selectedWorker
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
        private int EnteredCostOfWork;
        public int enteresCostOfWork
        {
            get { return EnteredCostOfWork; }
            set
            {
                EnteredCostOfWork = value;
                OnPropertyChanged("enteresCostOfWork");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public void CreateNewWork()
        {
            TypeOfWorkModel CreatingWork = new();
            CreatingWork.TypeOfWorkName = enteredNewWork;
            CreatingWork.QuantityHoursOfWork = enteredQuantityOfWork;
            CreatingWork.TotalPriceOfWork = enteresCostOfWork;

            int IdOfCreatingWork;
            using (WorksDBContext dbContext = new ())
            {
                dbContext.Works.Add(CreatingWork);
                dbContext.SaveChanges();
                IdOfCreatingWork = CreatingWork.ID;

            }

        }
    }
}

