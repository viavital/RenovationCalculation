using RenovationCalculation.ApplictionViewModel;
using RenovationCalculation.View;
using System;

namespace RenovationCalculation.Service
{
    //v: зробив ось такий класик для навігації між вікнами, щоб не писати все в кучі вьюмодел то зробив його окремо.
    // виходить що вьюмодел просто каже - відкрий Адд Воркер, а тут створюємо вьюшку, чіпляєм вьюмодел і підписуємся на закриття вікна.
    // Поідеї більше логіки сюди пхати не треба. Для ще одного вікна можна зробити аналогічний метод
    // Цей підхід не ідеальний, але найбільш простий і зрозумілий, як на мене. І не сильно ломає задумку МВВМ.
    // ось тут більше про це https://docs.microsoft.com/en-us/answers/questions/21808/opening-a-new-window-on-a-button-click-mvvm-wpf.html та https://stackoverflow.com/a/16173553
    class WindowNavService
    {
        public event Action ClosingInventoryWindow;
        public void CreateAddWorkerWindow()
        {
            var addWorkerVM = new AddingWorkerViewModel();
            var addWorkerWindow = new AddingNewWorker
            {
                DataContext = addWorkerVM
            };

            addWorkerVM.CloseAddWorkerWindowEvent += () => addWorkerWindow.Close();
            addWorkerWindow.Closing += (s, e) => addWorkerVM.Dispose(); //added to clear all neded data inside view model (subscribtions) when window will be closed.
            addWorkerWindow.Show();
        }

        public void AddInventoryWindow()
        {
            var addInventoryVM = new AddingInventoryViewModel();
            var addInventoryWindow = new AddingInventoryWindow
            {
                DataContext = addInventoryVM
            };

            addInventoryVM.CloseAddInventoryWindowEvent += () =>  addInventoryWindow.Close();
            addInventoryWindow.Closing += (s, e) => { addInventoryVM.Dispose(); ClosingInventoryWindow(); }; //added to clear all neded data inside view model (subscribtions) when window will be closed.
            addInventoryWindow.Show();
        }
    }
}
