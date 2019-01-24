using Microsoft.EntityFrameworkCore;
using POC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POC.Data
{
    /// <summary>
    /// Track Created and Modified fields Automatically with Entity Framework
    /// https://stackoverflow.com/questions/38618418/entity-framework-create-update-common-field-in-multiple-tables
    /// https://benjii.me/2014/03/track-created-and-modified-fields-automatically-with-entity-framework-code-first/ 
    /// 
    /// </summary>
    public partial class PocDbContext //SchoolContext
    {
        public PocDbContext()
        //public SchoolContext(IRequestContext requestContext)
    : base()
        {
            //this.RequestContextAccessor = requestContext ?? throw new ArgumentNullException(nameof(requestContext));
        }

        //     public SchoolContext(DbContextOptions<SchoolContext> options)
        //: base(options)
        //     {
        //         //this.RequestContextAccessor = requestContext ?? throw new ArgumentNullException(nameof(requestContext));
        //     }

        //public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
        //{
        //}



        public override int SaveChanges()
        {
            SetCommonDbTablesFields();

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            SetCommonDbTablesFields();

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        /// <inheritdoc/>
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            SetCommonDbTablesFields();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }


        public void SetCommonDbTablesFields()
        {
            var currentUsername = "test user";

            //var currentUsername = !string.IsNullOrEmpty(System.Web.HttpContext.Current?.User?.Identity?.Name)
            //? HttpContext.Current.User.Identity.Name : "Anonymous";

            var entities = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    ((BaseEntity)entity.Entity).AddedDate = DateTime.UtcNow;
                    ((BaseEntity)entity.Entity).UserCreated = currentUsername;
                    ((BaseEntity)entity.Entity).IP = "ip 123";
                }

                ((BaseEntity)entity.Entity).ModifiedDate = DateTime.UtcNow;
                ((BaseEntity)entity.Entity).UserModified = currentUsername;
            }

            var fieldAddedDate = "AddedDate";
            foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Added && e.Metadata.GetProperties().Any(_ => _.Name == fieldAddedDate)))
            {
                entry.CurrentValues[fieldAddedDate] = DateTime.Now;
            }

        }

    }
}
