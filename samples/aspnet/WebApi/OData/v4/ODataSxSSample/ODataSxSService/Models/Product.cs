using System;

namespace ODataSxSService.Models
{
    public class Product
    {
        public int Id
        {
            get;
            set;
        }

        public string Title
        {
            get; set;
        }

        public DateTime ManufactureDateTime
        {
            get;
            set;
        }
    }
}