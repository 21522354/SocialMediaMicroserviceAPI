namespace UserService.DataLayer.DTOs
{
    public class FollowUserRequest
    {
        public Guid SelfId { get; set; }
        public Guid UserFollowId { get; set; }      
    }
}
