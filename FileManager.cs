using System.Text.Json;

namespace MLOOP_L10
{
    public class FileManager
    {
        private List<File> Files = new List<File>();
        private const string DefaultFileName = "files.json";

        public FileManager()
        {
            LoadData(DefaultFileName);
        }

        public FileManager(string fileName)
        {
            LoadData(fileName);
        }

        public int NumOfFiles => Files.Count;

        private bool LoadData(string fileName)
        {
            try
            {
                if (System.IO.File.Exists(fileName))
                {
                    string jsonContent = System.IO.File.ReadAllText(fileName);
                    Files = JsonSerializer.Deserialize<List<File>>(jsonContent) ?? new List<File>();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Помилка завантаження: {ex.Message}");
            }
            return false;
        }

        public async Task<bool> SaveDataAsync(string fileName = DefaultFileName)
        {
            try
            {
                string jsonContent = JsonSerializer.Serialize(Files, new JsonSerializerOptions { WriteIndented = true });
                await System.IO.File.WriteAllTextAsync(fileName, jsonContent);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Помилка збереження: {ex.Message}");
                return false;
            }
        }

        public void AddFile(string fileName)
        {
            Files.Add(new File(fileName));
        }

        public void AddFile(File file)
        {
            Files.Add(file);
        }

        public int FindByName(string fileName)
        {
            for (int i = 0; i < NumOfFiles; i++)
            {
                if (Files[i].Name == fileName)
                {
                    return i;
                }
            }
            return -1;
        }

        public bool UpdateFile(int index, string newName, long newSize)
        {
            if (index < 0 || index >= NumOfFiles) return false;
            Files[index].Name = newName;
            Files[index].Size = newSize;
            return true;
        }

        public bool RemoveFile(string fileName)
        {
            int i = FindByName(fileName);
            if (i == -1) return false;
            Files.RemoveAt(i);
            return true;
        }

        public bool RemoveFile(int id)
        {
            if (id > NumOfFiles - 1 || id < 0) return false;
            Files.RemoveAt(id);
            return true;
        }

        public string GetFileName(int id)
        {
            if (id < 0 || id >= NumOfFiles) return string.Empty;
            return Files[id].Name;
        }

        public File GetFile(int id)
        {
            if (id < 0 || id >= NumOfFiles) return null;
            Files[id].IncrementAccessCount();
            return Files[id];
        }

        public List<File> GetAllFiles()
        {
            return new List<File>(Files);
        }

        public List<File> SearchByName(string searchTerm)
        {
            return Files.Where(f => f.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public List<File> GetFilesByDateRange(DateTime startDate, DateTime endDate)
        {
            return Files.Where(f => f.CreationDate >= startDate && f.CreationDate <= endDate).ToList();
        }

        public override string ToString()
        {
            if (NumOfFiles == 0) return "Список файлів порожній";

            var sb = new System.Text.StringBuilder();
            sb.AppendLine($" Всього файлів: {NumOfFiles}");
            sb.AppendLine(" " + new string('-', 50));

            for (int i = 0; i < NumOfFiles; i++)
            {
                sb.AppendLine($" [{i}] {Files[i].Name}");
            }
            return sb.ToString();
        }

        public string GetDetailedInfo()
        {
            if (NumOfFiles == 0) return " Список файлів порожній";

            var sb = new System.Text.StringBuilder();
            sb.AppendLine($" Детальна інформація про файли (всього: {NumOfFiles}):");
            sb.AppendLine(" " + new string('=', 60));

            for (int i = 0; i < NumOfFiles; i++)
            {
                sb.AppendLine($" [{i}]");
                sb.AppendLine(Files[i].ToString());
                sb.AppendLine(" " + new string('-', 40));
            }
            return sb.ToString();
        }
    }
}