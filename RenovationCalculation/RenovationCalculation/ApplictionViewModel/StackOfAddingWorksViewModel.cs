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
using RenovationCalculation.View;

namespace RenovationCalculation.ApplictionViewModel
{
    class StackOfAddingWorksViewModel : INotifyPropertyChanged
    {
        private bool IsEnabledMainWindow { get; set; } = true;
        public bool isEnabledMainWindow
        {
            get { return IsEnabledMainWindow; }
            set 
            {
                IsEnabledMainWindow = value;
                OnPropertyChanged();
            }
        }

        public List<TypeOfWorkModel> TypeOfWorks { get; set; }
        public List<TypeOfWorkModel> typeOfWorks
        {
            get { return TypeOfWorks; }
            set
            {
                TypeOfWorks = value;
                OnPropertyChanged();
            }
        }
        public List<WorkerModel> Workers { get; set; }

        private event Action AddNewWorkerEvent;
        WorkerModel AddWorkerMenuSelection = new WorkerModel() { Name = "Add..." };
        public void RefreshDataBase()
        {
            using (WorksDBContext db = new WorksDBContext())
            {
                Workers = db.Workers.ToList();
                Workers.Insert(0, AddWorkerMenuSelection);
                typeOfWorks = db.Works.ToList();
            }
        }
        public StackOfAddingWorksViewModel()
        {
            RefreshDataBase();
        }

        private string EnteredNewWork;
        public string enteredNewWork
        {
            get { return EnteredNewWork; }
            set
            {
                EnteredNewWork = value;
                OnPropertyChanged();
            }
        }
        private string SelectedWorker;
        public string selectedWorker
        {
            get { return SelectedWorker; }
            set
            {
                SelectedWorker = value;
                OnPropertyChanged();
                if (value == AddWorkerMenuSelection.Name) // opening window of adding new worker
                {
                    AddNewWorkerEvent += AddNewWorkerEventHandler;
                    AddNewWorkerEvent();
                    AddNewWorkerEvent -= AddNewWorkerEventHandler;
                }
            }
        }
        private void AddNewWorkerEventHandler()
        {
            {
                isEnabledMainWindow = false;
                Adding_a_new_worker adding_A_New_Worker = new();
                adding_A_New_Worker.Show();
            }
        }

        private int EnteredQuantityOfWork;
        public int enteredQuantityOfWork
        {
            get { return EnteredQuantityOfWork; }
            set
            {
                EnteredQuantityOfWork = value;
                OnPropertyChanged();
            }
        }
        private int EnteredCostOfWork;
        public int enteredCostOfWork
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

        private RelayCommand addWorkCommand;
        public RelayCommand AddWorkCommand
        {
            get
            {
                return addWorkCommand ??
                    (addWorkCommand = new RelayCommand(obj =>
                    {
                        TypeOfWorkModel CreatingWork = new();

                        var vm = obj as StackOfAddingWorksViewModel;
                        var nameOfWork = vm.EnteredNewWork;
                        var SelectedWorker_vm = vm.SelectedWorker;
                        var QuantityOfHours = vm.EnteredQuantityOfWork;
                        var CostOfWork = vm.EnteredCostOfWork;

                        CreatingWork.TypeOfWorkName = nameOfWork;
                        CreatingWork.QuantityHoursOfWork = QuantityOfHours;
                        CreatingWork.TotalPriceOfWork = CostOfWork;

                        int IdOfCreatingWork;
                        using (WorksDBContext dbContext = new())
                        {
                            dbContext.Works.Add(CreatingWork);
                            dbContext.SaveChanges();
                            IdOfCreatingWork = CreatingWork.ID;
                            List<WorkerModel> workers = dbContext.Workers.ToList<WorkerModel>();
                            WorkerModel workerUnderEdition = workers.Where(w => w.Name == SelectedWorker_vm).FirstOrDefault();
                            workerUnderEdition.WorkId = IdOfCreatingWork;
                            dbContext.Update(workerUnderEdition);
                            dbContext.SaveChanges();
                        }
                        RefreshDataBase();
                        //nameOfWork for example, other fields can be the same way.
                        //here you can save to DB or do another work.
                    }));
            }
        }
    }
}

