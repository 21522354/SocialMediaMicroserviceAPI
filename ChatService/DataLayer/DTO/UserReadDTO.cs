﻿namespace ChatService.DataLayer.DTO
{
    public class UserReadDTO
    {
        public Guid UserId { get; set; }
        public string NickName { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }
    }
}
