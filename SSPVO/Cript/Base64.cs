using System;
using System.IO;
using System.Security.Cryptography;

namespace SSPVO.Cript
{
    /// <summary>
    /// Класс для работы с кодировка Base64
    /// </summary>
    public class Base64
    {
        /// <summary>
        /// Чтение исходного файла и запись его в другой файл в закодированном виде.
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name="targetFile"></param>
        public static void EncodeFromFile(string sourceFile, string targetFile)
        {
            // Получаем входной и выходной потоки.
            // Не засоряем пример закрытием файлов при исключениях,
            // В реальном коде используйте using.
            FileStream inputFileStream =
                new FileStream(sourceFile, FileMode.Open, FileAccess.Read);
            FileStream outputFileStream =
                new FileStream(targetFile, FileMode.Create, FileAccess.Write);

            // Создаем объект конвертирования в Base64 - ToBase64Transform
            ToBase64Transform base64Transform = new ToBase64Transform();

            // Создаем байтовый массив под буфер конвертирования, по размеру
            // заданному в объекте base64Transform
            // Текущая реализация .Net Framework не позволяет кодировать
            // несколько блоков за один раз.
            byte[] outputBytes = new byte[base64Transform.OutputBlockSize];

            // Вычитываем первый блок.
            int InputBlockSize = base64Transform.InputBlockSize;
            byte[] inputBytes = new byte[InputBlockSize];
            int bytesRead = inputFileStream.Read(inputBytes, 0,
                InputBlockSize);

            int blockAtLine = 0;

            // По всему файлу, до последнего блока
            while (bytesRead == InputBlockSize)
            {
                // Преобразуем блок
                int transformed = base64Transform.TransformBlock(
                    inputBytes,
                    0,
                    inputBytes.Length,
                    outputBytes,
                    0);

                // Записываем преобразованный блок
                outputFileStream.Write(
                    outputBytes,
                    0,
                    transformed);

                // Формируем строки по 18 блоков
                if (blockAtLine == 18)
                {
                    outputFileStream.Write(new byte[] { 0x0D, 0x0A }, 0, 2);
                    blockAtLine = 0;
                }
                else
                    blockAtLine++;

                // Читаем следующий блок.
                bytesRead = inputFileStream.Read(inputBytes, 0,
                    InputBlockSize);
            }

            if (bytesRead > 0)
            {
                // Преобразуем последний блок.
                outputBytes = base64Transform.TransformFinalBlock(
                    inputBytes,
                    0,
                    bytesRead);

                // Записываем последний блок.
                outputFileStream.Write(outputBytes, 0, outputBytes.Length);
            }

            // Заканчиваем файл возвратом каретки на ненулевых файлах
            if (inputFileStream.Length > 0)
                outputFileStream.Write(new byte[] { 0x0D, 0x0A }, 0, 2);

            // Закрыываем потоки.
            inputFileStream.Close();
            outputFileStream.Close();
        }

        /// <summary>
        /// Чтение исходного файла и запись его в другой файл в раскодированном виде.
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name="targetFile"></param>
        public static void DecodeFromFile(string sourceFile,
            string targetFile)
        {
            // Получаем входной и выходной потоки.
            // Не засоряем пример закрытием файлов при исключениях,
            // В реальном коде используйте using.
            FileStream inputFileStream =
                new FileStream(sourceFile, FileMode.Open, FileAccess.Read);
            FileStream outputFileStream =
                new FileStream(targetFile, FileMode.Create, FileAccess.Write);

            // Создаем объект конвертирования из Base64 - FromBase64Transform
            // Игнорируем пробельные символы, для возможности декодирования
            // построчного Base64
            FromBase64Transform base64Transform = new FromBase64Transform(
                FromBase64TransformMode.IgnoreWhiteSpaces);

            // Создаем байтовый массив под буфер конвертирования, по размеру
            // заданному в объекте base64Transform
            // Текущая реализация .Net Framework не позволяет декодировать
            // несколько блоков за один раз.
            byte[] outputBytes = new byte[base64Transform.OutputBlockSize];

            // Можно декодировать и по одному байту в соответствии
            // с base64Transform.InputBlockSize, но мы используем размер блока
            // алгоритма - 4 байта
            int InputBlockSize = 4;

            // Вычитываем первый блок.
            byte[] inputBytes = new byte[InputBlockSize];
            int bytesRead = inputFileStream.Read(inputBytes, 0,
                InputBlockSize);

            // По всему файлу, до последнего блока
            while (bytesRead == InputBlockSize)
            {
                // Преобразуем блок
                int transformed = base64Transform.TransformBlock(
                    inputBytes,
                    0,
                    inputBytes.Length,
                    outputBytes,
                    0);

                // Записываем преобразованный блок
                outputFileStream.Write(
                    outputBytes,
                    0,
                    transformed);

                // Читаем следующий блок.
                bytesRead = inputFileStream.Read(inputBytes, 0,
                    InputBlockSize);
            }

            if (bytesRead > 0)
            {
                // Преобразуем последний блок.
                outputBytes = base64Transform.TransformFinalBlock(
                    inputBytes,
                    0,
                    bytesRead);

                // Записываем последний блок.
                outputFileStream.Write(outputBytes, 0, outputBytes.Length);
            }

            // Закрыываем потоки.
            inputFileStream.Close();
            outputFileStream.Close();
        }

        /// <summary>
        /// Конвертация одной исходной строки в другую строку формата Base64
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public static string EncodeFromString(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// Конвертация строки из Base64 в обычную строку
        /// </summary>
        /// <param name="base64EncodedData"></param>
        /// <returns></returns>
        public static string DecodeFromString(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
