namespace LocalStore.Application.Requests
{
    public class LoginRequest
    {
        public string UserName { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
    }
}
