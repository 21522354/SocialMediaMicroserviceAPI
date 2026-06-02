namespace UserService.DataLayer.DTOs.Request
{
    public class SetNickNameRequest
    {
        public int UserId { get; set; }
        public string NickName { get; set; }        
    }
}
