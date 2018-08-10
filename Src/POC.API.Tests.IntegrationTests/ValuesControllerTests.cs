using Microsoft.AspNetCore.JsonPatch;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace POC.API.Tests.IntegrationTests
{
    [TestClass]
    public class ValuesControllerTests : IntegrationTestBase
    {

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

