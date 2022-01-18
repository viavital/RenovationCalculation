using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RenovationCalculation.Model 
{
    class SiteModel : INotifyPropertyChanged
    {
        public int SiteId { get; set; } 

        private string nameOfSite;
        public string NameOfSite
        {
            get { return nameOfSite; }
            set
            { 
                nameOfSite = value;
                OnPropertyChanged();
            }
        }

        private string sitePicture;
        public string SitePicture
        {
            get { return sitePicture; }
            set 
            { 
                sitePicture = value;
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
