using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostService.Data_Layer;

namespace PostService.Controllers
{
    [Route("api/GetDataForTest")]
    [ApiController]
    public class GetDataForTestController : ControllerBase
    {
        private readonly PostServiceDBContext _context;

        public GetDataForTestController(PostServiceDBContext context)
        {
            _context = context;
        }
        [HttpGet("GetAllPost")]
        public IActionResult GetAllPost()
        {
            return Ok(_context.Posts.ToList());
        }
        [HttpGet("GetAllPostComment")]
        public IActionResult GetAllPostComment()
        {
            return Ok(_context.PostComments.ToList());
        }
        [HttpGet("GetAllPostLike")]
        public IActionResult GetAllPostLike()
        {
            return Ok(_context.PostLikes.ToList());
        }
        [HttpGet("GetAllPostMedia")]
        public IActionResult GetAllPostMedia()
        {
            return Ok(_context.PostMedias.ToList());
        }
        [HttpGet("GetAllReplyComment")]
        public IActionResult GetAllReplyComment()
        {
            return Ok(_context.ReplyComments.ToList());
        }
        [HttpGet("GetAllUnseenPost")]
        public IActionResult GetAllUnseenPost()
        {
            return Ok(_context.UnseenPosts.ToList());
        }
    }
}
