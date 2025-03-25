using NewsPage.Models.entities;

namespace NewsPage.repositories.interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(Guid id);
        Task<Category?> AddAsync(Category category);
        Task<Category?> UpdateAsync(Category category);
        Task DeleteAsync(Guid id);
    }
}
