using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NewsPage.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]

    public class AdvertiseController : ControllerBase{
        [HttpPost]
        public IActionResult CreateAdvertise(){
            return Ok("Create successfully");
        }
    }

}