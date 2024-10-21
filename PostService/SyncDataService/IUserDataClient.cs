﻿using PostService.Data_Layer.DTOs;

namespace PostService.SyncDataService
{
    public interface IUserDataClient
    {
        Task<UserReadDTO> GetUserById(Guid id); 
    }
}