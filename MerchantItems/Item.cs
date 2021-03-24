using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerchantItems
{
    public class Item
    {
        public int Health { get; set; }
        public int Strength { get; set; }
        public int Defence { get; set; }
        public int CritChance { get; set; }
        public int Accuracy { get; set; }
        public Item()
        {

        }
        public Item(int health, int strength, int defence, int critChance, int accuracy)
        {
            Health = health;
            Strength = strength;
            Defence = defence;
            CritChance = critChance;
            Accuracy = accuracy;
        }
    }
}
