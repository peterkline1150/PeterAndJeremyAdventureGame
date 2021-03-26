using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monsters
{
    public class BossPhaseOne : IMonster
    {
        public string Name { get; }
        public int CoinsDropped { get; }
        public int Health { get; set; }
        public int Strength { get; set; }
        public int Defence { get; set; }
        public int Accuracy { get; set; }
        public int CritChance { get; set; }
        public int ExperienceDropped { get; set; }
        public BossPhaseOne()
        {
            Name = "Boss";
            CoinsDropped = 0;
            Health = 1000;
            Strength = 200;
            Defence = 100;
            Accuracy = 80;
            CritChance = 20;
            ExperienceDropped = 0;
        }

        public void MakeNoise()
        {
            
        }

        public void DyingNoise()
        {
            
        }
    }
}
