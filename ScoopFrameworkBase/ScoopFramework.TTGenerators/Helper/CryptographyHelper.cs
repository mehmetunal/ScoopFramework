using System;
using System.Security.Cryptography;
using System.Text;

namespace ScoopFramework.TTGenerators.Helper
{
    public class CryptographyHelper
    {
        private const string publicKey = "LiX159maR9c0qxrM";
        private const string vectorKey = "Trm58Mrk+0k-Lzx5";

        private readonly ICryptoTransform _decryptor;
        private readonly ICryptoTransform _encryptor;
        private static readonly byte[] IV = Encoding.UTF8.GetBytes(vectorKey);
        private readonly byte[] _password;
        private readonly RijndaelManaged _cipher;
        private ICryptoTransform Decryptor { get { return _decryptor; } }
        private ICryptoTransform Encryptor { get { return _encryptor; } }

        public CryptographyHelper(string password = publicKey)
        {
            var md5 = new MD5CryptoServiceProvider();
            _password = md5.ComputeHash(Encoding.ASCII.GetBytes(password));
            _cipher = new RijndaelManaged();
            _decryptor = _cipher.CreateDecryptor(_password, md5.ComputeHash(IV));
            _encryptor = _cipher.CreateEncryptor(_password, md5.ComputeHash(IV));
        }

        public string Decrypt(string text)
        {
            byte[] input = Convert.FromBase64String(text);

            var newClearData = Decryptor.TransformFinalBlock(input, 0, input.Length);
            return Encoding.UTF8.GetString(newClearData);
        }

        public string Encrypt(string text)
        {
            var buffer = Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(Encryptor.TransformFinalBlock(buffer, 0, buffer.Length));
        }
    }
}
