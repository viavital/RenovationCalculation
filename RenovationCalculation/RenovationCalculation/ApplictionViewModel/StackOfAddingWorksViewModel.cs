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
using RenovationCalculation.Service;

namespace RenovationCalculation.ApplictionViewModel
{
    class StackOfAddingWorksViewModel : INotifyPropertyChanged
    {
        
        private readonly WindowNavService _windowNavService;
        private readonly UploadingDataBaseService _uploadingDataBaseService;

        
        public ObservableCollection<TypeOfWorkModel> TypeOfWorks { get; set; } = new ObservableCollection<TypeOfWorkModel>();

        //v: get set на приватних полях не пишемо
        private ObservableCollection<WorkerModel> ListOfWorkers = new ObservableCollection<WorkerModel>();
        public ObservableCollection<WorkerModel> listOfWorkers
        {
            get { return ListOfWorkers; }
            set
            {
                ListOfWorkers = value;
                OnPropertyChanged();
            }
        }
        public StackOfAddingWorksViewModel()
        {            
            _windowNavService = new();            
            _uploadingDataBaseService = new();
            _uploadingDataBaseService.RefreshedDatbaseEvent += RefreshedDataBaseEventHandler;
            _uploadingDataBaseService.RefreshDataBase();  // викликаємо 1-й раз для початкової загрузки бази         
        
            //v: такого підходу з RefreshingDataBaseModel треба позбутись. Ти її створюєш викликаєш метод в який передаєш свої лісти і потім викидаєш цю рефрешінг модел.
            // в тебе має бути джерело даних - модель, чи сторедж, де буде завжди актуальна інформація. Якщо хтось в неї щось дописав, твої лісти тут мають слухати ці хміни і автоматично актуалізуватись.
            // щоб не виникало ситуації коли в базі одні данні а на екрані інші
            // ось наприклад як ти почав робити TypeOfWorkModel яка є INotifyPropertyChanged і може сповіщати слухачів про зміни полів, так само і тут перероби.

            ListOfWorkers.Insert(0, _addWorkerMenuSelection);
        }
        private void RefreshedDataBaseEventHandler()
        {
            TypeOfWorks = _uploadingDataBaseService.typeOfWorks;
            listOfWorkers = _uploadingDataBaseService.listOfWorkers;
        }
       
        //v: видалив public event Action AddNewWorkerEvent; з цієї вьюмодельки нам немає кому кидати екшини
        private readonly WorkerModel _addWorkerMenuSelection = new WorkerModel() { Name = "Add..." };

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
                if (value == _addWorkerMenuSelection.Name) // opening window of adding new worker
                {
                    //v: ось таке рішення коли ти в сетері змінної викликаєш якусь додаткову логіку не дуже гарне.
                    // Це через те що в тебе один з елементів списку поводить себе не так як всі інші.
                    // Поки лишив так, можливо підгуглиш краще рішення, як роблять люди в таких випадках, якщо не знайдеш - лишай так.
                    _windowNavService.CreateAddWorkerWindow();
                }
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
                OnPropertyChanged();
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
                    //v: тут можна параметр не використовувати взагалі, так як ти знаходишся в цій вью моделі де потрібні якісь зміни.
                    (addWorkCommand = new RelayCommand(_ =>
                    {
                        TypeOfWorkModel CreatingWork = new();

                        CreatingWork.typeOfWorkName = EnteredNewWork;
                        CreatingWork.quantityHoursOfWork = EnteredQuantityOfWork;
                        CreatingWork.totalPriceOfWork = EnteredCostOfWork;

                        //v: хотілось би якось цю логіку винести щоб тут походу в ДБ не було. Я думаю коли ти переробиш роботу з модельками то ти до цього дійдеш.
                        int IdOfCreatingWork;
                        using (WorksDBContext dbContext = new())
                        {
                            dbContext.Works.Add(CreatingWork);
                            dbContext.SaveChanges();
                            IdOfCreatingWork = CreatingWork.ID;
                            List<WorkerModel> workers = dbContext.Workers.ToList<WorkerModel>();
                            WorkerModel workerUnderEdition = workers.Where(w => w.Name == SelectedWorker).FirstOrDefault();
                            workerUnderEdition.WorkId = IdOfCreatingWork;
                            dbContext.Update(workerUnderEdition);
                            dbContext.SaveChanges();
                        }
                        //v: те саме по цій рефрешінг модел, тут її викосиш.
                        
                        enteredNewWork = null;
                        enteredQuantityOfWork = 0;
                        enteredCostOfWork = 0;
                        listOfWorkers.Insert(0, _addWorkerMenuSelection);
                    }));
            }
        }
    }
}

