using System;
using System.ComponentModel.DataAnnotations;

namespace ODataActionsSample.Models
{
    public class Movie
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public DateTime? DueDate { get; set; }

        [Timestamp]
        public byte[] TimeStamp { get; set; }   

        public bool IsCheckedOut 
        {
            get { return DueDate.HasValue; }
        }
    }
}