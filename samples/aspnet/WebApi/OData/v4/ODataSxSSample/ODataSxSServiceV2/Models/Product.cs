using System;

namespace ODataSxSServiceV2.Models
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

        public DateTimeOffset ManufactureDateTime
        {
            get;
            set;
        }
    }
}