using System;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.IO;

namespace SSPVO.Cript
{
    public class SingleSigner
    {
        // Открываем хранилище 'My' и ищем сертификат для подписи сообщения. 
        public static X509Certificate2 GetSignerCert(string signerName)
        {
            // Открываем хранилище My.
            X509Store storeMy = new X509Store(StoreName.My,
                StoreLocation.CurrentUser);
            storeMy.Open(OpenFlags.ReadOnly);

            // Ищем сертификат для подписи.
            X509Certificate2Collection certColl =
                storeMy.Certificates.Find(X509FindType.FindBySubjectName,
                signerName, false);

            // Проверяем, что нашли требуемый сертификат
            if (certColl.Count == 0)
            {
                return null;
            }

            storeMy.Close();

            // Если найдено более одного сертификата,
            // возвращаем первый попавщийся.
            return certColl[0];
        }

        // Подписываем сообщение секретным ключем.
        public static byte[] SignMsg(Byte[] msg, X509Certificate2 signerCert, bool detached = true)
        {
            // Создаем объект ContentInfo по сообщению.
            // Это необходимо для создания объекта SignedCms.
            ContentInfo contentInfo = new ContentInfo(msg);

            // Создаем объект SignedCms по только что созданному
            // объекту ContentInfo.
            // SubjectIdentifierType установлен по умолчанию в 
            // IssuerAndSerialNumber.
            // Свойство Detached установлено по умолчанию в false, таким 
            // образом сообщение будет включено в SignedCms.
            SignedCms signedCms = new SignedCms(contentInfo, detached);

            // Определяем подписывающего, объектом CmsSigner.
            CmsSigner cmsSigner = new CmsSigner(signerCert);
            cmsSigner.DigestAlgorithm = new System.Security.Cryptography.Oid("1.2.840.113549.1.7.1");
            //cmsSigner.IncludeOption = X509IncludeOption.ExcludeRoot;
            //cmsSigner.SignerIdentifierType = SubjectIdentifierType.SubjectKeyIdentifier; 

            // Подписываем CMS/PKCS #7 сообение.
            signedCms.ComputeSignature(cmsSigner);

            // Кодируем CMS/PKCS #7 сообщение.
            return signedCms.Encode();
        }

        // Проверяем SignedCms сообщение и возвращаем Boolean значение определяющее результат проверки.
        public static bool VerifyMsg(byte[] encodedSignedCms)
        {
            // Создаем SignedCms для декодирования и проверки.
            SignedCms signedCms = new SignedCms();

            // Декодируем сообщение
            signedCms.Decode(encodedSignedCms);

            // Перехватываем криптографические исключения, для 
            // возврата о false значения при некорректности подписи.
            try
            {
                // Проверяем подпись. В данном примере не 
                // проверяется корректность сертификата подписавшего.
                // В рабочем коде, скорее всего потребуется построение
                // и проверка корректности цепочки сертификата.
                signedCms.CheckSignature(true);
            }
            catch (System.Security.Cryptography.CryptographicException e)
            {
                throw new System.Security.Cryptography.CryptographicException(e.Message);
                //return false;
            }

            return true;
        }

        public static bool VerifyDetachedMsg(Byte[] msg,
            byte[] encodedSignature)
        {
            // Создаем объект ContentInfo по сообщению.
            // Это необходимо для создания объекта SignedCms.
            ContentInfo contentInfo = new ContentInfo(msg);

            // Создаем SignedCms для декодирования и проверки.
            SignedCms signedCms = new SignedCms(contentInfo, true);

            // Декодируем подпись
            signedCms.Decode(encodedSignature);

            // Перехватываем криптографические исключения, для 
            // возврата о false значения при некорректности подписи.
            try
            {
                // Проверяем подпись. В данном примере не 
                // проверяется корректность сертификата подписавшего.
                // В рабочем коде, скорее всего потребуется построение
                // и проверка корректности цепочки сертификата.
                signedCms.CheckSignature(true);
            }
            catch (System.Security.Cryptography.CryptographicException e)
            {
                throw new System.Security.Cryptography.CryptographicException(e.Message);
                //return false;
            }

            return true;
        }
    }


}
