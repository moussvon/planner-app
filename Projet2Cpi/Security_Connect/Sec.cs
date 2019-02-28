using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Net.Sockets;
using System.Numerics;
using System.Windows.Forms;

namespace Projet2Cpi
{

    public class Sec
    {

        public const int ALPHA = 1;
        public const int ALPHANUM = 2;
        public const int PRINTABLE = 3;
        private const String ned_constant = "nekess_ya_Nedjima!";
        private static String pepper = decrypt("tKtPrTTDZXiOEr0ftJQyIv1MJIW4gU5MS5m4CMgROzo=$kJAimQgOqbM87df+4KfWVw==$bXiwfqMgyvB0kMdleHUKH94UmME=", ned_constant);

        private static BigInteger P = new BigInteger(new byte[] {  0xb, 0x3d, 0xc, 0xce, 0x89, 0x94, 0xa3, 0x7a, 0xbe, 0x7f, 0x25, 0xf3, 0xdc, 0x1d, 0x5d, 0xac, 0xc0, 0x17, 0x24, 0x8c
, 0x4b, 0xd8, 0x6f, 0xbc, 0x38, 0xb9, 0x46, 0x41, 0xb1, 0x3, 0xfa, 0x9, 0x40, 0x43, 0x33, 0x4a, 0xe7, 0xcf, 0x8c, 0x75,
0x6, 0xb5, 0xfc, 0xc0, 0xff, 0x30, 0xe4, 0x9d, 0x2f, 0x66, 0x1b, 0xe3, 0x56, 0x7a, 0x6e, 0x2b, 0xc3, 0xca, 0xa5, 0xf7,
0x88, 0x7e, 0xe8, 0x1d, 0xb5, 0xbf, 0xce, 0x4, 0xdd, 0x27, 0xc5, 0xe6, 0xc5, 0xe4, 0xa0, 0xd5, 0x77, 0x6d, 0x91, 0xfb,
0x63, 0x46, 0xef, 0x33, 0xb5, 0x7e, 0x38, 0x19, 0xe7, 0xa9, 0x55, 0xc9, 0xc3, 0x97, 0x6, 0xae, 0x6e, 0x2c, 0xb8, 0xc5,
0x2a, 0x98, 0xf7, 0xa2, 0x2c, 0xc6, 0x6c, 0xef, 0xdb, 0x3c, 0x64, 0x54, 0x90, 0x4e, 0xbd, 0xbf, 0xad, 0x3c, 0x95, 0xb7,
0x28, 0xfe, 0x0, 0xc8, 0x26, 0x94, 0x1a, 0xfc, 00 });

        private static BigInteger G = new BigInteger(2);
        public static byte[] dh_secret;
        public static String checksum(String data)
        {
            // BASE64(SHA1(MD5("<red>"+input+"</red>")))
            SHA1Managed sha = new SHA1Managed();
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] input = Encoding.ASCII.GetBytes("<nekess_nedjima>" + data + "</nekess_nedjima>");
            return base64_encode(sha.ComputeHash(md5.ComputeHash(input)));
        }

        public static String checksum(byte[] data)
        {
            // BASE64(SHA1(MD5("<red>"+input+"</red>")))
            SHA1Managed sha = new SHA1Managed();
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] input = Encoding.ASCII.GetBytes("<nekess_nedjima>" + data + "</nekess_nedjima>");
            return base64_encode(sha.ComputeHash(md5.ComputeHash(input)));
        }

        public static String encrypt(String data, String key)
        {
            try
            {
                if (key.Length > 32)
                {
                    throw new System.Security.Cryptography.CryptographicException("Key can't be longer than 32 characters");
                }
                key = key.PadRight(32);
                AesManaged aes = new AesManaged();
                //aes.BlockSize = 256;
                aes.Key = Encoding.ASCII.GetBytes(key);
                aes.Mode = CipherMode.CBC;
                aes.GenerateIV();
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                byte[] encrypted;
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(data);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
                String main_enc_data = base64_encode(encrypted);
                String enc_iv = base64_encode(aes.IV);
                return main_enc_data + "$" + enc_iv + "$" + checksum(main_enc_data + enc_iv);
            }
            catch (System.Security.Cryptography.CryptographicException e)
            {
                return "a Cryptographic exception has occured : " + e.Message;
            }
            catch (System.Exception e)
            {
                return "an exception has occured : " + e.Message;
            }
        }

        public static String encrypt(String data, byte[] key)
        {
            try
            {
                byte[] padded_key = new byte[32];
                if (key.Length > 32)
                    throw new System.Security.Cryptography.CryptographicException("Key can't be longer than 32 characters");
                else if (key.Length < 32)
                    Array.Copy(key, padded_key, key.Length);
                else
                    padded_key = key;
                AesManaged aes = new AesManaged();
                aes.Key = padded_key;
                aes.Mode = CipherMode.CBC;
                aes.GenerateIV();
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                byte[] encrypted;
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(data);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
                String main_enc_data = base64_encode(encrypted);
                String enc_iv = base64_encode(aes.IV);
                //MessageBox.Show("iv = " + hexify(aes.IV) + "\nkey = " + hexify(aes.Key) + "\ndata = " + hexify(Encoding.ASCII.GetBytes(data)));
                return main_enc_data + "$" + enc_iv + "$" + checksum(main_enc_data + enc_iv);
            }
            catch (System.Security.Cryptography.CryptographicException e)
            {
                return "a Cryptographic exception has occured : " + e.Message;
            }
            catch (System.Exception e)
            {
                return "an exception has occured : " + e.Message;
            }
        }

        public static String base64_encode(byte[] e)
        {
            return remove(System.Convert.ToBase64String(e), '\n');
        }

        public static String hexify(byte[] s)
        {
            StringBuilder hex = new StringBuilder(s.Length);
            foreach (byte b in s)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();

        }
        public static String decrypt(String raw_data, String key)
        {
            try
            {
                if (key.Length > 32)
                {
                    throw new System.Security.Cryptography.CryptographicException("Key can't be longer than 32 characters");
                }
                key = key.PadRight(32);
                String[] results = raw_data.Split('$');
                String chksum = results[2];
                if (checksum(results[0] + results[1]) != chksum)
                {
                    throw new System.Security.Cryptography.CryptographicException("File integrity check failed!");
                }
                byte[] data = System.Convert.FromBase64String(results[0]);
                byte[] iv = System.Convert.FromBase64String(results[1]);
                RijndaelManaged aes = new RijndaelManaged();
                aes.Key = Encoding.ASCII.GetBytes(key);
                aes.Mode = CipherMode.CBC;
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                String plain;
                using (MemoryStream msDecrypt = new MemoryStream(data))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plain = srDecrypt.ReadToEnd();
                        }
                    }
                }
                return plain;
            }
            catch (System.Security.Cryptography.CryptographicException e)
            {
                return "A Cryptographic exception has occured in decrypt : " + e.Message;
            }
            catch (System.Exception e)
            {
                return "an exception has occured : " + e.Message;
            }
        }

        public static String decrypt(String raw_data, byte[] key)
        {
            try
            {
                byte[] padded_key = new byte[32];
                if (key.Length > 32)
                    throw new System.Security.Cryptography.CryptographicException("Key can't be longer than 32 characters");
                else if (key.Length < 32)
                    Array.Copy(key, padded_key, key.Length);
                else
                    padded_key = key;
                String[] results = raw_data.Split('$');
                String chksum = results[2];
                if (checksum(results[0] + results[1]) != chksum)
                {
                    throw new System.Security.Cryptography.CryptographicException("File integrity check failed!");
                }
                byte[] data = System.Convert.FromBase64String(results[0]);
                byte[] iv = System.Convert.FromBase64String(results[1]);
                AesManaged aes = new AesManaged();
                aes.Key = padded_key;
                aes.Mode = CipherMode.CBC;
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                String plain;
                using (MemoryStream msDecrypt = new MemoryStream(data))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plain = srDecrypt.ReadToEnd();
                        }
                    }
                }
                return plain;
            }
            catch (System.Security.Cryptography.CryptographicException e)
            {
                return "A Cryptographic exception has occured in decrypt : " + e.Message;
            }
            catch (System.Exception e)
            {
                return "an exception has occured : " + e.Message;
            }
        }

        private static String getSalt(int len = 32)
        {
            byte[] salt = new byte[len];
            using (RNGCryptoServiceProvider random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }
            return base64_encode(salt);
        }

        public static String hash(String data)
        {
            String salt = getSalt();
            return hash(data, salt);
        }

        public static String hash(String data, String salt)
        {
            String mysalt = Encoding.ASCII.GetString(System.Convert.FromBase64String(salt));
            SHA256Managed sha = new SHA256Managed();
            byte[] input = Encoding.ASCII.GetBytes(data + salt + Sec.pepper);
            return base64_encode(sha.ComputeHash(input)) + '$' + salt;
        }

        public static bool hash_and_compare(String pass_plain, String resulting_hash)
        {
            // resulting_hash is the previously computed hash
            String[] res = resulting_hash.Split('$');
            return hash(pass_plain, res[1]) == resulting_hash;
        }

        public static byte[] positive(byte[] x)
        {
            byte[] positive = new byte[x.Length + 1];
            Array.Copy(x, positive, x.Length);
            return positive;
        }
        public static void dh(NetworkStream s, StreamReader r, StreamWriter w)
        {
            try
            {

                // Diffie hellman key exchange on GF(P)
                // genererate 
                byte[] rand = positive(Convert.FromBase64String(getSalt(128)));
                BigInteger a = new BigInteger(rand);
                // send g**secret to the server
                BigInteger g_a = BigInteger.ModPow(G, a, P);
                byte[] g_a_byte = g_a.ToByteArray().Reverse().ToArray();
                w.WriteLine(base64_encode(g_a_byte));
                w.Flush();
                BigInteger g_b = new BigInteger(positive(Convert.FromBase64String(r.ReadLine()).Reverse().ToArray()));
                // read g_b and compute g_b_a
                // compute g_b_a
                byte[] secret = BigInteger.ModPow(g_b, a, P).ToByteArray().Reverse().ToArray();
                if (secret.Length != 128)
                {
                    byte[] temp = new byte[128];
                    Array.Copy(secret, 1, temp, 0, 128);
                    Array.Copy(temp, secret, 128);
                }
                SHA256Managed sha = new SHA256Managed();
                dh_secret = sha.ComputeHash(secret, 0, 128);
            }
            catch (Exception)
            {
                // DH error
                OtherThread.disconnect();
            }
            // DONE DIFFIE HELLMAN
        }
        public static String remove(String data, char c)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                foreach (byte ch in data)
                    if (ch != c) builder.Append((char)ch);
                return builder.ToString();
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static String inspect(String data)
        {
            StringBuilder t = new StringBuilder();
            foreach (byte c in data)
            {
                if ((c >= 32) && (c < 127)) t.Append((char)c);
                else t.Append("\\x" + c.ToString());
            }
            return t.ToString();
        }

    }
}
