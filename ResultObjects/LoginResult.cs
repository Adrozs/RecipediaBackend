namespace Recipedia.ResultObjects
{
    public class LoginResult
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }

        public static LoginResult Failed(string message) => new LoginResult { Success = false, Message = message };
        public static LoginResult Successful(string token) => new LoginResult { Success = true, Token = token };
    }
}
