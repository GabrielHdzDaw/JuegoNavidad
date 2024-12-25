using System;
using System.Reflection.Emit;
using System.Text;
using System.Threading;


namespace JuegoNavidad
{
    internal class CSReign
    {
        const int LIGHT_ATTACK_DAMAGE = 10;
        const int MEDIUM_ATTACK_DAMAGE = 20;
        const int HEAVY_ATTACK_DAMAGE = 40;
        const int LIGHT_ATTACK_PROBABILITY = 80;
        const int MEDIUM_ATTACK_PROBABILITY = 50;
        const int HEAVY_ATTACK_PROBABILITY = 30;
        const int POTION_HEALING_AMMOUNT = 50;

        // UI
        public static void DrawUI(int x, int y, int width, int height, char character)
        {
            DrawRectangle(x, y, width, height, character);  
        }

        public static void DrawText(int x, int y, string text)
        {
           
            if (x >= 0 && y >= 0 && x < Console.WindowWidth && y < Console.WindowHeight)
            {
                Console.SetCursorPosition(x, y);
                Console.Write(text);
            }
            else
            {
                Console.WriteLine("Out of limits");
            }
        }

        public static void DrawMainMenu(string[] menuOptions)
        {
            for (int i = 0; i < menuOptions.Length; i++)
            {
                DrawText((Console.WindowWidth / 2) - 18, Console.WindowHeight / 2 + i + 1, menuOptions[i]);
            }
        }
        public static void DrawRectangle(int x, int y, int width, int height, char character)
        {
            for (int i = 0; i <= height; i++)
            {
                for (int j = 0; j <= width; j++)
                {
                    
                    Console.SetCursorPosition(x + j, y + i);

                    
                    if (i == 0 || i == height || j == 0 || j == width)
                    {
                        if (j == width || i == height)
                        {
                            Console.Write(character + "\n");
                        }
                        else
                        {
                            Console.Write(character);

                        }
                        
                    }
                }
            }
        }
        public static int GetDrawingWidth(string drawing)
        {
            string[] lines = drawing.Split('\n');
            int maxWidth = 0;
            foreach (string line in lines)
            {
                if (line.Length > maxWidth)
                    maxWidth = line.Length;
            }
            return maxWidth;
        }
        public static void DrawEnemy(int x, int y, string enemy)
        {
            int enemyWidth = GetDrawingWidth(enemy); 
            int startX = x - (enemyWidth / 2); 

            string[] lines = enemy.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                Console.SetCursorPosition(startX, y + i);

                if (i == lines.Length - 1)
                {
                    Console.Write(lines[i] + "\n");
                }
                else
                {
                    Console.Write(lines[i]);
                }
            }
        }

        // Menus
        string[] playerBattleOptions = { "Light (80% success, 10 damage)", "Medium (50% success, 20 damage)", "Heavy (30% success, 40 damage)", "Potion (recover 50 health points)" };
        static string[] mainMenuOptions = { "New Game", "Instructions", "Exit" };

        // Battle
        public static void PlayerTurn(string playerChoice, string[] playerInventory, Random generator, ref int computerHealth, ref int playerHealth, string enemyName )
        {
            bool success = false;
            int chosenAttackDamage = 0;
            switch (playerChoice)
            {
                case "1":
                    success = generator.Next(1, 101) <= LIGHT_ATTACK_PROBABILITY;
                    if (success)
                    {
                        computerHealth -= LIGHT_ATTACK_DAMAGE;
                        chosenAttackDamage = LIGHT_ATTACK_DAMAGE;
                    }
                    break;
                case "2":
                    success = generator.Next(1, 101) <= MEDIUM_ATTACK_PROBABILITY;
                    if (success)
                    {
                        computerHealth -= MEDIUM_ATTACK_DAMAGE;
                        chosenAttackDamage = MEDIUM_ATTACK_DAMAGE;
                    }
                    break;
                case "3":
                    success = generator.Next(1, 101) <= HEAVY_ATTACK_PROBABILITY;
                    if (success)
                    {
                        computerHealth -= HEAVY_ATTACK_DAMAGE;
                        chosenAttackDamage = HEAVY_ATTACK_DAMAGE;
                    }
                    break;
                case "4":
                    Console.Write("Enter object name: ");
                    string chosenObject = Console.ReadLine();
                    if (Array.Exists(playerInventory, elemento => elemento == chosenObject))
                    {
                        switch (chosenObject)
                        {
                            case "potion":
                                playerInventory[Array.IndexOf(playerInventory, chosenObject)] = "";
                                playerHealth += POTION_HEALING_AMMOUNT;
                                Console.WriteLine($"You healed {POTION_HEALING_AMMOUNT}");
                                break;
                        }
                    } else
                    {
                        Console.Write($"You don't have {chosenObject}");
                    }
                    break;
                    
            }

            if (success)
            {
                Console.WriteLine($"\nAttack successful! You dealt {chosenAttackDamage} damage to {enemyName}.\n");
            }
            else
            {
                Console.WriteLine("\nAttack missed!\n");
            }
        }
        public static void ComputerTurn(Random generator, ref int playerHealth)
        {
            int chosenAttackDamage = 0;
            bool success = false;
            int computerChoice = generator.Next(1, 4);
            string computerAttack = "";
            switch (computerChoice)
            {
                case 1:
                    computerAttack = "Light Strike";
                    success = generator.Next(1, 101) <= LIGHT_ATTACK_PROBABILITY;
                    if (success)
                    {
                        playerHealth -= LIGHT_ATTACK_DAMAGE;
                        chosenAttackDamage = LIGHT_ATTACK_DAMAGE;
                    }
                    break;
                case 2:
                    computerAttack = "Medium Strike";
                    success = generator.Next(1, 101) <= MEDIUM_ATTACK_PROBABILITY;
                    if (success)
                    {
                        playerHealth -= MEDIUM_ATTACK_DAMAGE;
                        chosenAttackDamage = MEDIUM_ATTACK_DAMAGE;
                    }
                    break;
                case 3:
                    computerAttack = "Heavy Strike";
                    success = generator.Next(1, 101) <= HEAVY_ATTACK_PROBABILITY;
                    if (success)
                    {
                        playerHealth -= HEAVY_ATTACK_DAMAGE;
                        chosenAttackDamage = HEAVY_ATTACK_DAMAGE;
                    }
                    break;
            }

            if (success)
            {
                Console.WriteLine($"\nThe computer attacks with a {computerAttack}!");
                Console.WriteLine($"Attack successful! Computer deals {chosenAttackDamage} damage to you.\n");
            }
            else
            {
                Console.WriteLine("\nAttack missed!\n");
            }
        }

        public static bool PlayerWon(int computerHealth)
        {
            return computerHealth <= 0;
        }
        public static bool ComputerWon(int playerHealth)
        {
            return playerHealth <= 0;
        }

        public static void BattleEndMessage(string victoryMessage, string defeatMessage, bool win)
        {
            Console.Write(win ? victoryMessage : defeatMessage);
        }
        public static void CorrectHealth(ref int playerHealth, ref int computerHealth)
        {
            playerHealth = Math.Max(playerHealth, 0);
            computerHealth = Math.Max(computerHealth, 0);
        }

        public static void DisplayHealth(int playerHealth, int computerHealth, string playerName, string enemyName)
        {
            Console.WriteLine($"{enemyName} health: {computerHealth}\nYour health: {playerHealth}");
        }

        public static bool Battle(Random generator, ref int playerHealth, ref int computerHealth, string[] playerInventory, string[] options, string playerName, string enemyName)
        {
            bool win = false;
            bool defeat = false;
            
            string playerChoice;
            while (!win && !defeat)
            {
                playerChoice = ValidateUserInput(options);

                PlayerTurn(playerChoice, playerInventory, generator, ref computerHealth, ref playerHealth, enemyName);

                if (computerHealth > 0)
                {
                    DisplayHealth(playerHealth, computerHealth, playerName, enemyName);
                }

                win = PlayerWon(computerHealth);

                if (!win)
                {
                    ComputerTurn(generator, ref playerHealth);
                    if (playerHealth > 0)
                    {
                        DisplayHealth(playerHealth, computerHealth, playerName, enemyName);
                    }

                    defeat = ComputerWon(playerHealth);
                }

                CorrectHealth(ref playerHealth, ref computerHealth);

                if (win || defeat)
                {
                    BattleEndMessage($"Congratulations! You defeated {enemyName}!\n", $"Game over! {enemyName} wins!\n", win);
                    DisplayHealth(playerHealth, computerHealth, playerName, enemyName);
                }
            }
            return win;
        }
        // Validate user input
        public static string ValidateUserInput(string[] choicesArray)
        {
            bool validChoice = false;
            string playerChoice;

            do
            {
                for (int i = 0; i < choicesArray.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {choicesArray[i]}");
                }
                Console.Write("Choose one option: ");

                playerChoice = Console.ReadLine();

                for (int i = 0; i < choicesArray.Length && !validChoice; i++)
                {
                    validChoice = playerChoice == (i + 1).ToString();
                }

                if (!validChoice)
                {
                    Console.WriteLine($"You must enter a number between 1 and {choicesArray.Length}");
                }
            } while (!validChoice);
            return playerChoice;
        }
        static void Main()
        {



            string goblin = @"
     _____
 .-,;='';_),-.
  \_\(),()/_/
    (,___,)
   ,-/`~`\-,___
  / /).:.('--._)
 {_[ (_,_)
     | Y |
    /  |  \
   """" """"
            ";
            string dog = @"
      __ __
   .-',,^,,'.
  / \(0)(0)/ \
  )/( ,_""_,)\(
  `  >-`~(   ' 
_N\ |(`\ |___
\' |/ \ \/_-,)
 '.(  \`\_<
    \ _\|
     | |_\_
     \_,_>-'
";

            Console.SetWindowSize(240, 60);
            Console.SetBufferSize(241, 61);


            DrawMainMenu(mainMenuOptions);
            //DrawRectangle(0, 0, Console.WindowWidth - 2, Console.WindowHeight - 2, '▓');
            //DrawEnemy(Console.WindowWidth / 2, Console.WindowHeight / 4, goblin); 
            //DrawEnemy(Console.WindowWidth / 2, Console.WindowHeight / 2, dog);

            Random generator = new Random();

            int playerHealth = 100;
            int computerHealth = 100;

            string[] playerInventory = new string[5];
            playerInventory[0] = "potion";

            string playerName = "HieN";
            string enemyName = "Patatones";

            //Battle(generator, ref playerHealth, ref computerHealth, playerInventory, options, playerName, enemyName);
        }
    }

}
