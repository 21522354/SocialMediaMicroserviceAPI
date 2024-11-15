using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostService.AsyncDataService;
using PostService.Data_Layer.DTOs;
using PostService.Data_Layer.Models;
using PostService.Data_Layer.Repository;
using PostService.SyncDataService;
using System.ComponentModel.Design;

namespace PostService.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private IPostCommentRepository _postCommentRepository;
        private readonly IPostLikeRepository _postLikeRepository;
        private readonly IPostMediaRepository _postMediaRepository;
        private readonly IReplyCommentRepository _replyCommentRepository;
        private readonly IUnseenPostRepository _unseenPostRepository;
        private readonly IUserDataClient _userDataClient;
        private readonly IMessageBusClient _messageBusClient;
        private readonly IMapper _mapper;

        public PostController(
            IPostRepository postRepository,
            IPostCommentRepository postCommentRepository,
            IPostLikeRepository postLikeRepository,
            IPostMediaRepository postMediaRepository,
            IReplyCommentRepository replyCommentRepository,
            IUnseenPostRepository unseenPostRepository,
            IUserDataClient userDataClient,
            IMessageBusClient messageBusClient,
            IMapper mapper
            )
        {
            _postRepository = postRepository;
            _postCommentRepository = postCommentRepository;
            _postLikeRepository = postLikeRepository;
            _postMediaRepository = postMediaRepository;
            _replyCommentRepository = replyCommentRepository;
            _unseenPostRepository = unseenPostRepository;
            _userDataClient = userDataClient;
            _messageBusClient = messageBusClient;
            _mapper = mapper;
        }
        [HttpGet("/{postId}")]
        public async Task<IActionResult> GetPostByPostId(Guid postId)
        {
            var post = await _postRepository.GetByIdAsync(postId);
            post.PostComments = (ICollection<PostComment>)await _postCommentRepository.GetCommentsByPostId(post.PostId);
            if (post == null)
            {
                throw new Exception("Can't find this post");
            }
            var user = await _userDataClient.GetUserById(post.UserId);
            var postReadDTO = (post, user).Adapt<PostReadDTO>();

            await _messageBusClient.PublishNewNotification(new NotificationReadDTO() { UserId = Guid.NewGuid(), PostId = postId, Message = "asdfdssffd" });

            return Ok(postReadDTO);
        }
        [HttpGet("/personalPage/{userId}")]
        public async Task<IActionResult> GetUserPersonalPage(Guid userId)
        {
            var listPost = await _postRepository.GetPostsByUserId(userId);
            var user = await _userDataClient.GetUserById(userId);
            // Sử dụng ánh xạ cho từng post trong danh sách
            var listPostReadDTO = listPost.Select(post => (post, user).Adapt<PostReadDTO>()).ToList();
            return Ok(listPostReadDTO);
        }
        [HttpGet("/{postId}/comments")]
        public async Task<IActionResult> GetPostComments(Guid postId)
        {
            var listPostComment = await _postCommentRepository.GetCommentsByPostId(postId);
            var listPostCommentReadDTO = new List<PostCommentReadDTO>();
            foreach (var item in listPostComment)
            {
                var user = await _userDataClient.GetUserById(item.UserId);
                var postCommentReadDTO = new PostCommentReadDTO()
                {
                    CommentId = item.CommentId,
                    UserId = user.UserId,
                    Name = user.Name,
                    Avatar = user.Avatar,
                    Message = item.Message,
                    NumberOfLike = item.NumberOfLike,
                    ReplyComment = await getReplyComment(item.CommentId)
                };
                listPostCommentReadDTO.Add(postCommentReadDTO);
            }
            return Ok(listPostCommentReadDTO);
        }

        private async Task<IEnumerable<PostCommentReadDTO>> getReplyComment(Guid commentId)
        {
            var listReplyComment = await _replyCommentRepository.GetReplyComments(commentId);
            if (listReplyComment.Count() == 0) return new List<PostCommentReadDTO>();
            var listReplyCommentReadDTO = new List<PostCommentReadDTO>();
            foreach (var item in listReplyComment)
            {
                var user = await _userDataClient.GetUserById(item.UserId);
                var replyCommentReadDTO = new PostCommentReadDTO()
                {
                    CommentId = item.ReplyCommentId,
                    UserId = user.UserId,
                    Name = user.Name,
                    Avatar = user.Avatar,
                    Message = item.Message,
                    NumberOfLike = item.NumberOfLike,
                    ReplyComment = await getReplyComment(item.ReplyCommentId)
                };
                listReplyCommentReadDTO.Add(replyCommentReadDTO);
            }
            return listReplyCommentReadDTO;
        }
        [HttpGet("/{postId}/likes")]
        public async Task<IActionResult> GetPostLikes(Guid postId)
        {
            var listPostLike = await _postLikeRepository.GetLikeForPost(postId);
            var listPostLikeReadDTO = new List<PostLikeReadDTO>();
            foreach (var item in listPostLike)
            {
                var user = await _userDataClient.GetUserById(item.UserId);
                var postLikeReadDTO = _mapper.Map<PostLikeReadDTO>(user);
                listPostLikeReadDTO.Add(postLikeReadDTO);
            }
            return Ok(listPostLikeReadDTO);
        }
        [HttpGet("/user/{userId}/feeds")]
        public async Task<IActionResult> GetUserNewFeeds(Guid userId)
        {
            var listUnSeenPost = await _unseenPostRepository.GetUnseenPostByUserId(userId);
            var feeds = new List<PostReadDTO>();        
            foreach (var item in listUnSeenPost)
            {
                var post = await _postRepository.GetByIdAsync(item.PostId);
                if (post == null)
                {
                    throw new Exception("Can't find this post with id = " + item.PostId);
                }
                var user = await _userDataClient.GetUserById(post.UserId);
                var postReadDTO = (post, user).Adapt<PostReadDTO>();
                feeds.Add(postReadDTO);
            }
            return Ok(feeds);
        }
        [HttpPost("/likePost")]
        public async Task<IActionResult> LikePost([FromBody] LikePostRequest request)
        {
            var post = await _postRepository.GetByIdAsync(request.PostId);
            if (post == null)
            {
                throw new Exception("Can't find this post");
            }
            var user = await _userDataClient.GetUserById(request.UserId);
            var postLike = await _postLikeRepository.FindPostLike(request.PostId, request.UserId);
            if (postLike != null)
            {
                await _postLikeRepository.DeletePostLikeAsync(postLike);
                return Ok("Unlike post successfully");
            }
            postLike = new PostLike()
            {
                UserId = request.UserId,
                PostId = request.PostId,
            };
            await _postLikeRepository.AddPostLikeAsync(postLike);
            return Ok("Like post successfully");
        }
        [HttpPost("/likeComment")]
        public async Task<IActionResult> LikeComment([FromBody] LikeCommentRequest request)
        {
            await _postCommentRepository.LikeComment(request.CommentId);
            return Ok("Like comment successfully");
        }
        [HttpPost("/commentPost")]
        public async Task<IActionResult> CommentPost([FromBody] CommentPostRequest request)
        {
            var post = await _postRepository.GetByIdAsync(request.PostId);
            if (post == null)
            {
                throw new Exception("Can't find this post");
            }
            var user = await _userDataClient.GetUserById(request.UserId);
            var postComment = new PostComment()
            {
                CommentId = Guid.NewGuid(),
                UserId = request.UserId,
                PostId = request.PostId,
                Message = request.Message,
                NumberOfLike = 0
            };
            await _postCommentRepository.CreateAsync(postComment);
            return Ok("Comment post successfully");
        }
        [HttpPost("/replyComment")]
        public async Task<IActionResult> ReplyComment([FromBody] ReplyCommentRequest request)
        {
            var postcomment = await _postCommentRepository.GetByIdAsync(request.CommentId);
            var replyComment = await _replyCommentRepository.GetByIdAsync(request.CommentId);
            if(postcomment == null && replyComment == null)
            {
                return BadRequest("Can't find this comment");
            }
            var newReplyComment = new ReplyComment()
            {
                ReplyCommentId = Guid.NewGuid(),
                UserId = request.UserId,
                CommentId = request.CommentId,
                Message = request.Message,
                NumberOfLike = 0
            };
            await _replyCommentRepository.CreateAsync(newReplyComment);
            return Ok("Reply comment successfully");
        }
        [HttpPost("/create")]
        public async Task<IActionResult> CreateNewPost([FromBody] PostCreateRequest request)
        {
            var listUserFollower = await _userDataClient.GetUserFollower(request.UserId);
            var post = new Post()
            {
                PostId = Guid.NewGuid(),
                UserId = request.UserId,
                PostTitle = request.PostTitle,
                CreatedDate = DateTime.UtcNow,
                NumberOfShare = 0,
            };
            await _postRepository.CreateAsync(post);
            int i = 1;
            foreach (var item in request.ImageAndVideo)
            {
                var postMedia = new PostMedia()
                {
                    Link = item,
                    STT = i,
                    PostId = post.PostId,
                };
                await _postMediaRepository.CreateAsync(postMedia);
                i++;
            }

            foreach (var item in listUserFollower)
            {
                var unSeenPost = new UnseenPost()
                {
                    PostId = post.PostId,
                    UserId = item.UserId,
                    IsAlreadySeen = false
                };
                await _unseenPostRepository.CreateAsync(unSeenPost);
            }
            return Ok(post.PostId);
        }
    }
}
