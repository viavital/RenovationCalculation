using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RenovationCalculation.Model;

namespace RenovationCalculation
{
    class WorksDBContext : DbContext
    {
        public DbSet<TypeOfWorkModel> Works { get; set; }
        public DbSet<WorkerModel> Workers { get; set; }
      //  public DbSet<SiteModel> Sites { get; set; }        // якщо розкоментити - вилазтить ексепшн
       

        public readonly string DbPath = "../../../DatabaseOfWorks.db";
        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite($"Data Source={DbPath}");
    }
}
