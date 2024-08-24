using Recipedia.Models;
using Recipedia.Repositories;

namespace Recipedia.Services
{
    public class AuthService
    {
        private readonly JwtRepository _jwtRepository;

        public AuthService(JwtRepository jwtRepository)
        {
            _jwtRepository = jwtRepository;
        }

        public string GenerateToken(User user)
        {
            try
            {
                return _jwtRepository.GenerateJwt(user);
            }
            catch (Exception ex)
            {
                // Rethrow the error from GenerateJWT to be handled further upp the chain
                throw new ApplicationException("An error occurred while generating the JWT.", ex);
            }
        }
    }
}
