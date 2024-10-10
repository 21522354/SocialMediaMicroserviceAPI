namespace UserService.DataLayer.DTOs
{
    public class ChangePasswordRequest
    {
        public Guid UserId { get; set; }
        public string NewPassword { get; set; }     
    }
}
