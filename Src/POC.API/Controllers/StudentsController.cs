using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Morcatko.AspNetCore.JsonMergePatch;
using Newtonsoft.Json;
using POC.API.Filters;
using POC.Service;
using POC.Service.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace POC.API.Controllers
{
    [Route("api/[controller]")]
    //[SwaggerTag("Students")]
    //[Route("students")]
    //[ApiController]
    //[Route("students")]
    //[Produces("application/json")] //only json
    [TypeFilter(typeof(DenyAccessForUserTypesFilter), Arguments = new object[] { new[] { "Guest" } })]
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
        [SwaggerOperation(OperationId = "getAllStudents", Tags = new[] { "Students" , "Another Group" })]
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
                var url = Url.Link(nameof(GetStudentById), new { id = item.ID });
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
        //[HttpGet("SomePath/{uid}", Name = nameof(GetSomePathById))]
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
                    var url = Url.Link(nameof(GetStudentById), new { id = model.ID });
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


        [HttpDelete("{uid}", Name = nameof(DeleteCitizenshipById))]
        //[HttpDelete("citizenships/{uid}", Name = nameof(DeleteCitizenshipById))]
        [SwaggerOperation(OperationId = "deleteCitizenshipById")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(object), 400)]
        [ProducesResponseType(typeof(object), 500)]
        public async Task<IActionResult> DeleteCitizenshipById(int uid)
        {
            await Task.CompletedTask;
            return this.NotFound();

            //var existingModel = await this.StudentQueries.GetStudentById(uid);
            //if (existingModel == null)
            //{
            //    return this.NotFound();
            //}

            //var student = Mapper.Map(existingModel);
            //var result = await this.StudentCommands.Delete(student);
            //if (result.ResultStatus == CommandStatus.Success)
            //{
            //    return this.NoContent();
            //}

            //return this.BadRequest(result.ResultValue);
        }




        /*
        public async Task<IActionResult> PostBulk([FromBody] IList<PostData<StudentModel>> studentList)
        {
            var resultList = new List<ResultData<StudentModel>>();

            foreach (var item in studentList)
            {
                var resultData = new ResultData<StudentModel>()
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                };

                try
                {
                    var resultSinglePost = (ObjectResult)await PostCoursesOfStudy(item.Data);
                    resultData.StatusCode = (int)resultSinglePost.StatusCode;

                    switch (resultData.StatusCode)
                    {
                        case (int)HttpStatusCode.OK:
                            var data = (StudentModel)resultSinglePost.Value;
                            resultData.Data = data;
                            break;
                        case (int)HttpStatusCode.BadRequest:
                            var messagesValue = (SomeMsgModel) resultSinglePost.Value;
                            resultData.Messages = messagesValue.Messages;
                            break;
                        default:
                            //resultData.Messages.Add(MessagesHelper.LoadMessageForBulkPostUnexpectedError());
                            //.LogException($"In '{this.GetType().Name}', an unexpected error occurred during multiple/bulk post request. input data: {JsonConvert.SerializeObject(item.Data)}");
                            break;
                    }
                }
                catch (DbUpdateException ex)
                {
                    var sqlException = ex.GetBaseException() as SqlException;
                    throw;
                }
                catch (Exception ex)
                {
                    
                }

                resultList.Add(resultData);
            }

            return StatusCode((int)HttpStatusCode.MultiStatus, resultList);
        }
        */


        public enum CommandStatus
        {
            /// <summary>
            /// Success
            /// </summary>
            Success,

            /// <summary>
            /// Failure
            /// </summary>
            Failure
        }


    }
}