﻿//using POC.Core.Data;
//using POC.Data;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace POC.Service
//{
//    public class UserService : IUserService
//    {
//        private IRepository<User> userRepository;
//        private IRepository<UserProfile> userProfileRepository;

//        public UserService(IRepository<User> userRepository, IRepository<UserProfile> userProfileRepository)
//        {
//            this.userRepository = userRepository;
//            this.userProfileRepository = userProfileRepository;
//        }

//        public IQueryable<User> GetUsers()
//        {
//            return userRepository.Table;
//        }

//        public User GetUser(long id)
//        {
//            return userRepository.GetById(id);
//        }

//        public void InsertUser(User user)
//        {
//            userRepository.Insert(user);
//        }

//        public void UpdateUser(User user)
//        {
//            userRepository.Update(user);
//        }

//        public void DeleteUser(User user)
//        {
//            userProfileRepository.Delete(user.UserProfile);
//            userRepository.Delete(user);
//        }
//    }
//}
