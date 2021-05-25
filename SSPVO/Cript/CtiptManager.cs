using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSPVO.Cript;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.IO;

namespace SSPVO.Cript
{

    /// <summary>
    /// Клас для работы с криптометодами
    /// </summary>
    public class CtiptManager
    {
        /// <summary>
        /// Формирвоание строки JWT по header и payload
        /// </summary>
        /// <param name="header"></param>
        /// <param name="payload"></param>
        /// <returns>Возвращает строку JWT по header и payload</returns>
        public static string GetJWT(string header, string payload)
        {
            // преобразуем header и payload в Base64 и соединяем их в строку JW
            string jw = Cript.Base64.EncodeFromString(header) + "." + Cript.Base64.EncodeFromString(payload);

            // подписываем строку JW
            string signedInBase64 = CtiptManager.SignMsg(jw);

            // проверяем подпись
            bool verifyResult = VerifyDetachedMsg(jw, signedInBase64);

            // возвращаем JWT
            return jw + "." + signedInBase64;
        }

        /// <summary>
        /// Подпсать текст msg
        /// </summary>
        /// <param name="msg">Подписываемый текст</param>
        /// <returns>Возвращает подпись в виде строки закодированную в base64</returns>
        public static string SignMsg(string msg)
        {
            // Переводим исходное сообщение в массив байтов.
            byte[] msgBytes = System.Text.Encoding.UTF8.GetBytes(msg);

            // Получаем сертификат ключа подписи; 
            // он будет использоваться для получения секретного ключа подписи.
            X509Certificate2 signerCert = SingleSigner.GetSignerCert(Properties.Settings.Default.signerName);

            // подписываем и получаем подпись в виде массива байт
            byte[] encodedSignedCms = SingleSigner.SignMsg(msgBytes, signerCert);

            // преобразуем подпись в base64 и сохраняем в строковой переменной
            return Convert.ToBase64String(encodedSignedCms);
        }

        /// <summary>
        /// Проверяем подписанную строку 
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool VerifyMsg(string msg)
        {
            // преобразуем подпись из base64 обратно в массив байт
            byte[] bytes = System.Convert.FromBase64String(msg);

            // проверка подписи
            if (SingleSigner.VerifyMsg(bytes))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Проверяем подписанную строку 
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool VerifyDetachedMsg(string msg, string signedInBase64)
        {
            byte[] msgBytes = System.Text.Encoding.UTF8.GetBytes(msg);

            // преобразуем подпись из base64 обратно в массив байт
            byte[] bytes = System.Convert.FromBase64String(signedInBase64);

            // проверка подписи
            if (SingleSigner.VerifyDetachedMsg(msgBytes, bytes))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Подпсать текст msg и помещает подпись в файл
        /// </summary>
        /// <param name="msg">Подписываемый текст</param>
        /// <returns>Возвращает подпись в виде строки закодированную в base64</returns>
        public static void SignMsgToFile(string msg, string fileName)
        {
            // Переводим исходное сообщение в массив байтов.
            Encoding unicode = Encoding.Unicode;
            byte[] msgBytes = unicode.GetBytes(msg);

            // Получаем сертификат ключа подписи; 
            // он будет использоваться для получения секретного ключа подписи.
            X509Certificate2 signerCert = SingleSigner.GetSignerCert(Properties.Settings.Default.signerName);

            // подписываем и получаем подпись в виде массива байт
            byte[] encodedSignedCms = SingleSigner.SignMsg(msgBytes, signerCert, false);

            // записываем подпись в файл
            File.WriteAllBytes(fileName, encodedSignedCms);
        }
    }
}
