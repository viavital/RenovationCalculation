using RenovationCalculation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenovationCalculation.Service
{
    class WorkersService
    {
        private WorkersService() { }
        private static WorkersService _instance;
        public static WorkersService GetInstance() //it should be singleton, because you need only one place to store data
        {
            if (_instance == null)
            {
                _instance = new WorkersService();
            }
            return _instance;
        }

        public event Action<WorkerModel> WorkerAddedEvent = delegate { };
        public event Action<WorkerModel> WorkerDeletedEvent = delegate { };

        private List<WorkerModel> _allWorkers;

        public List<WorkerModel> GetAllWorkers()
        {
            if (_allWorkers == null) //load info from DB only once, next times - using cached data in _allWorkers
            {
                using (var db = new WorksDBContext())
                {
                    _allWorkers = db.Workers.ToList();
                }
            }

            return _allWorkers;
        }

        public void AddWorker(WorkerModel worker)
        {
            using (var db = new WorksDBContext())
            {
                db.Workers.Add(worker);
                db.SaveChanges();
            }
            _allWorkers.Add(worker);
            WorkerAddedEvent(worker);
        }

        public void DeleteWorker(WorkerModel worker)
        {
            using (var db = new WorksDBContext())
            {
                db.Workers.Remove(worker);
                db.SaveChanges();
            }
            _allWorkers.Remove(worker);
            WorkerDeletedEvent(worker);
        }
    }
}
