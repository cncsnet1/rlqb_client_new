using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace rlqb_client.utils
{
    internal class EncUtil
    {
        public static string  md5enCry(string content)
        {
            /// <summary>
            /// 32位MD5加密
         

            // Step 1: Convert the password to bytes
            byte[] passwordBytes = Encoding.UTF8.GetBytes(content);

            // Step 2: Calculate MD5 hash
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(passwordBytes);

                // Step 3: Convert the hash bytes to a hexadecimal string
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }

                string pwdHash = sb.ToString();
                return pwdHash;
            }
            return "";
           
        }

        /**
         * udp数据发送加密
         */
        public  static string sendMessageEnc(string raw)
        {
            byte[] key = Encoding.UTF8.GetBytes("VHJhhh_76576ajsh");

            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = key;
                aesAlg.GenerateIV();

                // 创建加密器
                using (ICryptoTransform encryptor = aesAlg.CreateEncryptor())
                {
                    // 将字符串转换为字节数组
                    byte[] rawData = Encoding.UTF8.GetBytes(raw);

                    // 计算需要补齐的字节数
                    int padding = 16 - rawData.Length % 16;
                    // 补齐原始数据
                    byte[] paddedData = new byte[rawData.Length + padding];
                    Buffer.BlockCopy(rawData, 0, paddedData, 0, rawData.Length);
                    for (int i = rawData.Length; i < paddedData.Length; i++)
                    {
                        paddedData[i] = (byte)padding;
                    }

                    // 加密数据
                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            csEncrypt.Write(paddedData, 0, paddedData.Length);
                            csEncrypt.Flush(); 
                            // 合并iv和加密数据
                        byte[] result = new byte[aesAlg.IV.Length + msEncrypt.Length];
                        Buffer.BlockCopy(aesAlg.IV, 0, result, 0, aesAlg.IV.Length);
                        Buffer.BlockCopy(msEncrypt.ToArray(), 0, result, aesAlg.IV.Length, (int)msEncrypt.Length);
                            return Convert.ToBase64String(result);

                        }


                        // 返回Base64编码的加密结果
                    }
                }
            }
        }


    }
}
