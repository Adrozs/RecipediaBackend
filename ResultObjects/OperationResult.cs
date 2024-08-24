namespace Recipedia.ResultObjects
{
    public class OperationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }

        public OperationResult() { }

        public OperationResult(bool success, string message, int statusCode = 200)
        {
            Success = success;
            Message = message;
            StatusCode = statusCode;
        }

        public static OperationResult Failed(string message, int statusCode = 400) =>
            new OperationResult { Success = false, Message = message, StatusCode = statusCode };

        public static OperationResult Successful(string message, int statusCode = 200) =>
            new OperationResult { Success = true, Message = message, StatusCode = statusCode };
        
    }
}
