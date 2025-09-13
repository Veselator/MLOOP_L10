using System.Globalization;
using System.Text;

namespace MLOOP_L10
{
    public class Program
    {
        public const string TITLE = "Система управління файлами DNIEPER";

        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            FileManager MainFileManager = new FileManager();
            bool IsRunning = true;

            do
            {
                ShowTitle();
                ShowMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await AddNewFile(MainFileManager);
                        break;
                    case "2":
                        ShowAllFiles(MainFileManager);
                        break;
                    case "3":
                        ShowDetailedInfo(MainFileManager);
                        break;
                    case "4":
                        await SearchFiles(MainFileManager);
                        break;
                    case "5":
                        await RemoveFile(MainFileManager);
                        break;
                    case "6":
                        await UpdateFile(MainFileManager);
                        break;
                    case "7":
                        await ShowFilesByDateRange(MainFileManager);
                        break;
                    case "0":
                        await MainFileManager.SaveDataAsync();
                        Console.WriteLine(" Дані збережено. До побачення!");
                        IsRunning = false;
                        break;
                    default:
                        Console.WriteLine(" Невірний вибір. Спробуйте ще раз.");
                        break;
                }

                if (IsRunning)
                {
                    Console.WriteLine("\n Натисніть будь-яку клавішу для продовження...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
            while (IsRunning);
        }

        static void ShowTitle()
        {
            Console.WriteLine($"\n === {TITLE} ===\n");
        }

        static void ShowMenu()
        {
            Console.WriteLine(" ╔══════════════════════════════════════════╗");
            Console.WriteLine(" ║              ГОЛОВНЕ МЕНЮ                ║");
            Console.WriteLine(" ╠══════════════════════════════════════════╣");
            Console.WriteLine(" ║ 1. Додати новий файл                     ║");
            Console.WriteLine(" ║ 2. Показати всі файли                    ║");
            Console.WriteLine(" ║ 3. Детальна інформація про файли         ║");
            Console.WriteLine(" ║ 4. Пошук файлів                          ║");
            Console.WriteLine(" ║ 5. Видалити файл                         ║");
            Console.WriteLine(" ║ 6. Оновити файл                          ║");
            Console.WriteLine(" ║ 7. Файли за періодом                     ║");
            Console.WriteLine(" ║ 0. Вихід                                 ║");
            Console.WriteLine(" ╚══════════════════════════════════════════╝");
            Console.Write(" Ваш вибір: \n > ");
        }

        static async Task AddNewFile(FileManager manager)
        {
            Console.Write(" Введіть ім'я файлу: ");
            string fileName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(fileName))
            {
                Console.WriteLine(" Ім'я файлу не може бути порожнім!");
                return;
            }

            if (manager.FindByName(fileName) != -1)
            {
                Console.WriteLine(" Файл з таким іменем вже існує!");
                return;
            }

            manager.AddFile(fileName);
            Console.WriteLine($" Файл '{fileName}' успішно додано!");
        }

        static void ShowAllFiles(FileManager manager)
        {
            Console.WriteLine(manager.ToString());
        }

        static void ShowDetailedInfo(FileManager manager)
        {
            Console.WriteLine(manager.GetDetailedInfo());
        }

        static async Task SearchFiles(FileManager manager)
        {
            Console.Write(" Введіть термін для пошуку: ");
            string searchTerm = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                Console.WriteLine(" Термін пошуку не може бути порожнім!");
                return;
            }

            var foundFiles = manager.SearchByName(searchTerm);

            if (foundFiles.Count == 0)
            {
                Console.WriteLine(" Файли не знайдено.");
                return;
            }

            Console.WriteLine($" Знайдено файлів: {foundFiles.Count}");
            Console.WriteLine(new string('-', 40));

            for (int i = 0; i < foundFiles.Count; i++)
            {
                Console.WriteLine($" [{i}] {foundFiles[i].Name}");
            }
        }

        static async Task RemoveFile(FileManager manager)
        {
            if (manager.NumOfFiles == 0)
            {
                Console.WriteLine(" Список файлів порожній!");
                return;
            }

            Console.WriteLine(manager.ToString());
            Console.Write(" Введіть індекс файлу для видалення: ");

            if (int.TryParse(Console.ReadLine(), out int index))
            {
                string fileName = manager.GetFileName(index);
                if (manager.RemoveFile(index))
                {
                    Console.WriteLine($" Файл '{fileName}' успішно видалено!");
                }
                else
                {
                    Console.WriteLine(" Невірний індекс файлу!");
                }
            }
            else
            {
                Console.WriteLine(" Невірний формат індексу!");
            }
        }

        static async Task UpdateFile(FileManager manager)
        {
            if (manager.NumOfFiles == 0)
            {
                Console.WriteLine(" Список файлів порожній!");
                return;
            }

            Console.WriteLine(manager.ToString());
            Console.Write(" Введіть індекс файлу для оновлення: ");

            if (int.TryParse(Console.ReadLine(), out int index))
            {
                Console.Write(" Введіть нове ім'я файлу: ");
                string newName = Console.ReadLine();

                Console.Write(" Введіть новий розмір файлу (байт): ");
                if (long.TryParse(Console.ReadLine(), out long newSize))
                {
                    if (manager.UpdateFile(index, newName, newSize))
                    {
                        Console.WriteLine(" Файл успішно оновлено!");
                    }
                    else
                    {
                        Console.WriteLine(" Помилка оновлення файлу!");
                    }
                }
                else
                {
                    Console.WriteLine(" Невірний формат розміру!");
                }
            }
            else
            {
                Console.WriteLine(" Невірний формат індексу!");
            }
        }

        static async Task ShowFilesByDateRange(FileManager manager)
        {
            Console.Write(" Введіть початкову дату (дд.мм.рррр): ");
            if (DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime startDate))
            {
                Console.Write(" Введіть кінцеву дату (дд.мм.рррр): ");
                if (DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime endDate))
                {
                    var filesInRange = manager.GetFilesByDateRange(startDate, endDate);

                    if (filesInRange.Count == 0)
                    {
                        Console.WriteLine(" Файлів у вказаному діапазоні не знайдено.");
                        return;
                    }

                    Console.WriteLine($" Знайдено файлів у діапазоні: {filesInRange.Count}");
                    Console.WriteLine(new string('-', 40));

                    foreach (var file in filesInRange)
                    {
                        Console.WriteLine($"{file.Name} - {file.CreationDate.ToShortDateString()}");
                    }
                }
                else
                {
                    Console.WriteLine(" Невірний формат кінцевої дати!");
                }
            }
            else
            {
                Console.WriteLine(" Невірний формат початкової дати!");
            }
        }
    }
}