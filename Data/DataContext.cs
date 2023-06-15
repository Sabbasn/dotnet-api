using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_app.bin;
using Microsoft.EntityFrameworkCore;

namespace dotnet_app.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        public DbSet<User> Users => Set<User>();
    }
}