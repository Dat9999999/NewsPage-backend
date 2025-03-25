using NewsPage.Models.entities;

namespace NewsPage.repositories.interfaces
{
    public interface IArticleRepository
    {
        Task<Article> CreateAsync(Article article);
        Task<IEnumerable<Article>> GetAllAsync();
        Task<Article?> GetArticleWithCategoryAsync(Guid id);
        Task<Article?> GetByIdAsync(Guid id);
        Task UpdateAsync(Article article);
        Task DeleteAsync(Article article);
    }

}
