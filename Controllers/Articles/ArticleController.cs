using Microsoft.AspNetCore.Mvc;
using NewsPage.Models.entities;
using NewsPage.Models.RequestDTO;
using NewsPage.Models.ResponseDTO;
using NewsPage.repositories.interfaces;

namespace NewsPage.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IUserDetailRepository _userDetailRepository;

        public ArticleController(IArticleRepository articleRepository, IUserDetailRepository userDetailRepository)
        {
            _articleRepository = articleRepository;
            _userDetailRepository = userDetailRepository;
        }

        [HttpPost]
        public async Task<ActionResult<ArticleDTO>> CreateArticle([FromBody] ArticleCreateDTO articleCreateDTO)
        {
            // Mapping DTO to Article
            var newArticle = new Article
            {
                Title = articleCreateDTO.Title,
                Thumbnail = articleCreateDTO.Thumbnail,
                Content = articleCreateDTO.Content,
                Status = articleCreateDTO.Status,
                UserAccountId = articleCreateDTO.UserAccountId,
                CategoryId = articleCreateDTO.CategoryId
            };

            // Create  article
            var createdArticle = await _articleRepository.CreateAsync(newArticle);

            // Fetch UserDetails
            var userDetails = await _userDetailRepository.GetDetailByAccountID(createdArticle.UserAccountId);
            var articleDTO = MapToArticleDTO(createdArticle, userDetails);

            return CreatedAtAction(nameof(GetArticle), new { id = articleDTO.Id }, articleDTO);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArticleDTO>>> GetAllArticles()
        {
            var articles = await _articleRepository.GetAllAsync();
            var articleDTOs = new List<ArticleDTO>();

            foreach (var article in articles)
            {
                var userDetails = await _userDetailRepository.GetDetailByAccountID(article.UserAccountId);
                articleDTOs.Add(MapToArticleDTO(article, userDetails));
            }

            return Ok(articleDTOs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ArticleDTO>> GetArticle(Guid id)
        {
            var article = await _articleRepository.GetArticleWithCategoryAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            var userDetails = await _userDetailRepository.GetDetailByAccountID(article.UserAccountId);
            var articleDTO = MapToArticleDTO(article, userDetails);

            return Ok(articleDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ArticleDTO>> UpdateArticle(Guid id, Article updatedArticle)
        {
            var existingArticle = await _articleRepository.GetByIdAsync(id);
            if (existingArticle == null)
            {
                return NotFound();
            }

            existingArticle.Title = updatedArticle.Title;
            existingArticle.Thumbnail = updatedArticle.Thumbnail;
            existingArticle.Content = updatedArticle.Content;
            existingArticle.Status = updatedArticle.Status;
            existingArticle.UpdateAt = DateTime.Now;

            await _articleRepository.UpdateAsync(existingArticle);

            var userDetails = await _userDetailRepository.GetDetailByAccountID(existingArticle.UserAccountId);
            var articleDTO = MapToArticleDTO(existingArticle, userDetails);

            return Ok(articleDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(Guid id)
        {
            var article = await _articleRepository.GetByIdAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            await _articleRepository.DeleteAsync(article);

            return NoContent();
        }

        private ArticleDTO MapToArticleDTO(Article article, UserDetails userDetails)
        {
            return new ArticleDTO
            {
                Id = article.Id,
                Title = article.Title ?? string.Empty,
                Thumbnail = article.Thumbnail ?? string.Empty,
                Content = article.Content ?? string.Empty,
                Status = article.Status,
                PublishedAt = article.PublishedAt ?? null,
                UpdateAt = article.UpdateAt ?? null,
                Category = article.Category == null ? null : new CategoryDTO
                {
                    Id = article.Category.Id,
                    Name = article.Category.Name,
                    Topic = article.Category.Topic == null ? null : new Topic
                    {
                        Id = article.Category.Topic.Id,
                        Name = article.Category.Topic.Name
                    }
                },
                UserDetails = userDetails,
                UserAccountEmail = article.UserAccounts?.Email ?? "No email"
            };
        }

    }
}
