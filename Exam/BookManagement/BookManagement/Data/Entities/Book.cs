using System.Collections.Generic;

namespace BookManagement.Data.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public virtual HashSet<Genre> Genres { get; set; }
        public Book()
        {
            Genres = new HashSet<Genre>();
        }
    }
}