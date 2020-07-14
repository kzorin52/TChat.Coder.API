using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TChat.Coder.API
{
    class Main
    {
        /// <summary>
        ///Шифрует строку с помощью пароля и некоторых алгоритмов шифрования. Возвращает строку в кодировке Base64
        /// </summary>
        /// <param name="Text">Входной текст</param>
        /// <param name="Pass">Пароль для шифровки</param>
        /// <returns></returns>
        string Encode(string Text, string Pass)
        {
            return Helper2.Encrypt(Helper1.Encrypt(Text), Pass);
        }
        /// <summary>
        /// Расшифровывает строку в кодировке Base64, возвращает string
        /// </summary>
        /// <param name="EncodedText">Зашифрованный текст в формате Base64</param>
        /// <param name="Pass">Пароль для дешифровки</param>
        /// <returns></returns>
        string Decode(string EncodedText, string Pass)
        {
            return Helper2.Decrypt(Helper1.Decrypt(EncodedText), Pass);
        }
    }
}
