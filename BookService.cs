using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace LibraryManager
{
    public class BookService
    {
        private const string FilePath = "books.json";

        public List<Book> LoadBooks()
        {
            if (!File.Exists(FilePath)) return new List<Book>();

            var json = File.ReadAllText(FilePath);
            return JsonConvert.DeserializeObject<List<Book>>(json);
        }

        public void SaveBooks(List<Book> books)
        {
            var json = JsonConvert.SerializeObject(books, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }
    }
}
