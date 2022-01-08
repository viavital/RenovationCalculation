using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RenovationCalculation.Model
{
    public class TypeOfWorkModel : INotifyPropertyChanged
    {
        public int ID { get; set; }
        private string TypeOfWorkName;
        public string typeOfWorkName
        {
            get { return TypeOfWorkName; }
            set
            {
                TypeOfWorkName = value;
                OnPropertyChanged();
            }
        }
        private int QuantityHoursOfWork;
        public int quantityHoursOfWork
        {
            get { return QuantityHoursOfWork; }
            set
            {
                QuantityHoursOfWork = value;
                OnPropertyChanged();
            }
        }
        private int TotalPriceOfWork;
        public int totalPriceOfWork
        {
            get { return TotalPriceOfWork; }
            set
            {
                TotalPriceOfWork = value;
                OnPropertyChanged();
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
