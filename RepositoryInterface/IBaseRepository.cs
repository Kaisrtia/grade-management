namespace GradeManagement.RepositoryInterface {
  internal interface IBaseRepository<T> {
    // CRUD operations for generic entity T
    Task<int> create (T entity);
    Task<T?> getById (string id);
    Task<IEnumerable<T>> getAll ();
    Task<int> update (T entity);
    Task<int> delete (string id);
  }
}
