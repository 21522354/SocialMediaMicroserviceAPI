


using Microsoft.EntityFrameworkCore;

namespace PostService.Data_Layer.Repository
{
    public class Repository<T, ID> : IRepository<T, ID> where T : class
    {
        private readonly PostServiceDBContext _context;

        public Repository(PostServiceDBContext context)
        {
            _context = context;
        }
        public async Task<T> CreateAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            _context.SaveChanges();
            return entity;
        }

        public async Task DeleteAsync(ID id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if(entity == null)
            {
                throw new Exception("Can't found this entity");
            }
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(ID id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if(entity == null)
            {
                throw new Exception("Can't found this entity");
            }
            return entity;  
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);       
            await _context.SaveChangesAsync();
        }
    }
}
