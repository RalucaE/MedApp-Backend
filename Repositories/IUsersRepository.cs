using MedicalAPI.Models;

namespace MedicalAPI.Repositories
{
    public interface IUsersRepository : IRepository<User>
    {
        User? GetByEmail(string email);
    }
}
