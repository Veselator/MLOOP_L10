using System.Drawing;

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
            Size = 0;
        }

        public File(string name, DateTime creationDate, int accessCount, long size)
        {
            Name = name;
            CreationDate = creationDate;
            AccessCount = accessCount;
            Size = size;
        }

        public override string ToString()
        {
            AccessCount++;
            return $" Ім'я файлу:      \t\t{Name}\n Дата створення: \t\t{CreationDate.ToShortDateString()}\n Кількість звернень: \t\t{AccessCount}\n Розмір файлу:    \t\t{Size} байт";
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}
