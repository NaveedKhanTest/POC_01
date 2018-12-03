using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using POC.Service.Models;

namespace POC.API.Tests.IntegrationTestsV2
{
    // CachedStudentsController
    [TestClass]
    public class CachedStudents : BaseTest 
    {
        private string baseUrl = "api/CachedStudents";

        private string getStudentIdFormat = "http://localhost/CachedStudents/{0}";

        [TestMethod]
        public async Task GetStudentsById_IsOk()
        {
            var uid = 1;
            var url = $"{baseUrl}/{uid}";
            StudentModel expected = LoadStudentModel(uid);

            var actual = await this.Get($"{url}")
                      .Expect(200)
                      .As<StudentModel>();

            Assert.IsNotNull(actual);
            //Assert.AreEqual(actual.FirstMidName, expected.FirstMidName);

            Assert.IsFalse(string.IsNullOrWhiteSpace(actual.Link.Href), "Expected self link");
        }

        [TestMethod]
        public async Task GetAllStudents_IsOk()
        {
            var url = $"{baseUrl}";
            var students = await this.Get($"{url}")
                      .Expect(200)
                      .As<List<StudentModel>>();

            Assert.IsNotNull(students);
            //Assert.AreEqual(actual.FirstMidName, expected.FirstMidName);

            //Assert.IsFalse(string.IsNullOrWhiteSpace(actual.Link.Href), "Expected one link");

        }


        private StudentModel LoadStudentModel(int uid)
        {
            var expected = new StudentModel
            {
                ID = uid,
                FirstMidName = "Ramis",
                LastName = "last name Jadoon",
                EnrollmentDate = DateTime.Now,
                Link = new LinkModel(string.Format(getStudentIdFormat, uid), "self"),
                //Link.Self(string.Format(getStudentIdFormat, uid)),

            };
            return expected;
        }

       


    }
}
