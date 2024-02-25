using System.Collections.Generic;

namespace BookManagement.Data.Entities
{
    public class Genre
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public virtual HashSet<Book> Books { get; set; }
        public Genre()
        {
            Books = new HashSet<Book>();
        }
    }
}
