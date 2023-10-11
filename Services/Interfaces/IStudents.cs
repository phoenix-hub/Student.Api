using Student.Api.Model;

namespace Student.Api.Services.Interfaces
{
    public interface IStudents
    {
        IEnumerable<StudentDto> Get();
        StudentDto GetOne(int id);
        StudentDto SaveOne(StudentQuery student);
        bool SaveBulk(List<StudentQuery> students);
    }
}
