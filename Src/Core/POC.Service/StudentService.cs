using POC.Core.Data;
using POC.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public IEnumerable<Student> GetAll()
        //public IEnumerable<Dto.Horse> GetAll()
        {

            var studentByName = _studentRepository.FindByCondition(x => x.FirstMidName.Contains("A"));

            var studentById = _studentRepository.FindByCondition(x => x.ID == 1);


            var students = _studentRepository.GetAll();

            return students;
            //return horses.Select(Map);
        }

        public Student Get(int id)
        //public Dto.Horse Get(int id)
        {
            var student = _studentRepository.Get(id);
            return student;
            //return horse == null ? null : Map(horse);
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
