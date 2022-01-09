using RenovationCalculation.ApplictionViewModel;
using RenovationCalculation.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenovationCalculation.Service
{
    //v: зробив ось такий класик для навігації між вікнами, щоб не писати все в кучі вьюмодел то зробив його окремо.
    // виходить що вьюмодел просто каже - відкрий Адд Воркер, а тут створюємо вьюшку, чіпляєм вьюмодел і підписуємся на закриття вікна.
    // Поідеї більше логіки сюди пхати не треба. Для ще одного вікна можна зробити аналогічний метод
    // Цей підхід не ідеальний, але найбільш простий і зрозумілий, як на мене. І не сильно ломає задумку МВВМ.
    // ось тут більше про це https://docs.microsoft.com/en-us/answers/questions/21808/opening-a-new-window-on-a-button-click-mvvm-wpf.html та https://stackoverflow.com/a/16173553
    class WindowNavService
    {
        public void CreateAddWorkerWindow()
        {
            var addWorkerVM = new AddingWorkerViewModel();
            var addWorkerWindow = new Adding_a_new_worker
            {
                DataContext = addWorkerVM
            };
            addWorkerVM.CloseAddWorkerWindowEvent += () => addWorkerWindow.Close();
            addWorkerWindow.Show();
        }
    }
}
