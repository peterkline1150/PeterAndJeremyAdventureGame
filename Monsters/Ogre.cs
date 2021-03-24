using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;

namespace Monsters
{
    public class Ogre : IMonster
    {
        public string Name { get; }
        public int CoinsDropped { get; }
        public int Health { get; set; }
        public int Strength { get; set; }
        public int Defence { get; set; }
        public int Accuracy { get; set; }
        public int CritChance { get; set; }
        public int ExperienceDropped { get; set; }
        public Ogre()
        {
            Name = "Ogre";
            CoinsDropped = 100;
            Health = 20;
            Strength = 50;
            Defence = 5;
            Accuracy = 30;
            CritChance = 1;
            ExperienceDropped = 100;
        }

        public void MakeNoise()
        {
            SoundPlayer playOgreNoise = new SoundPlayer("OgreNoise.wav");
            playOgreNoise.Load();
            playOgreNoise.Play();
        }

        public void DyingNoise()
        {
            SoundPlayer playOgreDying = new SoundPlayer("OgreDyingSound.wav");
            playOgreDying.Load();
            playOgreDying.Play();
        }
    }
}
