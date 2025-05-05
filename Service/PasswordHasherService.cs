using Microsoft.AspNetCore.Identity;
using WebApi_Coris.Service.Abstractions;

namespace WebApi_Coris.Service
{
    public class PasswordHasherService : IPasswordHasher
    {
        private readonly PasswordHasher<object> _hasher = new();
        private static readonly object _dummyUser = new();

        public string Hash(string password)
            => _hasher.HashPassword(_dummyUser, password);

        public bool Verify(string hash, string password)
            => _hasher.VerifyHashedPassword(_dummyUser, hash, password)
               == PasswordVerificationResult.Success;
    }

}
