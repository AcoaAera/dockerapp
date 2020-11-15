using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace s3bucketapp.Models
{
    public class Item
    {
        public Item(string name, float confidence)
        {
            Name = name;
            Confidence = confidence;
        }

        public string Name { get; set; }
        public float Confidence { get; set; }

    }

    public static class ListItems
    {
        public static List<Item> list = new List<Item>();
    }
}
