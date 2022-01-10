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
        private readonly UploadingDataBaseService uploadingDataBaseService = new();
        public AddingWorkerViewModel()
        {
            uploadingDataBaseService.PropertyChanged += UploadingDataBaseService_PropertyChanged;
            uploadingDataBaseService.UploadDataBase();
        }

        private void UploadingDataBaseService_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            uploadingDataBaseService.listOfWorkers = workersInAddingWorkerVM;
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
        private string SelectedWorker;
        public string selectedWorker
        {
            get { return SelectedWorker; }
            set
            {
                SelectedWorker = value;
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

                        uploadingDataBaseService.AddWorker(CreatingWorker);      
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
                        string workerToRemove = obj as string;
                        if (workerToRemove != null)
                        {
                            uploadingDataBaseService.DeleteWorker(workerToRemove);
                        }
                    }
                    ));
            }
        }
        public event Action CloseAddWorkerWindowEvent;
        private RelayCommand CloseWindowCommand;
        public RelayCommand closeWindowCommand
        {
            get
            {
                return CloseWindowCommand ??
                    (CloseWindowCommand = new RelayCommand(obj =>
                    {
                        CloseAddWorkerWindowEvent();
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
