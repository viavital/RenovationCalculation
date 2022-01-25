using RenovationCalculation.Model;
using RenovationCalculation.Service;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RenovationCalculation.ApplictionViewModel
{
    class AddingWorkerViewModel : INotifyPropertyChanged, IDisposable
    {
        private readonly WorkersService _workersService;

        public ObservableCollection<WorkerModel> Workers { get; }

        public AddingWorkerViewModel()
        {
            _workersService = WorkersService.GetInstance();
            Workers = new ObservableCollection<WorkerModel>(_workersService.GetAllWorkers());
            _workersService.WorkerAddedEvent += OnWorkerAdded;
            _workersService.WorkerDeletedEvent += OnWorkerDeleted;            
        }
        private void OnWorkerAdded(WorkerModel worker)
        {
            Workers.Add(worker);
        }

        private void OnWorkerDeleted(WorkerModel worker)
        {
            if (Workers.Contains(worker))
            {
                Workers.Remove(worker);
            }
        }
        private void OnEnteredPricePerHour()
        {
            if (EnteredPricePerHour != null && EnteredPricePerHour.Length > 0)
            {
                int ParsedPricePerHour;
                if (int.TryParse(EnteredPricePerHour, out ParsedPricePerHour))
                {
                    PricePerHour = ParsedPricePerHour;
                }
                else
                {
                    EnteredPricePerHour = EnteredPricePerHour.Remove(EnteredPricePerHour.Length - 1);
                }
            }
        }

        private string enteredNameOfNewWorker;
        public string EnteredNameOfNewWorker
        {
            get { return enteredNameOfNewWorker; }
            set
            {
                enteredNameOfNewWorker = value;
                OnPropertyChanged();
            }
        }

       
        private string enteredpricePerHour;
        public string EnteredPricePerHour
        {
            get { return enteredpricePerHour; }
            set
            {
                enteredpricePerHour = value;
                OnPropertyChanged();
                OnEnteredPricePerHour();
            }
        }

        private int pricePerHour;
        public int PricePerHour
        {
            get { return pricePerHour; }
            set
            {
                pricePerHour = value;
              //  OnPropertyChanged();
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
            }
        }

        private RelayCommand addWorkerCommand;
        public RelayCommand AddWorkerCommand
        {
            get
            {
                return addWorkerCommand ??
                    (addWorkerCommand = new RelayCommand(_ =>
                    {
                        WorkerModel CreatingWorker = new();

                        CreatingWorker.Name = enteredNameOfNewWorker;
                        CreatingWorker.PricePerHour = pricePerHour;
                        _workersService.AddWorker(CreatingWorker);

                        EnteredNameOfNewWorker = null;
                        EnteredPricePerHour = null;
                    }));
            }
        }

        private RelayCommand removeWorkerCommand;
        public RelayCommand RemoveWorkerCommand
        {
            get
            {
                return removeWorkerCommand ??
                    (removeWorkerCommand = new RelayCommand(obj =>
                    {
                        var workerToRemove = obj as WorkerModel;
                        if (workerToRemove != null)
                        {
                            _workersService.DeleteWorker(workerToRemove);
                        }
                    }
                    ));
            }
        }

        public event Action CloseAddWorkerWindowEvent;
        private RelayCommand closeWindowCommand;
        public RelayCommand CloseWindowCommand
        {
            get
            {
                return closeWindowCommand ?? (closeWindowCommand = new RelayCommand(_ => CloseAddWorkerWindowEvent()));
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
            _workersService.WorkerAddedEvent -= OnWorkerAdded;
            _workersService.WorkerDeletedEvent -= OnWorkerDeleted;
        }
    }
}
