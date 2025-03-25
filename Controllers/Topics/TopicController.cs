using Microsoft.AspNetCore.Mvc;
using NewsPage.Models.entities;
using NewsPage.Models.RequestDTO;
using NewsPage.repositories.interfaces;

namespace NewsPage.Controllers.Topics
{
    [Route("/api/v1/[controller]")]
    [ApiController]
    public class TopicController : ControllerBase
    {
        private readonly ITopicRepository _topicRepository;

        public TopicController(ITopicRepository topicRepository)
        {
            _topicRepository = topicRepository;
        }

        // POST api/topics - Create
        [HttpPost]
        public async Task<IActionResult> CreateTopic([FromBody] TopicCreateDTO topicDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var topic = new Topic
            {
                Id = Guid.NewGuid(),
                Name = topicDto.Name
            };
            var createdTopic = await _topicRepository.AddTopicAsync(topic);
            return CreatedAtAction(nameof(GetTopicById), new { id = createdTopic.Id }, createdTopic);
        }

        // GET api/topics - Get all
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Topic>>> GetAllTopics()
        {
            var topics = await _topicRepository.GetAllTopicsAsync();
            return Ok(topics);
        }

        // GET api/topics/{id} - Get by Id
        [HttpGet("{id}")]
        public async Task<ActionResult<Topic>> GetTopicById(Guid id)
        {
            var topic = await _topicRepository.GetTopicByIdAsync(id);
            if (topic == null)
                return NotFound();

            return Ok(topic);
        }

        // PUT api/topics/{id} - Update
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTopic(Guid id, [FromBody] Topic updatedTopic)
        {
            if (id != updatedTopic.Id)
                return BadRequest();

            var result = await _topicRepository.UpdateTopicAsync(updatedTopic);
            if (result == null)
                return NotFound();

            return NoContent();
        }

        // DELETE api/topics/{id} - Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTopic(Guid id)
        {
            var isDeleted = await _topicRepository.DeleteTopicAsync(id);
            if (!isDeleted)
                return NotFound();

            return NoContent();
        }
    }
}
