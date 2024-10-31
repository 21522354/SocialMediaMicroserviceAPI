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
            // Lấy tất cả các thực thể
            var query = _context.Set<T>().AsQueryable();

            // Include tất cả các collection
            var entityType = _context.Model.FindEntityType(typeof(T));
            var navigations = entityType.GetNavigations();

            foreach (var navigation in navigations)
            {
                query = query.Include(navigation.Name);
            }

            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(ID id)
        {
            var entity = await _context.Set<T>().FindAsync(id);

            if (entity == null) return null;

            // Include các trường collection liên quan
            var entry = _context.Entry(entity);
            var navigations = entry.Navigations;

            foreach (var navigation in navigations)
            {
                // Load collection nếu nó là một ICollection
                if (navigation.IsLoaded == false)
                {
                    await navigation.LoadAsync();
                }
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
