﻿namespace PostService.Data_Layer.DTOs
{
    public class NotificationMessageDTO
    {
        public Guid UserId { get; set; }
        public string Message { get; set; }
        public Guid PostId { get; set; }
        public Guid CommentId { get; set; }     
        public string EventType { get; set; }   
    }
}