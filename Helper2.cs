using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TChat.Coder.API
{
    class Helper2
    {
         #region Fields


    private static RandomNumberGenerator rng = new RNGCryptoServiceProvider();

    #endregion Fields

         #region Methods

    /// <summary>
    ///Расшифровывает строку в кодировке Base64, возвращает string
    /// </summary>
    /// <param name="cipherText">Зашифрованная строка в кодировке base64</param>
    /// <param name="passPhrase">Пароль для раскодировки</param>
    /// <returns></returns>
    public static string Decrypt(string cipherText, string passPhrase)
    {
        try
            {
                var ciphertextS = DecodeFrom64(cipherText);
                var ciphersplit = Regex.Split(ciphertextS, "-");
                var passsalt = Convert.FromBase64String(ciphersplit[1]);
                var initVectorBytes = Convert.FromBase64String(ciphersplit[0]);
                var cipherTextBytes = Convert.FromBase64String(ciphersplit[2]);
                var password = new PasswordDeriveBytes(passPhrase, passsalt, "SHA512", 100);
                var keyBytes = password.GetBytes(256/8);
                var symmetricKey = new RijndaelManaged();
                symmetricKey.Mode = CipherMode.CBC;
                var decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
                var memoryStream = new MemoryStream(cipherTextBytes);
                var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                var plainTextBytes = new byte[cipherTextBytes.Length];
                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                memoryStream.Close();
                cryptoStream.Close();
                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
            }
            catch 
            {
                return "Error";
            }
    }

    /// <summary>
    ///Шифрует строку с помощью пароля. Возвращает строку в кодировке base64
    /// </summary>
    /// <param name="plainText">Входной текст</param>
    /// <param name="passPhrase">Пароль для шифровки</param>
    /// <returns></returns>
    public static string Encrypt(string plainText, string passPhrase)
    {
        var initvector = new byte[16]; //MUST BE 16 Bytes for AES 256
            var passsalt = new byte[16]; //For Salt
            rng.GetBytes(initvector);
            rng.GetBytes(passsalt);
            var initVectorBytes = initvector;
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            var password = new PasswordDeriveBytes(passPhrase, passsalt, "SHA512", 100);
            var keyBytes = password.GetBytes(256/8);
            var symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            var memoryStream = new MemoryStream();
            var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            var cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return
                EncodeTo64(Convert.ToBase64String(initVectorBytes) + "-" + Convert.ToBase64String(passsalt) + "-" +
                           Convert.ToBase64String(cipherTextBytes));
    }

    private static string DecodeFrom64(string encodedData)
    {
        var encodedDataAsBytes = Convert.FromBase64String(encodedData);
            var returnValue = Encoding.UTF8.GetString(encodedDataAsBytes);
            return returnValue;
    }

    private static string EncodeTo64(string toEncode)
    {
        var toEncodeAsBytes = Encoding.UTF8.GetBytes(toEncode);
            var returnValue = Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
    }

    #endregion Methods
    }
}
