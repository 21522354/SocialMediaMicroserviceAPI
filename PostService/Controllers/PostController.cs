﻿using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
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
        private readonly IPostHagtagRepository _postHagtagRepository;
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
            IPostHagtagRepository postHagtagRepository,
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
            _postHagtagRepository = postHagtagRepository;
            _userDataClient = userDataClient;
            _messageBusClient = messageBusClient;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPost()
        {
            return Ok(await _postRepository.GetAllPostAsync());
        }
        [HttpGet("{postId}")]
        public async Task<IActionResult> GetPostByPostId(Guid postId)
        {
            var post = await _postRepository.GetByIdAsync(postId);
            if (post == null)
            {
                return NotFound("Can't find this post");
            }
            post.PostComments = (ICollection<PostComment>)await _postCommentRepository.GetCommentsByPostId(post.PostId);
            var user = await _userDataClient.GetUserById(post.UserId);
            var postReadDTO = (post, user).Adapt<PostReadDTO>();
            return Ok(postReadDTO);
        }
        [HttpGet("personalPage/{userId}")]
        public async Task<IActionResult> GetUserPersonalPage(Guid userId)
        {
            var listPost = await _postRepository.GetPostsByUserId(userId);
            var user = await _userDataClient.GetUserById(userId);
            // Sử dụng ánh xạ cho từng post trong danh sách
            var listPostReadDTO = listPost.Select(post => (post, user).Adapt<PostReadDTO>()).ToList();
            return Ok(listPostReadDTO);
        }
        [HttpGet("explore/{userId}")]
        public async Task<IActionResult> GetRandomPost(Guid userId)
        {
            var listPost = await _postRepository.GetRandomPost(userId);
            var listPostReadDTO = new List<PostReadDTO>();
            foreach (var post in listPost)
            {
                var user = await _userDataClient.GetUserById(post.UserId);
                var postReadDTO = (post, user).Adapt<PostReadDTO>();
                listPostReadDTO.Add(postReadDTO);
            }
            return Ok(listPostReadDTO);
        }
        [HttpGet("{postId}/comments")]
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
                    Name = user.NickName,
                    Avatar = user.Avatar,
                    Message = item.Message,
                    NumberOfLike = item.NumberOfLike,
                    ReplyComment = await getReplyComment(item.CommentId)
                };
                listPostCommentReadDTO.Add(postCommentReadDTO);
            }
            return Ok(listPostCommentReadDTO);
        }
        [HttpGet("hagtags/find/{hagtag}")]
        public async Task<IActionResult> FindPostHagtag(string hagtag)
        {
            var listHagtagRelated = await _postHagtagRepository.GetRelatedHagtag(hagtag);   
            var listRelatedPostHagtagReadDTO = new List<RelatedPostHagtagReadDTO>();
            foreach (var item in listHagtagRelated)
            {
                var listPostByHagtag = await _postHagtagRepository.GetPostHagtagsByName(item.HagtagName);
                var listPost = listPostByHagtag.Select(x => x.Post).ToList();
                var relatedPostHagtagReadDTO = new RelatedPostHagtagReadDTO()
                {
                    HagtagName = item.HagtagName,
                    Total = listPost.Count
                };
                listRelatedPostHagtagReadDTO.Add(relatedPostHagtagReadDTO);
            }
            return Ok(listRelatedPostHagtagReadDTO);
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
                    Name = user.NickName,
                    Avatar = user.Avatar,
                    Message = item.Message,
                    NumberOfLike = item.NumberOfLike,
                    ReplyComment = await getReplyComment(item.ReplyCommentId)
                };
                listReplyCommentReadDTO.Add(replyCommentReadDTO);
            }
            return listReplyCommentReadDTO;
        }
        [HttpGet("{postId}/likes")]
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
        [HttpGet("user/{userId}/feeds")]
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
        [HttpGet("hagtag/{hagtag}")]
        public async Task<IActionResult> GetPostByHagtag(string hagtag)
        {
            var listPostByHagtag = await _postHagtagRepository.GetPostHagtagsByName(hagtag);
            var listPost = listPostByHagtag.Select(x => x.Post).ToList();
            var listPostReadDTO = new List<PostReadDTO>();
            foreach (var post in listPost)
            {
                var user = await _userDataClient.GetUserById(post.UserId);
                var postReadDTO = (post, user).Adapt<PostReadDTO>();
                listPostReadDTO.Add(postReadDTO);
            }
            return Ok(listPostReadDTO);
        }
        [HttpPost("markViewed")]
        public async Task<IActionResult> MarkViewed([FromBody] MarkViewedRequest markViewedRequest)
        {
            var unseenPost = await _unseenPostRepository.GetByBothId(markViewedRequest.PostId, markViewedRequest.UserId);   
            if (unseenPost == null)
            {
                return BadRequest();
            }
            await _unseenPostRepository.DeleteAsync(unseenPost);
            return Ok("Mark viewed successfully");
        }
        [HttpPost("likePost")]
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

            await _messageBusClient.PublishNewNotification(new NotificationMessageDTO() {
                UserId = post.UserId,
                UserInvoke = user.UserId,
                PostId = post.PostId,
                Message = $"{user.NickName} liked your post",
                EventType = "LikePost" });

            return Ok("Like post successfully");
        }
        [HttpPost("likeComment")]
        public async Task<IActionResult> LikeComment([FromBody] LikeCommentRequest request)
        {
            await _postCommentRepository.LikeComment(request.CommentId);

            return Ok("Like comment successfully");
        }
        [HttpPost("commentPost")]
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

            await _messageBusClient.PublishNewNotification(new NotificationMessageDTO() {
                UserId = post.UserId,
                UserInvoke = user.UserId,
                PostId = post.PostId,
                CommentId = postComment.CommentId,
                Message = $"{user.NickName} commented on your post",
                CheckTag = postComment.Message,
                EventType = "CommentPost" });

            return Ok("Comment post successfully");
        }
        [HttpPost("replyComment")]
        public async Task<IActionResult> ReplyComment([FromBody] ReplyCommentRequest request)
        {
            var postcomment = await _postCommentRepository.GetByIdAsync(request.CommentId);
            var replyComment = await _replyCommentRepository.GetByIdAsync(request.CommentId);
            var user = await _userDataClient.GetUserById(request.UserId);
            if(postcomment == null && replyComment == null)
            {
                return BadRequest("Can't find this comment");
            }
            var commentId = postcomment != null ? postcomment.CommentId : replyComment.CommentId;
            var userId = postcomment != null ? postcomment.UserId : replyComment.UserId;
            var newReplyComment = new ReplyComment()
            {
                ReplyCommentId = Guid.NewGuid(),
                UserId = request.UserId,
                CommentId = request.CommentId,
                PostId = request.PostId,
                Message = request.Message,
                NumberOfLike = 0
            };
            await _replyCommentRepository.CreateAsync(newReplyComment);

            await _messageBusClient.PublishNewNotification(new NotificationMessageDTO() { 
                UserId = userId,
                UserInvoke = user.UserId,
                PostId = request.PostId,
                CommentId = commentId, 
                Message = $"{user.NickName} responded to your comment",
                CheckTag = newReplyComment.Message,
                EventType = "ReplyComment" });

            return Ok("Reply comment successfully");
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateNewPost([FromBody] PostCreateRequest request)
        {
            var listUserFollower = await _userDataClient.GetUserFollower(request.UserId);
            var user = await _userDataClient.GetUserById(request.UserId);
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

            foreach (var item in request.ListHagtag)
            {
                var postHagtag = new PostHagtag()
                {
                    HagtagName = item,
                    PostId = post.PostId
                };
                await _postHagtagRepository.AddAsync(postHagtag);
            }

            foreach (var item in listUserFollower)
            {
                var unSeenPost = new UnseenPost()
                {
                    PostId = post.PostId,
                    UserId = item.UserId,
                };
                await _unseenPostRepository.CreateAsync(unSeenPost);
            }

            await _messageBusClient.PublishNewNotification(new NotificationMessageDTO() { 
                UserInvoke = post.UserId,
                PostId = post.PostId,
                Message = $"{user.NickName} created a new post",
                CheckTag = post.PostTitle,
                EventType = "NewPost" });

            return Ok(new {postId = post.PostId});
        }
    }
}
