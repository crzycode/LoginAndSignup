using System.Security.Cryptography;

namespace LoginAndSignup.Helper
{
    public class Passwordhash
    {
        private static RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
        private static readonly int saltsize = 16;
        private static readonly int hashsize = 20;
        private static readonly int iterations = 10000;

        public static string hashpassword(string password)
        {
            byte[] salt;
            rng.GetBytes(salt = new byte[saltsize]);
            var key = new Rfc2898DeriveBytes(password, salt, iterations);
            var hash = key.GetBytes(hashsize);

            var hashbyte = new byte[saltsize+hashsize];
            Array.Copy(salt,0,hashbyte,0,saltsize);
            Array.Copy(hash,0,hashbyte,saltsize,hashsize);
            var bash64hash = Convert.ToBase64String(hashbyte);
            return bash64hash;

        }
        public static bool verifypassword(string password, string bash64hash)
        {
            var hashbytes = Convert.FromBase64String(bash64hash);
            var salt = new byte[saltsize];
            Array.Copy(hashbytes,0,salt,0,saltsize);
            var key = new Rfc2898DeriveBytes(password, salt, iterations);
            byte[] hash = key.GetBytes(hashsize);
            for (int i = 0; i < hashsize; i++)
            {
                if (hashbytes[i + saltsize] != hash[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
