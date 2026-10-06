using System;
using System.Collections.Generic;
using System.Text;

namespace GameOPPStudy
{
    public class Inventory
    {
        private readonly List<Item> _items = new List<Item>();
        public int Capacity { get; }

        public Inventory(int capacity)
        {
            Capacity = capacity;
        }

        // 아이템 추가
        public bool AddItem(Item item)
        {
            if (_items.Count >= Capacity)
                return false;

            _items.Add(item);
            return true;
        }

        // 아이템 제거
        public bool RemoveItem(Item item)
        {
            return _items.Remove(item);
        }

        public IReadOnlyList<Item> GetItems() => _items;
    }

    public class Item
    {
        public string Name { get; set; }
        public string Description { get; set; }
        // 추가 사항 추가
    }
}
