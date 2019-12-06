using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ScoopFramework.TTGenerators.Helper
{
    public static class Crypto
    {

        private static byte[] _initVector = { 0xE1, 0xF1, 0xA6, 0xBB, 0xA9, 0x5B, 0x31, 0x2F, 0x81, 0x2E, 0x17, 0x4C, 0xA2, 0x81, 0x53, 0x61 };

        public static string Decrypt(string _value, string _key = "Scoop")
        {
            if (_value == null || string.IsNullOrEmpty(_key) ) return _value;
            ICryptoTransform ct = null;
            byte[] byt;
            byte[] _result;
            try
            {
                using (var mCSP = new RijndaelManaged())
                {
                    mCSP.Key = System.Text.Encoding.ASCII.GetBytes(_key);
                    mCSP.IV = _initVector;
                    ct = mCSP.CreateDecryptor(mCSP.Key, mCSP.IV);


                    byt = Convert.FromBase64String(_value.ToString());

                    using (var ms = new MemoryStream())
                    {
                        using (var cs = new CryptoStream(ms, ct, CryptoStreamMode.Write))
                        {
                            cs.Write(byt, 0, byt.Length);
                            cs.FlushFinalBlock();

                            cs.Close();
                            _result = ms.ToArray();
                        }
                    }
                }
            }
            catch
            {
                _result = null;
            }
            finally
            {
                if (ct != null)
                    ct.Dispose();
            }

            return ASCIIEncoding.UTF8.GetString(_result);
        }

        public static string Encrypt(string _value, string _key = "Scoop")
        {
            if (_value == null || string.IsNullOrEmpty(_key)) return _value;

            byte[] Value = Encoding.UTF8.GetBytes(_value.ToString());
            using (var mCSP = new RijndaelManaged())
            {

                mCSP.Key = System.Text.Encoding.ASCII.GetBytes(_key);
                mCSP.IV = _initVector;
                using (var ct = mCSP.CreateEncryptor(mCSP.Key, mCSP.IV))
                {
                    using (var ms = new MemoryStream())
                    {
                        using (var cs = new CryptoStream(ms, ct, CryptoStreamMode.Write))
                        {
                            cs.Write(Value, 0, Value.Length);
                            cs.FlushFinalBlock();
                            cs.Close();
                            return Convert.ToBase64String(ms.ToArray());
                        }
                    }
                }
            }
        }
    }
}
