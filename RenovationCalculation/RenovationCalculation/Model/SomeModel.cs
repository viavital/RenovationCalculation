using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RenovationCalculation.Model
{
    class SomeModel
    {
        public event Action<string> OnElementAdded = delegate { };

        public List<string> SomeStrings { get; }  = new();

        public void AddString(string str)
        {
            SomeStrings.Add(str);
            //here can be save to tb, etc...
            OnElementAdded(str);
        }
    }
}
