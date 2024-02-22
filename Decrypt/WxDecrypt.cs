using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace WXDBDecrypt.NET
{
    class WxDecrypt
    {
        const int IV_SIZE = 16;
        const int HMAC_SHA1_SIZE = 20;
        const int KEY_SIZE = 32;
        const int AES_BLOCK_SIZE = 16;
        const int DEFAULT_ITER = 64000;
        const int DEFAULT_PAGESIZE = 4096; //4048数据 + 16IV + 20 HMAC + 12
        const string SQLITE_HEADER = "SQLite format 3";



        static byte[] HexConvert(string hex)
        {
            int length = hex.Length;
            byte[] bytes = new byte[length / 2];
            for (int i = 0; i < length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }
       
        public static void DecryptDB(string inFile,string outFile,string password)
        {
           

            //解密密钥
            byte[] password_bytes = HexConvert(password);
             DecryptDB(inFile,outFile, password_bytes);
            

        }


        private static void DecryptDB(string file, string to_file, byte[] password_bytes)
        {
            //数据库头16字节是盐值
            byte[] salt_key = new byte[16];

            FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            fileStream.Read(salt_key, 0, 16);

            //HMAC验证时用的盐值需要亦或0x3a
            byte[] hmac_salt = new byte[16];
            for (int i = 0; i < salt_key.Length; i++)
            {
                hmac_salt[i] = (byte)(salt_key[i] ^ 0x3a);
            }
            //计算保留段长度
            long reserved = IV_SIZE;
            reserved += HMAC_SHA1_SIZE;
            reserved = ((reserved % AES_BLOCK_SIZE) == 0) ? reserved : ((reserved / AES_BLOCK_SIZE) + 1) * AES_BLOCK_SIZE;

            //密钥扩展，分别对应AES解密密钥和HMAC验证密钥
            byte[] key = new byte[KEY_SIZE];
            byte[] hmac_key = new byte[KEY_SIZE];
            OpenSSLInterop.PKCS5_PBKDF2_HMAC_SHA1(password_bytes, password_bytes.Length, salt_key, salt_key.Length, DEFAULT_ITER, key.Length, key);
            OpenSSLInterop.PKCS5_PBKDF2_HMAC_SHA1(key, key.Length, hmac_salt, hmac_salt.Length, 2, hmac_key.Length, hmac_key);

            long page_no = 0;
            long offset = 16;
            Console.WriteLine("开始解密...");
            var hmac_sha1 = HMAC.Create("HMACSHA1");
            hmac_sha1.Key = hmac_key;

            List<byte> decrypted_file_bytes = new List<byte>();
            FileStream tofileStream = new FileStream(to_file, FileMode.OpenOrCreate, FileAccess.Write);

            using (fileStream)
            {
                try
                {
                    // 当前分页小于计算分页数
                    while (page_no < fileStream.Length / DEFAULT_PAGESIZE)
                    {
                        // 读内容
                        byte[] decryped_page_bytes = new byte[DEFAULT_PAGESIZE];
                        byte[] going_to_hashed = new byte[DEFAULT_PAGESIZE - reserved - offset + IV_SIZE + 4];
                        fileStream.Seek((page_no * DEFAULT_PAGESIZE) + offset, SeekOrigin.Begin);
                        fileStream.Read(going_to_hashed, 0, Convert.ToInt32(DEFAULT_PAGESIZE - reserved - offset + IV_SIZE));

                        // 分页标志
                        var page_bytes = BitConverter.GetBytes(page_no + 1);
                        page_bytes = page_bytes.Take(4).ToArray();
                        page_bytes.CopyTo(going_to_hashed, DEFAULT_PAGESIZE - reserved - offset + IV_SIZE);
                        var hash_mac_compute = hmac_sha1.ComputeHash(going_to_hashed, 0, going_to_hashed.Length);

                        // 取分页hash
                        byte[] hash_mac_cached = new byte[hash_mac_compute.Length];
                        fileStream.Seek((page_no * DEFAULT_PAGESIZE) + DEFAULT_PAGESIZE - reserved + IV_SIZE, SeekOrigin.Begin);
                        fileStream.Read(hash_mac_cached, 0, hash_mac_compute.Length);

                        if (!hash_mac_compute.SequenceEqual(hash_mac_cached) && page_no == 0)
                        {
                            Console.WriteLine("Hash错误...");
                            return;
                        }
                        else
                        {
                            if (page_no == 0)
                            {
                                var header_bytes = Encoding.ASCII.GetBytes(SQLITE_HEADER);
                                header_bytes.CopyTo(decryped_page_bytes, 0);
                            }

                            // 加密内容
                            byte[] page_content = new byte[DEFAULT_PAGESIZE - reserved - offset];
                            fileStream.Seek((page_no * DEFAULT_PAGESIZE) + offset, SeekOrigin.Begin);
                            fileStream.Read(page_content, 0, Convert.ToInt32(DEFAULT_PAGESIZE - reserved - offset));

                            // iv
                            byte[] iv = new byte[16];
                            fileStream.Seek((page_no * DEFAULT_PAGESIZE) + (DEFAULT_PAGESIZE - reserved), SeekOrigin.Begin);
                            fileStream.Read(iv, 0, 16);

                            var decrypted_content = AESHelper.AESDecrypt(page_content, key, iv);
                            decrypted_content.CopyTo(decryped_page_bytes, offset);

                            // 保留
                            byte[] reserved_byte = new byte[reserved];
                            fileStream.Seek((page_no * DEFAULT_PAGESIZE) + DEFAULT_PAGESIZE - reserved, SeekOrigin.Begin);
                            fileStream.Read(reserved_byte, 0, Convert.ToInt32(reserved));
                            reserved_byte.CopyTo(decryped_page_bytes, DEFAULT_PAGESIZE - reserved);

                            tofileStream.Write(decryped_page_bytes, 0, decryped_page_bytes.Length);

                        }
                        page_no++;
                        offset = 0;
                    }
                }
                catch (Exception ex)
                {
                    File.AppendAllText("err.log", "page=>" + page_no.ToString() + "\r\n");
                    File.AppendAllText("err.log", "size=>" + fileStream.Length.ToString() + "\r\n");
                    File.AppendAllText("err.log", "postion=>" + ((page_no * DEFAULT_PAGESIZE) + offset).ToString() + "\r\n");
                    File.AppendAllText("err.log", ex.ToString() + "\r\n");
                }
            }

            tofileStream.Close();
            tofileStream.Dispose();
        }
    }
}
