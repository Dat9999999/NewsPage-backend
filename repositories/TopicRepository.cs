using Microsoft.EntityFrameworkCore;
using NewsPage.data;
using NewsPage.Models.entities;
using NewsPage.repositories.interfaces;

namespace NewsPage.repositories
{
    public class TopicRepository : ITopicRepository
    {
        private readonly ApplicationDbContext _context;

        public TopicRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Topic>> GetAllTopicsAsync()
        {
            return await _context.Topics.ToListAsync();
        }

        public async Task<Topic?> GetTopicByIdAsync(Guid id)
        {
            return await _context.Topics.FindAsync(id);
        }

        public async Task<Topic> AddTopicAsync(Topic topic)
        {
            _context.Topics.Add(topic);
            await _context.SaveChangesAsync();
            return topic;
        }

        public async Task<Topic?> UpdateTopicAsync(Topic updatedTopic)
        {
            var existingTopic = await _context.Topics.FindAsync(updatedTopic.Id);
            if (existingTopic == null) return null;

            existingTopic.Name = updatedTopic.Name;
            await _context.SaveChangesAsync();
            return existingTopic;
        }

        public async Task<bool> DeleteTopicAsync(Guid id)
        {
            var topic = await _context.Topics.FindAsync(id);
            if (topic == null) return false;

            _context.Topics.Remove(topic);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
