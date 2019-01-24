using Microsoft.VisualStudio.TestTools.UnitTesting;
using Morcatko.AspNetCore.JsonMergePatch;
using Newtonsoft.Json;
using POC.Service.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace POC.API.Tests.IntegrationTests
{
    [TestClass]
    public class ValuesControllerTests : IntegrationTestBase
    {
        private readonly string baseUrl = "api/Students";


        [TestMethod]
        public void GetAllValues()
        {
            var result = this.GetAll<string>(Configurations.ValuesUrlBase);
            Assert.IsNotNull(result.IsSuccessful);
        }

        [TestMethod]
        public void GetSingleValue()
        {
            var result = this.GetById<string>($"{Configurations.ValuesUrlBase}1");
            Assert.IsNotNull(result.IsSuccessful);
            Assert.IsTrue(result.StatusCode == System.Net.HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GetStudentById_IsOK()
        {
            var uid = 1;
            var url = $"api/Students/{uid}";
            //var url = $"{baseUrl}/{uid}";
            var newLastName = "Test lastName";
            var newItem = JsonConvert.SerializeObject(new { lastName = newLastName });

            var result = this.GetById<string>(url);

            Assert.IsNotNull(result.IsSuccessful);


        }

        [TestMethod]
        public async Task PatchStudent_IsOK()
        {
            var uid = 1;
            var url = $"api/Students/{uid}";
            //var url = $"{baseUrl}/{uid}";
            var newLastName = "Test lastName 1";
            var newItem = JsonConvert.SerializeObject(new { lastName = newLastName });

            var actual = await this.Patch($"{url}", newItem)
                      .Expect(200)
                      .As<StudentModel>();

            Assert.AreEqual(newLastName, actual.LastName, string.Format("Expected LastName {0}", newLastName));
        }

        //public async Task<HttpResponseMessage> Patch(string path, string json)
        //{
        //    var content = new StringContent(json, System.Text.Encoding.UTF8, JsonMergePatchDocument.ContentType);
        //    var httpResponse = await HttpClient.PatchAsync(path, content).ConfigureAwait(false);
        //    return await httpResponse.CheckServerError(this.TestContext);
        //}

        public TestContext TestContext { get; set; }

        //NOTE: other test project is working fine

        //[TestMethod]
        //public async Task PostStudents_IsOK()
        //{
        //    var newItem = new StudentModel()
        //    {
        //        EnrollmentDate = DateTime.Now,
        //        FirstMidName = "FirstMid Name",
        //        LastName = "Last name",
        //    };

            
        //    var url = $"{baseUrl}";
        //    var actual = this.Post($"{url}", newItem)
        //              .Expect(200)
        //              .As<StudentModel>();
        //    Assert.IsNotNull(actual);
        //}

        //[TestMethod]
        //public void CourseOfStudy_ShouldBe_PartiallyUpdated()
        //{
        //    var updatedName = "Updated name xyz";
        //    var id = 2;
        //    var url = $"{Configurations.MyxxxUrlBase}{id}";
        //    var patchDocument = new JsonPatchDocument();
        //    // /FileName e.g. FullName
        //    patchDocument.Replace("/FullName", updatedName);
        //    var resultPartiallyUpdated = this.PartiallyUpdate(url, patchDocument);
        //    Assert.IsTrue(resultPartiallyUpdated.IsSuccessful);

        //}




    }
}

