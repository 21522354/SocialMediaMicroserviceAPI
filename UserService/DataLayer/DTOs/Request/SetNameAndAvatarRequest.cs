namespace UserService.DataLayer.DTOs.Request
{
    public class SetNameAndAvatarRequest
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }      
    }
}
