using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RenovationCalculation.Model
{
    class SomeModel : INotifyPropertyChanged
    {
        private List<string> _someStrings = new List<string>();
        public List<string> SomeStrings
        {
            get { return _someStrings; }
        }

        public void AddString(string str)
        {
            _someStrings.Add(str);
            //here can be save to tb, etc...
            OnPropertyChanged("SomeStrings");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
