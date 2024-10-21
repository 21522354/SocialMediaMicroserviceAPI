namespace PostService.Data_Layer.Repository
{
    public interface IRepository<T, Id> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(Id id);
        Task<T> CreateAsync(T entity);
        Task UpdateAsync(T entity);     
        Task DeleteAsync(Id id);        
    }
}
