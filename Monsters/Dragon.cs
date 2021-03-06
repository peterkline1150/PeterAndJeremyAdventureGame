using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;

namespace Monsters
{
    public class Dragon : IMonster
    {
        public string Name { get; }
        public int CoinsDropped { get; }
        public int Health { get; set; }
        public int Strength { get; set; }
        public int Defence { get; set; }
        public int Accuracy { get; set; }
        public int CritChance { get; set; }
        public int ExperienceDropped { get; set; }
        public Dragon()
        {
            Name = "Dragon";
            CoinsDropped = 30;
            Health = 200;
            Strength = 50;
            Defence = 50;
            Accuracy = 90;
            CritChance = 5;
            ExperienceDropped = 200;
        }

        public void MakeNoise()
        {
            SoundPlayer playDragonNoise = new SoundPlayer("DragonNoise.wav");
            playDragonNoise.Load();
            playDragonNoise.Play();
        }

        public void DyingNoise()
        {
            SoundPlayer playDragonDying = new SoundPlayer("DragonDyingSound.wav");
            playDragonDying.Load();
            playDragonDying.Play();
        }
    }
}
