using System.Collections.ObjectModel;

namespace RenovationCalculation.Model
{
    class SomeModel
    {
        public ObservableCollection<string> SomeStrings { get; }  = new();

        public void AddString(string str)
        {
            SomeStrings.Add(str);
            //here can be save to tb, etc...
        }
    }
}
