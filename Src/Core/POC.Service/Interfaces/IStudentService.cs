using POC.Core.Data;
using System.Collections.Generic;

namespace POC.Service
{
    public interface IStudentService
    {

        IEnumerable<Student> GetAll();
        Student Get(int id);

        //Todo: Use Dto
        //IEnumerable<Dto.Horse> GetAll();
        //Dto.Horse Get(int id);

    }
}