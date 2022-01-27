using RenovationCalculation.Model;
using RenovationCalculation.Service;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Collections.Specialized;
using System.Windows;

namespace RenovationCalculation.ApplictionViewModel
{
     class StackOfAddingWorksViewModel : INotifyPropertyChanged, IDisposable
    {
        private readonly WindowNavService _windowNavService;
        private readonly TypeOfWorksService _typeOfWorkService;
        private readonly WorkersService _workersService;
        private readonly TotalSumCounterService _totalSumCounter = new();
       

        private readonly WorkerModel _editWorkersSelection = new WorkerModel() { Name = "Add / Remove ..." };
        public ObservableCollection<TypeOfWorkModel> TypeOfWorks { get; }
        public ObservableCollection<WorkerModel> ListOfWorkers { get; }
              
        private int totalSumOfRenovation;
        public int TotalSumOfRenovation
        {
            get { return totalSumOfRenovation; }
            set 
            {
                totalSumOfRenovation = value;
                OnPropertyChanged();
            }
        }
        private int sumOfInventory;
        public int SumOfInventory
        {
            get { return sumOfInventory; }
            set
            {
                sumOfInventory = value;
                OnPropertyChanged();
            }
        }
        
        public StackOfAddingWorksViewModel()
        {
            _windowNavService = new();            

            _typeOfWorkService = TypeOfWorksService.GetInstance();
            TypeOfWorks = new ObservableCollection<TypeOfWorkModel>(_typeOfWorkService.GetAllWorks());
            SumOfInventory = _totalSumCounter.CountSumOfInventory();
            _typeOfWorkService.WorkAddedEvent += OnTypeOfWorkAdded;
            _typeOfWorkService.WorkUpdatedEvent += OnTypeOfWorkUpdated;
            _typeOfWorkService.WorkDeletedEvent += OnTypeOfWorkDeleted;
            TypeOfWorks.CollectionChanged += OnTypeOfWorksChanged;
            _windowNavService.ClosingInventoryWindow += OnClosingInventoryWindow;

            TotalSumOfRenovation = _totalSumCounter.CountTotalSum(TypeOfWorks); 
            _workersService = WorkersService.GetInstance();
            ListOfWorkers = new ObservableCollection<WorkerModel>(_workersService.GetAllWorkers());
            ListOfWorkers.Insert(0, _editWorkersSelection);
            _workersService.WorkerAddedEvent += OnWorkerAdded;
            _workersService.WorkerDeletedEvent += OnWorkerDeleted;    
        }

        private void OnClosingInventoryWindow()
        {
            SumOfInventory = _totalSumCounter.CountSumOfInventory();
            TotalSumOfRenovation = _totalSumCounter.CountTotalSum(TypeOfWorks);
        }

        private void OnTypeOfWorkAdded(TypeOfWorkModel work)
        {
           TypeOfWorks.Add(work);           
        }
        private void OnTypeOfWorkUpdated(TypeOfWorkModel work)
        {
           int IndexOfUpdatedWork = TypeOfWorks.IndexOf(work);
            //TypeOfWorkModel FindingWork = TypeOfWorks.FirstOrDefault(u => u.ID == work.ID); // при такому методі не викликається CollectionChanged
            TypeOfWorks[IndexOfUpdatedWork] = work;
            SelectedWork = default;      
            WorkerOnSelectedWork = null;
        }
        private void OnTypeOfWorkDeleted(TypeOfWorkModel work)
        {            
            TypeOfWorks.Remove(work);            
            WorkerOnSelectedWork = null;
        }
        private void OnTypeOfWorksChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            TotalSumOfRenovation = _totalSumCounter.CountTotalSum(TypeOfWorks);
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
        
        private TypeOfWorkModel selectedWork;
        public TypeOfWorkModel SelectedWork
        {
            get { return selectedWork; }
            set
            {
                selectedWork = value;
                ChangingSelectionOfWork();
                OnPropertyChanged();
            }
        }

        private string enteredQuantityOfWork;
        public string EnteredQuantityOfWork
        {
            get { return enteredQuantityOfWork; }
            set 
            { 
                enteredQuantityOfWork = value;
                OnPropertyChanged();
                OnEnteringQuantityOfWork();
            }
        }
        private int quantityOfWork;
        public int QuantityOfWork
        {
            get { return quantityOfWork; }
            set
            {
                quantityOfWork = value;
                OnPropertyChanged();
            }
        }
        private bool isEditRemoveBtnEnabled;
        public bool IsEditRemoveBtnEnabled
        {
            get { return isEditRemoveBtnEnabled; }
            set
            {
                isEditRemoveBtnEnabled = value;
                OnPropertyChanged();
            }
        }
        private void OnEnteringQuantityOfWork()
        {
            if (EnteredQuantityOfWork != null && EnteredQuantityOfWork.Length > 0)
            {
                int ParsedQuantityOfWork;
                if (int.TryParse(EnteredQuantityOfWork, out ParsedQuantityOfWork))
                {
                    this.QuantityOfWork = ParsedQuantityOfWork;
                }
                else
                {
                    EnteredQuantityOfWork = EnteredQuantityOfWork.Remove(EnteredQuantityOfWork.Length - 1);
                }
            }
        }
       
        private string enteredCostOfMaterials;
        public string EnteredCostOfMaterials
        {
            get { return enteredCostOfMaterials; }
            set
            {
                enteredCostOfMaterials = value;
                OnPropertyChanged();
                OnEnteredCostOfMaterials();
            }
        }
        private int сostOfMaterials;
        public int CostOfMaterials
        {
            get { return сostOfMaterials; }
            set
            {
                сostOfMaterials = value;
                OnPropertyChanged();
            }
        }
        private void OnEnteredCostOfMaterials()
        {
            if (EnteredCostOfMaterials != null && EnteredCostOfMaterials.Length > 0)
            {
                int ParsedCostOfMaterials;
                if (int.TryParse(EnteredCostOfMaterials, out ParsedCostOfMaterials))
                {
                    CostOfMaterials = ParsedCostOfMaterials;
                }
                else
                {
                    EnteredCostOfMaterials = EnteredCostOfMaterials.Remove(EnteredCostOfMaterials.Length - 1);
                }
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
        private void ChangingSelectionOfWork()
        {
            if (SelectedWork != null)
            {                
                IsEditRemoveBtnEnabled = true;
                WorkerOnSelectedWork = ListOfWorkers.FirstOrDefault(u => u.ID == SelectedWork.WorkerID);
            }
           else
            {
                IsEditRemoveBtnEnabled = false;                
            }        
            if (WorkerOnSelectedWork == null)
            {
                WorkerOnSelectedWork = new WorkerModel() { Name = "Can't find info", PricePerHour = 0 }; // потрібно ще придумати щоб навіть якщо ми видалили імя робочого, інфа про тариф збереглась
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
                    (addWorkCommand = new RelayCommand(_ =>
                    {
                        TypeOfWorkModel CreatingWork = new();
                        if (enteredNewWork != null)
                        {
                            CreatingWork.typeOfWorkName = enteredNewWork.Trim();
                            CreatingWork.quantityHoursOfWork = QuantityOfWork;
                            CreatingWork.CostOfMaterials = CostOfMaterials;
                            if (CreatingWork.typeOfWorkName != "" && CreatingWork.quantityHoursOfWork > 0 && SelectedWorker != null && CreatingWork.CostOfMaterials >= 0)
                            {
                                CreatingWork.WorkerID = SelectedWorker.ID;
                                CreatingWork.TotalCostOfWork = SelectedWorker.PricePerHour * QuantityOfWork + CostOfMaterials;
                                _typeOfWorkService.AddWork(CreatingWork);
                            }
                            else
                            {
                                MessageBox.Show("Inputed wrong data");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Input name of work");
                        }
                        EnteredNewWork = null;
                        EnteredQuantityOfWork = null;
                        EnteredCostOfMaterials = null;
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

        private RelayCommand removeWorkCommand;
        public RelayCommand RemoveWorkCommand
        {
            get
            {
                return removeWorkCommand ??
                    (removeWorkCommand = new RelayCommand(obj =>
                    {
                        var workToRemove = obj as TypeOfWorkModel;
                        if (workToRemove != null)
                        {
                            _typeOfWorkService.DeleteWork(workToRemove);
                        }
                    }));
            }
        }

        private RelayCommand showInventoryWindow;
        public RelayCommand ShowInventoryWindow
        {
            get
            {
                return showInventoryWindow ??
                    (showInventoryWindow = new RelayCommand(_ =>
                    {
                        _windowNavService.AddInventoryWindow();
                    }));
            }
        }
        public void Dispose()
        {
            _typeOfWorkService.WorkAddedEvent -= OnTypeOfWorkAdded;
            _workersService.WorkerAddedEvent -= OnWorkerAdded;
            _typeOfWorkService.WorkUpdatedEvent -= OnTypeOfWorkUpdated;
            _typeOfWorkService.WorkDeletedEvent -= OnTypeOfWorkDeleted;
            _workersService.WorkerDeletedEvent -= OnWorkerDeleted;
            _windowNavService.ClosingInventoryWindow -= OnClosingInventoryWindow;
        }
    }
}

