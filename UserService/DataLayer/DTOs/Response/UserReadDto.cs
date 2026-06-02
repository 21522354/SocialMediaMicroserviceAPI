namespace UserService.DataLayer.DTOs.Response
{
    public class UserReadDto
    {
        public int UserId { get; set; }
        public string NickName { get; set; } 
        public string FullName { get; set; }
        public string Avatar { get; set; }         
    }
}
