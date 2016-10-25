using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PressureHeight.ListHelper
{
    class DataHelper
    {
        public static ObservableCollection<Item> CreateItems()
        {
            ObservableCollection<Item> collection = new ObservableCollection<Item>();
            for (var i = 0; i < 100; i++)
            {
                collection.Add(new Item
                {
                    Title = "Title " + i.ToString(),
                    Category = DateTime.Now.ToString(),
                    Image = ""
                });
            }
            return collection;
        }
    }
}
