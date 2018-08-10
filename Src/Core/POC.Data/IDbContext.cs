using Microsoft.EntityFrameworkCore;
using POC.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace POC.Data
{
    public interface IDbContext
    {
        //DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity;
        int SaveChanges();
    }
}
