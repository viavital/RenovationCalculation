using RenovationCalculation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenovationCalculation.Service
{
    class SitesServise
    {
        private SitesServise() { }
        private static SitesServise _siteServiseInstance;

        public static SitesServise GetInstanceOfSites() 
        {
            if (_siteServiseInstance == null)
            {
                _siteServiseInstance = new SitesServise();
            }
            return _siteServiseInstance;
        }

        private List<SiteModel> _allSites;

        public List<SiteModel> GetAllSites()
        {
            if (_allSites == null) //load info from DB only once, next times - using cached data in _allWorks
            {
                using (var db = new WorksDBContext())
                {
                   // _allSites = db.Sites.ToList();
                }
            }

            return _allSites;
        }
    }
}
