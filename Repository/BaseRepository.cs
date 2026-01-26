using GradeManagement.RepositoryInterface;

namespace GradeManagement.Repository {
  internal class BaseRepository<T>: IBaseRepository<T> {
    // connect to SQL database
    public async Task<int> create (T entity) {
      throw new NotImplementedException();
    }

    public async Task<int> delete (string id) {
      throw new NotImplementedException();
    }

    public async Task<IEnumerable<T>> getAll () {
      throw new NotImplementedException();
    }

    public async Task<T?> getById (string id) {
      throw new NotImplementedException();
    }

    public async Task<int> update (T entity) {
      throw new NotImplementedException();
    }
  }
}
