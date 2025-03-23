using NewsPage.data;
using NewsPage.Models.entities;
using NewsPage.repositories.interfaces;

namespace NewsPage.repositories
{
    public class UserDetailRepository : IUserDetailRepository
    {
        private readonly ApplicationDbContext _context;
        public UserDetailRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<UserDetails>  CreateInfo(string FullName, string sex, DateTime Birthday, Guid userAccountId)
        {
            Guid id = Guid.NewGuid();
            var userInfo = new UserDetails { Id = id 
                , FullName = FullName, 
                Sex = sex, 
                Birthday = Birthday, 
                UserAccountId = userAccountId, 
                Avatar = "default_avatar.jpg"
            };
            await _context.UserDetails.AddAsync(userInfo);
            await _context.SaveChangesAsync();
            return userInfo;

        }
    }
}
