namespace Recipedia.Exceptions
{
    public class UserNotFoundException : ApiException
    {
        public override int StatusCode => StatusCodes.Status404NotFound;
        public UserNotFoundException(string message) : base(message) { }
    }
}
