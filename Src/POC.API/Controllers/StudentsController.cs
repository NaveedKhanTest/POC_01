using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Morcatko.AspNetCore.JsonMergePatch;
using POC.Service;
using POC.Service.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace POC.API.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    //[Route("students")]
    //[Produces("application/json")] //only json
    public class StudentsController : ControllerBase
    {
        protected IStudentService StudentService;

        //Not for bigger service we can create saperate services
        //e.g IStudentQueries & Commands
        public StudentsController(IStudentService _studentService)
        {
            this.StudentService = _studentService ?? throw new ArgumentNullException(nameof(_studentService));
            this.StudentService = _studentService;
        }

        [HttpGet(Name = nameof(GetAllStudents))]
        [SwaggerOperation(OperationId = "getAllStudents")]
        [ProducesResponseType(typeof(List<StudentModel>), 200)]
        //[ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(object), 400)]
        //[ProducesResponseType(typeof(MessageModel), 400)]
        [ProducesResponseType(typeof(object), 500)]
        //[ProducesResponseType(typeof(MessageModel), 500)]
        public async Task<IActionResult> GetAllStudents()
        {
            //List<StudentModel> 
            List<StudentModel> StudentModel = await this.StudentService.GetAllAsync();

            foreach (var item in StudentModel)
            {
                var url = Url.Link(nameof(GetStudentById), new { uid = item.ID });
                item.Link = new LinkModel(url, LinkModel.SELF);
                //{
                //    LinkModel
                //    LinkModel.Self(Url.Link(nameof(GetStudentById), new { uid = item.ID }))
                //    //LinkModel.Self(Url.Link(nameof(GetStudentById), new { uid = item.ID }))
                //};
            }

            return this.Ok(StudentModel);
        }
        
        [HttpGet("{id}", Name = nameof(GetStudentById))]
        [SwaggerOperation(OperationId = "getStudentById")]
        [ProducesResponseType(typeof(StudentModel), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(object), 400)]
        [ProducesResponseType(typeof(object), 500)]
        public async Task<IActionResult> GetStudentById(int id)
        {
            if (this.ModelState.IsValid)
            {
                var model =  await StudentService.GetAsync(id);
                if (model != null)
                {
                    var url = Url.Link(nameof(GetStudentById), new { uid = model.ID });
                    model.Link = new LinkModel(url, LinkModel.SELF);
                    return this.Ok(model);
                }

                return this.NotFound();
            }

            return this.BadRequest(this.ModelState);
        }


        [HttpPatch("{id}", Name = nameof(PatchStudent))]
        [SwaggerOperation(OperationId = "patchStudent")]
        [ProducesResponseType(typeof(StudentModel), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(object), 400)]
        [ProducesResponseType(typeof(object), 500)]
        [Consumes(JsonMergePatchDocument.ContentType)]
        public async Task<IActionResult> PatchStudent(int id, [FromBody] JsonMergePatchDocument<StudentModel> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            //send {"lastName": "ssss",  "firstMidName": "bbbbbb", } from post man
            //Todo: test with postman not wokring for
            //[ { "op": "add", "path": "/lastName", "value": "some name"} ]

            await Task.CompletedTask;

            var existingModel = await StudentService.GetAsync(id);
            patchDocument.ApplyTo(existingModel);
            var updatedStudent = await StudentService.UpdateAsync(existingModel);

            return Ok(updatedStudent);

            #region patch code not in us

            //var existingModel = await this.CampusQueries.GetCampusById(uid);

            //if (existingModel == null)
            //{
            //    return this.NotFound();
            //}

            //patchDocument.ApplyTo(existingModel);
            //TryValidateModel(existingModel);

            //if (!ModelState.IsValid)
            //{
            //    return this.BadRequest(ModelState);
            //}

            //var entity = Map(existingModel);

            //    var model = await Commands.Update(entity);
            //    var selfurl = Url.Link(nameof(GetStudentById), new { uid = model.id });
            //    model.Links = new[] { Link.Self(selfurl) };
            //    return Ok(model);


            //return this.BadRequest(new MessageValuesModel() { Values = entity.Messages });
            //var existingModel = new StudentModel()
            //{
            //    ID = 1,
            //    FirstMidName = "some name",
            //    LastName = "last name 1"
            //};
            #endregion


        }





        [HttpPost(Name = nameof(PostStudent))]
        [SwaggerOperation(OperationId = "postStudent")]
        [ProducesResponseType(typeof(object), 201)]
        [ProducesResponseType(typeof(object), 400)]
        [ProducesResponseType(typeof(object), 500)]
        public async Task<IActionResult> PostStudent([FromBody] StudentModel student)
        {
            student = student ?? new StudentModel();
            
            //not valid
            if (student == null)
            {
                return this.BadRequest(new { Values = "invalid model with messages .." });
            }

            var id = await StudentService.PostAsync(student);
            if (id != null && id > 0)
            {
                //var url = Url.Link(nameof(GetStudentById), new { id = id });
                //model.Link = new LinkModel(url, LinkModel.SELF);
                //return 201 with id
                return this.Ok(id);
            }

            return this.BadRequest(new { Values = "some error ... model with messages .." });
        }



    }
}