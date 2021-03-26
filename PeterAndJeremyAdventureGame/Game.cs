using MerchantItems;
using Monsters;
using System;
using System.Collections.Generic;
using System.Media;
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
        private SoundPlayer playSound;
        bool hasFled = false;
        bool canLevelUp = true;

        public void Start()
        {
            Title = "Adventure Game";
            CursorVisible = false;

            string[,] grid =
            {
                {"┌", "─", "┬", "─", "─", "─", "┬", "─", "─", "─", "┬", "─", "┬", "─", "─", "┬", "─", "─", "─", "─", "─", "┬", "─", "┬", "─", "┬", "─", "─", "─", "┐", },
                {"│", " ", "│", " ", " ", " ", "│", "$", " ", " ", "│", "M", "│", " ", " ", "│", "$", " ", " ", " ", " ", "│", "$", "│", "$", "│", " ", " ", "@", "│", },
                {"│", " ", "│", " ", "│", " ", "/", " ", " ", "o", "/", " ", "/", " ", " ", "├", "─", "┐", " ", "│", "P", "│", "w", "│", " ", "+", "B", " ", " ", "│", },
                {"│", " ", "│", " ", "│", "t", "│", " ", " ", " ", "│", " ", "│", " ", " ", "│", "M", "│", " ", "└", "─", "┤", " ", "│", " ", "│", " ", " ", " ", "│", },
                {"│", "*", "│", " ", "├", "─", "┴", "─", "─", "─", "┘", " ", "│", "d", "K", "│", " ", "│", " ", " ", " ", "+", " ", "│", " ", "└", "─", "─", "─", "┤", },
                {"│", " ", "│", " ", "│", "P", " ", " ", " ", " ", " ", " ", "├", "─", "┬", "┘", " ", "│", " ", "┌", "─", "┤", " ", "│", " ", " ", " ", " ", "$", "│", },
                {"│", "M", "│", " ", "└", "─", "─", "┬", "─", " ", "┌", "─", "┘", "D", "│", " ", " ", " ", " ", "│", "$", "│", " ", "└", "─", "+", "─", "─", "─", "┤", },
                {"│", " ", "│", "t", " ", " ", " ", "│", " ", " ", "│", "$", " ", " ", "│", " ", "┌", "─", "─", "┘", " ", "│", " ", " ", " ", " ", " ", " ", "$", "│", },
                {"├", "/", "┴", "─", "─", "┐", " ", "│", " ", "┌", "┘", " ", " ", " ", "/", " ", "│", " ", " ", " ", "o", "├", "─", "─", "─", "─", "─", "┬", "─", "┤", },
                {"│", "T", " ", " ", "$", "│", " ", "│", " ", "│", "t", " ", " ", " ", "│", " ", "│", " ", "┌", "─", "─", "┘", " ", " ", " ", "o", "$", "│", "P", "│", },
                {"│", " ", " ", " ", " ", "│", " ", "│", " ", "└", "─", "─", "─", "┬", "┘", " ", "│", " ", "│", " ", " ", " ", " ", " ", " ", "┌", "─", "┘", " ", "│", },
                {"│", " ", " ", " ", " ", "│", " ", " ", " ", " ", " ", "t", "$", "│", " ", " ", "│", " ", "│", " ", "┌", "─", "─", "─", "/", "┤", " ", "d", " ", "│", },
                {"├", "─", "─", "/", "┬", "┘", " ", "│", " ", "┌", "─", "─", "─", "┘", " ", " ", " ", " ", "│", " ", "│", " ", " ", "D", " ", "│", " ", "│", " ", "│", },
                {"│", " ", " ", " ", "│", "$", "t", "│", " ", "│", " ", " ", "t", " ", " ", "┌", "─", " ", "│", " ", "│", "t", " ", " ", " ", "│", " ", "│", " ", "│", },
                {"│", " ", "┌", "─", "┴", "─", "─", "┘", " ", "│", " ", "┌", "─", "─", "┬", "┘", " ", " ", " ", " ", "└", "┬", "─", "─", "─", "┤", " ", "│", " ", "│", },
                {"│", " ", "│", " ", " ", " ", " ", " ", " ", "│", " ", "│", "o", "$", "│", " ", " ", "│", " ", " ", " ", "│", "o", " ", "$", "│", " ", "│", " ", "│", },
                {"│", " ", " ", " ", "│", "P", "┌", "─", "┬", "┘", " ", "│", " ", " ", "│", " ", "┌", "┘", " ", " ", " ", "/", " ", " ", " ", "/", " ", "│", " ", "│", },
                {"│", " ", "│", " ", "└", "─", "┘", "$", "│", "M", " ", "/", " ", " ", "│", " ", "│", " ", " ", "│", " ", "│", " ", " ", " ", "│", " ", "│", " ", "│", },
                {"│", "K", "│", " ", " ", " ", " ", " ", "O", " ", "P", "│", "$", "t", "│", " ", "│", "T", " ", "│", "o", "│", "W", " ", "D", "│", "M", "│", "K", "│", },
                {"└", "─", "┴", "─", "─", "─", "─", "─", "─", "─", "─", "┴", "─", "─", "┴", "─", "┴", "─", "─", "┴", "─", "┴", "─", "─", "─", "┴", "─", "┴", "─", "┘", },
            };

            adventureWorld = new World(grid);
            user = new Player(1, 1);

            SeedMerchantItems();

            PlaySound("OpeningReformat.wav", 2);
            //Tell the user the controls and what the objective is
            DisplayIntro();

            //Tells the user the story of the game
            DisplayStory();
            PlaySound("OpeningReformat.wav", 3);

            PlaySound("LotharAmbientReformat.wav", 2);
            RunGameLoop();
            PlaySound("LotharAmbientReformat.wav", 3);
        }

        private void RunGameLoop()
        {
            while (user.Health > 0 && adventureWorld.GetElementAt(user.X, user.Y) != "@")
            {
                hasFled = false;

                //Draw everything
                DrawFrame();

                //Check for player input, move the player as needed
                HandlePlayerInput();

                //Check if the player has reached certain items on the map
                string elementAtPlayerPos = adventureWorld.GetElementAt(user.X, user.Y);
                if (elementAtPlayerPos != " " && elementAtPlayerPos != "/")
                {
                    PlaySound("LotharAmbientReformat.wav", 3);
                    switch (elementAtPlayerPos)
                    {
                        case "M":
                            MerchantEncounter();
                            break;
                        case "$":
                            ChestEncounter();
                            break;
                        case "T":
                            TrollEncounter();
                            break;
                        case "t":
                            TrollEncounter();
                            if (hasFled == false)
                                adventureWorld.DeleteElementAtLocation(user.X, user.Y);
                            break;
                        case "D":
                            DragonEncounter();
                            break;
                        case "d":
                            DragonEncounter();
                            if (hasFled == false)
                                adventureWorld.DeleteElementAtLocation(user.X, user.Y);
                            break;
                        case "P":
                            TrapEncounter();
                            break;
                        case "K":
                            KeyEncounter();
                            break;
                        case "+":
                            DoorEncounter();
                            break;
                        case "W":
                            WizardEncounter();
                            break;
                        case "w":
                            WizardEncounter();
                            if (hasFled == false)
                                adventureWorld.DeleteElementAtLocation(user.X, user.Y);
                            break;
                        case "O":
                            OgreEncounter();
                            break;
                        case "o":
                            OgreEncounter();
                            if (hasFled == false)
                                adventureWorld.DeleteElementAtLocation(user.X, user.Y);
                            break;
                        case "B":
                            BossEncounter();
                            break;
                        case "*":
                            TutorialEncounter();
                            break;
                        default:
                            break;
                    }
                    PlaySound("LotharAmbientReformat.wav", 2);
                }

                //See if the user has enough experience to level up
                if (user.Level < 10)
                {
                    int xpToLevelUp = GetXPToLevelUp();
                    while (user.Experience >= xpToLevelUp && canLevelUp)
                    {
                        LevelUp();
                        xpToLevelUp = GetXPToLevelUp();

                        if (user.Level < 10)
                        {
                            WriteLine($"You need {xpToLevelUp} Experience to reach the next level.\n");
                        }
                        else
                        {
                            WriteLine("You have reached the maximum level.\n");
                            canLevelUp = false;
                        }

                        PressEnterToContinue();
                    }
                }

                //Render the Console
                System.Threading.Thread.Sleep(20);

            }
            PlaySound("LotharAmbientReformat.wav", 3);

            if (user.Health > 0)
            {
                Clear();

                Print("And so, the brave villager had defeated the mighty wizard, Lothar,\n" +
                    "and his band of horrible creatures. The castle fell silent, and the village was once again safe and sound.\n" +
                    ".\n" +
                    ".\n" +
                    ".\n" +
                    ".\n" +
                    ".\n" +
                    "For now.....\n");

                PressEnterToContinue();
            }

            //Display the outro
            Clear();
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
                    user.Health = user.MaxHealth;
                    WriteLine($"You have increased your Total Health and renewed your current health\n" +
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

        private void TutorialEncounter()
        {
            Clear();
            PlaySound("MerchantReformat.wav", 2);

            MerchantTalking("Greetings! I am your friendly Dungeon Merchant!\n");
            FarmerTalking("What the hell?\n");
            MerchantTalking("My friends and I will provide you with help.\n");
            FarmerTalking("What’s a merchant doing in an abandoned castle?\n");
            MerchantTalking("Doesn’t seem abandoned to me.\n");
            FarmerTalking("But it was.\n");
            MerchantTalking("Merchants follow the action.\n");
            FarmerTalking("There were some spooky noises--\n");
            MerchantTalking("Spooky noises don’t come from abandoned castles.\n");
            FarmerTalking("Exactly!\n");
            MerchantTalking("I’m sure you’ll find my services very useful.\n");
            FarmerTalking("Why?\n");
            MerchantTalking("I sell all manner of items for the adventure-seeking hero, and I never carry the same items twice!\n");
            FarmerTalking("But I’m a farmer.\n");
            MerchantTalking("Where’s your pitchfork?\n");
            FarmerTalking("I didn’t bring it.\n");
            MerchantTalking("You came here to investigate spooky noises in an abandoned castle, and you didn’t even bring your pitchfork?\n");
            FarmerTalking("I didn’t think I’d need it.\n");
            MerchantTalking("You’re going to be very glad I’m here. Step right up and let’s see what we can do for you.\n");
            FarmerTalking("I didn’t bring much money with me.\n");
            MerchantTalking("That’s alright. Old castle dungeons like this are full of treasure chests with gold in them.\n");
            FarmerTalking("That’s oddly convenient.\n");
            MerchantTalking("No more so than finding ammunition for a gun in the ruins of an ancient civilization.\n");
            FarmerTalking("...true...\n");
            MerchantTalking("Find a few of those chests and we’ll have you suited up in no time.\n" +
                "Or if you defeat some of the creatures in the dungeon they might drop some gold, too.\n");
            FarmerTalking("Creatures?\n");
            MerchantTalking("Nothing fancy, just the standard Trolls, and Ogres.\n");
            FarmerTalking("Trolls and Ogres?!\n");
            MerchantTalking("*coughs* And a Dragon. *coughs*\n");
            FarmerTalking("Wait why would Trolls and Ogres carry gold?\n");
            MerchantTalking("Why else? They like shiny things.\n");
            FarmerTalking("I’m a farmer! I don’t fight monsters!\n");
            MerchantTalking("Don’t worry, now that you’re on a hero’s quest you’ll gain experience\n" +
                "every time you defeat a monster and you’ll get stronger as you go!\n");
            FarmerTalking("This is a lot of information to take in.\n");
            MerchantTalking("Fortunately, for you, that’s all the information I have.\n");
            FarmerTalking("What do I do now?\n");
            MerchantTalking("Start looking for those spooky noises!");

            PlaySound("MerchantReformat.wav", 3);
            adventureWorld.DeleteElementAtLocation(user.X, user.Y);
        }

        private void MerchantTalking(string words)
        {
            DrawMerchant();
            WriteLine("\n\n");
            ForegroundColor = ConsoleColor.Yellow;
            WriteLine(words);
            ResetColor();
            PressEnterToContinue();
            Clear();
        }

        private void FarmerTalking(string words)
        {
            DrawFarmer();
            WriteLine("\n\n");
            ForegroundColor = ConsoleColor.Green;
            WriteLine(words);
            ResetColor();
            PressEnterToContinue();
            Clear();
        }

        private void ChestEncounter()
        {
            Clear();

            Random rng = new Random();
            int numberOfCoins = rng.Next(10, 21);
            user.TotalCoins += numberOfCoins;

            PlaySound("ChestMusic.wav");
            DrawChest();

            WriteLine($"\nYou found a treasure chest containing {numberOfCoins} coins!\n" +
                "These coins have been added to your coin pouch.\n\n" +
                $"You now have a total of {user.TotalCoins} coins.\n");

            adventureWorld.DeleteElementAtLocation(user.X, user.Y);

            PressEnterToContinue();
        }

        private void KeyEncounter()
        {
            Clear();

            PlaySound("KeyMusic.wav");
            DrawKey();
            user.NumberOfKeys++;

            WriteLine("\nCongratulations, you have found a key!\n" +
                "I wonder what this could be used for........\n");

            WriteLine($"\nYou now have {user.NumberOfKeys} keys in your possession.\n");

            adventureWorld.DeleteElementAtLocation(user.X, user.Y);

            PressEnterToContinue();
        }

        private void DoorEncounter()
        {
            Clear();

            PlaySound("KnockingOnDoor.wav");
            DrawDoorClosed();
            WriteLine("\nYou nervously approach the door...\n");

            PressEnterToContinue();

            if (user.NumberOfKeys > 0)
            {
                user.NumberOfKeys--;

                Clear();

                DrawDoorOpen();
                PlaySound("KeyInDoor.wav");
                WriteLine("\n\nYou insert the key into the keyhole. The door slowly creaks open.\n" +
                    $"You now have {user.NumberOfKeys} keys remaining.\n");

                adventureWorld.DeleteElementAtLocation(user.X, user.Y);
            }
            else
            {
                PlaySound("DoorClosing.wav");
                WriteLine("You do not have a key to open this door.\n" +
                    "Return when you have found the key!\n");

                if (adventureWorld.IsPositionWalkable(user.X-1, user.Y))
                {
                    user.X--;
                }
                else
                {
                    user.Y++;
                }
            }

            PressEnterToContinue();
        }

        private void TrapEncounter()
        {
            Clear();

            DrawTrap();

            WriteLine("\nYou stumble upon a trap that is in your way!\n");

            Random rng = new Random();

            int randomNum = rng.Next(0, 101);

            if (randomNum <= 50)
            {
                PlaySound("TrapSnapping.wav");

                WriteLine("You stumble in the trap and activate it!\n" +
                    $"You take {Convert.ToInt32(user.Health * 0.2)} damage!\n");
                user.Health -= Convert.ToInt32(user.Health * 0.2);
            }
            else
            {
                PlaySound("DisarmTrap.wav");

                WriteLine("You manage to disarm the trap, pass it, and take no damage!\n");
            }

            adventureWorld.DeleteElementAtLocation(user.X, user.Y);
            WriteLine($"Current Health: {user.Health}\n");
            PressEnterToContinue();
        }

        private void MerchantEncounter()
        {
            Clear();

            DrawMerchant();
            PlaySound("MerchantArrive.wav");

            WriteLine("\nWelcome to my shop, traveler! Please choose from my selection of wares:\n");

            List<Item> listOfItems = _itemRepo.ReturnListOfItems();

            Random rng = new Random();
            int randOne = rng.Next(0, 11);
            int randTwo = rng.Next(0, 11);
            int randThree = rng.Next(0, 11);
            while (randTwo == randOne)
            {
                randTwo = rng.Next(0, 11);
            }

            while (randThree == randOne || randThree == randTwo)
            {
                randThree = rng.Next(0, 11);
            }

            List<Item> refinedListOfItems = new List<Item>();
            refinedListOfItems.Add(listOfItems[randOne]);
            refinedListOfItems.Add(listOfItems[randTwo]);
            refinedListOfItems.Add(listOfItems[randThree]);

            WriteLine("Which item would you like to purchase?\n\n" +
                "--------------------------------------\n\n" +
                $"You have a total of {user.TotalCoins} coins.\n" +
                $"Health: {user.Health} out of {user.MaxHealth}\n" +
                $"Strength: {user.Strength}\n" +
                $"Defence: {user.Defence}\n" +
                $"Accuracy: {user.Accuracy}%\n" +
                $"Critical Strike Chance: {user.CritChance}%\n\n" +
                $"--------------------------------------\n");

            int count = 0;
            foreach (Item item in refinedListOfItems)
            {
                count++;
                WriteLine($"{count}: {item.Name} / {item.Description}\n");
            }

            count++;
            WriteLine($"{count}: Leave the shop without buying anything.\n");

            string choice = ReadLine();

            while (choice != "1" && choice != "2" && choice != "3" && choice != "4")
            {
                WriteLine("Invalid input! Please enter another number.\n");
                choice = ReadLine();
            }

            int choiceOfItem = int.Parse(choice) - 1;
            if (choiceOfItem == 3)
            {
                PlaySound("MerchantAngry.wav");

                WriteLine("\nC'mon man, I've got kids to feed here!\n" +
                    "If you find any coins along your journey, you know where to come.\n");
            }
            else if (user.TotalCoins >= refinedListOfItems[choiceOfItem].Cost)
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
                    case "Dragon Breath Shield":
                        WriteLine($"You purchased a Dragon Breath Shield. You are now greatly defended from the dragon's fiery breath.\n");
                        user.HasDragonProtection = true;
                        WriteLine($"You have now acquired Dragon Protection.\n");
                        break;
                }
                PlaySound("MerchantHappy.wav");

                user.TotalCoins -= refinedListOfItems[choiceOfItem].Cost;
                WriteLine($"You now have {user.TotalCoins} coins remaining.\n");
            }
            else
            {
                PlaySound("MerchantAngry.wav");

                WriteLine("\nAre you trying to rip me off?!?!?!? Get out of my store!\n");
            }
            PressEnterToContinue();
        }

        private void BossEncounter()
        {
            IMonster lotharPhaseOne = new BossPhaseOne();
            IMonster lotharPhaseTwo = new BossPhaseTwo();

            Clear();

            PlaySound("OrganBossBattleReformat.wav", 2);
            DrawMonster(lotharPhaseOne);  // for alive lothar
            Print("\nWhat mortal dares disturb my work?!?! I am the mighty Lothar, and you will crumble at my feet!\n");
            PressEnterToContinue();
            PlaySound("OrganBossBattleReformat.wav", 3);

            PlaySound("StandardBossBattleReformat.wav", 2);
            MonsterEncounter(lotharPhaseOne);
            PlaySound("StandardBossBattleReformat.wav", 3);

            if (user.Health > 0 && hasFled == false)
            {
                Clear();

                DrawMonster(lotharPhaseOne);  // for defeated lothar

                WriteLine("\nYou emerge victorious from the fight!\n" +
                    "As Lothar gasps his final breath, you hear him say something...");

                PressEnterToContinue();

                Clear();

                Print("You may have defeated me, but my final task is complete!\n", 40);
                Print("My creation will surely crush you beneath its feet!\n", 40);

                PressEnterToContinue();

                Clear();

                PlaySound("TechnoBossBattleReformat.wav", 2);
                DrawMonster(lotharPhaseTwo);
                MonsterEncounter(lotharPhaseTwo);
                PlaySound("TechnoBossBattleReformat.wav", 3);

                adventureWorld.DeleteElementAtLocation(user.X, user.Y);
            }
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
            WriteLine($"\nA(n) {monster.Name} appears!\n");
            monster.MakeNoise();
            PressEnterToContinue();

            while (user.Health > 0 && monster.Health > 0 && hasFled == false)
            {
                Clear();

                WriteLine("What would you like to do?\n" +
                    $"1: Attack the {monster.Name}\n" +
                    $"2: Flee from the {monster.Name}\n\n" +
                    $"----------------------------\n\n" +
                    $"Enemy health: {monster.Health}\n" +
                    $"Enemy Strength: {monster.Strength}\n" +
                    $"Enemy Defence: {monster.Defence}\n\n" +
                    $"Your Health: {user.Health}\n" +
                    $"Your Strength: {user.Strength}\n" +
                    $"Your Defence: {user.Defence}\n");

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
            else if (monster.Health <= 0)
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

                PressEnterToContinue();
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
                        PressEnterToContinue();
                    }
                    else
                    {
                        monster.Health -= user.Strength - monster.Defence;
                        WriteLine($"Your attack lands! You deal {(user.Strength - monster.Defence)} damage to the {monster.Name}!\n");
                        PressEnterToContinue();
                    }
                }
                else
                {
                    WriteLine($"You are not strong enough to penetrate the {monster.Name}'s defences! You deal no damage!\n");
                    PressEnterToContinue();
                }
            }
            else
            {
                WriteLine("Your attack misses! You deal no damage.\n");
                PressEnterToContinue();
            }

            if (user.HasDragonProtection == false && monster.Name == "Dragon" && monster.Health > 0)
            {
                user.Health -= 2 * monster.Strength;
                WriteLine($"The {monster.Name} fries you with its fiery breath!\n" +
                    $"You have no dragon protection, so the dragon deals {2 * monster.Strength} Damage!\n");

                PressEnterToContinue();
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
                                PressEnterToContinue();
                            }
                            else
                            {
                                user.Health -= monster.Strength - user.Defence;
                                WriteLine($"The {monster.Name} hits you viciously! You take {monster.Strength - user.Defence} damage!\n");
                                PressEnterToContinue();
                            }
                        }
                        else
                        {
                            WriteLine($"The {monster.Name} is not strong enough to penetrate your defences! You take no damage!\n");
                            PressEnterToContinue();
                        }
                    }
                    else
                    {
                        WriteLine($"The {monster.Name} misses you, dealing no damage!\n");
                        PressEnterToContinue();
                    }
                }
            }
        }

        private void FleeFromMonster(IMonster monster)
        {
            Random rng = new Random();
            int randomNum = rng.Next(0, 101);

            if (monster.Name == "Boss" || monster.Name == "Mighty Minotaur")
            {
                WriteLine("Puny mortal! Come back when you are prepared to fight me!\n");
                user.X--;
                PressEnterToContinue();
            }
            else if (randomNum >= 40)
            {
                WriteLine($"You manage to flee from the {monster.Name} without taking any damage!\n");

                PressEnterToContinue();
                //RunGameLoop();
            }
            else
            {
                user.Health -= Convert.ToInt32(user.Health * 0.5);
                WriteLine($"The {monster.Name} attacks you while you are running and deals {Convert.ToInt32(user.Health * 0.5)} damage!\n");

                PressEnterToContinue();
                //RunGameLoop();
            }
            hasFled = true;
        }

        private void PressEnterToContinue()
        {
            ForegroundColor = ConsoleColor.Red;
            WriteLine("\nPress Enter to Continue...\n");
            ResetColor();

            ConsoleKeyInfo keyInfo = ReadKey(true);
            ConsoleKey key = keyInfo.Key;

            while(key != ConsoleKey.Enter)
            {
                keyInfo = ReadKey(true);
                key = keyInfo.Key;
            }
        }

        private void YouAreDead()
        {
            Clear();

            WriteLine("Oh dear, it appears you are dead...");
            PressEnterToContinue();
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
                case ConsoleKey.S:
                    DisplayStats();
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
                "The rules are simple, find the exit to the castle (marked by @)!\n\n" +
                "However, your journey will not be so simple. There are multiple enemies that are hiding in the darkness.\n\n" +
                "If you step on them, they will become hostile and attack you!\n" +
                "Some are visible (marked by T, O, D, and W), while others are not.\n\n" +
                "Navigate through the rooms, watch out for traps, loot the chests ($), and find the keys to open the doors\n" +
                "(+) further into the castle.\n\n" +
                "Once you reach the end, you will find a boss waiting for you. Defeat this boss and you will be able to exit\n" +
                "the castle!\n\n" +
                "(If you ever want to view your stats, simple press the S Key)\n\n\n\n\n");

            ResetColor();

            PressEnterToContinue();
        }

        private void DisplayStats()
        {
            Clear();
            WriteLine("Current Stats:\n\n" +
                "--------------------\n\n" +
                $"Level: {user.Level}\n" +
                $"Total Experience Gained: {user.Experience}\n" +
                $"Health: {user.Health} out of {user.MaxHealth}\n" +
                $"Strength: {user.Strength}\n" +
                $"Defence: {user.Defence}\n" +
                $"Accuracy: {user.Accuracy}%\n" +
                $"Critical Strike Chance: {user.CritChance}%\n" +
                $"Coins: {user.TotalCoins}\n" +
                $"Number of Keys in Possession: {user.NumberOfKeys}\n" +
                $"Protected from Dragon Fire: {user.HasDragonProtection}\n\n");

            PressEnterToContinue();
        }

        private void DisplayStory()
        {
            Clear();
            WriteLine(@"
,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
,,,,,,,,,,,?,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
,,,,,,,,,,,Z,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
,,,,,,,,,,,Z,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,.,,,,,,,,,,,,
,,,,,,,,,,?Z=,,,,,,,,,,,,,,,,,,,,,,,,,$,,,,,,,,,,,,,,,,,,,,,,,,,..,.,,,,,,,,,,,,
,,,,,,,,,:O$Z,,,,,,,~,,,,,,,,:,:,,,,,,ZO,,,,,,,,,,,,,,,,,,,,,,,,....,,,,,,,,,,,,
,,,,,,,,,,$ZZ,:,,,,,=:,,,,,,~Z:,,,,,,,ZZ,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
,,,,,,,,,I????,,,,,,7,,,,,,:OZ,,,,,,,ZZZ7,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
,,,,,,,,,?I?I7,,,,,,ZI,,,,,,ZZ7II?IIIZZZZ,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
,,,,,,,,,?II77,,,,,,ZZ,,,,,+ZZZI??IIIZZZZ:,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
,,,,,,,,,?III7=~~,,ZZZ,,,,,OZ$$II,,?ZZZZZO,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
,,,,,+,,,?I?III??I?ZZZO,,,,Z$ZZZ:,:+ZZZZZZ,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
,,,,,?,,,?7IIIII=II$ZZ$,,,7?IIII,:,IIIIIII+,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
,,,,,$,,,I?II?I,,~ZZZZZZ7$$.7II7.:.~+777I+7:,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
,,,,,Z??I??III:,,,ZZZZZZ$Z$Z$ZZZZ$ZZ$$Z$ZZZZ,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
,,,,:ZZI,IIIII,,,7IIIIIIIZZZZZZZZZZZZZZZZZZZ,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
,,,,OO$=,7.III,,,I7IIII7?ZZZZZZZZZZZZZZZZZZZ=,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
,,,,ZZZ7:?IIII,,,I,IIIII$I777??IIIII7I?I7II7?,,,::,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
,,,=ZZZZ,?III7,::I~7IIIIZ7$$$??77,,,77?7$$$$I,,,IIII?,,,,,,,,,,,,,,,,,,,,,,,,,,,
,,,OZZZZ:?IIIII?I???IIIIZI++7??77...77?7I??I7,,,7777:,,,,,,,,,,,,,,,,,,,,,,,,,,,
,,.$ZZZZZ?II777777$7I?I7I7$$7I?7I$$$$7?77$$7I,,,777I:,,,,,,,,,,,,,,,,,,,,,,,,,,,
:,7??IIIIIII7II$7777III$ZZ$$7?II7.,,$7?I77I$7$$$$$Z$=,,,,,,,,,,,,,,,,,,,,,,,,,,,
,,+I777I7?II7777I777III$7$=+I??77...77?II$$$$$7$777~:,,,,,,,,,,,,,,,,,,,,,,,,,,,
,,+~77777I?I77777777$$$$$$IIII?777777I7$$$?$$$,,Z$$Z~,,,,,,,,,,,,,,,,,,,,,,,,,,,
,,=.777777$$77777777$$$$$$$I$$$7IIII$$$7$$7ZO8,:7777:,,,,,,,,,,,,,,,,,,,,,,,,,,,
,,+77$$$$$$$77777777$$$$II$$$$$$$Z8D88DDDD8888,,77II:,,,,,,,,,,,,,,,,,,,,,,,,,,,
,,=7777777$$7777OD8888888888888888O88888888888N,7777:,,,,,,,,,,,,,,,,,,,,,,,,,,,
,,=7777O88D8D88888888888888888888D8888888888888.7777:,,,,,,,,,,,,,,,,,,,,,,,,,,,
ID88888D88888888888888888888888O888888888888888Z7777:,,,,,,,,,,,,,,,,,,,,,,,,,,,
88888888888888888888888888888888888888888888888888D8D,,,,,,,,,,,,,,,,,,,,,,,,,,,
888888888888D8888888D8888888O888888888888888888888888:,,,,,,,,,,,,,,,,,,,,,,,,,,
8888888888O8888888888888OO8D8888888888888888888888888N,,,,,,,,,,,,,,,,,,,,,,,,,,
88888888888888888888888888DD8888OOO8888888888888888888.,,,,,,,,,,,,,,,,,,,,,,,,,
888888888888888888888888888888888888888888888888888888D,,,,,,,,,,,,,,,,,,,,,,,,,
888888888OOO888D8888888888888888888888888888888888888888D8$,,,,,,,,,,,,,,,,,,,,,
8888O88888888888O88888888888888888D888888888888888888888888N,::,,,,,,,,,,,,,,,,,
88888888888888888888888OO88888888888O888888888888888888888888?,,,,,,,,,,,,,,,,,,
8888888888888888888888888888888888888888888888888888888888888DN$+,,,,,,,,,,,,,,,
8888888888888888888888888888888888888888888888888888888888888888888D8D8DZ,,,,,,,
88888888888888888888888888888888888888888888888888888888888888888888888888OD:,,:
NDNNNMNNNDNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNM
NNMMNNMNNMMNDNDNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN
NN=$8D?OIM,:++M:MMNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNMZZMDN?NN$MMNM+7?$$IZNNN
NNNNMNMMDNNDDMDDNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNMMMNDNNNNNNDNNNMNNNNNNNN
NNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN");
            Print("\n\nHigh up on DeathRock Mountain, there stood an abandoned castle.\n\n");
            Print("The farmers who lived in the village below told stories about Lothar, the vicious wizard,\n" +
                "who had once lived there.\n" +
                "Year by year the stories became more legend than memory.\n\n");
            Print("One night the villagers woke to horrible sounds right outside their doors.\n" +
                "Sounds of creatures they had never heard before.\n" +
                "Then, suddenly, the noises stopped.\n\n");
            PressEnterToContinue();
            Clear();

            WriteLine(@"
                                      ▓▓                                                    
                                      ██                                                    
                                      ██                                                    
                                      ████                                                  
                                      ████                                                  
                                      ██████                                                
                                      ██████                                                
                                      ██████                                                
                                      ████████                                              
                                ██    ████▓▓██                                              
                                ██▓▓▓▓████▓▓██                                              
                                ██████████▓▓██                                              
                                ██████▓▓██▓▓██                                              
                                ██████▓▓██▓▓██▓▓        ▓▓                                  
                              ▓▓▓▓████▓▓████████      ▓▓██                                  
                              ██▓▓██▓▓▓▓▓▓██████▓▓    ████                                  
                              ██▓▓░░▒▒▓▓▓▓██▓▓████    ██████                                
                              ██▓▓▓▓░░▓▓▓▓██▓▓██████████████                                
                              ████▓▓▓▓▓▓▓▓▓▓██████████████████                              
                  ██          ████▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓██▓▓██████                              
                  ██          ████▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓██████                              
                ▓▓██▓▓      ▓▓████▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓██▓▓██                              
                ████████    ████▓▓██▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓░░▓▓████      ██                    
              ▓▓▓▓████████▓▓██▓▓▒▒██▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓░░▓▓████      ██▓▓                  
              ▓▓▒▒████▓▓████▓▓▒▒▓▓██▓▓▓▓▓▓▒▒▓▓▓▓▓▓▓▓▓▓▓▓░░██████  ██████▓▓▓▓                
              ▓▓░░▓▓▓▓▓▓▓▓▓▓▓▓██████▓▓▓▓▒▒▒▒▓▓▓▓▓▓▓▓▓▓░░░░██████  ████▓▓▓▓▓▓▓▓              
              ▓▓░░▓▓▓▓▓▓▓▓▓▓▓▓██▓▓▓▓▓▓▓▓▒▒▒▒▒▒▓▓▓▓▓▓▓▓▓▓▓▓▓▓██████████▓▓░░▓▓▓▓▓▓            
              ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▒▒▓▓▒▒▒▒▓▓▓▓▓▓██▓▓▓▓▓▓████████▓▓▒▒░░▓▓▓▓            
              ▓▓▓▓▓▓██▓▓▓▓▓▓▓▓▒▒▓▓▓▓▓▓▒▒▒▒▒▒▓▓▒▒▒▒▓▓▓▓██▓▓▓▓▓▓▓▓▓▓████▓▓▓▓░░▒▒▓▓            
              ██▓▓▓▓▒▒▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▒▒▒▒░░▒▒▓▓▒▒▓▓▓▓██▓▓▒▒▒▒▓▓▒▒████▓▓▓▓░░▓▓▓▓▓▓          
              ██▓▓▒▒░░▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▒▒▓▓░░░░░░▒▒▓▓████▒▒▒▒▒▒▒▒▒▒████▓▓▓▓▓▓░░▓▓▓▓          
              ██▓▓▒▒▓▓▒▒▓▓▓▓▓▓▓▓▓▓▓▓▒▒▒▒▒▒░░░░░░▓▓▓▓██▓▓▓▓▒▒▒▒▒▒▓▓████▓▓▓▓▓▓▓▓▓▓▓▓          
              ██▓▓▓▓▓▓▒▒▓▓██▓▓██▓▓▓▓▓▓▓▓░░░░░░▒▒▓▓▓▓██▓▓▓▓▒▒▒▒▒▒▓▓████▓▓▓▓▓▓▓▓▓▓▓▓          
              ██▓▓▒▒▓▓▓▓▓▓▓▓██▓▓▓▓▓▓▓▓▓▓░░░░░░▒▒▓▓▓▓██▒▒░░▒▒▒▒▒▒▓▓████▓▓▓▓▓▓▓▓▓▓▓▓▓▓        
            ▓▓██▓▓▓▓▒▒▒▒▓▓▓▓▓▓▓▓▓▓▓▓▒▒▒▒░░░░░░▒▒▓▓████▒▒░░░░▒▒▒▒▓▓████▓▓▓▓██▓▓▓▓▓▓▓▓        
            ████▓▓▒▒▒▒▒▒▒▒▓▓▓▓▓▓▒▒▓▓▒▒▒▒░░░░░░░░▓▓▓▓▒▒░░░░░░░░▒▒▓▓▓▓▓▓▓▓▓▓██▓▓▓▓▓▓██        
    ▓▓      ██▓▓▓▓▓▓░░░░░░▒▒▒▒▒▒▒▒▒▒░░░░░░  ░░░░▓▓▒▒░░░░░░░░░░▒▒▓▓▓▓▓▓▓▓▓▓██▓▓▓▓▓▓██        
    ████    ██▓▓░░▓▓▓▓░░░░░░░░▒▒░░▓▓▒▒░░    ░░▒▒░░░░░░░░░░░░░░▒▒▒▒▓▓▓▓▓▓██░░▒▒▓▓▓▓██        
    ██████████▓▓░░▓▓▒▒▒▒░░░░░░░░░░░░░░░░    ░░░░░░░░░░░░░░░░░░▓▓▒▒░░░░▓▓▓▓░░▓▓▓▓▓▓██        
  ▓▓██▒▒████▓▓▓▓▓▓▒▒▒▒░░░░░░░░░░░░▒▒░░░░      ░░░░░░░░  ░░░░░░▓▓▒▒░░▒▒▓▓▓▓▓▓▓▓▓▓▒▒██    ████
  ██▓▓▒▒▓▓▓▓▓▓▓▓░░██▒▒░░░░░░░░░░░░░░░░░░      ░░░░░░    ░░░░░░▓▓▒▒▒▒▒▒▒▒▒▒▒▒▒▒▓▓▒▒██    ████
  ██▓▓▒▒▓▓▓▓▓▓▓▓▒▒██▒▒▒▒░░░░░░░░  ░░░░░░        ░░    ░░░░░░▒▒▒▒▒▒▒▒▒▒░░░░▓▓▓▓▓▓▓▓██  ██████
██▓▓▓▓░░▓▓▓▓▓▓▓▓▓▓▓▓▓▓▒▒░░▒▒░░░░    ░░░░              ░░░░░░▒▒▓▓░░░░░░▒▒▓▓▒▒▒▒▓▓▓▓██████▓▓██
██▓▓▓▓░░▓▓▓▓▓▓▓▓▓▓▓▓▒▒▒▒░░▒▒░░░░      ░░              ░░░░▒▒▒▒▒▒░░░░░░▒▒▓▓▒▒▓▓▓▓▓▓████▓▓▓▓██
██▓▓▓▓▓▓▓▓▒▒▓▓▓▓▓▓▓▓▒▒░░░░░░░░░░                  ░░  ░░░░░░░░░░░░░░░░▒▒▓▓▒▒▓▓▓▓▓▓▓▓▓▓▓▓▓▓██
██▓▓▓▓░░▓▓▒▒▒▒▒▒▒▒▓▓▒▒░░░░░░░░░░                  ░░    ░░░░░░░░░░░░░░▒▒▓▓▒▒▓▓░░▓▓▓▓▓▓▓▓▒▒██
██▓▓▓▓▓▓▓▓▒▒▒▒▒▒▒▒▒▒▓▓░░░░░░░░░░                                ░░░░░░░░▒▒▓▓▓▓▒▒▒▒▓▓▓▓▓▓░░██
██▓▓▓▓▓▓▓▓▒▒▒▒░░▒▒▒▒▒▒░░░░░░░░░░    ░░        ░░                ░░░░░░░░▓▓▒▒▒▒▒▒▓▓▓▓▓▓▓▓░░██
██▓▓▓▓▓▓██▓▓▒▒░░▒▒▒▒▒▒░░░░░░░░░░    ░░                      ░░░░░░░░░░▒▒░░▒▒▒▒▒▒▓▓▓▓▓▓▓▓▒▒▒▒
▓▓██▓▓▓▓▓▓██▒▒▒▒░░░░░░░░░░░░░░░░                          ░░░░░░░░░░░░░░░░░░▒▒▓▓▒▒▓▓▒▒▓▓▓▓▒▒
▓▓▓▓▓▓▓▓▓▓▒▒▓▓░░░░░░░░░░░░░░░░░░                    ░░    ░░░░  ░░░░░░░░░░░░░░▓▓▒▒▓▓▓▓▓▓▒▒▒▒
  ▓▓▓▓▓▓▓▓▓▓▒▒░░░░░░░░░░░░░░░░░░                    ░░    ░░░░  ░░░░░░░░░░░░░░▒▒▓▓░░▓▓▒▒▒▒  
  ▓▓▓▓▓▓▓▓▒▒▒▒▒▒░░░░░░░░░░░░░░        ░░            ░░░░        ░░░░░░░░░░░░▓▓▒▒▓▓▓▓▓▓▒▒    
    ▓▓▓▓▓▓▓▓▒▒░░░░░░░░░░░░░░░░        ░░        ░░▒▒            ░░░░░░░░░░▓▓▓▓▒▒▒▒▓▓▒▒▒▒    
    ▒▒▓▓▓▓▓▓▒▒░░░░░░░░░░░░░░                                    ░░░░░░░░░░▒▒▒▒░░▓▓▒▒▒▒      
      ▒▒▓▓▓▓▒▒░░░░░░░░░░░░                                    ░░░░░░░░░░░░▓▓▓▓▒▒▒▒▒▒        
        ▒▒▓▓▒▒░░░░░░░░░░░░  ░░    ░░                ░░  ░░    ░░░░░░░░░░▒▒▒▒▒▒▒▒▒▒          
            ▒▒▒▒░░░░░░░░░░                            ░░░░    ░░░░░░▒▒▓▓▒▒▓▓▒▒▒▒            
              ▒▒▒▒▒▒  ░░                  ░░          ░░      ░░░░░░▒▒▒▒▒▒▒▒▒▒              
                ▒▒▒▒▒▒░░░░      ░░        ░░        ░░░░      ░░░░▒▒▒▒▒▒▒▒                  
                    ▒▒░░░░      ░░                    ▒▒░░░░  ░░▒▒▒▒▒▒                      
                    ▒▒░░░░      ░░                    ░░░░░░  ░░▒▒▒▒▒▒                      
                      ▒▒▒▒░░    ░░              ▒▒░░        ▒▒▒▒                            
                          ▒▒▒▒          ░░                                                  
                        ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░                              
                  ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░                          ");
            Print("\n\nSlowly the villagers began to emerge from their cottages. They could not believe their eyes.\n" +
                "The fields were burnt, their stores turned upside down, and stone statues had been reduced to rubble.\n" +
                "They stood in disbelief at their humble lives, now torn to shreds.\n\n");
            PressEnterToContinue();
            Clear();

            WriteLine(@"

.............,..,,,....,..........,..............,.,........
.............,,,....,............,,:,,......................
...............,..............,,:??????,,,.......,..........
............................,,:???????????:.................
...........,...,...........,:~??????????????.,...,..........
..........,,,,..,.......,,,:?????????????????,,.............
.............,,.,,......,::???????????????????,..,....,.....
..........,.,..,,.,.....::?????????????????????,,:..........
.............,,,.,,,...::+?????????????????????+,:..........
...........,..,.,,.,..,::???????????????????????,,....,..,,,
.........,.,.,,,,,,,..::I???????????????????III?,,,,,,,,,,.,
........,,,,.,.,.,,,..::??????????IIIIIIIIIIII7I7:........,.
........,,:::::,,,.,.,::II?IIIIIIIIIIIIIII7IIIIII,.......,..
.........,,,,::::::,.::~IIIIIIIIIIIIIIIIIIIIIIIII,....,....,
.........,,,,,,,:,,..:::IIIIIIIIIIIIIIIIIIIIIIIII:....,,....
.........,..:,::,:,..:::IIIIIIIIIIIII77777777777I,.........,
........,,::,,,~:,...:,~7777777777777777777777777:.,.,....,.
........,.,:,,,:,:,,.::~7777777777777777777777777:.,.....,,,
........,,,.,,:::::..:,~7777777777~777~777777777 :,,,....,,,
........::~,,,,:::,..::~IIIIIIII77===::::,:,7=7 =,...,,,,,..
........:,::,,~::::,.,::~=+=~:~~,:=,,.,:..::,,::~,......,,..
........,,,::,:,:::..,::,,,:=~==~,~=,.,.:=.,,,===.,....,.,..
........,::,:,,.,,:.,,~=:,:~~~=::=?+,.,,=,,....~:.,...,,..,.
........,,:,,,::~::.,,~:..,.,?~::,:,.,,,,:~~~=+~::..........
........,:::,,,::::.,,:~,:~=:,,~:,::,,,~,+,=I=,=::,.,,,,,,,,
........,,:,:::~:::,,,~~.,,~=,==::::~=~=,:=~:=?+:,.........,
........,:,,::::::::,,=~~:.,==,,:,,:,~=~~=+,~+=.:.....,...,.
........,,,::::::~:,,,=~=::..,,:==,~:::,,.,:,,~::...........
........::,::::~:~::,,~~?7?=+,,:+~~:,.+:..:,.,:,,......,....
........,::::~:,,,,,.,,,,.~???I+?I7I+?7??II=?I7I:...........
..,......,,,,:,,,,,.......,,,,,,,,.,,,,,,::,,:+?,...........
.........,,,,,,,,.,II+??III?77I~??7I,:,,,...,,,,,...........
...,........,,,,==III+7=7=+7+77=II77=7I+=,+7~+I?,...........
...,...........+??+=++?7+=+,,,+???I77I=III77?77:,.,.......,.
...............,,......,.~::7~?I?=??I7=?+,~~+7:,:...,,,,,,,,
.......................,.,.,.,...,,,.:7?I7==?+,:,,...,......
.....................................,.,,...,,,,..,..,......");
            Print("\n\nOne by one the villagers began to notice the castle on the hill.\n" +
                "A firelight was flickering in a window.\n\n");
            Print("Then the castle shook with a thunderous roar.\n");

            PressEnterToContinue();
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
            else if (monster.Name == "Mighty Minotaur")
            {
                WriteLine(@"
.............~...................7.........................,
............I,...................I.........................,
............~7...?.........+.....?+........................,
............~?..=,.........=I....==........................,
...........I~7..+=.........+?....?=........................,
...........7:~..+?.........,~....+=........................,
...........~:~..+?.........+=....?~........................,
............I~7.++...=~....+:...I++........................,
............7=:.,7=..I$7?:==7...?+I........................,
.............7~:,=:.7II7?I+:,?:+~=.........................,
..............?++?$+Z$?+I=:?I?=I,..........................,
...............?7?$Z=I77~:+I??==...........................,
................=?~+=$7===~?==?I...........................,
..................$+I++:I?:77777$I7~:?.....................,
..................$$7+=,~I?$III7~=:,:I$....................,
...................III=7=7+=II$I=~:,.,?7...................,
..................,I7$I?O7?$7I?++:::..,?$..................,
...................77I7I??7=Z??I++::,:,77..................,
..................~??O$$77+I+7?=?7?=,+=7:..................,
.................IZ7~?77I?=,I,::$7I77+$,7..................,
.................~~=I+$Z+~~II::?+7I=$I:+,..................,
...................,I~I?=:I=.:~:$.$=~I7~$..................,
....................,II?I,?==?:....~+7I+:?.................,
.....................7==7I~?:+~7...:?:~==I,................,
.....................+??:,==:7?+....+:,:~~?................,
......................++,,=~,I7?=...=+,:~:7:...............,
.....................7+=:7~~+~++,...$?:~~=7I...............,
....................??==:,=7?,I......$+~?~7I,..............,
...................7?=~?:II+$?=+,.I~~:+7=+?7I..............,
.................?,+77+Z~=:+?7+??+=,.7$?~~+?...............,
...........:....+7~77+77???7:777I+I77?$~~+I$...............,
....?...:.:.....+~77+?=7$7=~+++~:=7I+I=+:,$,=.......:=.....,
.?.,,,.,,~........==$7I$I=~I=?$?I?7++=II....I...?=:$I+.....,
.,:,.:=~::........,7::?:?+~+?I77II7+?~....I?+:+7$+:,.,.....,
,,::~=,~::........,I7I?:+~~??=77$=~~=7~~Z+7I+,I?:,,........,
++~,~:,~:=.........$I$~:~I:~=~?7I~II=~~+?..?=~,............,
~~:,:?~~...........7+:?,~I?+?+:,=7:+:.....+7,..............,
::~,::~............=I~=?=~~~7===7=::,....::................,
+::,::7...........:=:~~~I=?,++?=:~:::....,.................,
.~:~:~~~:7=$..7==~~$??+I~?:~~?$++~::,......................,
..?~~,~~~$??7+=7..Z?+?=?:+,7,?.I+:,I.......................,
..,+~$?::=~?......~+?7?I:I:+=~.7?==?I......................,
..+I:++,,~+7,....?++=:~=,....:.:=?I+.......................,
..$?+:Z$.........?==~=~.........:?++=......................,
....+I...........=???+I~.........+=~:~.....................,
....?$............?~+I:,,.......7~::II.....................,
....I,..............?~,+==.......I??,......................,
....=...................~=++.....7?I+?.....................,
......................+~+?7:....:+?????....................,
......................II7=II...7$7II++?....................,
.....................I???+?==..?I??Z+7?I...................,
.....................I?7I???...:??+7++=,...................,");
            }
            else if (monster.Name == "Boss" && monster.Health > 0)
            {
                WriteLine(@"
                  .... .                                    
               ..,:::,...                                   
              ..:=+=?=~,..                                  
              .,===?:==~..                                  
              .,=7??~$+~,.                                  
              ..:==7,=~:..                                  
               ..I?D:,,...                                  
                .ZD.....                                    
                +7=.        ..ZI7I..                        
              ..DM.       . .+8++:~                         
              ..7 .       ..O?$7?+~                         
              .?O.      ..8M8ZNM7M+~.                       
              .O8.   ...7N+D7$77+,Z8.                       
              ~O     .N$DNNM$NON$DZ8.                       
              ?.   .:=ODM8N8ZD87ND?M7.                      
            .=7....7ZOZONNMD8D$Z$ONN8+.                     
            I~+=N.?=8OO+DNNNN$OMZ8MNN8                      
          .N7ONDN77+$8OO??DNND87$8=OMMN                     
          . M8NDM++8O?8O=$MNM8Z~77+8DDN....,.... .          
          ..OMMMM?+8INM8DZNN88ONZIN8MMNN...,......          
         ..8DMNMM+8MNMMMNNMNN8NON8DND$$ODM.,~::+~...        
         .ZZMMNMDNO..,DMMMMNNDDOMN7M8DNNIO7M.~==,:,.:..     
         .8OM8NMM    .7MNDDMMNM$O$7$DMM8NZ++I=?,?=~::,.     
         .$~ZMN.   ....DMIDNNN8N88$MNDMOD8Z=87$O7~Z:.+.     
         Z$. ..    Z:..=MDMNNNN$8888NM8ZDD$NMZ7Z+Z=,,=.     
       ..$ .    ..~DODI .O8MDNMDDOODNM.=DMDMN8~~::..:..     
       .?8 .       7N8DOMOMNMMMDDODD+D..  7O$Z7::,:....     
       .O ..       M8ONMNNDMNNNMDDODMOM.....OM .....  .     
       ?Z.          ..MNNMDNNM8~MM$DMNNON . .               
    .. DN            .MNDN8OONMNDNDMNNMD8MD.....            
    . 8M.           .8NND88D88888MMDNMMO$$$MNN..            
    ..7$.           ?DMMD8D8888888NDDNMOZO$$OMMM.           
     ,D.       ...D7MMN888D8D88DD8NNNNNND8O$$$OMD.          
    .D        ..D$DMDNNNNMN8DDDO88MMMMNNNDOOZ7$OMM.         
    .?.       .+ZNMDNMMNNOO8O888D8MMMMMMNNN8ZO$$$MM.        
  ..8M.       D8MMMMMNNNNNMMNMMMMMMMMNMMMMNDDOZ$$NMM ..     
  ..8..      .+MMNMNMMNNNDDDNNNMMNNMMMMMMMMMMM8DZDMMM       
  .8M..       OMNNNNNNNNND7DDDNMMMMMMMMMMMMDNDNNNDNMMM.     
  .D.        ,OMNNNNNNNNDDDDDD8M   .. ....MMNNM7DNMNND...   
  MI         OMMNNNNDNNDNDD888D.        DMNDNNND8D,MDDN..   
..N         .MMNNNNNDMNNNDD88D        .MNMNNM .?$:  .,M=    
 :Z.        .NNNNDDDDMDDNND8DD. . .. .MMDD8M......  . ...   
.O?.         .MMMDDDDNDDNNNDIO.      ?MMNDN .               
 +          .$NNIDDDDNDNDDNMD?.       .  MDD                
 .          .+M8DNM$DNNMNDNN+....      ...MNN               
          . MNNNN..DDNNNDDNN.              .  ..            
         ..O7NNM   .DNMND8D,                 ..  :.         
        ...IDD8Z     MNDD7..                     ~          
 .... =. .MMMDM ..  .MNN..                 ..=      .=.     
         DMNM? . ,. .,            ....     ..         .     
       . NNNM.    I.           , .   :: ...                 
         NNM..     .  ... ?  ..                             
        ..$          .. .                                   ");
            }
            else
            {
                WriteLine(@"
                                         ...=77..                               
                                        ..$NMMOO$..                             
                                         .NMNNNO?+..                            
                                        ..D7II77.?..                            
                                        ..$NDO8O7O..                            
                                        ..+I?77III...                           
                                          .D$?7$8:DO:.                          
                                          ?8DI?M=MDOI?.                         
                                      ....$D$ZONMDN8=I~?                        
                                      .~MN8=Z7MMM8MI~:I..                       
                                    ..,7N~7+===$8N$~~:,.,. .                    
                                    .,+88M$?+IDIIN+?+I+???...                   
                                    ~?I,IDN,8ZOZ$?I?+?II+?I~,                   
                                  .ID?Z:?I7+++~,=7=O?O$O?77I?,                  
                                 ..7?N=?$7?7I?=,=IMMM7I?7IOI+O.                 
                                  Z+Z,+7+$=~:.=+$IODMM?I8NZ8$?:                 
                               ..I~?M8Z?=?7?+??7$77MMN7M?++?I8:.      ..        
                               ..$.MMD$II7++8+?=~NDN8MOM7I??=DM.     O~         
                               .$.DMNM?777?$ZI77Z$ZMODMMMNDNMMM+..OO$7N.        
                               ..INNNMN?77O?7?ZIIN8OZMNMMMMNMDDD$7:8DZ+.        
                               .ZNNNNMMO7$I8$8Z=NNOMMNON8?~?7I+7:=?D8O$Z        
                              .$$NNMMMNOOZI77?MMDMZNMN..M+,~,,:ZD~::N8?7        
                              .Z7=NDNNO8OIMONM8IMNNDMNNM++?$,::,,..$Z?.,,.      
                              .I?O$ZOOZ+DIZIOND8DMMO.IM8:=8::::,,,,,~O.:..      
                              .$$$$Z$DD7IMMNN8MM88NDMM=O++7?+::,,,,.......      
                             .+$7$$$OMO?DDMN8$:$O:=DMMD=+~++++:,,...:..7$.      
                             .I$$$$I88ZNDNN77$$=,Z+OOND+=8++++=,,..7,..~,       
                            .?7$$N+NZDMDNODN=7.Z:+=D8DN+=7+++++$:,$O..,.        
                           .OI$$D7?MDNODMZNM++$,=~+=OO==+===+++$:~..:.?:        
                            M$ONN=+M8NONDNNN+?=I+?+8ON7=Z=++$:?O~$DM~,.         
                            .DZND$NZN788DNM.ZI?+??+ZZN+=8==+++IIZODZ..7         
                     ..  D..MNNNZDMDM$NZNMD8MII??+MD7D~+$==++++$~7=77I.         
                    .:DNMMMDDMNMNNMDD$MNDO8M7M8?+OMN$N==+====$$$=.,=.$          
                    ....:8NN7IMZMM,Z$$NNNM8O8NMMO8DO$7==~===+++++Z.$$~          
                     =  .DNDMM8N~?DO8N8MOO8DDD8OZZZNO++======+++7=..:.          
                     =$..=NMDDZDNMI$I7IZMODD+I7?ZZ$I8I=~=====++?I .I,.          
                     .,=?787DZ..ZO.77$7IOMMN$ :?7$$7M$+======+++.  7..          
                    .IN+DDOI8.. . I~7$778MNO..=+?77ZM$++=====++.....            
                    =7I$N$$.7Z$.  .=IZO78NNZ...~~8$DM$+?====++M..~Z.            
                  ..+Z7NZ$:Z.     .:?+=$DMNN7  I$=+?M$+?======...:..            
                  .=I$DI78~:       ++=??7MNN7..8~~++D7+?=====I......            
                  ~ZZD7DD:..       =7$$7+MINO7.OI+$$OI+?=====..:,               
               ...~IM77+:.          =M8D7O7NNZ+.Z?7$7I??====:.~$                
               .~?7ZOZ?.            .D7=O+NMDDI .~8O8D?I~===.,,..               
                :IOOZ?.              .$7DMMMNO$   ZIZNI?$++~,,.                 
            .. .~N7D7,               .=MNNNNDO8   II$N+7N?I,:                   
             =I=N8I..:                 .MNNNMO.  . 88DOZ8N:Z..                  
            ..IIZ8.                    .DNDND8     7DNM$8Z:.                    
            ~?IZ$OI7                  ..8DNDZD      OD$OND+.                    
           :$$DO+...                   .NDNDDN      ON$DZ7I.                    
         .:8IZ7+..                      $NDDNN.     .8NM8N.                     
         .7ID$?..                     .MMODDNM      .DNM8N.                     
       .?I$D$I..                      D=DN8DM77,    IN8MNN                      
      .:IIZZI.                      78ND8DNNNO,:,.,.DMDDMD.                     
      .~??$I.                     .77O8DNN~+==++++++ZDO8DMO,                    
    ..:77Z?                       $$7:ZZ===++===++++N8ODDN=:..                  
    .~Z+O~.                         ... ... .,:~=+++MOD88N+~:,.                 
    .~+?,                                    ...:~~+DN+$8N+~:,.                 
    ,+?                                           .=Z??$ZN=~:,.                 
   .+                                               .~8+~~.                     
   ,.                                                 .                         ");
            }
        }

        private void DrawTrap()
        {
            WriteLine(@"
                     MM        M?                                               
                     M        M  M                                              
                      7  M      M M       M                                     
                    MMM   M   MMM $     M 8                                     
              MM    M?M   IM$MMMM  M  M M                                       
                   M M M  ?MM? M8  MM M M8                                      
             ?    MM M M  MMM? ZM  M ?   MMM     M                              
             M $   MM M M MM   NM    M ?  MM  M8M                               
             MM MM8M MM M  MMMMM   ?  ? M   M M M?                              
             MMM    ?MM MM       MMM  M  ?M   IO M                              
          MM  MMMMMMMMN               MMM  MO7  MM                              
          MNM M    MM                   MIN   ?MMM            M                 
          MMMMM ?MMM                     M    MM$M           MIM     M          
          MM MMMMMM                         ?M  M?  MM      M MMM   M M         
          M MMMMMM                          M  7MM?M MM I    M  ?  M MMM        
          M M MMM M                          I M7 MDMM MM  MM IMMMM   8     M   
          MMMMMMM M                            M MMM MMMMM?M  M  ?  7?  M  M M  
            MM M   M                            MM8M DMMM?MMM  ?M  MN MM MM  M  
           M      MM                     MM     7MMM MMMMMMM  $   M MM  M M MM  
            ? M   M                   M    M        MMMMM?MMM   M   M ? ?M ?MM  
             ?M  ?MMM  MM           M M    ?  M   ?M M M MM?MMM  M8  M M M  MM  
              O  MMMM MM           MMM    M  ?M  IMOMM MMMM       MMM M M   MMM 
               M?M?  M   M        M      MM  M   M?  MM O             M MM   M  
                MMZ M   M M       M     MM  ?$  MM   MZM                MMMMMMM 
                 M      MM?       MM ?MMM  ?M  MM  MM                    MMMM M 
     $M ?M         M M  M IM       M MMM? MMM    MMM                      M?MMM 
   ?M MMM? M        MM  M MM OMM    MM      ?MMMMM                      M MM MM 
   ?MMMMMMIM         M  M M MM  MMM  MMMM?M?MMMM                         M MMMM 
 MMM MMM?M            M M MMM MZI   MMMMMMM MM                         M  M?MM  
?M MMM                  MM$MM   ?M MMM?N  MM                          M MM MMM  
?MM?MMM                  M M?MMM?IM M  ?? MM      MM         MIM      M MMMMMM  
 M M? MMMMM M              NMM ?MM??M?   M? ?    M ?        MM  M    M7?IMMMM   
   MMMMMM    MM M MMM       M MM7M?M M?  M  M?  M  MM M    M  MM  M   MM MM$M   
       MM MDM??M  MMM?M?8  N MMMMMMZM  M  M  ? M M7    M    MM   ?M M?M? M M    
               MMMM M M M M?7M  M?MMMM  M  M   M   MM   M D     M 7 M M M M     
                    MMMMM MMMMMMMMMM  M  M  M   $?   MMMM   MM M MM M M MM      
                        MM M?M  M      M  ?M M   ?M7          M  ZM M MM        
                         MM M M            IMM $M  MM?      MM   M? MM          
                                                MM      MMM    ?MMM             ");
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

        private void DrawFarmer()
        {
            WriteLine(@"
.,..,.,,.,..,..,.,,.,..?OZ?IZOOI..,.,,.,..,..,.,,.,..,.,,.,,
,.,,.,..,.,,.,,.,..,.,ZIII??+=~~~+=I..,.,,.,,.,..,.,,.,..,. 
.,..,.,I7OOZ,..,.,,.,.$IIII7I=~~:=+I$=.,..,..,.,,.,..,.,,.,,
,.,,.,..77ZZO===+OO,.O77????I?~==~==?I,.,,.,,.,..,.,,.,..,. 
.,..,.,,O77$ZZZO?===?+===+=7+7++II77II.,..,..,.,,.,..,.,,.,,
,.,,.,..,777$$$ZOO8OO~+=~~===?=IO$IIIZ,.,,.,,.,..,.,,.,..,. 
,.,,.,..,.7777$$$ZOOD888OO~:~~~~~+~+==Z:,,.,,.,..,.,,.,..,. 
.,..,.,,.,..,O7$$ZZOO8O8D??????IO=:?+===++=O.,.,,.,..,.,,.,,
,.,,.,..,.,,7,777$ZZZOI??????I???????888O+=??+O..,.,,.,..,. 
.,..,.,,.,..,.O7777$ODI=DOO8+++8OOD~+OO8OOOZZ$OOIO,..,.,,.,,
,.,,.,..,.,,.,,.Z7778D==$8DI+?++D88?~OO$OZ$$$$$$$7O,,.,..,. 
.,..,.,,.,..,..,.,,O?D==~O,+==+~:Z,?~O8$$$$7777OO.,..,.,,.,,
,.,,.,..,.,,.,,.,..,+8=~~~~==,~=~~~::D~777OOZ.,..,.,,.,..,. 
,.,,.,..,.,,.,,.,..,88~~~~~~~:~~:~:::88.,,.,,.,..,.,,.,..,. 
.,..,.,,.,..,..,.,,.,.=~~D:==+==:~~::8.,..,..,.,,.,..,.,,.,,
,.,,.,..,.,,.,,.,..,.,=~~~=.8+++DD~::.,.,,.,,.,..,.,,.,..,. 
.,..,.,,.,..,..,.,,.,.8~~::.....::~:8,.,..,..,.,,.,..,.,,.,,
,.,,.,..,.,,.,,.,..,..,O~::~:::~:::=..,.,,.,,.,..,.,,.,..,. 
.,..,.,,.,..,..,.,,.,..,.~::::::~:~.,,.,..,..,.,,.,..,.,,.,,
,.,,.,..,.,,.,,.,..........O?~:O?..........,,.,..,.,,.,..,. 
,.,,.,..,.,,.,,.,..,.,,.,.?Z~~~=$+.,..,.,,.,,.,..,.,,.,..,. 
.,..,.,,.,..,..,.,,.,..OD$+=?~~?+==D,,.,..,..,.,,.,..,.,,.,,
,.,,.,..,.,,.,,.,..,OZZOO$OZO+ZOZ=+Z$O,.,,.,..,..,.,,.,..,. 
.,..,.,,.,..,..,.,?$Z8OZO+8ZZ8ZZZI?OOO8,..,..,.,,.,..,.,,.,,
,.,,.,..,.,,.,. I?OODZOO8+?ZO8O$8??8OZZ.,,.,..,..,.,,.,..,. 
.,..,.,,.,..,..,+++8D88D8I7?++=+7$?88O8,..,..,.,,.,..,.,,.,,
,.,,.,..,.,,.,$:=88?8,,D?I?=+++?I=+7D+~+,,.,..,..,.,,.,..,. 
,.,,.,..,.,,.,:::: ,.,O$++?+IOO=+++$Z?+Z,,.,..,..,.,,.,..,. 
.,..,.,,.,..,.Z~:~:.,..O7=+7?==?D?+7~~~,..,....,,.,..,.,,.,,
,.,,.,..,.,,.,.8~~~~.,,77+++IIOO++7,:~:8,,.,..,..,.,,.,..,. 
.,..,.,,.,..,..,.~~~$::~IDII????I78.::~~..,..,.,,.,..,.,,.,,
,.,,.,..,.,,.,,.,..O:::,:O+?????$8O,:~~~,,.,,.,..,.,,.,..,. 
.,..,.,,.,..,..,.,,:::::=++7?+=?=$?.~~~8..,..,.,,.,..,.,,.,,
,.,,.,..,.,,.,,.,..,.+::,++++++++=II~~~.,,.,,.,..,.,,.,..,. 
,.,,.,..,.,,.,,.,..,.~I?I?+++O++?++??~~.,,.,,.,..,.,,.,..,. 
.,..,.,,.,..,..,.,,.Z~?8++++??8+++++==~$..,..,.,,.,..,.,,.,,
,.,,.,..,.,,.,,.,..,+,7$+++++I,I+++NOO:D,,.,,.,..,.,,.,..,. 
.,..,.,,.,..,..,.,,.??I$?????7.7???OOOND..,..,.,,.,..,.,,.,,
,.,,.,..,.,,.,,.,..,~=+??????D,.D??OOOOD,,.,,.,..,.,,.,..,. 
.,..,.,,.,..,..,.,,.,7=$77???8.,I??8??7D..,..,.,,.,..,.,,.,,
,.,,.,..,.,,.,,.,..,.,~.OD7III,.DI?777O.,,.,,.,..,.,,.,..,. 
,.,,.,..,.,,.,,.,..,.,,.DZZZO.,.,,DOOZZ.,,.,,.,..,.,,.,..,. 
.,..,.,,.,..,..,.,,.,..,DZZZO,.,..,OZZZ,..,..,.,,.,..,.,,.,,
,.,,.,..,.,,.,,.,..,.,,I77Z$D.,.,,.,O$$I,,.,,.,..,.,,.,..,. 
.,..,.,,.,..,..,.,,.,II77$$$D,.,..,.Z$$$7D,..,.,,.,..,.,,.,,
,.,,.,..,.,,.,,.,..,.$II7$.~..,.,,.,ZOO$77I,,.,..,.,,.,..,. 
.,..,.,,.,..,..,.,,.,..,.,,.,,.,..,.,,.$77D..,.,,.,..,.,,.,,");
        }

        private void DrawChest()
        {
            WriteLine(@"
              MMMM                      
           MMO    MMM                   
         MM          MMM+               
        MM              =MMD     .      
       MM                   MMM         
        =MMN              ...  MMM      
            MMM            .MM.   MM    
               MMM..      MM     . M    
            MMM M MMM ..:M .      .M7   
         MMM    M  . MMMM   .    . M..  
     ?MMO       M     . MMM  .    MM    
   MM~   ..  .  M  ... .   MMM.  MM     
   M MMM  M,    M. ..   .  ...$MMM      
   M    MMMZMM .M    .       MMMM       
. .M .    M  MM M .      NMM,   M       
  .M  .   MMMM.MMM    MMM       M       
   M.. .    .M   .MMMM          M       
   M  . . .  .      M           M       
   MMM .  ..        M           M       
      MMM           M           M       
         MMM        M        MMM        
            MMM     M    :MMM           
               MMM  M MMM               
                  MMMM                  ");
        }

        private void DrawKey()
        {
            WriteLine(@"
       ..... .                          
     ...7??77....                       
    .:ZI?+..??...                       
  ..O$+I,....$$..                       
  .?I=......,IZ...                      
 ..77$... $IZ$O....                     
...,$I....$D8O.......                   
 ....OZZ7Z77OIDZ......                  
    ..........O$Z........               
     ..........88OZ........             
       ..........O$O, ......            
        ..........OOOO.......           
         ...........DO8Z.......         
           ..........8888.......        
            ..........,NZO$$......      
              ..........ZODD,......     
                 ........DN8DO........  
                  .......,ONN8DZ....... 
                   .....,88DDNDO8.....  
                     ..OI888D8NN8DO     
                     ..ZOZO88D8.MDD     
                       ..ZZ7.......     
                      . ..7I......      
                         .. ...         ");
        }

        private void DrawDoorOpen()
        {
            WriteLine(@"
               :::888888::,                         :::88888DOZ88:              
              M~~~?IIII~~~IM                       M=~~+II?D?~7878D             
          .M~~+III=+II~?III~~~M                 MZ~~III+=D??+?78ID?$            
        ~Z++II7+NND8888NN$IIII++$~           ~$++?III8NDDZ????78ID?$+Z~         
       $~III~MN888II7778888MM=II?=:         :~?II+MMDDDD8?????I8ID?$8I~+        
       I~IIM88Z77III7778?IID88$I?~M         D~?IZMDDDDD8O??~$?I87D?7?8~$        
      M+IIM8D?I7IIII7778?7III88MII~M       M=IIMDDDDDDD8O??~$?I87D?7?8I+M       
    ~8=?IM87D?77I7IIII78?77I7D$8DI?=8:   ~8=?INDDDDDDDDZ??~7$?I8ID?7?7D?=8~     
    M~IIM887D?7II7IIII78?77I7DI8DMII~I   O~IIMDDDDDDDDDZ??~7$?78ID?7?IDII~M     
    M~IIM877D?7I77IIII78?77IIDI7DMII~I   O~IIMDDDDDDDDDZ??~7$?787D?7?I8$I~M     
   ~~~~$8?77D?7777III778?77IIDI7$8=~~7   +~~=DDDDDDDDDDZI???7DD87D?$?78I~~~,    
   ~~IIO8?77D?7777II7778?7777DI7$8III7   +II7DDDDDDDDDDZI?77?77??DD8?78$II~,    
   ~~II7??????????????????????????8II7   +II7DDDDDDDDDDZ??DD8877+77I??D$II~,    
   ~~II7777777777777777777777777778II7   +II7DDDDDDDDDDZ??~7$?8888877+7$II~,    
   ~~IIO88888888888888888888888888III7   +II7DDDDDDDDDDZ??~I$?787D?8888$II~,    
   ~~~~$8?77D?I777II77I8?7777DII$8=~~7   +~~=DDDDDDDDDDZ??~I$?787D?$?78I~~~,    
   ~~IIO8?$7D?7777III7I8?7777DI778III7   +II7DDDDDDDDDDZ??~8I?78ID?$?78$II~,    
   ~~IIO8?O8D?7I77IIII78?7777DI778III7   +II7DDDDDDDDDDZ??~8I?78ID?$?I8$II~,    
   ~~IIO8?77D?77I7II7I78?7777DII78III7   +II7DDDDDDDDDDZ??~7O?I8ID?7?I8$II~,    
   ~~IIO8?77D?77I7II7I78?7777DII$8III7   +II7DDDDDDDDDDZ??~IO?I87D?7?78$II~,    
   ~~~~$8?77D?777III7I78?7II7DII78=~~7   +~~=DDDDDDDDDDZ??~7O?I8ID?$?I8I~~~,    
   ~~IIO8?O8D?777III7I78?7II7DII$8III7   +II7DDDDDDDDDDZ??~8I?I8ID?$?I8$II~,    
   ~~IIO8?77D?777III7778?III7DI7$8III7   +II7DDDDDDDDDDZ??~8I?78ID?7?I8$II~,    
   ~~IIO8?77D?777III7778?77IIDI7$8III7   +II7DDDDDDDDDDZ??~7$?787D?$?78$II~,    
   ~~IIZDDDDDDDDDDDDDDDDDDDDDDDDDDIII7   +II7DDDDDDDDDDZ??~I$?I87D?$?8D7II~,    
   ~~~~777I7777+77777+7777I7777+778~~7   +~~=DDDDDDDDDDZ??~I$?78O8D??77Z~~~,    
   ~~IIZDDDDDDDDDDDDDDDDDDDDDDDDDDIII7   +II7DDDDDDDDDDZ??~78D??777+77D7II~,    
   ~~IIO8?I7D?7I777I7I78?7II7DI7$8III7   +II7DDDDDDDDDDZI?I?77+77888888$II~,    
   ~~??O8?77D?7I77III778?77I7DI7$8???7   +???DDDDDDDDDDZI?77IOO888I$?I8$??~,    
   ~~IIO8?7ID?7777III7I8?7777DI7$8III7   +II7DDDDDDDDDDZ??D88?78ID?7?78$II~,    
    MMMMM888M88888D8888M88888M88DMMMMI   OMMMMMMMMMMMMMZ??~I$?I87D?7?78MMMM     
                                                       Z??~7$?I87D?$8,          
                                                       Z??~7$?7878Z~            
                                                       Z??~7$?7D,               
                                                       Z??~7$D                  
                                                       =ZZZ8,                   ");
        }

        private void DrawDoorClosed()
        {
            WriteLine(@"
                                      MMOM                                      
                                7M~..Z7 .MO .IM.                                
                           . . M77.II.?.:N Z...Z.                               
                          .MMMN8N    .M,.M. ..~MOMMM=                           
                        ..MZ.  .IOMMMMI8.OMMMMM....,?M                          
                      ..,,$     N?.M.  8.,..~ N     .M.:.                       
                    . M. .=. ~OM8=.M   O    ~ ?MMD..$ . I$.                     
                    .,M.    =NM,$~.M.: O  . ~ ~ OM8N.   .M.                     
                    .M.. .7 M ,.Z..M. .8....:  ..D+O.I  .,M                     
                ....:N. .OII.. . ......... .. , ....M .. ,:,....                
                 ..MZO.D..M..M: .NM.:M ..M.  M,. MM.?$..N?8MN. .                
                 .,M.  . M, ..   . .      .  ..   . .= .. .:M.                  
                 .,M  .MN+$$$$II???M???8????I????M$$$$DN   .M.                  
                 .~D  :DM. ~... =  M   Z    ~  ~ M=,  MM.. .M.                  
                  OI  ,.M..=Z:. +  M   Z    ~ .~.M.,  MM.  .M.                  
              ....MOZI?MM  .7:. :  M   Z  . ~ .+.M.+   MM,M7M,...               
                .M Z..++M  .::. .  M   Z  . ~  :.M.7  .MM?.=.M~                 
                .M + OM.M.  .:..   M  ,Z ., ~  :.M.M  .MMM...M+.                
                .M.. .M.M~...:..   M..~O ., ~  ,.M.N. =M$$...DM.                
                .M.   M.M....:...  M..,Z .. ~  ~.M. ..7MMO...MM.                
             ....M... M.M=. .:..   M   Z    ~ .: M. ,.,M.8. .7+....             
                .Z.  MM.M,. .:..:  M  :Z    ~ ., M.    MOM  .D..                
                $O. .,M.MI  .:..,  M  7Z   .~  ,.M. . .MMD,...M.                
                ~N ..,M.M   .:...  M..IO    ~    M.  ..M.D....M.                
              . .M,...M.M.. .:. ,  M..:Z    ~ ...M.. ..M,Z  .IM.                
                 M7...M.M D..:. :  M. ,$    ~   .M..8D M87. .IM                 
                .MI....NM.O .:. .  M  $Z....~ . .M. O8.MM , ~$M.                
                M?I. 7.MM=I=~+~~:~:M::ZO:=:~=~::~M=~NO=MI . .ZN.                
                MMZOO$D+    . ..   .              .  . .M7$88DM.                
                .M .  $,.M.  +M.  .M?   M..MN.  =M. .=M.M..  M=.                
                .M   .$7...    .   .       .    . .   . M?  .~M.                
                .M  ,ZNNMMMNMNNNNNNNDDDM888NDDDDDNMMNMMMM~  .:M.                
                D?.. .8DM :..O     ,,..M   D     M..$. DD= ...M.                
                +NDI~MM M  :.O..   ,: .M   N     N  =. DOM7.D+M,                
                MM..  : M  ~.O     .:..M . M     M..=..8M.   7M.                
                MMO. 8M.M  + O   .7.:I.M  .M..   M..~  8~DI..,M.                
                7M. MM .M... O  : 8 =Z.M.  M...,.M.... 8.=MD.MM.                
                 .NMM...:=MMMMMMMMMMM=.MM:IMMMMMMMMMMN:,..=MMM..                
                   ..,...   ..8.                ..    ....~.                    
                 .. ..      ....        .        $,        $..                  
                  ., . .    .....  ...  ..   ......     . ..M.                  
                .,.   ::::::::::::::::  $.,::::::::::::::,. .=..                
              . M.          :. .        M         ,.          ...               
              .M      ......~.          ,..      . .  ......   .:..             
              ..   ......  .  ......... .........  ,... ..   .                  
                       . . $                       .+. . . .                    ");
        }

        private void PlaySound(string audioLink, int whatToDo = 1)
        {
            if (whatToDo == 1)
            {
                playSound = new SoundPlayer(audioLink);
                playSound.Load();
                playSound.Play();
            }
            else if (whatToDo == 2)
            {
                playSound = new SoundPlayer(audioLink);
                playSound.Load();
                playSound.PlayLooping();
            }
            else
            {
                playSound.Stop();
            }
        }

        private void Print(string words, int speed = 10)
        {
            foreach (char letter in words)
            {
                Write(letter);
                System.Threading.Thread.Sleep(speed);
            }
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
            Item eleventhItem = new Item("Dragon Breath Shield", "Very useful for fighting dragons - £100", 0, 0, 0, 0, 0, 100);

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
            _itemRepo.AddToListOfItems(eleventhItem);
        }
    }
}
