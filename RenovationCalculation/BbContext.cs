using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenovationCalculation.Model;
using Microsoft.EntityFrameworkCore;

namespace RenovationCalculation
{
    class BbContext : DbContext
    {
        public DbSet<TypeOfWorkModel> Works { get; set; }
        public DbSet<WorkerModel> Workers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
           => options.UseSqlite($"Data Source=..//..//DatabaseOfWorks.db");
    }
}
