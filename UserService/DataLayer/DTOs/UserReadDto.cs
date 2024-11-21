namespace UserService.DataLayer.DTOs
{
    public class UserReadDto
    {
        public Guid UserId { get; set; }    
        public string Name { get; set; }
        public string Avatar { get; set; }         
    }
}
