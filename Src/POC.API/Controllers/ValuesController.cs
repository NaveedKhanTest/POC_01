using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace POC.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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


        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value " + id;
        }

        // POST api/values
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
}
