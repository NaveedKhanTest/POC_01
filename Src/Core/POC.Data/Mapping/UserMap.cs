//using Microsoft.EntityFrameworkCore;
//using POC.Core.Data;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace POC.Data.Mapping
//{
//    public class UserMap : IEntityTypeConfiguration<User>
//    {
//        public UserMap()
//        {
//            //key
//            HasKey(t => t.ID);
//            //properties
//            Property(t => t.UserName).IsRequired();
//            Property(t => t.Email).IsRequired();
//            Property(t => t.Password).IsRequired();
//            Property(t => t.AddedDate).IsRequired();
//            Property(t => t.ModifiedDate).IsRequired();
//            Property(t => t.IP);
//            //table
//            ToTable("Users");
//        }
//    }
//}
