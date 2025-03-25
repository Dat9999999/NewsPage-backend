using Microsoft.EntityFrameworkCore;
using NewsPage.data;
using NewsPage.Models.entities;
using NewsPage.repositories.interfaces;

namespace NewsPage.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly ApplicationDbContext _context;

        public ArticleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Article?> CreateAsync(Article article)
        {
            article.Id = Guid.NewGuid();
            article.PublishedAt = null;
            article.UpdateAt = null;

            await _context.Articles.AddAsync(article);
            await _context.SaveChangesAsync();


            return await _context.Articles
                .Include(a => a.UserAccounts)
                .Include(a => a.Category)
                    .ThenInclude(c => c.Topic)
                .FirstOrDefaultAsync(a => a.Id == article.Id);
        }

        public async Task<IEnumerable<Article>> GetAllAsync()
        {
            return await _context.Articles
                .Include(a => a.Category)
                    .ThenInclude(c => c.Topic)
                .Include(a => a.UserAccounts)
                .ToListAsync();
        }

        public async Task<Article?> GetArticleWithCategoryAsync(Guid id)
        {
            return await _context.Articles
                .Include(a => a.Category)
                    .ThenInclude(c => c.Topic)
                .Include(a => a.UserAccounts)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Article?> GetByIdAsync(Guid id)
        {
            return await _context.Articles.FindAsync(id);
        }

        public async Task UpdateAsync(Article article)
        {
            article.UpdateAt = DateTime.Now;
            _context.Articles.Update(article);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Article article)
        {
            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
        }
    }
}
