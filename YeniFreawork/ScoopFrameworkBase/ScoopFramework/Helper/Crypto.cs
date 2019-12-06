using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ScoopFramework.Helper
{
    public class Crypto
    {
        private static byte[] _salt = Encoding.ASCII.GetBytes("scoopkey");
        static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes).Replace("=", "_");
        }
        static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }


        public static string Decrypt(string _value, string _key = "<!'^+5%&/(=?_~,|")
        {
            if (_value == null || string.IsNullOrEmpty(_key)) return _value;
            ICryptoTransform ct = null;
            byte[] byt;
            byte[] _result;
            try
            {
                using (var mCSP = new RijndaelManaged())
                {
                    var _keys = new byte[32];
                    Array.Copy(System.Text.Encoding.ASCII.GetBytes(_key), _keys, _key.Length);

                    mCSP.Key = _keys;
                    mCSP.IV = _initVector;
                    ct = mCSP.CreateDecryptor(mCSP.Key, mCSP.IV);


                    byt = Convert.FromBase64String(Base64Decode(_value.ToString()));

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

        private static byte[] _initVector = { 0xE1, 0xF1, 0xA6, 0xBB, 0xA9, 0x5B, 0x31, 0x2F, 0x81, 0x2E, 0x17, 0x4C, 0xA2, 0x81, 0x53, 0x61 };
        public static string Encrypt(string _value, string _key = "<!'^+5%&/(=?_~,|")
        {
            if (_value == null || string.IsNullOrEmpty(_key)) return _value;

            byte[] Value = Encoding.UTF8.GetBytes(_value.ToString());
            using (var mCSP = new RijndaelManaged())
            {
                var _keys = new byte[32];
                Array.Copy(System.Text.Encoding.ASCII.GetBytes(_key), _keys, _key.Length);
                mCSP.Key = _keys;
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
                            return Base64Encode(Convert.ToBase64String(ms.ToArray()));
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Encrypt the given string using AES.  The string can be decrypted using 
        /// DecryptStringAES().  The sharedSecret parameters must match.
        /// </summary>
        /// <param name="plainText">The text to encrypt.</param>
        /// <param name="sharedSecret">A password used to generate a key for encryption.</param>
        /// 
        public static string EncryptStringAES(string plainText, string sharedSecret = "<!'^+5%&/(=?_~,|")
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException("plainText");
            if (string.IsNullOrEmpty(sharedSecret))
                throw new ArgumentNullException("sharedSecret");

            string outStr = null;                       // Encrypted string to return
            RijndaelManaged aesAlg = null;              // RijndaelManaged object used to encrypt the data.

            try
            {
                // generate the key from the shared secret and the salt
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, _salt);

                // Create a RijndaelManaged object
                aesAlg = new RijndaelManaged();
                aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);

                // Create a decryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    // prepend the IV
                    msEncrypt.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
                    msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                    }
                    outStr = Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            // Return the encrypted bytes from the memory stream.
            return outStr;
        }

        /// <summary>
        /// Decrypt the given string.  Assumes the string was encrypted using 
        /// EncryptStringAES(), using an identical sharedSecret.
        /// </summary>
        /// <param name="cipherText">The text to decrypt.</param>
        /// <param name="sharedSecret">A password used to generate a key for decryption.</param>
        public static string DecryptStringAES(string cipherText, string sharedSecret = "<!'^+5%&/(=?_~,|")
        {
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentNullException("cipherText");
            if (string.IsNullOrEmpty(sharedSecret))
                throw new ArgumentNullException("sharedSecret");

            // Declare the RijndaelManaged object
            // used to decrypt the data.
            RijndaelManaged aesAlg = null;

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            try
            {
                // generate the key from the shared secret and the salt
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, _salt);

                // Create the streams used for decryption.                
                byte[] bytes = Convert.FromBase64String(cipherText);
                using (MemoryStream msDecrypt = new MemoryStream(bytes))
                {
                    // Create a RijndaelManaged object
                    // with the specified key and IV.
                    aesAlg = new RijndaelManaged();
                    aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                    // Get the initialization vector from the encrypted stream
                    aesAlg.IV = ReadByteArray(msDecrypt);
                    // Create a decrytor to perform the stream transform.
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            return plaintext;
        }

        private static byte[] ReadByteArray(Stream s)
        {
            byte[] rawLength = new byte[sizeof(int)];
            if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
            {
                throw new SystemException("Stream did not contain properly formatted byte array");
            }

            byte[] buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
            if (s.Read(buffer, 0, buffer.Length) != buffer.Length)
            {
                throw new SystemException("Did not read byte array properly");
            }

            return buffer;
        }

        public string CriptoSession(object item)
        {
            try
            {
                return Encrypt(Newtonsoft.Json.JsonConvert.SerializeObject(item));

            }
            catch (System.Exception ex)
            {
                throw new AggregateException(ex.Message);
            }
        }

        public T DecryptSession<T>(string result)
        {
            try
            {
                return ParseString<T>(Decrypt(result.Replace("_", "=")), "json");

            }
            catch (System.Exception ex)
            {
                throw new AggregateException(ex.Message);
            }
        }

        public T GetPageSecurity<T>(object _criptoData)
        {
            try
            {
                if (_criptoData == null)
                {
                    throw new ArgumentNullException("_criptoData");
                }
                return ParseString<T>(Decrypt(_criptoData.ToString().Replace("_", "=")), "json");

            }
            catch (System.Exception ex)
            {
                throw new AggregateException(ex.Message);
            }
        }

        protected static T ParseString<T>(string str, string format)
        {
            if (format == "xml")
            {
                return (T)new System.Xml.Serialization.XmlSerializer(typeof(T)).Deserialize(new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(str ?? "")));
            }
            else
            {
                return (T)Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str);
            }
        }

    }
}
