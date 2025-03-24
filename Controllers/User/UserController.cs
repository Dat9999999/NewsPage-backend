using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsPage.Models;
using NewsPage.repositories.interfaces;
using System.Security.Claims;

namespace NewsPage.Controllers.User
{
    [Route("/api/v1/[controller]")]
    public class UserController: ControllerBase
    {
        private readonly IUserDetailRepository _userDetailRepository;
        private readonly IUserAccountRepository _userAccountRepository;
        public UserController(IUserDetailRepository userDetailRepository, IUserAccountRepository userAccountRepository)
        {
            _userDetailRepository = userDetailRepository;
            _userAccountRepository = userAccountRepository;
        }


        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> Profile(Guid accountId)
        {
            try
            {
                var userDetails = await _userDetailRepository.GetDetailByAccountID(accountId);
                if (userDetails == null)
                {
                    return NotFound(new { message = "Người dùng không tồn tại" });
                }
                return Ok(userDetails);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new {message = "Lỗi hệ thống khi tải thông tin người dùng"});
            }
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> updatePrfile(Guid AccountId, [FromBody] UpdateProfileDTO updateProfileDTO)
        {

            try
            {
                var userAccount = await _userAccountRepository.GetById(AccountId);
                if (userAccount == null)
                {
                    return NotFound(new {message = "Người dùng không tồn tại"});
                }
                var userEmailFromToken = User.FindFirst(ClaimTypes.Name)?.Value;
                var roleFromToken = User.FindFirst(ClaimTypes.Role)?.Value;
                Console.WriteLine(userEmailFromToken);
                Console.WriteLine(userAccount.Email);
                if (roleFromToken != "Admin" && userEmailFromToken != userAccount.Email)
                {
                    return BadRequest(new { message = "Bạn không có quyền chỉnh sửa đối với người dùng này" });
                }

                await _userDetailRepository.UpdateProfile(userAccount.Id, updateProfileDTO);

                return Ok(new {success = true, data = updateProfileDTO });
            }
            catch (Exception e){
                Console.WriteLine(e);
                return StatusCode(500, new { message = "Đã xảy ra lỗi hệ thống khi thực hiện chỉnh sửa thông tin người dùng. Vui lòng thử lại sau!" });
            }
        }

    }
}
