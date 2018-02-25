using System;
namespace Sweesh.Core.Abstract.Managers
{
    public interface IHashManager
    {
        string BasicHash(string data);
        string PasswordHash(string data, string salt);
        string GenerateSalt(int length);
        bool VerifyHash(string data, string salt, string hash);
    }
}
