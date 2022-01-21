﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using RenovationCalculation.Model;

namespace RenovationCalculation.ApplictionViewModel
{
    class WelcomeWindowViewModel : INotifyPropertyChanged
    {
        private List<SiteModel> SiteModels;


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }   
}
