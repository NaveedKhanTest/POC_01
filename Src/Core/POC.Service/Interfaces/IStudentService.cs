using POC.Core.Data;
using POC.Service.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POC.Service
{
    public interface IStudentService
    {
        Task<List<StudentModel>>  GetAll();
        Task<List<StudentModel>> GetAllAsync();
        //IEnumerable<Student> GetAll();
        Task<StudentModel> GetAsync(int id);

        Task<int> PostAsync(StudentModel studentModel);
        Task<StudentModel> UpdateAsync(StudentModel studentModel);

        // Add

        //Todo: Use Dto
        //IEnumerable<Dto.Horse> GetAll();
        //Dto.Horse Get(int id);

    }
}