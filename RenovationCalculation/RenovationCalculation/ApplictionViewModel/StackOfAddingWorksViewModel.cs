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
using System.Collections.Specialized;

namespace RenovationCalculation.ApplictionViewModel
{
    class StackOfAddingWorksViewModel : INotifyPropertyChanged
    {
        //v: викосив IsEnabledMainWindow - виглядало як костиль, без нього зараз наче норм працює

        private readonly WindowNavService _windowNavService;
        private readonly SomeModel _someModel;

        //v: прибрав звідси set
        public ObservableCollection<TypeOfWorkModel> TypeOfWorks { get; } = new ObservableCollection<TypeOfWorkModel>();

        //v: get set на приватних полях не пишемо
        private ObservableCollection<WorkerModel> Workers = new ObservableCollection<WorkerModel>();
        public ObservableCollection<WorkerModel> workers
        {
            get { return Workers; }
            set
            {
                Workers = value;
                OnPropertyChanged();
            }
        }
        public StackOfAddingWorksViewModel()
        {
            _windowNavService = new();
            //v: такого підходу з RefreshingDataBaseModel треба позбутись. Ти її створюєш викликаєш метод в який передаєш свої лісти і потім викидаєш цю рефрешінг модел.
            // в тебе має бути джерело даних - модель, чи сторедж, де буде завжди актуальна інформація. Якщо хтось в неї щось дописав, твої лісти тут мають слухати ці хміни і автоматично актуалізуватись.
            // щоб не виникало ситуації коли в базі одні данні а на екрані інші
            // ось наприклад як ти почав робити TypeOfWorkModel яка є INotifyPropertyChanged і може сповіщати слухачів про зміни полів, так само і тут перероби.
            RefreshingDataBaseModel refreshingDataBaseModel = new();
            refreshingDataBaseModel.RefreshDataBase(workers, TypeOfWorks);
            workers.Insert(0, _addWorkerMenuSelection);
            _someModel = new SomeModel();
            _someModel.OnElementAdded += OnElementAdded;
        }

        private void OnElementAdded(string elem)
        {
            //todo here you can add element to collection....
            Console.WriteLine(elem);
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
                        //todo this line for test only:
                        _someModel.AddString("ATATA!!");

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
                        RefreshingDataBaseModel refreshingDataBaseModel = new RefreshingDataBaseModel();
                        refreshingDataBaseModel.RefreshDataBase(workers, TypeOfWorks);
                        enteredNewWork = null;
                        enteredQuantityOfWork = 0;
                        enteredCostOfWork = 0;
                        workers.Insert(0, _addWorkerMenuSelection);
                    }));
            }
        }
    }
}

