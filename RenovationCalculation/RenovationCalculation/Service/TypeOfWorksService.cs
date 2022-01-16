using RenovationCalculation.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RenovationCalculation.Service
{
    class TypeOfWorksService
    {
        private TypeOfWorksService() { }
        private static TypeOfWorksService _instance;

        public static TypeOfWorksService GetInstance() //it should be singleton, because you need only one place to store data
        {
            if (_instance == null)
            {
                _instance = new TypeOfWorksService();
            }
            return _instance;
        }

        public event Action<TypeOfWorkModel> WorkAddedEvent = delegate { };
        public event Action<TypeOfWorkModel> WorkUpdatedEvent = delegate { };

        private List<TypeOfWorkModel> _allWorks;

        public List<TypeOfWorkModel> GetAllWorks()
        {
            if (_allWorks == null) //load info from DB only once, next times - using cached data in _allWorks
            {
                using (var db = new WorksDBContext())
                {
                    _allWorks = db.Works.ToList();
                }
            }

            return _allWorks;
        }

        public void AddWork(TypeOfWorkModel workToAdd)
        {
            using (var db = new WorksDBContext())
            {
                db.Works.Add(workToAdd);
                db.SaveChanges();
            }
            _allWorks.Add(workToAdd);
            WorkAddedEvent(workToAdd);
        }
        public void UpdateWork(TypeOfWorkModel workToUpdate)
        {
            using (var db = new WorksDBContext())
            {
                db.Works.Update(workToUpdate);
                db.SaveChanges();
            }
           TypeOfWorkModel FindingWork = _allWorks.FirstOrDefault(u => u.ID == workToUpdate.ID);
           FindingWork = workToUpdate;
           WorkUpdatedEvent(workToUpdate);
        }
    }
}
