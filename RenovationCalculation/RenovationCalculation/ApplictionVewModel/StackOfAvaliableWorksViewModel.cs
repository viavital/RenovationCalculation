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

namespace RenovationCalculation.ApplictionVewModel
{
    class StackOfAvaliableWorksViewModel
    {
        private TypeOfWorkModel SelectedTypeOfWork;
        public List<TypeOfWorkModel> TypeOfWorks { get; set; }

        public StackOfAvaliableWorksViewModel()
        {
            using (WorksDBContext db = new WorksDBContext())
            {
                
                TypeOfWorks = db.Works.ToList();
            }
        }
         TypeOfWorkModel SelectedWork
        {
            get { return SelectedTypeOfWork; }
            set
            {
                SelectedTypeOfWork = value;
                OnPropertyChanged("SelectedPhone");
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

