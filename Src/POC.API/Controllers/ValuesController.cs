using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Swashbuckle.AspNetCore.Annotations;

namespace POC.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Create, read, update and delete Values -- using SwaggerTag")] // https://github.com/domaindrivendev/Swashbuckle.AspNetCore 
    public class ValuesController : ControllerBase
    {
        //[FromHeader(Name = "header-name-xxx")]
        //[Required(ErrorMessage = "HeaderNameXxx in ValuesController is required.")]
        //public string HeaderNameXxx { get; set; }


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
        //[HttpGet("{id}")]
        [HttpGet("{id}", Name = nameof(GetValueById))]
        //[HttpGet("{id}/somechildrens", Name = nameof(Get))]

        public ActionResult<provider> GetValueById(int id)
        {
            // check the header
            StringValues headerValues;
            var organisationId = string.Empty;
            if (Request.Headers.TryGetValue("SomeHeaderId", out headerValues))
            {
                organisationId = headerValues.FirstOrDefault();
            }
            var headerAuthorization = Request.Headers.FirstOrDefault(h => h.Key.Equals("Authorization"));

            var bearerVal = string.Empty;
            if (Request.Headers.TryGetValue("Authorization", out headerValues))
            {
                bearerVal = headerValues.FirstOrDefault();
            }


            //// Check that the Authorization header is present in the HTTP request and that it is in the
            //// format of "Authorization: Bearer <token>"
            //System.Net.Http.Headers.AuthenticationHeaderValue authorizationHeader = context.HttpContext.Request.Headers.Authorization;

            //if ((authorizationHeader == null) || (authorizationHeader.Scheme.CompareTo("Bearer") != 0) || (String.IsNullOrEmpty(authorizationHeader.Parameter)))
            //{
            //    // return HTTP 401 Unauthorized
            //}

            if (this.ModelState.IsValid)
            {
                var model = new provider();
                //return this.NotFound();
                return model;
            }

            //return this.BadRequest(this.ModelState);
            return new provider(); 
        }

        // POST api/values

        [SwaggerOperation(
            Summary = "Creates a new value",
            Description = "Requires admin privileges",
            OperationId = "postValue"
            )]
        [SwaggerResponse(201, "The value was created -- using SwaggerTag", typeof(string))]
        //[ProducesResponseType(typeof(SomeModel), 200)]

        [SwaggerResponse(400, "The value data is invalid -- using SwaggerTag")]
        [ProducesResponseType(404)] //for swagger response
        //[SwaggerOperation(OperationId = "postValue")]
        [ProducesResponseType(typeof(object), 500)]
        //[ProducesResponseType(500)]

        [HttpPost]
        public ActionResult Post([FromBody] string value)
        {
            if (this.ModelState.IsValid)
            {
            }
            return this.BadRequest(this.ModelState);
        }

        [HttpPost(Name = nameof(PostSomeValue))]
        public async Task<IActionResult> PostSomeValue([FromBody] object model)
        {
            await Task.Delay(10);
            throw new NotImplementedException();
        }


        #region Patch example

        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(object), 400)]
        [ProducesResponseType(typeof(object), 500)]
        [HttpPatch("{id}", Name = nameof(PatchSomeValue))]
        public async Task<IActionResult> PatchSomeValue(int id, [FromBody] JsonPatchDocument<object> patchDocument)
        {
            //if not found
            // return this.NotFound();

            await Task.Delay(1);
            throw new NotImplementedException();
        }

        #endregion


        // PUT api/values/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] string value)
        {
            if (this.ModelState.IsValid)
            {
                //if not found
                return this.NotFound();
            }

            return this.BadRequest(this.ModelState);

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
