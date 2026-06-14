using Microsoft.AspNetCore.Mvc;
using PostService.Data_Layer.DTOs;
using PostService.Data_Layer.Models;
using PostService.Service;

namespace PostService.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPost()
        {
            return Ok(await _postService.GetAllPosts());
        }

        [HttpGet("{postId}")]
        public async Task<IActionResult> GetPostByPostId(int postId)
        {
            return Ok(await _postService.GetPostByPostId(postId));
        }

        [HttpGet("personalPage/{userId}")]
        public async Task<IActionResult> GetUserPersonalPage(int userId)
        {
            return Ok(await _postService.GetUserPersonalPage(userId));
        }

        [HttpGet("personalPage/reels/{userId}")]
        public async Task<IActionResult> GetPersonalPageReel(int userId)
        {
            return Ok(await _postService.GetPersonalPageReel(userId));
        }

        [HttpGet("personalPage/posts/{userId}")]
        public async Task<IActionResult> GetPersonalPagePost(int userId)
        {
            return Ok(await _postService.GetPersonalPagePost(userId));
        }

        [HttpGet("explore/{userId}")]
        public async Task<IActionResult> GetRandomPost(int userId)
        {
            return Ok(await _postService.GetRandomPost(userId));
        }

        [HttpGet("{postId}/comments")]
        public async Task<IActionResult> GetPostComments(int postId)
        {
            return Ok(await _postService.GetPostComments(postId));
        }

        [HttpGet("hagtags/find/{hagtag}")]
        public async Task<IActionResult> FindPostHagtag(string hagtag)
        {
            return Ok(await _postService.FindPostHagtag(hagtag));
        }

        [HttpGet("{postId}/likes")]
        public async Task<IActionResult> GetPostLikes(int postId)
        {
            return Ok(await _postService.GetPostLikes(postId));
        }

        [HttpGet("user/{userId}/feeds")]
        public async Task<IActionResult> GetUserNewFeeds(int userId)
        {
            return Ok(await _postService.GetUserNewFeeds(userId));
        }

        [HttpGet("hagtag/{hagtag}")]
        public async Task<IActionResult> GetPostByHagtag(string hagtag)
        {
            return Ok(await _postService.GetPostByHagtag(hagtag));
        }

        [HttpPost("markViewed")]
        public async Task<IActionResult> MarkViewed([FromBody] MarkViewedRequest request)
        {
            return Ok(await _postService.MarkViewed(request));
        }

        [HttpPost("likePost")]
        public async Task<IActionResult> LikePost([FromBody] LikePostRequest request)
        {
            return Ok(await _postService.LikePost(request));
        }

        [HttpPost("likeComment")]
        public async Task<IActionResult> LikeComment([FromBody] LikeCommentRequest request)
        {
            return Ok(await _postService.LikeComment(request));
        }

        [HttpDelete("deleteComment/{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            return Ok(await _postService.DeleteComment(commentId));
        }

        [HttpPost("commentPost")]
        public async Task<IActionResult> CommentPost([FromBody] CommentPostRequest request)
        {
            return Ok(await _postService.CommentPost(request));
        }

        [HttpPut("updateComment")]
        public async Task<IActionResult> UpdateComment([FromBody] UpdateCommentRequest request)
        {
            return Ok(await _postService.UpdateComment(request));
        }

        [HttpPost("replyComment")]
        public async Task<IActionResult> ReplyComment([FromBody] ReplyCommentRequest request)
        {
            return Ok(await _postService.ReplyComment(request));
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateNewPost([FromBody] PostCreateRequest request)
        {
            return Ok(await _postService.CreateNewPost(request));
        }

        [HttpPost("create/reel")]
        public async Task<IActionResult> CreateNewReel([FromBody] ReelCreateRequest request)
        {
            return Ok(await _postService.CreateNewReel(request));
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePost([FromBody] PostUpdateRequest request)
        {
            return Ok(await _postService.UpdatePost(request));
        }

        [HttpGet("reels/{userId}")]
        public async Task<IActionResult> GetReels(int userId)
        {
            return Ok(await _postService.GetReels(userId));
        }

        [HttpDelete("{postId}")]
        public async Task<IActionResult> DeletePost(int postId)
        {
            return Ok(await _postService.DeletePost(postId));
        }

        [HttpPost("savePost")]
        public async Task<IActionResult> SavePost([FromBody] SavePostDTO request)
        {
            return Ok(await _postService.SavePost(request));
        }

        [HttpDelete("unSavePost")]
        public async Task<IActionResult> UnSavePost([FromBody] SavePostDTO request)
        {
            return Ok(await _postService.UnSavePost(request));
        }

        [HttpGet("savePost/{userId}")]
        public async Task<IActionResult> GetSavedPost(int userId)
        {
            return Ok(await _postService.GetSavedPost(userId));
        }

        [HttpGet("likePosts/{userId}")]
        public async Task<IActionResult> GetLikesPostForUser(int userId)
        {
            return Ok(await _postService.GetLikesPostForUser(userId));
        }
    }
}
