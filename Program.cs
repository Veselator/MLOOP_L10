using System;
using System.Text;
using System.Text.Json;

namespace MLOOP_L10
{
    internal class Program
    {
        static string[] line = { 
            "//////////////////",
            "\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"
        };
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine($"\n {line[0]}    Лабораторна робота №10    {line[0]}");
            Console.WriteLine(  $" {line[1]}      Тема: Серіалізація      {line[1]}");
            Console.WriteLine(  $" {line[0]}           Виконав            {line[0]}");
            Console.WriteLine(  $" {line[1]}        Соломка Борис         {line[1]}\n");

            File[] files =
            {
                new File("document1.txt", new DateTime(2026, 1, 15), 5, 1024),
                new File("image.jpg", new DateTime(2022, 2, 10), 12, 4096),
                new File("report.pdf", new DateTime(2023, 3, 5), 3, 2048),
                new File("data.csv", new DateTime(2024, 3, 20), 8, 512),
                new File("archive.zip", new DateTime(2003, 4, 2), 2, 8192)
            };

            Console.WriteLine(" Створений масив файлів:");
            foreach (var file in files)
            {
                Console.WriteLine($"{file}\n");
            }

            SerializeFiles(files, "files.json");
            Console.WriteLine("\n Файли серіалізовано у files.json");

            File[] deserializedFiles = await DeserializeFiles("files.json");
            Console.WriteLine("\n Десеріалізовані файли:");
            foreach (var file in deserializedFiles)
            {
                Console.WriteLine($"{file}\n");
            }

            Console.Write("\n Введіть початкову дату (формат ДД.ММ.РРРР):\n > ");
            DateTime startDate;
            if (!DateTime.TryParse(Console.ReadLine(), out startDate)) { startDate = new DateTime(2007, 7, 27); }

            Console.Write(" Введіть кінцеву дату (формат ДД.ММ.РРРР):\n > ");
            DateTime endDate;
            if (!DateTime.TryParse(Console.ReadLine(), out endDate)) { endDate = DateTime.Now; }

            File[] filteredFiles = GiveMeFilteredFilesByDate(deserializedFiles, startDate, endDate);
            Console.WriteLine("\n Файли, створені у заданому діапазоні, відсортовані за розміром (спадання):");
            foreach (var file in filteredFiles)
            {
                Console.WriteLine($"{file}\n");
            }

            Console.ReadLine();
        }

        static async void SerializeFiles(File[] files, string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                await JsonSerializer.SerializeAsync(fs, files);
            }
        }

        static async Task<File[]> DeserializeFiles(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                return await JsonSerializer.DeserializeAsync<File[]>(fs);
            }
        }

        static File[] GiveMeFilteredFilesByDate(File[] files, DateTime startDate, DateTime endDate)
        {
            return files.Where(f => f.CreationDate >= startDate && f.CreationDate <= endDate).OrderByDescending(f => f.Size).ToArray();
        }
    }
}
