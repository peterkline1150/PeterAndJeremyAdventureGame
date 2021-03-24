using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monsters
{
    public interface IMonster
    {
        string Name { get; }
        int CoinsDropped { get; }
        int Health { get; set; }
        int Strength { get; set; }
        int Defence { get; set; }
        int Accuracy { get; set; }
        int CritChance { get; set; }
        int ExperienceDropped { get; set; }
        void MakeNoise();
        void DyingNoise();
    }
}
