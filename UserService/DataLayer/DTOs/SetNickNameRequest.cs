namespace UserService.DataLayer.DTOs
{
    public class SetNickNameRequest
    {
        public Guid UserId { get; set; }
        public string NickName { get; set; }        
    }
}
