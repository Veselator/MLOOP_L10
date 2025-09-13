using System.Text.Json.Serialization;

namespace MLOOP_L10
{
    public class File
    {
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public int AccessCount { get; set; }
        public long Size { get; set; }

        public File() { }

        public File(string name)
        {
            Name = name;
            CreationDate = DateTime.Now;
            AccessCount = 0;
            Size = new Random().Next(0, 10000);
        }

        public File(string name, DateTime creationDate, int accessCount, long size)
        {
            Name = name;
            CreationDate = creationDate;
            AccessCount = accessCount;
            Size = size;
        }

        public void IncrementAccessCount()
        {
            AccessCount++;
        }

        public override string ToString()
        {
            return $" Ім'я файлу:      \t\t{Name}\n Дата створення: \t\t{CreationDate.ToShortDateString()}\n Кількість звернень: \t\t{AccessCount}\n Розмір файлу:    \t\t{Size} байт";
        }

        public override int GetHashCode()
        {
            return Name?.GetHashCode() ?? 0;
        }

        public override bool Equals(object obj)
        {
            if (obj is File other) return Name == other.Name;
            return false;
        }
    }
}