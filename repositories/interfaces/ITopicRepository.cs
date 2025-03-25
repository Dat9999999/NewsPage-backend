using NewsPage.Models.entities;

namespace NewsPage.repositories.interfaces
{
    public interface ITopicRepository
    {
        Task<IEnumerable<Topic>> GetAllTopicsAsync();
        Task<Topic?> GetTopicByIdAsync(Guid id);
        Task<Topic> AddTopicAsync(Topic topic);
        Task<Topic?> UpdateTopicAsync(Topic updatedTopic);
        Task<bool> DeleteTopicAsync(Guid id);
    }
}
