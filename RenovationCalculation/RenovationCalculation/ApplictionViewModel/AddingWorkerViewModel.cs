using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using RenovationCalculation.Model;

namespace RenovationCalculation.ApplictionViewModel
{
    class AddingWorkerViewModel : INotifyPropertyChanged
    {
        public event Action ToUpdateDataOfWorkers;
        private string EnteredNameOfNewWorker;
        public string enteredNameOfNewWorker
        {
            get { return EnteredNameOfNewWorker; }
            set
            {
                EnteredNameOfNewWorker = value;
                OnPropertyChanged();
            }
        }      

        private RelayCommand AddWorkerCommand;
        public RelayCommand addWorkerCommand
        {
            get
            {
                return AddWorkerCommand ??
                    (AddWorkerCommand = new RelayCommand(obj =>
                    {
                        WorkerModel CreatingWorker = new();

                        var vmAddNewWorker = obj as AddingWorkerViewModel;
                        var nameOfWorker = vmAddNewWorker.EnteredNameOfNewWorker;

                        CreatingWorker.Name = nameOfWorker;

                        using (WorksDBContext dbContext = new())
                        {
                            dbContext.Workers.Add(CreatingWorker);
                            dbContext.SaveChanges();                            
                        }
                        ToUpdateDataOfWorkers();
                      enteredNameOfNewWorker = null;
                    }));
            }
        }

        private RelayCommand RemoveWorkerCommand;
        public RelayCommand removeWorkerCommand
        {
            get
            {
                return RemoveWorkerCommand ??
                    (RemoveWorkerCommand = new RelayCommand(obj =>
                    {
                        WorkerModel workerToRemove = obj as WorkerModel;
                        if (workerToRemove != null)
                        {
                            using (WorksDBContext dbContext = new())
                            {
                                dbContext.Workers.Remove(workerToRemove);
                                dbContext.SaveChanges();
                            }
                        }
                    }
                    ));
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
