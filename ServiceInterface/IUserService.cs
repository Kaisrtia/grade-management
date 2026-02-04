using GradeManagement.Entity;
using System.Threading.Tasks;

namespace GradeManagement.ServiceInterface {
  internal interface IUserService {
    Task<bool> changeInfo(User user);
  }
}
