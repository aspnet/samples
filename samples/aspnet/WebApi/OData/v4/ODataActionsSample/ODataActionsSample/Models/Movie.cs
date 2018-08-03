using System;

namespace ODataActionsSample.Models
{
    public class Movie
    {
        public int ID { get; set; }
        
        public string Title { get; set; }
        
        public int Year { get; set; }
        
        public DateTimeOffset? DueDate { get; set; }

        public bool IsCheckedOut 
        {
            get { return DueDate.HasValue; }
        }
    }
}