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

        public Player(int initialXPosition, int initialYPosition)
        {
            Strength = 40;
            MaxHealth = 50;
            Health = 50;
            Defence = 30;
            Accuracy = 60;
            CritChance = 10;
            X = initialXPosition;
            Y = initialYPosition;
            HasDragonProtection = false;
            Level = 0;
            Experience = 0;
            TotalCoins = 20;
            NumberOfKeys = 0;
            PlayerMarker = "^";
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
