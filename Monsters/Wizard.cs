using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;

namespace Monsters
{
    public class Wizard : IMonster
    {
        public string Name { get; }
        public int CoinsDropped { get; }
        public int Health { get; set; }
        public int Strength { get; set; }
        public int Defence { get; set; }
        public int Accuracy { get; set; }
        public int CritChance { get; set; }
        public int ExperienceDropped { get; set; }
        public Wizard()
        {
            Name = "Wizard";
            CoinsDropped = 50;
            Health = 200;
            Strength = 150;
            Defence = 100;
            Accuracy = 80;
            CritChance = 30;
            ExperienceDropped = 400;
        }

        public void MakeNoise()
        {
            SoundPlayer playWizardNoise = new SoundPlayer("WizardNoise.wav");
            playWizardNoise.Load();
            playWizardNoise.Play();
        }

        public void DyingNoise()
        {
            SoundPlayer playWizardDying = new SoundPlayer("WizardDyingSound.wav");
            playWizardDying.Load();
            playWizardDying.Play();
        }
    }
}
