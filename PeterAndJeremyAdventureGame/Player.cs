using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace PeterAndJeremyAdventureGame
{
    public class Player
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; }
        public int Strength { get; set; }
        public int MaxHealth { get; set; }
        public int Health { get; set; }
        public int Defence { get; set; }
        public int Accuracy { get; set; }
        public int CritChance { get; set; }
        public int TotalCoins { get; set; }
        public int NumberOfKeys { get; set; }
        public bool HasDragonProtection { get; set; }
        private string PlayerMarker;
        private ConsoleColor PlayerColor;

        public Player()
        {

        }

        public Player(int strength, int health, int maxHealth, int defence, int accuracy, int critChance, int initialXPosition, int initialYPosition)
        {
            Strength = strength;
            Health = health;
            MaxHealth = maxHealth;
            Defence = defence;
            Accuracy = accuracy;
            CritChance = critChance;
            X = initialXPosition;
            Y = initialYPosition;
            HasDragonProtection = false;
            Level = 0;
            Experience = 0;
            TotalCoins = 0;
            NumberOfKeys = 1;
            PlayerMarker = "@";
            PlayerColor = ConsoleColor.Yellow;
        }

        public void Draw()
        {
            ForegroundColor = PlayerColor;
            SetCursorPosition(X, Y);
            Write(PlayerMarker);
            ResetColor();
        }
    }
}
