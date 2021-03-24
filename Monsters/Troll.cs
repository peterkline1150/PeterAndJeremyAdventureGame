using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;

namespace Monsters
{
    public class Troll : IMonster
    {
        public string Name { get; }
        public int CoinsDropped { get; }
        public int Health { get; set; }
        public int Strength { get; set; }
        public int Defence { get; set; }
        public int Accuracy { get; set; }
        public int CritChance { get; set; }
        public int ExperienceDropped { get; set; }
        public Troll()
        {
            Name = "Troll";
            CoinsDropped = 50;
            Health = 10;
            Strength = 15;
            Defence = 15;
            Accuracy = 50;
            CritChance = 10;
            ExperienceDropped = 100;
        }

        public void MakeNoise()
        {
            SoundPlayer playTrollNoise = new SoundPlayer("TrollNoise.wav");
            playTrollNoise.Load();
            playTrollNoise.Play();
        }

        public void DyingNoise()
        {
            SoundPlayer playTrollDying = new SoundPlayer("TrollDyingSound.wav");
            playTrollDying.Load();
            playTrollDying.Play();
        }
    }
}
