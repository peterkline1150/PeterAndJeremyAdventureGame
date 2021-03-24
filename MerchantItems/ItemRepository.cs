using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerchantItems
{
    public class ItemRepository
    {
        private readonly List<Item> _items = new List<Item>();

        public void AddToListOfItems(Item item)
        {
            _items.Add(item);
        }

        public List<Item> ReturnListOfItems()
        {
            return _items;
        }

        public void DeleteItem(Item item)
        {
            _items.Remove(item);
        }
    }
}
