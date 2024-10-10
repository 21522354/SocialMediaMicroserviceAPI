namespace UserService.DataLayer.DTOs
{
    public class UserReadDto
    {
        public string Name { get; set; }
        public string Avatar { get; set; }
        public int UserFollowing { get; set; }
        public int UserFollower { get; set; }           
    }
}
