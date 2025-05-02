using APIDevelopmentinASP.NETCore.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace APIDevelopmentinASP.NETCore.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }


       
       
    }
}
