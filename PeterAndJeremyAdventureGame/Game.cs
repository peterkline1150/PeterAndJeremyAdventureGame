using MerchantItems;
using Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace PeterAndJeremyAdventureGame
{
    public class Game
    {
        private World adventureWorld;
        private Player user;
        private ItemRepository _itemRepo = new ItemRepository();
        public void Start()
        {
            Title = "Adventure Game";
            CursorVisible = false;

            string[,] grid =
            {
                {"┌", "─", "┬", "─", "─", "─", "┬", "─", "─", "─", "┬", "─", "┬", "─", "─", "┬", "─", "─", "─", "─", "─", "┬", "─", "┬", "─", "┬", "─", "─", "─", "┐", },
                {"│", "X", "│", " ", " ", " ", "│", "$", " ", " ", "│", "M", "│", " ", " ", "│", "$", " ", " ", " ", " ", "│", " ", "│", "$", "│", " ", " ", " ", "│", },
                {"│", " ", "│", " ", "│", " ", "/", " ", " ", "O", "/", " ", "/", " ", " ", "├", "─", "┐", " ", "│", "P", "│", " ", "│", " ", "+", " ", "W", " ", "│", },
                {"│", " ", "│", " ", "│", " ", "│", " ", " ", " ", "│", " ", "│", " ", " ", "│", "M", "│", " ", "└", "─", "┤", " ", "│", " ", "│", " ", " ", " ", "│", },
                {"│", "M", "│", " ", "├", "─", "┴", "─", "─", "─", "┘", " ", "│", " ", " ", "│", " ", "│", " ", " ", " ", "+", " ", "│", " ", "└", "─", "─", "─", "┤", },
                {"│", " ", "│", " ", "│", "P", " ", " ", " ", " ", " ", " ", "├", "─", "┬", "┘", " ", "│", " ", "┌", "─", "┤", " ", "│", " ", " ", " ", " ", "$", "│", },
                {"│", " ", "│", " ", "└", "─", "─", "┬", "─", " ", "┌", "─", "┘", "D", "│", " ", " ", " ", " ", "│", "K", "│", " ", "└", "─", "+", "─", "─", "─", "┤", },
                {"│", " ", "│", "T", " ", " ", " ", "│", " ", " ", "│", " ", " ", " ", "│", " ", "┌", "─", "─", "┘", " ", "│", " ", " ", " ", " ", " ", " ", "$", "│", },
                {"├", "/", "┴", "─", "─", "┐", " ", "│", " ", "┌", "┘", " ", " ", " ", "/", " ", "│", " ", " ", " ", "O", "├", "─", "─", "─", "─", "─", "┬", "─", "┤", },
                {"│", "T", " ", " ", "$", "│", " ", "│", " ", "│", "T", " ", " ", " ", "│", " ", "│", " ", "┌", "─", "─", "┘", " ", " ", " ", "O", "$", "│", "P", "│", },
                {"│", " ", " ", " ", " ", "│", " ", "│", " ", "└", "─", "─", "─", "┬", "┘", " ", "│", " ", "│", " ", " ", " ", " ", " ", " ", "┌", "─", "┘", " ", "│", },
                {"│", " ", " ", " ", " ", "│", " ", " ", " ", " ", " ", " ", "$", "│", " ", " ", "│", " ", "│", " ", "┌", "─", "─", "─", "/", "┤", " ", "D", " ", "│", },
                {"├", "─", "─", "/", "┬", "┘", " ", "│", " ", "┌", "─", "─", "─", "┘", " ", " ", " ", " ", "│", " ", "│", " ", " ", "D", " ", "│", " ", "│", " ", "│", },
                {"│", " ", " ", " ", "│", " ", " ", "│", " ", "│", " ", " ", " ", " ", " ", "┌", "─", " ", "│", " ", "│", "T", " ", " ", " ", "│", " ", "│", " ", "│", },
                {"│", " ", "┌", "─", "┴", "─", "─", "┘", " ", "│", " ", "┌", "─", "─", "┬", "┘", " ", " ", " ", " ", "└", "┬", "─", "─", "─", "┤", " ", "│", " ", "│", },
                {"│", " ", "│", " ", " ", " ", " ", " ", " ", "│", " ", "│", "O", "$", "│", " ", " ", "│", " ", " ", " ", "│", " ", " ", " ", "│", " ", "│", " ", "│", },
                {"│", " ", " ", " ", "│", "P", "┌", "─", "┬", "┘", " ", "│", " ", "O", "│", " ", "┌", "┘", " ", " ", " ", "/", " ", " ", "O", "/", " ", "│", " ", "│", },
                {"│", " ", "│", " ", "└", "─", "┘", "G", "│", " ", " ", "/", " ", " ", "│", " ", "│", " ", " ", "│", " ", "│", " ", " ", " ", "│", " ", "│", " ", "│", },
                {"│", "K", "│", " ", " ", " ", " ", " ", " ", " ", "P", "│", "$", " ", "│", " ", "│", " ", " ", "│", " ", "│", " ", " ", " ", "│", "M", "│", "K", "│", },
                {"└", "─", "┴", "─", "─", "─", "─", "─", "─", "─", "─", "┴", "─", "─", "┴", "─", "┴", "─", "─", "┴", "─", "┴", "─", "─", "─", "┴", "─", "┴", "─", "┘", },
            };

            adventureWorld = new World(grid);
            user = new Player(20, 55, 55, 10, 70, 1, 1, 2);

            SeedMerchantItems();

            //Tell the user the controls and what the objective is
            DisplayIntro();

            RunGameLoop();
        }

        private void RunGameLoop()
        {
            while (user.Health > 0)
            {
                //Draw everything
                DrawFrame();

                //Check for player input, move the player as needed
                HandlePlayerInput();

                //Check if the player has reached certain items on the map
                string elementAtPlayerPos = adventureWorld.GetElementAt(user.X, user.Y);
                if (elementAtPlayerPos == "X")
                {
                    //OgreEncounter();
                    //TrollEncounter();
                    //DragonEncounter();
                    //WizardEncounter();
                    MerchantEncounter();
                }

                //See if the user has enough experience to level up
                if (user.Level < 10)
                {
                    int xpToLevelUp = GetXPToLevelUp();
                    if (user.Experience >= xpToLevelUp)
                    {
                        LevelUp();
                        PressAnyKey();
                    }
                }

                //Render the Console
                System.Threading.Thread.Sleep(20);

            }

            //Display the outro
            DisplayOutro();
        }

        private int GetXPToLevelUp()
        {
            int xpNeededToLevelUp;
            switch (user.Level)
            {
                case 0:
                    xpNeededToLevelUp = 100;
                    break;
                case 1:
                    xpNeededToLevelUp = 300;
                    break;
                case 2:
                    xpNeededToLevelUp = 600;
                    break;
                case 3:
                    xpNeededToLevelUp = 1100;
                    break;
                case 4:
                    xpNeededToLevelUp = 1700;
                    break;
                case 5:
                    xpNeededToLevelUp = 2500;
                    break;
                case 6:
                    xpNeededToLevelUp = 3500;
                    break;
                case 7:
                    xpNeededToLevelUp = 6000;
                    break;
                case 8:
                    xpNeededToLevelUp = 11000;
                    break;
                case 9:
                default:
                    xpNeededToLevelUp = 21000;
                    break;
            }
            return xpNeededToLevelUp;
        }

        private void LevelUp()
        {
            WriteLine("Congratulations, you have earned enough experience to gain a level!\n" +
                "Choose which stat you wish to increase:\n" +
                "1: Health\n" +
                "2: Strength\n" +
                "3: Defence\n" +
                "4: Accuracy\n" +
                "5: Critical Strike Chance\n");

            int choice = int.Parse(ReadLine());

            switch (choice)
            {
                case 1:
                    user.Level++;
                    WriteLine($"You have leveled up! You are now level {user.Level}");
                    user.MaxHealth += user.Level * 10;
                    WriteLine($"You have increased your Total Health\n" +
                        $"Your total health is now {user.MaxHealth}");
                    break;
                case 2:
                    user.Level++;
                    WriteLine($"You have leveled up! You are now level {user.Level}");
                    user.Strength += user.Level * 5;
                    WriteLine($"You have increased your Total Strength\n" +
                        $"Your total strength is now {user.Strength}");
                    break;
                case 3:
                    user.Level++;
                    WriteLine($"You have leveled up! You are now level {user.Level}");
                    user.Defence += user.Level * 5;
                    WriteLine($"You have increased your Total Defence\n" +
                        $"Your total defence is now {user.Defence}");
                    break;
                case 4:
                    user.Level++;
                    WriteLine($"You have leveled up! You are now level {user.Level}");
                    user.Accuracy += user.Level * 2;
                    WriteLine($"You have increased your Accuracy\n" +
                        $"Your accuracy is now {user.Accuracy}");
                    break;
                case 5:
                    user.Level++;
                    WriteLine($"You have leveled up! You are now level {user.Level}");
                    user.CritChance += user.Level * 2;
                    WriteLine($"You have increased your Citical String Chance\n" +
                        $"Your Critical Strike Chance is now {user.CritChance}%");
                    break;
                default:
                    WriteLine("Invalid input, please try again.");
                    LevelUp();
                    break;
            }
        }

        private void MerchantEncounter()
        {
            Clear();

            DrawMerchant();

            WriteLine("\nWelcome to my shop, traveler! Please choose from my selection of wares:\n");

            List<Item> listOfItems = _itemRepo.ReturnListOfItems();

            Random rng = new Random();
            int randOne = rng.Next(0, 10);
            int randTwo = rng.Next(0, 10);
            int randThree = rng.Next(0, 10);
            while (randTwo == randOne)
            {
                randTwo = rng.Next(0, 10);
            }

            while (randThree == randOne || randThree == randTwo)
            {
                randThree = rng.Next(0, 10);
            }

            List<Item> refinedListOfItems = new List<Item>();
            refinedListOfItems.Add(listOfItems[randOne]);
            refinedListOfItems.Add(listOfItems[randTwo]);
            refinedListOfItems.Add(listOfItems[randThree]);

            WriteLine("Which item would you like to purchase?\n" +
                $"You have a total of {user.TotalCoins} coins.\n\n");

            int count = 0;
            foreach (Item item in refinedListOfItems)
            {
                count++;
                WriteLine($"{count}: {item.Name} / {item.Description}\n");
            }

            string choice = ReadLine();

            while (choice != "1" && choice != "2" && choice != "3")
            {
                WriteLine("Invalid input! Please enter another number.\n");
                choice = ReadLine();
            }

            int choiceOfItem = int.Parse(choice) - 1;

            if (user.TotalCoins >= refinedListOfItems[choiceOfItem].Cost)
            {
                switch (refinedListOfItems[choiceOfItem].Name)
                {
                    case "Minor Health Potion":
                        WriteLine($"You purchased a Minor Health Potion. You’ve restored {refinedListOfItems[choiceOfItem].Health} Health.\n");
                        user.Health += refinedListOfItems[choiceOfItem].Health;
                        if (user.Health > user.MaxHealth)
                        {
                            WriteLine("Your health was already very high, so you could not fully utilize the potion.\n");
                            user.Health = user.MaxHealth;
                        }
                        WriteLine($"Your health is now {user.Health}.\n");
                        break;
                    case "Greater Health Potion":
                        WriteLine($"You purchased a Greater Health Potion. You’ve restored {refinedListOfItems[choiceOfItem].Health} Health.\n");
                        user.Health += refinedListOfItems[choiceOfItem].Health;
                        if (user.Health > user.MaxHealth)
                        {
                            WriteLine("Your health was already very high, so you could not fully utilize the potion.\n");
                            user.Health = user.MaxHealth;
                        }
                        WriteLine($"Your health is now {user.Health}.\n");
                        break;
                    case "Longsword":
                        WriteLine($"You purchased a Longsword. You’ve increased your Strength by {refinedListOfItems[choiceOfItem].Strength}.\n");
                        user.Strength += refinedListOfItems[choiceOfItem].Strength;
                        WriteLine($"Your strength is now {user.Strength}.\n");
                        break;
                    case "Battleaxe":
                        WriteLine($"You purchased a Battleaxe. You’ve increased your Strength by {refinedListOfItems[choiceOfItem].Strength}.\n");
                        user.Strength += refinedListOfItems[choiceOfItem].Strength;
                        WriteLine($"Your strength is now {user.Strength}.\n");
                        break;
                    case "Recurve Bow":
                        WriteLine($"You purchased a Recurve Bow. You’ve increased your Accuracy by {refinedListOfItems[choiceOfItem].Accuracy}.\n");
                        user.Accuracy += refinedListOfItems[choiceOfItem].Accuracy;
                        WriteLine($"Your accuracy is now {user.Accuracy}%.\n");
                        break;
                    case "Chestplate":
                        WriteLine($"You purchased a Chestplate. You’ve increased your Defence by {refinedListOfItems[choiceOfItem].Defence}.\n");
                        user.Defence += refinedListOfItems[choiceOfItem].Defence;
                        WriteLine($"Your defence is now {user.Defence}.\n");
                        break;
                    case "Platelegs":
                        WriteLine($"You purchased a pair of Platelegs. You’ve increased your Defence by {refinedListOfItems[choiceOfItem].Defence}.\n");
                        user.Defence += refinedListOfItems[choiceOfItem].Defence;
                        WriteLine($"Your defence is now {user.Defence}.\n");
                        break;
                    case "Full Helmet":
                        WriteLine($"You purchased a Full Helmet. You’ve increased your Defence by {refinedListOfItems[choiceOfItem].Defence}.\n");
                        user.Defence += refinedListOfItems[choiceOfItem].Defence;
                        WriteLine($"Your defence is now {user.Defence}.\n");
                        break;
                    case "Potion of Luck":
                        WriteLine($"You purchased a Potion of Luck. You’ve increased your Chance for Critical Damage by {refinedListOfItems[choiceOfItem].CritChance}%.\n");
                        user.CritChance += refinedListOfItems[choiceOfItem].CritChance;
                        WriteLine($"Your critical strike chance is now {user.CritChance}%.\n");
                        break;
                    case "2 Handed Sword":
                        WriteLine($"You purchased a 2 Handed Sword. You’ve increased your Strength by {refinedListOfItems[choiceOfItem].Strength}.\n");
                        user.Strength += refinedListOfItems[choiceOfItem].Strength;
                        WriteLine($"Your strength is now {user.Strength}.\n");
                        break;
                }

                user.TotalCoins -= refinedListOfItems[choiceOfItem].Cost;
                WriteLine($"You now have {user.TotalCoins} coins remaining.\n");
            }
            else
            {
                WriteLine("Are you trying to rip me off?!?!?!? Get out of my store!\n");
            }
            PressAnyKey();
        }

        private void WizardEncounter()
        {
            IMonster wizardEnemy = new Wizard();

            Clear();

            DrawMonster(wizardEnemy);

            MonsterEncounter(wizardEnemy);
        }

        private void DragonEncounter()
        {
            IMonster dragonEnemy = new Dragon();

            Clear();

            DrawMonster(dragonEnemy);

            MonsterEncounter(dragonEnemy);
        }

        private void OgreEncounter()
        {
            IMonster ogreEnemy = new Ogre();

            Clear();

            DrawMonster(ogreEnemy);

            MonsterEncounter(ogreEnemy);
        }

        private void TrollEncounter()
        {
            IMonster trollEnemy = new Troll();

            Clear();

            DrawMonster(trollEnemy);

            MonsterEncounter(trollEnemy);
        }

        private void MonsterEncounter(IMonster monster)
        {
            WriteLine($"A wild {monster.Name} appears!\n");
            monster.MakeNoise();
            PressAnyKey();

            while (user.Health > 0 && monster.Health > 0)
            {
                Clear();

                WriteLine("What would you like to do?\n" +
                    $"1: Attack the {monster.Name}\n" +
                    $"2: Flee from the {monster.Name}\n\n" +
                    $"Enemy health: {monster.Health}\n" +
                    $"Your Health: {user.Health}\n");

                string userChoice = ReadLine();
                switch (userChoice)
                {
                    case "1":
                        AttackMonster(monster);
                        break;
                    case "2":
                        FleeFromMonster(monster);
                        break;
                    default:
                        break;
                }
            }

            if (user.Health <= 0)
            {
                YouAreDead();
            }
            else
            {
                Clear();

                user.Experience += monster.ExperienceDropped;
                user.TotalCoins += monster.CoinsDropped;

                monster.DyingNoise();
                DrawMonster(monster);
                WriteLine($"\nCongratulations, you managed to kill the {monster.Name}!\n\n" +
                    $"{monster.CoinsDropped} Coins have been added to your coin pouch.\n" +
                    $"You now have a total of {user.TotalCoins} Coins.\n\n" +
                    $"You gain {monster.ExperienceDropped} experience points.\n" +
                    $"You now have a total of {user.Experience} experience points.\n");

                adventureWorld.DeleteElementAtLocation(user.X, user.Y);

                PressAnyKey();
            }
        }

        private void AttackMonster(IMonster monster)
        {
            Random rng = new Random();

            int randomNum = rng.Next(0, 101);

            if (randomNum >= 0 && randomNum <= user.Accuracy)
            {
                if (user.Strength > monster.Defence)
                {
                    int crit = rng.Next(0, 101);

                    if (crit >= 0 && crit <= user.CritChance)
                    {
                        monster.Health -= Convert.ToInt32(1.5 * (user.Strength - monster.Defence));
                        WriteLine($"Your attack critically strikes! You deal {Convert.ToInt32(1.5 * (user.Strength - monster.Defence))} damage to the {monster.Name}!\n");
                        PressAnyKey();
                    }
                    else
                    {
                        monster.Health -= user.Strength - monster.Defence;
                        WriteLine($"Your attack lands! You deal {(user.Strength - monster.Defence)} damage to the {monster.Name}!\n");
                        PressAnyKey();
                    }
                }
                else
                {
                    WriteLine($"You are not strong enough to penetrate the {monster.Name}'s defences! You deal no damage!\n");
                    PressAnyKey();
                }
            }
            else
            {
                WriteLine("Your attack misses! You deal no damage.\n");
                PressAnyKey();
            }

            if (user.HasDragonProtection == false && monster.Name == "Dragon")
            {
                user.Health -= 2 * monster.Strength;
                WriteLine($"The {monster.Name} fries you with its fiery breath!\n" +
                    $"You have no dragon protection, so the dragon deals {2 * monster.Strength} Damage!\n");

                PressAnyKey();
            }
            else
            {
                int enemyRandomNum = rng.Next(0, 101);
                if (monster.Health > 0)
                {
                    if (enemyRandomNum >= 0 && enemyRandomNum <= monster.Accuracy)
                    {
                        if (monster.Strength > user.Defence)
                        {
                            int monsterCrit = rng.Next(0, 101);

                            if (monsterCrit >= 0 && monsterCrit <= monster.CritChance)
                            {
                                user.Health -= Convert.ToInt32(1.5 * (monster.Strength - user.Defence));
                                WriteLine($"The {monster.Name} deals an especially fatal blow!\n" +
                                    $"You take {Convert.ToInt32(1.5 * (monster.Strength - user.Defence))} damage!\n");
                                PressAnyKey();
                            }
                            else
                            {
                                user.Health -= monster.Strength - user.Defence;
                                WriteLine($"The {monster.Name} hits you viciously! You take {monster.Strength - user.Defence} damage!\n");
                                PressAnyKey();
                            }
                        }
                        else
                        {
                            WriteLine($"The {monster.Name} is not strong enough to penetrate your defences! You take no damage!\n");
                            PressAnyKey();
                        }
                    }
                    else
                    {
                        WriteLine($"The {monster.Name} misses you, dealing no damage!\n");
                        PressAnyKey();
                    }
                }
            }
        }

        private void FleeFromMonster(IMonster monster)
        {
            Random rng = new Random();
            int randomNum = rng.Next(0, 101);

            if (randomNum >= 40)
            {
                WriteLine($"You manage to flee from the {monster.Name} without taking any damage!\n");

                PressAnyKey();
                RunGameLoop();
            }
            else
            {
                user.Health -= monster.Strength;
                WriteLine($"The {monster.Name} attacks you while you are running and deals {monster.Strength} damage!\n");
                PressAnyKey();
            }
        }

        private void PressAnyKey()
        {
            WriteLine("\nPress any key to continue...\n");

            ReadKey(true);
        }

        private void YouAreDead()
        {
            Clear();

            WriteLine("Oh dear, it appears you are dead...");
            PressAnyKey();
        }

        private void HandlePlayerInput()
        {
            ConsoleKeyInfo keyInfo = ReadKey(true);
            ConsoleKey key = keyInfo.Key;
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    if (adventureWorld.IsPositionWalkable(user.X, user.Y - 1))
                    {
                        user.Y -= 1;
                    }
                    break;
                case ConsoleKey.DownArrow:
                    if (adventureWorld.IsPositionWalkable(user.X, user.Y + 1))
                    {
                        user.Y += 1;
                    }
                    break;
                case ConsoleKey.LeftArrow:
                    if (adventureWorld.IsPositionWalkable(user.X - 1, user.Y))
                    {
                        user.X -= 1;
                    }
                    break;
                case ConsoleKey.RightArrow:
                    if (adventureWorld.IsPositionWalkable(user.X + 1, user.Y))
                    {
                        user.X += 1;
                    }
                    break;
                default:
                    break;
            }
        }

        private void DrawFrame()
        {
            Clear();
            adventureWorld.Draw();
            user.Draw();
        }

        private void DisplayIntro()
        {
            ForegroundColor = ConsoleColor.DarkCyan;
            WriteLine("\n ------------------------------------------------\n" +
                "| Welcome to Lothar, the Console Adventure Game! |\n" +
                " ------------------------------------------------\n\n" +
                "The rules are simple, make it to the center and save the princess (marked by O)!\n\n" +
                "However, your journey will not be so simple. There are multiple enemies that are hiding in the darkness.\n\n" +
                "If you step on them, they will become hostile and attack you!\n\n" +
                "Navigate through the room and find the key (marked by /) to open the door further into the dungeon.\n\n" +
                "Once you reach the center, you will find a boss waiting for you. Defeat this boss and you will be able to save\n" +
                "the princess!\n\n\n\n\n");

            ResetColor();

            WriteLine("Press any key to continue to the game...");

            ReadKey(true);
        }

        private void DisplayOutro()
        {
            ForegroundColor = ConsoleColor.Red;
            WriteLine("T");
            ForegroundColor = ConsoleColor.Blue;
            WriteLine("h");
            ForegroundColor = ConsoleColor.Green;
            WriteLine("a");
            ForegroundColor = ConsoleColor.Yellow;
            WriteLine("n");
            ForegroundColor = ConsoleColor.Cyan;
            WriteLine("k");
            ForegroundColor = ConsoleColor.Magenta;
            WriteLine("s");
            ForegroundColor = ConsoleColor.White;
            WriteLine(" ");
            WriteLine("F");
            ForegroundColor = ConsoleColor.Red;
            WriteLine("o");
            ForegroundColor = ConsoleColor.Yellow;
            WriteLine("r");
            ForegroundColor = ConsoleColor.DarkBlue;
            WriteLine(" ");
            WriteLine("P");
            ForegroundColor = ConsoleColor.DarkCyan;
            WriteLine("l");
            ForegroundColor = ConsoleColor.Green;
            WriteLine("a");
            ForegroundColor = ConsoleColor.Red;
            WriteLine("y");
            ForegroundColor = ConsoleColor.DarkYellow;
            WriteLine("i");
            ForegroundColor = ConsoleColor.Blue;
            WriteLine("n");
            ForegroundColor = ConsoleColor.Magenta;
            WriteLine("g");
            ForegroundColor = ConsoleColor.Gray;
            WriteLine("!");

            ResetColor();
            WriteLine("Press any key to exit the console...");
            ReadKey(true);
        }

        private void DrawMonster(IMonster monster)
        {
            if (monster.Name == "Troll")
            {
                WriteLine(@"
..777ZO=.         ...                   
.7...ZO+.       . Z~Z..                 
7..D$?Z+Z.     .II7II?~.                
=. OD~NDD.   ..7Z8DOZ7=I.....           
I.M$ZI88.   .,D?O$$Z8ZN=,O7I+...        
.87D$DN.. ..+=OOOI77$$+=N8$$=ZN..       
   D7DDZ...+IOIZOOZ8D8DMD8Z7D7OI+..     
  ..I7ZZI7+7Z7I7+7O7DNNN77$MI$7O$=.     
    ?+??ZZ78DN77=77ZDD7$+ID$OZ7887.     
   .I=+7ZO8ODN7IN=~+7=~I?I$OOIZ88D+.    
    OD+$ZOOD..,I$:=+I?:7IDI$8OIIIZ?:    
    .78ZOD..  .I$D=+==O$$8+$ZDO??IZ==.  
     ....      .Z8I~~:$ZZD78D7..?I$ZI?. 
      I         .O7~=:Z$$N$ZDO?,$I7$?++ 
      +         .OIO77$ZZOO8O7?$D=$I$I. 
      .?       .ZO7$$7:I$88OZIO=+DDZ8.. 
       I.      .Z$Z$$I77$ZZOZ78NN88Z..  
       ..    ..ZZ7Z77IIIO$?87ZIO.ZZZ.   
       .O.  .$8OOOOZZ77$8DDIIOIZ$D.     
        .$.$7Z$8OZN87OZ$DD8ZZOD??..     
         ..7ZOOZZ$O8O$OZMDO8Z7$ZZ?.     
         .7ODN888DDDOOO.:DDOZ7?OD$:     
          .78NOONN?..O8..D8.8N7IDD.     
          ...7OOO8.. ........$7ZO8.     
            .$$OO7$.       .,:Z8D8.     
         ..Z$$8ZZOO.       I..ZO$O=     
          8ZZ8D7.=....O..:....ZZ$IO~..  
            .  .,.=~...  .....7D8IOZ.   
                   ..=...~.             ");
            }
            else if (monster.Name == "Ogre")
            {
                WriteLine(@"
                   ~                                        
            . ,   I                                         
           Z??=?+$=.                                        
            88~7$=?++:...                                   
  ....      ZDO===+Z.,.,...                                 
........  DD8OZ~Z7~7O.......                                
  .........Z88OO$N$=,,,....,,                               
   ....,..,:8Z~$$ZI.7....,,,,...,.     ..                   
     .,....887888..=.,,,,,,...,,,,...                       
      ,.....N$N=,=,.,,,,,,,,.,,..........                   
     .....,8$+,,,,,.,,,,,:,,,,,,.....,......                
      .....O8,,.,.,,,,,,,,,:,,,,,,..,,..........            
          N8,,,,,,,,,,,,,,,,,,,,,,,,,,..,....  .            
      ..O8O:,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,...            
       D7??+~,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,....          
       8ZZ$$DO~,,,,,,,....,:::~~=,,,,,,,:,,,,,,....         
      .$88DNZ7IZ7?OZ7+~+?==O7Z?OO7,,,,,,,,,,,,,,,.....      
       .D?8D888OOO77I?+?+ZOOZ,==7+++:,,,,,,,,,,,.,....      
       D$+,ZDDNO8I+$I++=ZOD8$7ZZD$++?I~,,,,,,,,,,.,..       
      ,87=.,$DN8NZZI?777$O8Z$Z7DNZ777+:=:,,,,,,,,,,...      
     .N.Z7..,,ZDNO$8$?=~O7DZ8DN8NOZ7=:+$7+7,,,,,,,,.....    
    .O$.O7 ....INDZN8?=~+O$D88DD7I?=$=+$+$+7+,,,,,,,  ..... 
        O?  ...,.,..O7ZOO$88OZI?+::I??O$?$O?~:,,,,,,,,..... 
       .Z$    ,,.,..8D7??$OO8O+=I===O$Z$I$I?IZ+,,,,..,.     
       .OZ.    .....888$IZ8ZZ??$77$ZZZ$++~?+==7+,,,,.,,. .  
        D+  .   ....ZO?+=~,7I$IZOZ8Z8DOZZZ$~~==I+,,,,,.,.   
        +7        :7ZDO?==++I$ZODDD88ON8DO8Z8$O8$....,,.,   
        :.    .8Z8..88Z7IO+?III$ZODD8DD=,NN8OIDO8?.......   
        + .7D+.$ . .~DZDNNNZ$ZZZ$888D8?.,IODZ$ODDN......    
                .  .DD8NDDDND8OO88DDDD?...NOZ8$D8N          
         .         .OND8D8DDDO8O8D8DDD$..7$$=$8D$           
                   .NNDDDNDDDD88DDDDDD,.=Z8?$7$ON:          
                  .OO8N8D8D8NDODDDDNDN?.Z~,=8O7I8?          
                   $7O?ZDD88ZND8DDNND8O,O.O$:ZDNN8          
                   O$$$Z8ZDDDD88DDDDDD88:DO DONMM8          
                  .DZ7??Z8DDDDDODDDNDDDO   N8DD,.Z          
                   O+?~=7ZDDD8OD88DDDDDD.       .  ...      
                   $O+:~?$8DDDZZ8NND8DDD.  .                
                   O7=:??$O,DDOO NDNDDND   .                
                   =7$+I8Z. DDZ  ONDDD8O.  .                
                   ?O8OOZD+ ,..  DODDNDD                    
                   DZ888D88      7N8Z$O~                    
                   OO7O88OO.     DDDNN.                     
                   .DNDONN~      ND88O.                     
                    788DD.       .888OO                     
                   .D88ZO..     .  DND8N8                   
                   .8ZOZ$8                                  
                   .ZI?$+I?                                 
                  ..8$7$D$$                                 
                     .:.~.                                  
                    . .                                     ");
            }
            else if (monster.Name == "Dragon")
            {
                WriteLine(@"
                                       ZZ$$7Z$ZOOZO888Z$?   
                                 .Z$$$$$Z777$7ZZOO888I      
                                 Z7Z$Z$$$ZZ$7$$$ZO88Z       
   .$8Z                    ?.  $$$$$$$ZZ$$Z$ZZOOO888=       
  .$7$$7ZZ$OO88O          I. .Z7Z?88ZZZZZZ7$ZOZZ88O87       
  $7OO$ZZ$ZZZ88.          ?$ZOZZZ$$$$$$$$$$OOO88O88=,8      
 .7$OZZZZZZZO8D$.         .OOOOO8OOOZ$$Z$$$$ZZ8D7.     Z    
  $ZZZZOZZZZZZ88$=....,:,,:+$OOZ$Z$$$$$ZZOZOO8DO            
.$Z$OZZZZO$ZZOOOD8~::::~~~~Z8$ZZO$Z$$$ZOO88O888.            
   7OOZZO8OZZZOO88=~~~~==~~$ZZZ$Z$O$ZZ$ZZZ88DDD8            
     $OOZZ$$ZZ8O8O~~~~~~~=~7Z$ZZZ$$ZOOZOZOO8.$7..           
      .7O8O$$$$O8D~~~~~=~~~$ZO$ZZZZZZZ8OOODDI    .          
       .:$ZOO$$$ZO8~~~:=~~~$8ZZZ$ZZZOOO88D8                 
       :::$7OZ88$$OO:~~~~~=$OOO$ZZZZOO8D88                  
      ~::,?IO$ZDI=ZZ,~~~~~$$ZZO$ZZZOO88O88.                 
      :~$$ZZOO$ZODZ::::::$$$OZZ$ZZOO888O8O.                 
      ,+~:+7OOZD,ZO:,:::~$Z8Z$$7ZO8O8I=                     
       ~+=?8:ZZOOO$?:::ZZ8OZ$$$$OOO8I++?+?                  
        ,~O??$Z8D7Z8::8OO8$ZZZ$OOOO=I$+??+?+                
          .$7$88OZZ88Z8ZO$$ZZ8OOO~++??===+==                
          =?7$888O8Z8O8$$$ZOO$ZO:~~=====++=:                
          =?I$O8OOO$D877$Z8O8ZZ~~~~==+++++~                 
     $I   $7I$O7$$DOO8$$O88OO:~~~~~:~~+++=                  
      I7. Z7ZZZ7$Z88O8O$OO: ::::::~~:~+++=+=:,              
    .:$8Z.$8ZO8ZOZZOOZDOOO .,:::::::~==+====~:,:            
        ZZO8.$Z$O7I$7ZZ8$8 .,,,::,:~~=~=+=~~::,,            
         Z$Z    ZOO7$IZZ$O$..,,,,,::~=~=+~:,,...            
                   78O7$ZZO$Z$      .::~,,,,..              
                   ZOO8OO$ZZZ8$                             
                   .OZ787OZZ$$8Z                            
                       Z7Z$8$$$7O        $Z8I$?$            
                       $$? .$?7Z$$      OZO$. .Z            
                       .,I  Z7?IZZ$    OZZ.   .             
                            .$  I78Z7OZ$$                   
                                 .Z$7OZI                    
                                     .                      ");
            }
            else if (monster.Name == "Wizard")
            {
                WriteLine(@"
                     IN:~+IIII~        
                 IM~==IIIIIIIIIZ=       
               N?IIIIIIIIM7I$II7I:.     
              NIIIIIIIIIIIZ$$$IMIIM     
             N7IIIIIIIIIII$$II$$$II$    
            N7IIIIIIIIIIZ$$$N.NI$ZI,    
           +MIIIIIIIIIZNM$$$N   7$I7    
         N7$$$$DMM8$$$$I+I$$N    I$?    
         .N7I=IIIIIIIII$$$$$$D   .7N    
        NIIIIIIIIIIIIIIII$$$N7     .    
    .N=IIIIIIIIIIIIIIIIII$$$$$IN        
NI~7IIIIMN7=Z,:M==?=N7$$$$$I$$$$$7N     
,ZZZZZZZZN=$?M=,,,==N?::NN$$$$$$$$$=DN. 
   NZZZZZZNNM=~,,,=N=~=NN~+N~MZZ8D8ZZZN 
       $NNNM::N===N?::,?NNN~MZZZNNN     
         .N++==MM?????=~~+++  ~7?=~N    
         NOI++++I?II++I=N?+:N Z??===8   
        .+++:++++++++=:++++:N NO7?7$ND  
        N+:,+++++=++::+:::$+  MNNN$M$.  
        .~+::++:+::+:::+::~: .7$N$$7    
        ::,+:+::+:=+::+:::MM  NMZOZN    
  ,,   ,==,::+::=:?:::+::~N$I  O$8ZN    
  , ,,. +:~=:?~:=:==:~+N?N$IIO.NOZON,   
  , , , N~==:==:=+=:+++=N$$IIIMNM87.,   
         N::::::+::=M=+8$$$I$NN+MNN     
          ~:::~+:::NNMI$?I$$OMN+M$~=    
          N+:++::=M$II$$N I$N~MM7:,N    
         .7+:+++O$IIIII$Z N7?==N8M=8.   
         M7Z++8NNMMMNN8ZN  M8ZZZO8NZN   
        NI$$N~NO$N$8ZZMNN  NNMZZO$ZZN   
       :=I$$:$$$7$O7$I$$I   +MZZNM$N?   
       NI$$$=IIII7IIIII$$N   ?ZZOONI    
       M=7$$=IIIIIIIIII7$7   .DZN8MD    
       ~:~DN?IIIIIIIIIII$$N    7M$N     
       :8?=MIIIIIIIIIIIII$7.    DZN     
       N=~NIIIIIIIIIIIIII$$N    N$      
       .   ~III7IIIIIIII$I$$~   :M?     
           =III$IIIIIIIIMIM$7   NN      
           ~III$$IIIIIIII$I$$D  ?Z      
          NIIII$$IIIIIIIINII$7  I7      
          MIIIIM7IIIIIIIII$IION. ZN     
         NMNZII$NIIIIIIIIIIM$I$N$ON     
       N=II7IIZ$$$IIIIIIIIII?Z$$$O7N.   
      8$$$III$N$MIIIIMIIIII?II$$$O7$$O  
      NZZZZZN. MN$7ZN?IIIIIIIII$$O7NN   
                ZZZZZZZM NNNNZ   OON    
                .  ZO                   ");
            }
        }

        private void DrawMerchant()
        {
            WriteLine(@"
                .MDND+      .,                                                  
                  ?.D       ?.                                                  
                 .=,,      =.                                                   
                .=?.      8. .                                                  
               .D==,8        ,                                                  
                D8III$   ,   8                                    .?.           
                +$ZOIM. ?.   D                                      ZD          
                D?++I7:,?.                                         . N8Z.       
      ...       Z$NMO78:,...,$M~..,.                               ,$OMNM       
   +888+$       OMI8OZDDM$ZI$:.  ....7I$?N8~. .                  .M8NZMM~?.     
   ~NO8D$. .  ,=?M+Z?N+?Z8N7ZN.             . I$Z+8$$7OI =,+ZDN=N8N+7DZZZ7.     
   =?=D7?+DZ8OII$7O$$N7$I?ND7I=.           .7=I+Z$D8IOZOZZD.+D8M7.O8NNMN8.~     
  :I8NNZNO7ZMZMMNDZ~7NOZ$I+OO7I7.           NI7ZDMID8DD:::8OMN8MO~778  +8I8     
  .ID=ONMZ=IZ8DM:N$OI.,:=~=.==+=:.,I+O,,8  :Z.+?+ N7NZDOON8ZMMMMMN+M    . .     
      MIOD: I7+I,DO7I$,+7+Z~?I,.,~.~,:.:IZ8=~+NNOM~MMMDMNN8NMNMZOMNM.           
       $?$7,?7Z$?+.,.I77=~IMMZ7.  . ..  ..M+   ONMOZZ$MMDO.MODDM8OD.............
      .. ~=~?=~Z$I7 NNNN,?? DDOZ           D.. $O7$..$+:~+?~8ZZOZO~~~~~~~~~~~~~~
      . ..O., :IO,O,$.~=MD.M .:D=         .8  87=~,  :~~=.~. $,$$~~~~~~~~~~~~~~~
    .,.....O.ZD: 8$ ~7=,7?~::.. ?. .. ,.:.  . 8Z::~=~=~~======8OO~=~~~~~~~~~~~~~
    ....,.,. :O...~M8:7D=O?+, .Z8,. ... ,:,::,DN.:.,..:,....,.ZN~..:~           
    ....,:...ZZ:,..=8N.N$M +==.I:~+:..,..,.,,~O? ~::,..,,,..~IMO?.=~:~..        
    ..,:,,....~::~7??.DD,$=M~7I,:,,:.~~,..:?+?NO7$~.,...==.,+IM8:,II~,:,..      
   .,.I?+=+7.=:+=?N+,7$N7.I?D:....~...:,.,::,:7IZ=..=,..=~..+?MM::+:,.+?=~.     
   ,~=+,.:~ZO$+=:=.7,?,+O...,~....~....==.,:+O+M~Z.+::~~~+..:++ON,+.....,=,..   
    .,+~,~:,.,,~=?+I.:=~:.,..,..,::~,..,+,.,,:::..==::,....?I=~MI$+,.:......    
    ..,.,.,,......,.. . .......~.:,.,,..,..,.~:....:..,...:,.~,..::..,....      
=,,.~:,,  ..     ...   . . ..      ....... .   ... .. ......,,: ,:.   .         ");
        }

        private void SeedMerchantItems()
        {
            Item firstItem = new Item("Minor Health Potion", "+20 Health - £20", 20, 0, 0, 0, 0, 20);
            Item secondItem = new Item("Greater Health Potion", "+50 Health - £40", 50, 0, 0, 0, 0, 40);
            Item thirdItem = new Item("Longsword", "+10 Strength - £20", 0, 10, 0, 0, 0, 20);
            Item fourthItem = new Item("Battleaxe", "+20 Strength - £40", 0, 20, 0, 0, 0, 40);
            Item fifthItem = new Item("Recurve Bow", "+10 Accuracy - £20", 0, 0, 0, 0, 10, 20);
            Item sixthItem = new Item("Chestplate", "+30 Defence - £70", 0, 0, 30, 0, 0, 70);
            Item seventhItem = new Item("Platelegs", "+20 Defence - £40", 0, 0, 20, 0, 0, 40);
            Item eighthItem = new Item("Full Helmet", "+10 Defence - £20", 0, 0, 10, 0, 0, 20);
            Item ninthItem = new Item("Potion of Luck", "+10 Chance for Critical Strike - £70", 0, 0, 0, 10, 0, 70);
            Item tenthItem = new Item("2 Handed Sword", "+30 Strength - £70", 0, 30, 0, 0, 0, 70);

            _itemRepo.AddToListOfItems(firstItem);
            _itemRepo.AddToListOfItems(secondItem);
            _itemRepo.AddToListOfItems(thirdItem);
            _itemRepo.AddToListOfItems(fourthItem);
            _itemRepo.AddToListOfItems(fifthItem);
            _itemRepo.AddToListOfItems(sixthItem);
            _itemRepo.AddToListOfItems(seventhItem);
            _itemRepo.AddToListOfItems(eighthItem);
            _itemRepo.AddToListOfItems(ninthItem);
            _itemRepo.AddToListOfItems(tenthItem);
        }
    }
}
