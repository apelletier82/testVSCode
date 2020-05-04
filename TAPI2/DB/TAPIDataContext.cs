using System;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TAPI2.Entities;
using TAPI2.Services.Abstract;
using System.Threading.Tasks;
using System.Threading;

namespace TAPI2.DB
{
    public class TAPIDataContext : DbContext
    {
        private IUserIdentity _userIdentity;
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {                
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        
        public TAPIDataContext(DbContextOptions<TAPIDataContext> options, IUserIdentity userIdentity)
            : base(options)
        {
            _userIdentity = userIdentity;
            Database.EnsureCreated();            
        }        

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            addAuditInformationToEntity();            
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            addAuditInformationToEntity();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void addAuditInformationToEntity()
        {
            foreach(var e in this.ChangeTracker.Entries<Contact>())
                if (e.State == EntityState.Added)
                {
                    e.Entity.CreationDate = DateTime.Now;                    
                    e.Entity.CreationUser = _userIdentity.Username;
                }
                else if (e.State == EntityState.Modified)
                {
                    e.Entity.ChangeDate = DateTime.Now;
                    e.Entity.ChangeUser = _userIdentity.Username;
                }
        }
    }
}