﻿    using System.Collections.Generic;
    using System.IO;
    using System;
    using System.Text;
    using System.Diagnostics;

class Program
    {
        static void Main(string[] args)
        {
            string inputFileName = "/Users/rom/Study/LZW/LZW/input.txt";
            string compressedFileName = "/Users/rom/Study/LZW/LZW/compressed.lzw";
            string decompressedFileName = "/Users/rom/Study/LZW/LZW/decompressed.txt";

            LZWCompressionPerformanceTest.Run(10, 10000);

        // Чтение данных из файла
        string inputText = File.ReadAllText(inputFileName, Encoding.Unicode);
            byte[] inputData = Encoding.Unicode.GetBytes(inputText);

            // Сжатие данных методом LZW
            byte[] compressedData = LZWCompression.Compress(inputData);

            // Запись сжатых данных в файл
            File.WriteAllBytes(compressedFileName, compressedData);

            // Чтение сжатых данных из файла
            byte[] compressedInputData = File.ReadAllBytes(compressedFileName);

            // Распаковка сжатых данных методом LZW
            byte[] decompressedData = LZWCompression.Decompress(compressedInputData);

            // Запись распакованных данных в файл
            string decompressedText = Encoding.Unicode.GetString(decompressedData);
            File.WriteAllText(decompressedFileName, decompressedText, Encoding.Unicode);

            Console.WriteLine("Compression and decompression completed successfully!");
        }
    }


    public static class LZWCompression
    {
        // Метод сжатия данных методом LZW
        public static byte[] Compress(byte[] data)
        {
            // Создание словаря с символами ASCII кодов от 0 до 255
            var dictionary = new Dictionary<string, int>();
            for (int i = 0; i < 256; i++)
            {
                dictionary.Add(((char)i).ToString(), i);
            }

            // Создание списка для хранения сжатых данных
            var compressedData = new List<int>();
            var word = ((char)data[0]).ToString();
            for (int i = 1; i < data.Length; i++)
            {
                var c = (char)data[i];
                var wc = word + c;
                if (dictionary.ContainsKey(wc))
                {
                    word = wc;
                }
                else
                {
                    compressedData.Add(dictionary[word]);
                    dictionary.Add(wc, dictionary.Count);
                    word = c.ToString();
                }
            }

            compressedData.Add(dictionary[word]);

            // Преобразование списка сжатых данных в массив байтов
            // и возврат результата
            var compressedBytes = new List<byte>();
            for (int i = 0; i < compressedData.Count; i++)
            {
                compressedBytes.AddRange(
                    System.BitConverter.GetBytes(compressedData[i])
                    );
            }

            return compressedBytes.ToArray();
        }

        // Метод разархивирования данных LZW
        public static byte[] Decompress(byte[] data)
        {
            // Создание словаря с символами ASCII кодов от 0 до 255
            var dictionary = new Dictionary<int, string>();
            for (int i = 0; i < 256; i++)
            {
                dictionary.Add(i, ((char)i).ToString());
            }

            // Создание списка для хранения сжатых данных
            var compressedData = new List<int>();
            for (int i = 0; i < data.Length; i += 4)
            {
                compressedData.Add(System.BitConverter.ToInt32(data, i));
            }

            // Создание списка для хранения распакованных данных
            var decompressedData = new List<string>();
            var w = dictionary[compressedData[0]];
            decompressedData.Add(w);
            for (int i = 1; i < compressedData.Count; i++)
            {
                string entry = null;
                if (dictionary.ContainsKey(compressedData[i]))
                {
                    entry = dictionary[compressedData[i]];
                }
                else if (compressedData[i] == dictionary.Count)
                {
                    entry = w + w[0];
                }

                decompressedData.Add(entry);
                dictionary.Add(dictionary.Count, w + entry[0].ToString());
                w = entry;
            }

            // Преобразование списка распакованных данных в массив байтов
            // и возврат результата
            var decompressedBytes = new List<byte>();
            for (int i = 0; i < decompressedData.Count; i++)
            {
                decompressedBytes.AddRange(
                    System.Text.Encoding.ASCII.GetBytes(decompressedData[i])
                    );
            }

            return decompressedBytes.ToArray();
        }
}


