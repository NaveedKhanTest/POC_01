using POC.Core.Data;
using POC.Data;
using POC.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC.Service
{
    public class StudentService : IStudentService
    {
        private readonly IRepository<Student> _studentRepository;

        public StudentService(IRepository<Student> studentRepository)
        {
            _studentRepository = studentRepository;
        }

        //ToDo: user Models VM 
        //private readonly IRepository<Models.Horse> _horseRepository;

        //public HorseService(IRepository<Models.Horse> horseRepository)
        //{
        //    _horseRepository = horseRepository;
        //}


        public async Task<List<StudentModel>> GetAllAsync()
        //public IEnumerable<Student> GetAll()
        //public IEnumerable<Dto.Horse> GetAll()
        {
            var model = new List<StudentModel>();

            var studentByName = _studentRepository.FindByCondition(x => x.FirstMidName.Contains("A"));

            //var studentById = _studentRepository.FindByCondition(x => x.ID == 1);
            var students = await _studentRepository.GetAllAsync();

            foreach (var item in students.ToList())
            {
                model.Add(Map(item));
            }

            return model;
            //return horses.Select(Map);
        }

        private StudentModel Map(Student value)
        {
            value = value ?? throw new ArgumentNullException();

            return new StudentModel
            {
                ID = value.ID,
                EnrollmentDate = value.EnrollmentDate,
                FirstMidName = value.FirstMidName,
                LastName = value.LastName
            };
        }

        public async Task<StudentModel> GetAsync(int id)
        //public Dto.Horse Get(int id)
        {
            var student = (await _studentRepository.FindByConditionAync( x => x.ID == id) ).FirstOrDefault();
            if (student != null)
            {
                var model = Map(student);
                return model;
            }
            return null;

        }

        public Task<List<StudentModel>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<int> PostAsync(StudentModel studentModel)
        {
            var studentEntity = MapToEntity(studentModel);
            //user Update for patch/update _studentRepository.Update(studentEntity)
            var student = await _studentRepository.PostAsync(studentEntity);
            //todo : may be map student to student model

            await Task.Delay(1);
            return student.ID;
        }

        public async Task<StudentModel> UpdateAsync(StudentModel studentModel)
        {
            var studentEntity = MapToEntity(studentModel);
            await Task.Delay(1);

            //user Update for patch/update _studentRepository.Update(studentEntity)
            var student = _studentRepository.Update(studentEntity);

            var model = Map(student);

            return model;
        }


        private Student MapToEntity(StudentModel model)
        {
            model = model ?? throw new ArgumentNullException(nameof(model));

            return new Student
            {
                ID = model.ID,
                EnrollmentDate = model.EnrollmentDate,
                FirstMidName = model.FirstMidName,
                LastName = model.LastName
            };
        }


        //private static Dto.Horse Map(Models.Horse horse)
        //{
        //    return new Dto.Horse
        //    {
        //        Id = horse.Id,
        //        Name = horse.Name,
        //        Starts = horse.RaceStarts,
        //        Win = horse.RaceWins,
        //        Place = horse.RacePlace,
        //        Show = horse.RaceShow,
        //        Earnings = horse.Earnings
        //    };
        //}
    }
}
