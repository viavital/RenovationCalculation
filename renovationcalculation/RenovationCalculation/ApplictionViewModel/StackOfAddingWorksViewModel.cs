﻿using RenovationCalculation.Model;
using RenovationCalculation.Service;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;

namespace RenovationCalculation.ApplictionViewModel
{
    partial class StackOfAddingWorksViewModel : INotifyPropertyChanged, IDisposable
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

        public StackOfAddingWorksViewModel()
        {
            _windowNavService = new();            

            _typeOfWorkService = TypeOfWorksService.GetInstance();
            TypeOfWorks = new ObservableCollection<TypeOfWorkModel>(_typeOfWorkService.GetAllWorks());           
            _typeOfWorkService.WorkAddedEvent += OnTypeOfWorkAdded;
            _typeOfWorkService.WorkUpdatedEvent += OnTypeOfWorkUpdated;
            _typeOfWorkService.WorkDeletedEvent += OnTypeOfWorkDeleted;
            _totalSumCounter.TotalSumCountedEvent += OnSumCounted;  // повертає суму і присвоює її в вікно Тотал сум


            _totalSumCounter.CountTotalSum(TypeOfWorks); 
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
            _totalSumCounter.CountTotalSum(TypeOfWorks);
        }
        private void OnTypeOfWorkUpdated(TypeOfWorkModel work)
        {
            TypeOfWorkModel FindingWork = TypeOfWorks.FirstOrDefault(u => u.ID == work.ID);
            FindingWork = work;
            SelectedWork = default;
            _totalSumCounter.CountTotalSum(TypeOfWorks);
        }
        private void OnTypeOfWorkDeleted(TypeOfWorkModel work)
        {            
            TypeOfWorks.Remove(work);
            _totalSumCounter.CountTotalSum(TypeOfWorks);
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
        private void OnSumCounted(int Sum)
        {
            TotalSumOfRenovation = Sum;
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
            if (SelectedWork != null)
            {
                IsRemoveBtnEnabled = true;
                IsEditBtnEnabled = true;
                WorkerOnSelectedWork = ListOfWorkers.FirstOrDefault(u => u.ID == SelectedWork.WorkerID);
            }
           else
            {
                IsEditBtnEnabled = false;
                IsRemoveBtnEnabled = false;
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
        public void Dispose()
        {
            _typeOfWorkService.WorkAddedEvent -= OnTypeOfWorkAdded;
            _workersService.WorkerAddedEvent -= OnWorkerAdded;
            _typeOfWorkService.WorkUpdatedEvent -= OnTypeOfWorkUpdated;
            _typeOfWorkService.WorkDeletedEvent -= OnTypeOfWorkDeleted;
            _workersService.WorkerDeletedEvent -= OnWorkerDeleted;
            _totalSumCounter.TotalSumCountedEvent -= OnSumCounted;  
            NeedToCountTotalSumEvent -= OnNeededCountTotalSum;
        }
    }
}

