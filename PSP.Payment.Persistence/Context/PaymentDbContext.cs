using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;

namespace PSP.Payment.Persistence.Context
{
    public class PaymentDbContext : DbContext
    {
        public PaymentDbContext(
             DbContextOptions<PaymentDbContext> options)
             : base(options)
        {
        }

        //public DbSet<TopupTransaction> TopupTransactions => Set<TopupTransaction>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PaymentDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
