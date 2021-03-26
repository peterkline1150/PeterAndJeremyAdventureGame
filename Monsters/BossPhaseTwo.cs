using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monsters
{
    public class BossPhaseTwo : IMonster
    {
        public string Name { get; }
        public int CoinsDropped { get; }
        public int Health { get; set; }
        public int Strength { get; set; }
        public int Defence { get; set; }
        public int Accuracy { get; set; }
        public int CritChance { get; set; }
        public int ExperienceDropped { get; set; }
        public BossPhaseTwo()
        {
            Name = "Mighty Minotaur";
            CoinsDropped = 1000;
            Health = 2000;
            Strength = 400;
            Defence = 200;
            Accuracy = 60;
            CritChance = 25;
            ExperienceDropped = 1000;
        }

        public void MakeNoise()
        {

        }

        public void DyingNoise()
        {

        }
    }
}
