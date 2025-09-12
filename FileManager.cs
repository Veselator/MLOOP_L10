using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MLOOP_L10
{
    [Serializable]
    public class FileManager
    {
        private List<File> Files = new List<File>();

        public FileManager()
        {

        }

        public FileManager(string fileName)
        {
            LoadData(fileName);
        }

        public int NumOfFiles
        {
            get
            {
                return Files.Count;
            }
        }

        private bool LoadData(string fileName)
        {
            try
            {
                Files = JsonSerializer.Deserialize<List<File>>(fileName);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        private async Task<bool> SaveDataAsync(string fileName)
        {
            try
            {
                using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
                {
                    await JsonSerializer.SerializeAsync<List<File>>(fs, Files);
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public void AddFile(string fileName)
        {
            Files.Add(new File(fileName));
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
            return string.Empty;
        }

        public override string ToString()
        {
            return Files.ToString();
        }
    }
}
