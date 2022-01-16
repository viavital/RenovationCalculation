using RenovationCalculation.Model;
using RenovationCalculation.Service;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;

namespace RenovationCalculation.ApplictionViewModel
{
    class StackOfAddingWorksViewModel : INotifyPropertyChanged, IDisposable
    {

        private readonly WindowNavService _windowNavService;
        private readonly TypeOfWorksService _typeOfWorkService;
        private readonly WorkersService _workersService;

        private readonly WorkerModel _editWorkersSelection = new WorkerModel() { Name = "Add / Remove ..." };

        public ObservableCollection<TypeOfWorkModel> TypeOfWorks { get; }
        public ObservableCollection<WorkerModel> ListOfWorkers { get; }

        public StackOfAddingWorksViewModel()
        {
            _windowNavService = new();

            _typeOfWorkService = TypeOfWorksService.GetInstance();
            TypeOfWorks = new ObservableCollection<TypeOfWorkModel>(_typeOfWorkService.GetAllWorks());
            _typeOfWorkService.WorkAddedEvent += OnTypeOfWorkAdded;
            _typeOfWorkService.WorkUpdatedEvent += OnTypeOfWorkUpdated;

            _workersService = WorkersService.GetInstance();
            ListOfWorkers = new ObservableCollection<WorkerModel>(_workersService.GetAllWorkers());
            ListOfWorkers.Insert(0, _editWorkersSelection);
            _workersService.WorkerAddedEvent += OnWorkerAdded;
            _workersService.WorkerDeletedEvent += OnWorkerDeleted;
            ChangingSelectionOfWorkEvent += ChangingSelectionOfWorkEventHandler;

        }    

        private void OnTypeOfWorkAdded(TypeOfWorkModel work)
        {
            TypeOfWorks.Add(work);
        }
        private void OnTypeOfWorkUpdated(TypeOfWorkModel work)
        {
            TypeOfWorkModel FindingWork = TypeOfWorks.FirstOrDefault(u => u.ID == work.ID);
            FindingWork = work;
        }

        private void OnWorkerAdded(WorkerModel worker)
        {
            ListOfWorkers.Add(worker);
        }

        private void OnWorkerDeleted(WorkerModel worker)
        {
            if (ListOfWorkers.Contains(worker))
            {
                ListOfWorkers.Remove(worker);
            }
        }

        private string enteredNewWork;
        public string EnteredNewWork
        {
            get { return enteredNewWork; }
            set
            {
                enteredNewWork = value;
                OnPropertyChanged();
            }
        }
        
        private WorkerModel selectedWorker;
        public WorkerModel SelectedWorker
        {
            get { return selectedWorker; }
            set
            {
                selectedWorker = value;
                OnPropertyChanged();
                if (value == _editWorkersSelection) // opening window of adding new worker
                {
                    //v: ось таке рішення коли ти в сетері змінної викликаєш якусь додаткову логіку не дуже гарне.
                    // Це через те що в тебе один з елементів списку поводить себе не так як всі інші.
                    // Поки лишив так, можливо підгуглиш краще рішення, як роблять люди в таких випадках, якщо не знайдеш - лишай так.
                    _windowNavService.CreateAddWorkerWindow();
                }
            }
        }

        private event Action ChangingSelectionOfWorkEvent;
        private TypeOfWorkModel selectedWork;
        public TypeOfWorkModel SelectedWork
        {
            get { return selectedWork; }
            set
            {
                selectedWork = value;
                ChangingSelectionOfWorkEvent();
                OnPropertyChanged();
            }
        }

        private int enteredQuantityOfWork;
        public int EnteredQuantityOfWork
        {
            get { return enteredQuantityOfWork; }
            set
            {
                enteredQuantityOfWork = value;
                OnPropertyChanged();
            }
        }
        private int enteredCostOfMaterials;
        public int EnteredCostOfMaterials
        {
            get { return enteredCostOfMaterials; }
            set
            {
                enteredCostOfMaterials = value;
                OnPropertyChanged();
            }
        }

        private WorkerModel workerOnSelectedWork;
        public WorkerModel WorkerOnSelectedWork
        {
            get { return workerOnSelectedWork; }
            set 
            { 
                workerOnSelectedWork = value;
                OnPropertyChanged();
            }
        }
        private void ChangingSelectionOfWorkEventHandler()
        {
            WorkerOnSelectedWork = ListOfWorkers.FirstOrDefault(u => u.ID == SelectedWork.WorkerID);
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
                    (addWorkCommand = new RelayCommand(_ =>
                    {
                        TypeOfWorkModel CreatingWork = new();
                        CreatingWork.typeOfWorkName = enteredNewWork;
                        CreatingWork.quantityHoursOfWork = enteredQuantityOfWork;
                        CreatingWork.CostOfMaterials = enteredCostOfMaterials;
                        CreatingWork.WorkerID = SelectedWorker.ID;
                        CreatingWork.TotalCostOfWork = SelectedWorker.PricePerHour * EnteredQuantityOfWork + SelectedWork.CostOfMaterials;

                        _typeOfWorkService.AddWork(CreatingWork);

                        EnteredNewWork = null;
                        EnteredQuantityOfWork = 0;
                        EnteredCostOfMaterials = 0;
                        SelectedWorker = null;
                    }));
            }
        }
        private RelayCommand editWorkCommand;
        public RelayCommand EditWorkCommand
        {
            get
            {
                return editWorkCommand ??
                    (editWorkCommand = new RelayCommand(_ =>
                    {
                        SelectedWork.TotalCostOfWork = WorkerOnSelectedWork.PricePerHour * SelectedWork.quantityHoursOfWork + selectedWork.CostOfMaterials;

                        _typeOfWorkService.UpdateWork(SelectedWork);
                        
                        SelectedWorker = null;
                    }));
            }
        }

        public void Dispose()
        {
            _typeOfWorkService.WorkAddedEvent -= OnTypeOfWorkAdded;
            _workersService.WorkerAddedEvent -= OnWorkerAdded;
            _workersService.WorkerDeletedEvent -= OnWorkerDeleted;
        }
    }
}

