using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public AddingWorkerViewModel()
        {
            RefreshingDataBaseModel refreshingDataBaseModel = new();
            refreshingDataBaseModel.RefreshDataBase(workersInAddingWorkerVM);
        }
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
        private ObservableCollection<WorkerModel> WorkersInAddingWorkerVM { get; set; } = new ObservableCollection<WorkerModel>();
        public ObservableCollection<WorkerModel> workersInAddingWorkerVM
        {
            get { return WorkersInAddingWorkerVM; }
            set
            {
                WorkersInAddingWorkerVM = value;
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
                        RefreshingDataBaseModel refreshingDataBaseModel = new();
                        refreshingDataBaseModel.RefreshDataBase(workersInAddingWorkerVM);
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
