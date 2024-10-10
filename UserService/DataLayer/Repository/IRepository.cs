namespace UserService.DataLayer.Repository
{
    public interface IRepository<T, ID> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(); 
        Task<T> GetByIdAsync(ID id);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity); 
        Task DeleteAsync(int id);
    }
}
