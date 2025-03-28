using Microsoft.EntityFrameworkCore;
using NewsPage.data;
using NewsPage.Enums;
using NewsPage.Models.entities;
using NewsPage.Models.ResponseDTO;
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

        public async Task<Article?> UpdateAsync(Article article)
        {
            article.UpdateAt = DateTime.Now;
            _context.Articles.Update(article);
            await _context.SaveChangesAsync();

            return await _context.Articles
                .Include(a => a.UserAccounts)
                .Include(a => a.Category)
                    .ThenInclude(c => c.Topic)
                .FirstOrDefaultAsync(a => a.Id == article.Id);
        }


        public async Task DeleteAsync(Article article)
        {
            var comments = await _context.Comments
            .Where(c => c.ArticleId == article.Id)
            .ToListAsync();

            // Xóa tất cả Comment
            if (comments.Any())
            {
                _context.Comments.RemoveRange(comments);
            }

            _context.Articles.Remove(article);

            await _context.SaveChangesAsync();
        }

        public async Task<PaginatedResponseDTO<Article>> GetPaginatedArticlesAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            Guid? categoryId,
            Guid? userAccountId,
            DateTime? publishedAt,
            ArticleStatus? status,
            Guid? topicId,
            string? sortBy,
            string? sortOrder)
        {
            var query = _context.Articles
                .Include(a => a.Category)
                    .ThenInclude(c => c.Topic)
                .Include(a => a.UserAccounts)
                .AsQueryable();

            // Tìm kiếm theo Title hoặc Content
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(a => a.Title.Contains(searchTerm) || a.Content.Contains(searchTerm));
            }

            // Lọc theo CategoryId
            if (categoryId.HasValue)
            {
                query = query.Where(a => a.CategoryId == categoryId.Value);
            }

            // Lọc theo UserAccountId
            if (userAccountId.HasValue)
            {
                query = query.Where(a => a.UserAccountId == userAccountId.Value);
            }

            // Lọc theo PublishedAt
            if (publishedAt.HasValue)
            {
                var date = publishedAt.Value.Date;
                query = query.Where(a => a.PublishedAt.HasValue && a.PublishedAt.Value.Date == date);
            }

            // Lọc theo Status
            if (status.HasValue)
            {
                query = query.Where(a => a.Status == status.Value);
            }

            // Lọc theo TopicId
            if (topicId.HasValue)
            {
                query = query.Where(a => a.Category.TopicId == topicId.Value);
            }

            // Xử lý sắp xếp
            sortBy = sortBy?.ToLower() ?? "publishedat"; // Mặc định 
            sortOrder = sortOrder?.ToLower() ?? "desc";  // Mặc định

            switch (sortBy)
            {
                case "title":
                    query = sortOrder == "asc" ? query.OrderBy(a => a.Title) : query.OrderByDescending(a => a.Title);
                    break;
                case "publishedat":
                    query = sortOrder == "asc" ? query.OrderBy(a => a.PublishedAt) : query.OrderByDescending(a => a.PublishedAt);
                    break;
                case "updateat":
                    query = sortOrder == "asc" ? query.OrderBy(a => a.UpdateAt) : query.OrderByDescending(a => a.UpdateAt);
                    break;
                default:
                    query = query.OrderByDescending(a => a.PublishedAt); // default
                    break;
            }

            var totalCount = await query.CountAsync();

            // Phân trang
            var articles = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResponseDTO<Article>(articles, totalCount, pageNumber, pageSize);
        }


    }
}
