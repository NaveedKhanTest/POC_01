//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Text;

//using Microsoft.EntityFrameworkCore;
//using POC.Core;

//namespace POC.Data
//{
//       public class __PocDbContext : DbContext, IDbContext
//    {
//        //public PocDbContext()
//        //    : base("name=DbConnectionString")
//        //{
//        //}


//        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        //{
//        //    //// get the configuration from the app settings
//        //    //var config = new ConfigurationBuilder()
//        //    //      .SetBasePath(Directory.GetCurrentDirectory())
//        //    //      .AddJsonFile("appsettings.json")
//        //    //      .Build();

//        //    // define the database to use
//        //    //optionsBuilder.UseSqlServer(config.GetConnectionString("StandardDatabase"));

//        //    //to fix error add: Microsoft.EntityFrameworkCore.SqlServer nuget package
//        //    optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EFGetStarted.ConsoleApp.NewDb;Trusted_Connection=True;");

//        //    //optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["BloggingDatabase"].ConnectionString);
//        //}

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
//        {

//            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
//            .Where(type => !String.IsNullOrEmpty(type.Namespace))
//            .Where(type => type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>));
//            foreach (var type in typesToRegister)
//            {
//                dynamic configurationInstance = Activator.CreateInstance(type);
//                modelBuilder.ApplyConfiguration(configurationInstance);
//                //modelBuilder.Configurations.Add(configurationInstance);
//            }
//            base.OnModelCreating(modelBuilder);
//        }

//        //public new DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
//        ////public new IDbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
//        //{
//        //    return base.Set<TEntity>();
//        //}





//    }
//}
