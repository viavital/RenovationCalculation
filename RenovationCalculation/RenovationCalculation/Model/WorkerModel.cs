using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenovationCalculation.Model
{
    class WorkerModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
      //  public int PricePerHour { get; set; }
        public int WorkId { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
