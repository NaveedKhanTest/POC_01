using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace POC.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Create, read, update and delete Values -- using SwaggerTag")] // https://github.com/domaindrivendev/Swashbuckle.AspNetCore 
    public class ValuesController : ControllerBase
    {



        //[HttpPatch("{id:int}", Name = nameof(PartiallyUpdateMy))]
        //public ActionResult PartiallyUpdateMyXxx(int id, [FromBody] JsonPatchDocument<MyModel> patchDoc)
        //{
        //    if (patchDoc == null)
        //    {
        //        return BadRequest();
        //    }

        //    var existingEntity = db.GetSingle(id); 
        //    var existingItemAsMessage = existingEntity;

        //    patchDoc.ApplyTo(existingItemAsMessage, ModelState);

        //    TryValidateModel(existingEntity);

        //    if (!ModelState.IsValid){
        //        return BadRequest(ModelState); }

        //    //Map each field
        //    mapService.Update(existingEntity, existingItemAsMessage);

        //    var updated = repository.Update(id, existingEntity);

        //    repository.Save();

        //    return Ok(updated);
        //}


        /// <summary>
        /// Get all values 
        /// </summary>
        /// <returns>string array</returns>
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {

            return new string[] { "value1", "value2" };
        }


        /// <summary>
        /// Get by Id 
        /// GET api/values/5
        /// </summary>
        /// <param name="id">id number</param>
        /// <returns>returns string</returns>
        [HttpGet("{id}")]
        public ActionResult<provider> Get(int id)
        {
            var model = new provider();
            return model;

        }

        // POST api/values

        [SwaggerOperation(
            Summary = "Creates a new value",
            Description = "Requires admin privileges"
            )]
        [SwaggerResponse(201, "The value was created -- using SwaggerTag", typeof(string))]
        [SwaggerResponse(400, "The value data is invalid -- using SwaggerTag")]

        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }







    public class provider
    {
        public string provideruid { get; set; }
        public string providercode { get; set; }
        public string provider_name { get; set; }
        public string providertype { get; set; }
        public string heptype { get; set; }
        public string provider_effectivefrom { get; set; }
    }




}
