using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenovationCalculation.ApplictionViewModel
{
   partial class StackOfAddingWorksViewModel
    {
   
        private bool isEditBtnEnabled;
        public bool IsEditBtnEnabled
        {
            get { return isEditBtnEnabled; }
            set 
            {
                isEditBtnEnabled = value;
                OnPropertyChanged();
            }
        }

       
        private bool isRemoveBtnEnabled;
        public bool IsRemoveBtnEnabled
        {
            get { return isRemoveBtnEnabled; }
            set
            {
                isRemoveBtnEnabled = value;
                OnPropertyChanged();
            }
        }
    }
}
