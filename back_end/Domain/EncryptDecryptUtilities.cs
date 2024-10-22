using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace back_end.Models
{
    public class EncryptDecryptUtilities
    {
        public static MemoryStream generateStreamFromString(string value)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(value ?? ""));
        }

        public byte[] encryptId(int id)
        {
            int keyBytesCount = 32; // 32 for 256 bits encryption
            int IVbytesCount = 16; // for 128 bits
            byte[] aesKey = new byte[keyBytesCount]; 
            // IV is a cryptography concept
            byte[] aesIV = new byte[IVbytesCount];
            byte[] bin = new byte[10]; //This is intermediate storage for the dencryption.
            byte[] encrypted;
            byte keySeed = 3;
            byte IVSeed = 5;
            for (int i = 0; i < keyBytesCount; i++)
            {
                aesKey[i] = keySeed;
            }
            for (int i = 0; i < IVbytesCount; i++)
            {
                aesIV[i] = IVSeed;
            }

            // Create a Aes object with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = aesKey;
                aesAlg.IV = aesIV;
                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new())
                {
                    using (CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new(csEncrypt))
                        {
                            // Write all data to the stream.
                            swEncrypt.Write($"{id}");
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        public string getRealId(string encryptedID)
        {
            // Declare the string used to hold the decrypted text.
            string plaintext = null;
            byte[] aesKey = new byte[32];
            byte[] aesIV = new byte[16];
            byte[] encrypted;
            for (int i = 0; i < 32; i++)
            {
                aesKey[i] = 3;
            }
            for (int i = 0; i < 16; i++)
            {
                aesIV[i] = 5;
            }
            byte[] encryptedIDBytes = Convert.FromBase64String(encryptedID);
            // Create a Aes object with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = aesKey;
                aesAlg.IV = aesIV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new(encryptedIDBytes))
                {
                    using (CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }
    }
}
