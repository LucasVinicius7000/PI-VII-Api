namespace LocalStore.Application.Responses
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public string Role { get; set; }
        public Boolean IsApproved { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Id { get; set; }

    }
}
