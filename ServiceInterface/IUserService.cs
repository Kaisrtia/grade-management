using GradeManagement.Model;
using System.Threading.Tasks;

namespace GradeManagement.ServiceInterface {
  internal interface IUserService {
    Task<bool> changeInfo(User user);
  }
}
