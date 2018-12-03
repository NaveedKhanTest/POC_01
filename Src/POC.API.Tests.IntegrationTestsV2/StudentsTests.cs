using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using POC.Service.Models;

namespace POC.API.Tests.IntegrationTestsV2
{
    // StudentsController
    [TestClass]
    public class StudentsTests : BaseTest 
    {
        private string baseUrl = "api/students";

        private string getStudentIdFormat = "http://localhost/students/{0}";

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

            Assert.IsFalse(string.IsNullOrWhiteSpace(actual.Link.Href), "Expected one link");
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


        // StudentModel expected = GetStudentModel(uid);

        [TestMethod]
        public async Task PostStudent_IsOK()
        {
            var newStudent = LoadStudentModel(0);

            var url = $"{baseUrl}";

            var headersSetting = new Action<RequestBuilder>(
                r => r.And(h => h.Headers.Remove(ApiParametersConstant.ApiToken))
                .AddHeader(ApiParametersConstant.ApiToken, "new token 111"));


            var response = await this.Post($"{url}", newStudent, onBeforeSend: headersSetting)
                      .Expect(200);
            //.As<SomeModel>();
            //.As<int>(); to use for int etc remove where T : class

            if (response.Content.Headers.ContentType.MediaType == MediaTypeNames.Application.Json)
            {
                var json = await response.Content.ReadAsStringAsync();
                var valueId = JsonConvert.DeserializeObject<int>(json);
                Assert.IsNotNull(valueId > 0);

            }

            Assert.IsNotNull(response);
        }


        [TestMethod]
        public async Task GetCurriculumSwaggerWithoutDefaultHeaders()
        {
            var headersSetting = new Action<RequestBuilder>(
                r => r.And(h => h.Headers.Remove(ApiParametersConstant.ApiToken))
                .AddHeader(ApiParametersConstant.ApiToken, "new token 111"));

            var responseMessage = await this.Get("/swagger/v1.0/swagger.json", onBeforeSend: headersSetting)
                                            .Expect(200);
            var content = await responseMessage.Content.ReadAsStringAsync();
            dynamic swagger = JsonConvert.DeserializeObject(content);
            Assert.IsNotNull(swagger.paths, "paths is null");
        }


    }
}
