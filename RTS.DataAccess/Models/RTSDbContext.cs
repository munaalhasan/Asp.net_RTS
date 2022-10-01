using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace RTS.DataAccess.Models
{
    public class RTSDbContext :DbContext
    {
        public RTSDbContext(DbContextOptions<RTSDbContext> options) : base(options)
        {

        }        

        public RTSDbContext()
        {
                
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            foreach (var foreignkey in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            {
                foreignkey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<ItemRequest> ItemRequests { get; set; }
        public DbSet<Status> RequestStatus { get; set; }        
        public DbSet<Transaction> Transactions { get; set; }

    }
}
