﻿using NewsPage.Enums;
using NewsPage.Models.entities;
using NewsPage.Models.ResponseDTO;

namespace NewsPage.repositories.interfaces
{
    public interface IArticleRepository
    {
        Task<Article> CreateAsync(Article article);
        Task<Article?> GetArticleWithCategoryAsync(Guid id);
        Task<Article?> GetByIdAsync(Guid id);
        Task<Article?> UpdateAsync(Article article);
        Task DeleteAsync(Article article);


        Task<PaginatedResponseDTO<Article>> GetPaginatedArticlesAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            Guid? categoryId,
            Guid? userAccountId,
            DateTime? publishedAt,
            ArticleStatus? status,
            Guid? topicId,
            string? sortBy,
            string? sortOrder
        );
    }

}
