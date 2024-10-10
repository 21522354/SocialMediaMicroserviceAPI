namespace UserService.DataLayer.DTOs
{
    public class SetNameAndAvatarRequest
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }      
    }
}
