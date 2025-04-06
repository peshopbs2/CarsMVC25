using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsMVC25.Data.Entities
{
    public class Car : BaseEntity
    {
        public string Brand { get; set; }
        public string ModelName { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }
        public string Fuel { get; set; }
        public string ImageUrl { get; set; }
    }
}
