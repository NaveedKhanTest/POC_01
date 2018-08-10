using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POC.Data;
using POC.Service;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace POC.Web.Controllers
{
    public class StudentsController : Controller
    {
        private readonly IStudentService _studentService;

        //private readonly SchoolContext _context;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;

            //_context = context;
        }

        public ActionResult Index()
        {
            var students = _studentService.GetAll();

            var model = students.ToList();

            return View(model);
        }

        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Students.ToListAsync());
        //}

        //// GET: /<controller>/
        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
