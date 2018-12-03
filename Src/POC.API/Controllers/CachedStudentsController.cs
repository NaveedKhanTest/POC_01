using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LazyCache;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using POC.API.Filters;
using POC.Service;
using POC.Service.Models;
using Swashbuckle.AspNetCore.Annotations;

#region LazyCache Ref/Help

// https://github.com/alastairtree/LazyCache/wiki/Quickstart
// Add LazyCache.AspNetCore nuget (include pre-release)
// Add the LazyCache services in you aspnet core Startup.cs 
// Take IAppCache as a dependency in the constructor of the class in which you want to use it
// Wrap the call you want to cache in a lambda and use the cache:
#endregion

namespace POC.API.Controllers
{
    [Route("api/CachedStudents")]
    //[Route("api/[controller]")]

    [TypeFilter(typeof(DenyAccessForUserTypesFilter), Arguments = new object[] { new[] { "Guest" } })]
    public class CachedStudentsController : ControllerBase
    {
        private readonly IAppCache cache;
        protected IStudentService StudentService;

        //Not for bigger service we can create saperate services
        //e.g IStudentQueries & Commands
        public CachedStudentsController(IStudentService _studentService, IAppCache cache)
        {
            this.StudentService = _studentService ?? throw new ArgumentNullException(nameof(_studentService));
            this.StudentService = _studentService;
            this.cache = cache;
        }



        [HttpGet("{id}", Name = nameof(GetStudentByIdCached))]
        [SwaggerOperation(OperationId = "getStudentByIdCached")]
        [ProducesResponseType(typeof(StudentModel), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(object), 400)]
        [ProducesResponseType(typeof(object), 500)]
        public async Task<IActionResult> GetStudentByIdCached(int id)
        {
            if (this.ModelState.IsValid)
            {
                //Func<IEnumerable<Product>> productGetter = () => dbContext.Products.ToList();
                //var productsWithCaching = cache.GetOrAdd("HomeController.Get", productGetter);

                // define a func to get the products but do not Execute() it
                
                Func<Task<StudentModel>> studentGetter = () => GetStudent(id);
                //var model = await cache.GetOrAdd("CachedStudents.GetStudentByIdCached", studentGetter);


                // sliding expiration: keep cachethem there as long as they have been accessed in the last 5 minutes
                // var model = await cache.GetOrAdd("CachedStudents.GetStudentByIdCached", studentGetter, new TimeSpan(0, 5, 0));

                // Add products to the cache for at most 15 sec
                var model = await cache.GetOrAdd("CachedStudents.GetStudentByIdCached", studentGetter, DateTimeOffset.Now.AddSeconds(15));

                // Add products to the cache and keep them there as long as they 
                // have been accessed in the last 5 minutes
                //cache.Add("all-products", products, new TimeSpan(0, 5, 0));

                //var model = await GetStudent(id);

                if (model != null)
                {
                    var url = Url.Link(nameof(GetStudentByIdCached), new { id = model.ID });
                    model.Link = new LinkModel(url, LinkModel.SELF);
                    return this.Ok(model);
                }

                return this.NotFound();
            }

            return this.BadRequest(this.ModelState);
        }

        private async Task<StudentModel> GetStudent(int id)
        {
            return await StudentService.GetAsync(id);
        }



        [HttpGet(Name = nameof(GetAllStudentsCached))]
        [SwaggerOperation(OperationId = "GetAllStudentsCached")]
        [ProducesResponseType(typeof(List<StudentModel>), 200)]
        //[ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(object), 400)]
        //[ProducesResponseType(typeof(MessageModel), 400)]
        [ProducesResponseType(typeof(object), 500)]
        public async Task<IActionResult> GetAllStudentsCached()
        {
            Func<Task<List<StudentModel>>> studentGetter = () => GetAllStudents();
            var students = await cache.GetOrAdd("CachedStudents.GetAllStudentsCached", studentGetter, DateTimeOffset.Now.AddSeconds(15));

            //List<StudentModel> StudentModel = await GetAllStudents();

            foreach (var item in students)
            {
                var url = Url.Link(nameof(GetStudentByIdCached), new { id = item.ID });
                item.Link = new LinkModel(url, LinkModel.SELF);
                
            }

            return this.Ok(students);
        }

        private async Task<List<StudentModel>> GetAllStudents()
        {
            return await this.StudentService.GetAllAsync();
        }
    }
}