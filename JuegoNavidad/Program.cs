using System;
using System.CodeDom;
using System.Text;
using System.Threading;


namespace JuegoNavidad
{
    internal class CSKingdom
    {
        const int LIGHT_ATTACK_DAMAGE = 10;
        const int MEDIUM_ATTACK_DAMAGE = 20;
        const int HEAVY_ATTACK_DAMAGE = 40;
        const int LIGHT_ATTACK_PROBABILITY = 80;
        const int MEDIUM_ATTACK_PROBABILITY = 50;
        const int HEAVY_ATTACK_PROBABILITY = 30;
        const int POTION_HEALING_AMMOUNT = 50;
        const int MANA_RECOVER_AMMOUNT = 50;

        const string TITLE = @"
                                          .-++=:                              
                                        :%@@@@@%-                            
                                       .#@@@@@@@@-                           
                                        =#@@@@@@%.                           
                                         +%@@@@+.                            
                                          .....                              
                                                         
                                          -%%%%:..                           
                                         #@@@@@@%+                           
                                        .%@@@@@@@*.                          
                                         =@@@@@@@#.                          
                                         .:*@@@@@=.                          
                                           ..=@@#.                           
                                            .+@%.                            
                                          .-*@+.                             
                                         .:-=..                              
                                         .-.                                 
              ____ ____ _  _ ____ ____ ___    _  _ _ __ _ ____ ___  ____ _  _  
              |___ ==== |--| |--| |--< |--'   |-:_ | | \| |__, |__> [__] |\/| 
                                 ";

        const string BOSS_DEMON = @"
                                             ,--,  ,.-.
               ,                   \,       '-,-`,'-.' | ._
              /|           \    ,   |\         }  )/  / `-,',
              [ ,          |\  /|   | |        /  \|  |/`  ,`
              | |       ,.`  `,` `, | |  _,...(   (      .',
              \  \  __ ,-` `  ,  , `/ |,'      Y     (   /_L\
               \  \_\,``,   ` , ,  /  |         )         _,/
                \  '  `  ,_ _`_,-,<._.<        /         /
                 ', `>.,`  `  `   ,., |_      |         /
                   \/`  `,   `   ,`  | /__,.-`    _,   `\
               -,-..\  _  \  `  /  ,  / `._) _,-\`       \
                \_,,.) /\    ` /  / ) (-,, ``    ,        |
               ,` )  | \_\       '-`  |  `(               \
              /  /```(   , --, ,' \   |`<`    ,            |
             /  /_,--`\   <\  V /> ,` )<_/)  | \      _____)
       ,-, ,`   `   (_,\ \    |   /) / __/  /   `----`
      (-, \           ) \ ('_.-._)/ /,`    /
      | /  `          `/ \\ V   V, /`     /
   ,--\(        ,     <_/`\\     ||      /
  (   ,``-     \/|         \-A.A-`|     /
 ,>,_ )_,..(    )\          -,,_-`  _--`
(_ \|`   _,/_  /  \_            ,--`
 \( `   <.,../`     `-.._   _,-`
";

        const string BOSS_KNIGHT = @"
                                 _A_
                                / | \
                               |.-=-.|
                               )\_|_/(
                            .=='\   /`==.
                          .'\   (`:')   /`.
                        _/_ |_.-' : `-._|__\_
                       <___>'\    :   / `<___>
                       /  /   >=======<  /  /
                     _/ .'   /  ,-:-.  \/=,'
                    / _/    |__/v^v^v\__) \
                    \(\)     |V^V^V^V^V|\_/
                     (\\     \`---|---'/
                       \\     \-._|_,-/
                        \\     |__|__|
                         \\   <___X___>
                          \\   \..|../
                           \\   \ | /
                            \\  /V|V\
                             \|/  |  \
                              '--' `--`
";

        const string SKELETON = @"
      .-.
     (o.o)
      |=|
     __|__
   //.=|=.\\
  // .=|=. \\
  \\ .=|=. //
   \\(_=_)//
    (:| |:)
     || ||
     () ()
     || ||
     || ||
    ==' '==
";

        const string GOBLIN = @"
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
        const string DOG = @"
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

        const string KNIGHT = @"
  ,^.
  |||
  |||       _T_
  |||   .-.[:|:].-.
  ===_ /\| ""'""  |/
   E]_|\/ \--|-|''''|
   O  `'  '=[:]| A  |
          /""""|  P |
         /"""" `.__.'
        []""/"" \[]
        | \     / |
        | |     | |
      <\\\)     (///>
";

        // UI
        public static int SelectMainMenu(string[] options)
        {
            Console.CursorVisible = false;
            int menuStartY = (Console.WindowHeight / 2) + 7;
            int selectedOption = 0;
            while (true)
            {

                for (int i = 0; i < options.Length; i++)
                {
                    int optionX = Math.Max(0, (Console.WindowWidth / 2) - (options[i].Length / 2));
                    int optionY = menuStartY + i;
                    if (i == selectedOption)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        DrawText(optionX, optionY, 0, $"> {options[i]}");
                        Console.ResetColor();
                    }
                    else
                    {
                        DrawText(optionX, optionY, 0, $"  {options[i]}");
                    }

                    Thread.Sleep(100);
                }

                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.UpArrow)
                {
                    selectedOption = (selectedOption > 0) ? selectedOption - 1 : options.Length - 1;
                }
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    selectedOption = (selectedOption < options.Length - 1) ? selectedOption + 1 : 0;
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    return selectedOption;
                }
            }
        }
        public static int Select(int menuX, int menuY, string[] options)
        {
            Console.CursorVisible = false;
            int selectedOption = 0;
            while (true)
            {

                for (int i = 0; i < options.Length; i++)
                {

                    int optionY = menuY + i;
                    if (i == selectedOption)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        DrawText(menuX, optionY, 0, $"> {options[i]}");
                        Console.ResetColor();
                    }
                    else
                    {
                        DrawText(menuX, optionY, 0, $"  {options[i]}");
                    }

                    Thread.Sleep(100);
                }

                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.UpArrow)
                {
                    selectedOption = (selectedOption > 0) ? selectedOption - 1 : options.Length - 1;
                }
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    selectedOption = (selectedOption < options.Length - 1) ? selectedOption + 1 : 0;
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    return selectedOption;
                }
            }
        }
        public static void DrawUI(int x, int y, int width, int height, char character)
        {
            DrawRectangle(x, y, width, height, character);
        }

        public static void DrawTransition1()
        {
            string[] patterns =
            {
                new string(' ', Console.WindowWidth),
                new string('░', Console.WindowWidth),
                new string('░', Console.WindowWidth),

                new string('▒', Console.WindowWidth),
                new string('▒', Console.WindowWidth),

                new string('▓', Console.WindowWidth),
                new string('▓', Console.WindowWidth),

                new string('█', Console.WindowWidth)
            };

            string whiteLine = new string('█', Console.WindowWidth);
            int whiteLines = Console.WindowHeight - 5;

            for (int i = 0; i < Console.WindowHeight + (patterns.Length * 2) + whiteLines; i++)
            {
                Console.CursorVisible = false;
                for (int j = 0; j < patterns.Length; j++)
                {
                    int layerPos = i - j;
                    if (layerPos >= 0 && layerPos < Console.WindowHeight)
                    {
                        Console.SetCursorPosition(0, layerPos);
                        Console.Write(patterns[j]);
                    }
                }

                for (int w = 0; w < whiteLines; w++)
                {
                    int whitePos = i - patterns.Length - w;
                    if (whitePos >= 0 && whitePos < Console.WindowHeight)
                    {
                        Console.SetCursorPosition(0, whitePos);
                        Console.Write(whiteLine);
                    }
                }

                for (int j = 0; j < patterns.Length; j++)
                {
                    int layerPos = i - patterns.Length - whiteLines - j;
                    if (layerPos >= 0 && layerPos < Console.WindowHeight)
                    {
                        Console.SetCursorPosition(0, layerPos);
                        Console.Write(patterns[patterns.Length - 1 - j]);
                    }
                }

                Thread.Sleep(1);
                if (i > Console.WindowHeight / 2)
                {
                    Console.Clear(); //Clean residual lines
                }
            }
            Console.Clear();
        }

        public static void DrawText(int x, int y, int timeBetweenKeyStrokes, string text)
        {
            Console.CursorVisible = false;
            if (x >= 0 && y >= 0 && x < Console.WindowWidth && y < Console.WindowHeight)
            {
                Console.SetCursorPosition(x, y);
                for (int i = 0; i < text.Length; i++)
                {
                    Console.Write(text[i]);
                    Thread.Sleep(timeBetweenKeyStrokes);
                }
            }
            else
            {
                Console.WriteLine("Out of limits");
            }
        }

        public static void EraseText(int x, int y, int spaceBetweenKeyStrokes, string text)
        {
            string spaces = new string(' ', text.Length);
            DrawText(x, y, spaceBetweenKeyStrokes, spaces);
        }

        public static int DrawMainMenu(string[] menuOptions, string title)
        {
            Console.CursorVisible = false;
            string[] titleLines = title.Split('\n');
            int titleY = Console.WindowHeight / 2 - menuOptions.Length - 15;

            for (int i = 0; i < titleLines.Length; i++)
            {
                string line = titleLines[i];
                int titleX = ((Console.WindowWidth - line.Length) / 2) - 5;
                Console.SetCursorPosition(titleX, titleY++);
                Console.Write(line);

                if (i == titleLines.Length - 4)
                {
                    Thread.Sleep(2000);
                }
                else
                {
                    Thread.Sleep(80);
                }
            }

            Thread.Sleep(500);

            int selectedOption = SelectMainMenu(menuOptions);
            return selectedOption;
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

        // Battle
        public static void PlayerTurn(int playerChoice, string[] playerInventory, Random generator, ref int computerHealth, ref int playerHealth, string enemyName)
        {
            bool success = false;
            int chosenAttackDamage = 0;
            switch (playerChoice)
            {
                case 0:
                    success = generator.Next(1, 101) <= LIGHT_ATTACK_PROBABILITY;
                    if (success)
                    {
                        computerHealth -= LIGHT_ATTACK_DAMAGE;
                        chosenAttackDamage = LIGHT_ATTACK_DAMAGE;
                    }
                    break;
                case 1:
                    success = generator.Next(1, 101) <= MEDIUM_ATTACK_PROBABILITY;
                    if (success)
                    {
                        computerHealth -= MEDIUM_ATTACK_DAMAGE;
                        chosenAttackDamage = MEDIUM_ATTACK_DAMAGE;
                    }
                    break;
                case 2:
                    success = generator.Next(1, 101) <= HEAVY_ATTACK_PROBABILITY;
                    if (success)
                    {
                        computerHealth -= HEAVY_ATTACK_DAMAGE;
                        chosenAttackDamage = HEAVY_ATTACK_DAMAGE;
                    }
                    break;
                case 3:
                    int selection = Select(Console.WindowWidth / 2, 40, playerInventory);
                    switch (selection)
                    {
                        case 1:
                            break;
                    }
                    break;

            }

            if (success)
            {

                DrawText(24, 30, 0, $"Attack successful! You dealt {chosenAttackDamage} damage to {enemyName}.");
            }
            else
            {
                DrawText(24, 30, 0, "Attack missed!");
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
                DrawText(24, 30, 0, $"The computer attacks with a {computerAttack}!");
                DrawText(24, 30, 0, $"Attack successful! Computer deals {chosenAttackDamage} damage to you.");
            }
            else
            {
                DrawText(24, 30, 0, "Attack missed!");
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
            DrawText(Console.WindowWidth / 2, (Console.WindowHeight / 2) + 10, 10, win ? victoryMessage : defeatMessage);
        }

        public static void CorrectHealth(ref int playerHealth, ref int computerHealth)
        {
            playerHealth = Math.Max(playerHealth, 0);
            computerHealth = Math.Max(computerHealth, 0);
        }

        public static void DisplayHealth(int playerHealth, int computerHealth, string playerName, string enemyName)
        {
            DrawText(24, 30, 0, $"{enemyName} health: {computerHealth} Your health: {playerHealth}");
        }

        public static bool Battle(Random generator, ref int playerHealth, ref int computerHealth, string[] playerInventory, string[] options, string playerName, string enemyName, string enemyDrawing)
        {
            DrawEnemy(Console.WindowWidth / 2, Console.WindowHeight / 2, enemyDrawing);

            bool win = false;
            bool defeat = false;

            int playerChoice;
            while (!win && !defeat)
            {
                playerChoice = Select((Console.WindowWidth / 2) - 20, 40, options);

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

        // Menus
        static string[] playerBattleOptions = { "Light (80% success, 10 damage)", "Medium (50% success, 20 damage)", "Heavy (30% success, 40 damage)", "Potion (recover 50 health points)" };
        static string[] mainMenuOptions = { "New Game", "Instructions", "Exit" };

        static void Main()
        {
            Console.SetWindowSize(170, 44);
            Console.SetBufferSize(171, 45);

            int playerHealth = 100;
            int playerMana = 100;
            int playerStrength = 5;

            int computerHealth = 100;

            string[] playerInventory = new string[5];
            playerInventory[0] = "potion";

            string playerName = "HieN";
            string enemyName = "Patatones";

            Random generator = new Random();

            // MaxWiwdth = 170 MaxHeight = 44
            // maxWidth = Console.LargestWindowWidth;
            // int maxHeight = Console.LargestWindowHeight;
            //Console.WriteLine($"Largest console size: {maxWidth} x {maxHeight}");

            //DrawEnemy(Console.WindowWidth / 2, Console.WindowHeight / 2, GOBLIN);
            //DrawText((Console.WindowWidth / 2) + 5, (Console.WindowHeight / 2) + 5, 0, "Press F11, I'm telling ya!");
            //Thread.Sleep(5000);

            //Console.Clear();
            

            //int selectedOption = DrawMainMenu(mainMenuOptions, TITLE);

            Console.Clear();

            DrawRectangle(5, 0, Console.WindowWidth - 11, Console.WindowHeight - 15, '▓'); //Frame
            DrawRectangle(5, 30, 30, 10, '#');
            DrawRectangle(55, 30, 50, 10, '#');
            DrawRectangle(105, 30, 50, 10, '#');

            //Battle(generator, ref playerHealth, ref computerHealth, playerInventory, playerBattleOptions, playerName, enemyName, GOBLIN);

            //switch (selectedOption)
            //{
            //    case 0:
            //        Battle(generator, ref playerHealth, ref computerHealth, playerInventory, playerBattleOptions, playerName, enemyName, GOBLIN);
            //        break;
            //    case 1:
            //        //ShowInstructions();
            //        break;
            //    case 2:
            //        break;
            //}

            Console.ReadLine();
            
            //DrawEnemy(Console.WindowWidth / 2, Console.WindowHeight / 4, goblin); 
            //DrawEnemy(Console.WindowWidth / 2, Console.WindowHeight / 2, dog);



            //Battle(generator, ref playerHealth, ref computerHealth, playerInventory, options, playerName, enemyName, GOBLIN);
        }
    }

}
