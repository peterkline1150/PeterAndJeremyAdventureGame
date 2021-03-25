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
            CoinsDropped = 10;
            Health = 50;
            Strength = 40;
            Defence = 20;
            Accuracy = 40;
            CritChance = 0;
            ExperienceDropped = 50;
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
