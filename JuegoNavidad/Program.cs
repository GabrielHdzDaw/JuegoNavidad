using System;
using System.Data;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;


namespace JuegoNavidad
{
    internal class CSKingdom
    {
        // Epilepsy
        public static void Epilepsy(int interval, int duration)
        {
            Console.CursorVisible = false;
            DateTime startTime = DateTime.Now;

            while ((DateTime.Now - startTime).TotalMilliseconds < duration)
            {
                Console.BackgroundColor = (ConsoleColor)new Random().Next(Enum.GetValues(typeof(ConsoleColor)).Length);
                Console.Clear();
                Thread.Sleep(interval);
            }
            Console.ResetColor();
        }
        // Matrix
        const string LETTERS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_+-=<>?;:,.¿¡[]{}|\\/~`¿¡一二三四五六七八九十";
        const int TRAIL = 15;
        const int NUMBER_OF_TRAILS = 25;
        const int MIN_DELAY = 1;
        const int MAX_DELAY = 50;

        
        public static readonly object ConsoleLock = new object();//Esto es para que no pete con tantos trails

        
        public static ConsoleColor GetTrailColor(int position)//Escoger el color de las letras en función de dónde estén en el trail
        {
            if (position == 0)
                return ConsoleColor.White;
            else if (position < 5)
                return ConsoleColor.Green;
            else if (position < 10)
                return ConsoleColor.DarkGreen;
            else
                return ConsoleColor.DarkGray;
        }

        public static void RunTrail(int x, int delay, Random gen, CancellationToken token)
        {
            int height = Console.WindowHeight;
            char[] letters = new char[TRAIL];
            for (int i = 0; i < TRAIL; i++)
            {
                letters[i] = GetRandomLetter(gen);
            }

            int y = 0;

            while (!token.IsCancellationRequested)
            {
                lock (ConsoleLock)
                {
                    for (int i = 0; i < TRAIL; i++)
                    {
                        int drawPosition = y - i;

                        if (drawPosition >= 0 && drawPosition < height)
                        {
                            Console.SetCursorPosition(x, drawPosition);
                            Console.ForegroundColor = GetTrailColor(i);
                            Console.Write(letters[i]);
                        }

                        // Borrar la última letra del trail
                        if (drawPosition - TRAIL >= 0)
                        {
                            Console.SetCursorPosition(x, drawPosition - TRAIL);
                            Console.Write(" ");
                        }
                    }
                }

                y++;

                // Reiniciar cuando el trail sale de la pantalla
                if (y - TRAIL >= height)
                {
                    y = 0;
                    x = gen.Next(0, Console.WindowWidth);
                    for (int i = 0; i < TRAIL; i++)
                    {
                        letters[i] = GetRandomLetter(gen);
                    }
                }

                Thread.Sleep(delay); // Delay ajustado al mínimo
            }
        }

        public static char GetRandomLetter(Random random)
        {
            return LETTERS[random.Next(LETTERS.Length)];
        }

        public static void Matrix(int duration)
        {
            Console.CursorVisible = false;
            Random gen = new Random();

            // Token para manejar la cancelación
            CancellationTokenSource cts = new CancellationTokenSource();

            // Hacemos array de threads
            Thread[] threads = new Thread[NUMBER_OF_TRAILS];

            for (int i = 0; i < NUMBER_OF_TRAILS; i++)
            {
                // Crear un hilo por trail
                int startX = gen.Next(0, Console.WindowWidth);
                int delay = gen.Next(MIN_DELAY, MAX_DELAY);
                threads[i] = new Thread(() => RunTrail(startX, delay, gen, cts.Token));
                threads[i].Start();
            }

         
            Thread.Sleep(duration);
            cts.Cancel();

            // Unir los hilos para asegurarse de que terminen
            foreach (var thread in threads)
            {
                thread.Join();
            }
        }
        

        // CSKingdom vars
        const int LIGHT_ATTACK_DAMAGE = 10;
        const int MEDIUM_ATTACK_DAMAGE = 20;
        const int HEAVY_ATTACK_DAMAGE = 40;
        const int LIGHT_ATTACK_PROBABILITY = 80;
        const int MEDIUM_ATTACK_PROBABILITY = 50;
        const int HEAVY_ATTACK_PROBABILITY = 30;
        const int POTION_HEALING_AMMOUNT = 50;
        const int MANA_RECOVER_AMMOUNT = 50;

        // Sounds
        static SoundPlayer menuSelectSound = new SoundPlayer(@"Sounds\menuSelect.wav");
        static SoundPlayer menuAcceptSound = new SoundPlayer(@"Sounds\menuAccept.wav");
        static SoundPlayer textClickSound = new SoundPlayer(@"Sounds\textClickSound.wav");
        static SoundPlayer magicHitSound = new SoundPlayer(@"Sounds\magicHit.wav");
        static SoundPlayer meleeHitSound = new SoundPlayer(@"Sounds\meleeHit.wav");
        static SoundPlayer meleeHitSound2 = new SoundPlayer(@"Sounds\meleeHit2.wav");
        static SoundPlayer missHitSound = new SoundPlayer(@"Sounds\missHit.wav");
        static SoundPlayer potionSound = new SoundPlayer(@"Sounds\potionSound.wav");
        static SoundPlayer title1Sound = new SoundPlayer(@"Sounds\title1.wav");
        static SoundPlayer title1LowVolumeSound = new SoundPlayer(@"Sounds\title1LowVolume.wav");
        static SoundPlayer title2Sound = new SoundPlayer(@"Sounds\title2.wav");
        static SoundPlayer insertCoinSound = new SoundPlayer(@"Sounds\insertCoin.wav");
        static SoundPlayer powerOutSound = new SoundPlayer(@"Sounds\powerOut.wav");
        static SoundPlayer thunderSound = new SoundPlayer(@"Sounds\thunderSound.wav");
        static SoundPlayer switchSound = new SoundPlayer(@"Sounds\switch.wav");
        static SoundPlayer beepBoopSound = new SoundPlayer(@"Sounds\beepBoop.wav");

        // Drawings

        const string BEACH = @"
          |
        \ _ /
      -= (_) =-
        /   \         _\/_
          |           //o\  _\/_
   _____ _ __ __ ____ _ | __/o\\ _
 =-=-_-__=_-= _=_=-=_,-'|""'""""-|-,_
  =- _=-=- -_=-=_,-""          |
    =- =- -=.--""
              Playa
";
        const string FOREST = @"
            ,@@@@@@@,
    ,,,.   ,@@@@@@/@@,  .oo8888o.
 ,&%%&%&&%,@@@@@/@@@@@@,8888\88/8o
,%&\%&&%&&%,@@@\@@@/@@@88\88888/88'
%&&%&%&/%&&%@@\@@/ /@@@88888\88888'
%&&%/ %&%%&&@@\ V /@@' `88\8 `/88'
`&%\ ` /%&'    |.|        \ '|8'
    |o|        | |         | |
    |.|        | |         | |
 \\/ ._\//_/__/  ,\_//__\\/.  \_//__/_  
              Bosque
";
        const string FOREST_SCENE = @"
    .                  .-.    .  _   *     _   .
           *          /   \     ((       _/ \       *    .
         _    .   .--'\/\_ \     `      /    \  *    ___
     *  / \_    _/ ^      \/\'__        /\/\  /\  __/   \ *
       /    \  /    .'   _/  /  \  *' /    \/  \/ .`'\_/\   .
  .   /\/\  /\/ :' __  ^/  ^/    `--./.'  ^  `-.\ _    _:\ _
     /    \/  \  _/  \-' __/.' ^ _   \_   .'\   _/ \ .  __/ \
   /\  .-   `. \/     \ / -.   _/ \ -. `_/   \ /    `._/  ^  \
  /  `-.__ ^   / .-'.--'    . /    `--./ .-'  `-.  `-. `.  -  `.
@/        `.  / /      `-.   /  .-'   / .   .'   \    \  \  .-  \%
@&8jgs@@%% @)&@&(88&@.-_=_-=_-=_-=_-=_.8@% &@&&8(8%@%8)(8@%8 8%@)%
@88:::&(&8&&8:::::%&`.~-_~~-~~_~-~_~-~~=.'@(&%::::%@8&8)::&#@8::::
`::::::8%@@%:::::@%&8:`.=~~-.~~-.~~=..~'8::::::::&@8:::::&8:::::'
 `::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::.'
";
        const string VILLAGE = @"
  ~         ~~          __
       _T      .,,.    ~--~ ^^
 ^^   // \                    ~
      ][O]    ^^      ,-~ ~
   /''-I_I         _II____
__/_  /   \ ______/ ''   /'\_,__
  | II--'''' \,--:--..,_/,.-{ },
; '/__\,.--';|   |[] .-.| O{ _ }
:' |  | []  -|   ''--:.;[,.'\,/
'  |[]|,.--'' '',   ''-,.    |
  ..    ..-''    ;       ''. '  
             Aldea
";
        const string VILLAGE_SCENE = @"
                                                           |>>>
                   _                      _                |
    ____________ .' '.    _____/----/-\ .' './========\   / \
   //// ////// /V_.-._\  |.-.-.|===| _ |-----| u    u |  /___\
  // /// // ///==\ u |.  || | ||===||||| |T| |   ||   | .| u |_ _ _ _ _ _
 ///////-\////====\==|:::::::::::::::::::::::::::::::::::|u u| U U U U U
 |----/\u |--|++++|..|'''''''''''::::::::::::::''''''''''|+++|+-+-+-+-+-+
 |u u|u | |u ||||||..|              '::::::::'           |===|>=== _ _ ==
 |===|  |u|==|++++|==|              .::::::::.           | T |....| V |..
 |u u|u | |u ||HH||         \|/    .::::::::::.
 |===|_.|u|_.|+HH+|_              .::::::::::::.              _
                __(_)___         .::::::::::::::.         ___(_)__
---------------/  / \  /|       .:::::;;;:::;;:::.       |\  / \  \-------
______________/_______/ |      .::::::;;:::::;;:::.      | \_______\________
|       |     [===  =] /|     .:::::;;;::::::;;;:::.     |\ [==  = ]   |
|_______|_____[ = == ]/ |    .:::::;;;:::::::;;;::::.    | \[ ===  ]___|____
     |       |[  === ] /|   .:::::;;;::::::::;;;:::::.   |\ [=  ===] |
_____|_______|[== = =]/ |  .:::::;;;::::::::::;;;:::::.  | \[ ==  =]_|______
 |       |    [ == = ] /| .::::::;;:::::::::::;;;::::::. |\ [== == ]      |
_|_______|____[=  == ]/ |.::::::;;:::::::::::::;;;::::::.| \[  === ]______|_
   |       |  [ === =] /.::::::;;::::::::::::::;;;:::::::.\ [===  =]   |
___|_______|__[ == ==]/.::::::;;;:::::::::::::::;;;:::::::.\[=  == ]___|_____
";
        const string CASTLE = @"
 [][][] /""\ [][][]
  |::| /____\ |::|
  |[]|_|::::|_|[]|
  |::::::__::::::|
  |:::::/||\:::::|
  |:#:::||||::#::|
 #%*###&*##&*&#*&##
##%%*####*%%%###*%*#
      Castillo
";
        const string CASTLE_SCENE = @"
                           o                    
                       _---|         _ _ _ _ _ 
                    o   ---|     o   ]-I-I-I-[ 
   _ _ _ _ _ _  _---|      | _---|    \ ` ' / 
   ]-I-I-I-I-[   ---|      |  ---|    |.   | 
    \ `   '_/       |     / \    |    | /^\| 
     [*]  __|       ^    / ^ \   ^    | |*|| 
     |__   ,|      / \  /    `\ / \   | ===| 
  ___| ___ ,|__   /    /=_=_=_=\   \  |,  _|
  I_I__I_I__I_I  (====(_________)___|_|____|____
  \-\--|-|--/-/  |     I  [ ]__I I_I__|____I_I_| 
   |[]      '|   | []  |`__  . [  \-\--|-|--/-/  
   |.   | |' |___|_____I___|___I___|---------| 
  / \| []   .|_|-|_|-|-|_|-|_|-|_|-| []   [] | 
 <===>  |   .|-=-=-=-=-=-=-=-=-=-=-|   |    / \  
 ] []|`   [] ||.|.|.|.|.|.|.|.|.|.||-      <===> 
 ] []| ` |   |/////////\\\\\\\\\\.||__.  | |[] [ 
 <===>     ' ||||| |   |   | ||||.||  []   <===>
  \T/  | |-- ||||| | O | O | ||||.|| . |'   \T/ 
   |      . _||||| |   |   | ||||.|| |     | |
../|' v . | .|||||/____|____\|||| /|. . | . ./
.|//\............/...........\........../../\\\
";

        static string[] stages = {VILLAGE, CASTLE, FOREST, BEACH};
        const string HANGED_1 = @"
 +---+
 |   |
     |
     |
     |
     |
=========
";
        const string HANGED_2 = @"
 +---+
 |   |
 O   |
     |
     |
     |
=========
";
        const string HANGED_3 = @"
 +---+
 |   |
 O   |
 |   |
     |
     |
=========
";
        const string HANGED_4 = @"
 +---+
 |   |
 O   |
/|   |
     |
     |
=========
";
        const string HANGED_5 = @"
 +---+
 |   |
 O   |
/|\  |
     |
     |
=========
";
        const string HANGED_6 = @"
+ ---+
 |   |
 O   |
/|\  |
/    |
     |
=========
";
        const string HANGED_7 = @"
 +---+
 |   |
 O   |
/|\  |
/ \  |
     |
=========
";
        static string[] hangedFrames = { HANGED_1, HANGED_2, HANGED_3, HANGED_4, HANGED_5, HANGED_6, HANGED_7 };
        const string ARCADE_MACHINE_FRAME_1 = @"
  *%%%%%%%%%%%%%%*                                
  :%%%%%%%%%%%%%%:                                
   ##.=******=.##                                 
   ##.*%%%%%%*.##                                 
   #%:.::::::.:%#                                 
   ..............                                 
 =%% ..#%%%%#.  %%=                               
:::::.::::::::.::::.                              
                                                  
  *%%%%%%%%%%%%%%*                                
  *%%%%%%%%%%%%%%*                                
  *%%%%%%%%%%%%%%*
";
        const string ARCADE_MACHINE_FRAME_2 = @"
  ::::::::::::::::                                
                                                  
   %%%%%%%%%%%%%%                                 
   %*.*%%%%%%#.+%                                 
   %*.#%%%%%%#.+%                                 
   %#-........-#%                                 
   ...............                                
 *%# ..%%%%%%.. *%*                               
 ...   ......   ...                               
********************                              
 .#%%%%%%%%%%%%%%%.                               
 .#%%%%%%%%%%%%%%%.                               
 .#%%%%%%%%%%%%%%%.                               
 .::::::::::::::::. 
";
        const string ARCADE_MACHINE_FRAME_3 = @"
  -%%%%%%%%%%%%%%%%-                               
   -===============                                
   .%#.        .#%:                                
   .%=:#%%%%%%%:=%:                                
   .%=:#%%%%%%%:=%:                                
   .%%-        -%%:                                
   ::::::::::::::::                                
  %%= ..%%%%%%.. =%%                               
                                                   
=%%%%%%%%%%%%%%%%%%%%+                             
  :****************:                               
  -%%%%%%%%%%%%%%%%-                               
  -%%%%%%%%%%%%%%%%-                               
  +%%%%%%%%%%%%%%%%+    
";
        const string ARCADE_MACHINE_FRAME_4 = @"
   ::::::::::::::::                                
   ::::::::::::::::                                
   *%%%%%%%%%%%%%%*                                
   =%= -------- -%=                                
   -%::%%%%%%%%-.%=                                
   -%::%%%%%%%%-.%=                                
   -%%=::::::::-%%=                                
   ----------------                                
 :%%:...%%%%%%. ::%%:                              
 ::::  ::::::::  ::::                              
%%%%%%%%%%%%%%%%%%%%%%                             
  .----------------.                               
  =%%%%%%%%%%%%%%%%+                               
  =%%%%%%%%%%%%%%%%+                               
  =%%%%%%%%%%%%%%%%+                               
  ::::::::::::::::::      
";
        const string ARCADE_MACHINE_FRAME_5 = @"
  *%%%%%%%%%%%%%%%%*                               
   ::::::::::::::::.                               
   *%%%########%%%*                                
   *%..########:.%*                                
   *%.-%%%%%%%%- %*                                
   *%.:########-.%*                                
   *%%=--------=%%*                                
  .==-==========-==.                               
 =%% :.:%%%%%%:.- %%=                              
.====  ========  -===:                             
######################.                            
                                                   
  *%%%%%%%%%%%%%%%%*                               
  *%%%%%%%%%%%%%%%%*                               
  *%%%%%%%%%%%%%%%%*                               
 =#%%%%%%%%%%%%%%%%#=       
";
        const string ARCADE_MACHINE_FRAME_6 = @"
   ::::::::::::::::::                               
   ==================                               
   =****************=                               
    %%*          *%%                                
    %% -%%%%%%%%= %%                                
    %% -%%%%%%%%= %%                                
    %%.:********:.%%                                
    %%%*++++++++*%%%                                
   -================-                               
  *%% -.:%%%%%%-.- %%#                              
 =***+ .********. +***+                             
:++++++++++++++++++++++-                            
  ....................                              
  .#%%%%%%%%%%%%%%%%#:                              
  :#%%%%%%%%%%%%%%%%%:                              
  :#%%%%%%%%%%%%%%%%%:                              
  :#%%%%%%%%%%%%%%%%%:                              
  ::::::::::::::::::::        
";
        const string ARCADE_MACHINE_FRAME_7 = @"
  -%%%%%%%%%%%%%%%%%%-                              
   ..................                               
   =%%%%%%%%%%%%%%%%=                               
   .%#: ........ :#%.                               
   .%# =%%%%%%%%=.*%.                               
   .%# =%%%%%%%%=.*%.                               
   .%#..========..*%.                               
   .%%%##########%%%.                               
   =++=++++++++++=++=                               
  %%* -.-%%%%%%-.- *%%                              
 ####+ :########: =####.                            
========================                            
++++++++++++++++++++++++                            
  :******************:                              
  -%%%%%%%%%%%%%%%%%%-                              
  -%%%%%%%%%%%%%%%%%%-                              
  -%%%%%%%%%%%%%%%%%%-                              
 :#%%%%%%%%%%%%%%%%%%#:          
";
        const string ARCADE_MACHINE_FRAME_8 = @"
   ::::::::::::::::::                               
  -##################=                              
   -================-                               
   -%%%##########%%%=                               
   -%+.-********-.+%-                               
   -%+.+%%%%%%%%+.+%-                               
   -%+.+%%%%%%%%+.=%-                               
   -%*..::::::::..+%-                               
   -%%%%%%%%%%%%%%%%-                               
   ++++++++++++++++++                               
 .%%= :.=%%%%%%=.: =%%:                             
-%%%%= -%%%%%%%%= -#%%%-                            
::::::::::::::::::::::::                            
%%%%%%%%%%%%%%%%%%%%%%%%.                           
  :==================:                              
  =%%%%%%%%%%%%%%%%%%+                              
  =%%%%%%%%%%%%%%%%%%+                              
  =%%%%%%%%%%%%%%%%%%+                              
  =%%%%%%%%%%%%%%%%%%+                              
 .::::::::::::::::::::.            
";
        const string ARCADE_MACHINE_FRAME_9 = @"
   %%%%%%%%%%%%%%%%%%%%                              
      CSHARP KINGDOM                                               
   :%%%%%%%%%%%%%%%%%%-                              
    *%%=          =%%#                               
    *%+ %%%%%%%%%% =%#                               
    *%+ %%%%%%%%%% =%#                               
    *%+ %%%%%%%%%% =%#                               
    *%%            %%#                               
    *%%%%%%%%%%%%%%%%#                               
   -***+**********+***-                              
  +%%: + *%%%%%%# + .%%*                             
 *%%%%  =%%%%%%%%=  %%%%#                            
                                                     
*%%%%%%%%%%%%%%%%%%%%%%%%#                           
                                                     
   %%%%%%%%%%%%%%%%%%%%                              
   %%%%%%%%%%%%%%%%%%%%                              
   %%%%%%%%%%%%%%%%%%%%                              
   %%%%%%%%%%%%%%%%%%%%                              
  %%%%%%%%%%%%%%%%%%%%%%              
";
        const string ARCADE_MACHINE_FRAME_10 = @"
   --------------------                              
   %%%%%%%%%%%%%%%%%%%%.                             
      CSHARP KINGDOM                                            
    %%%%%%%%%%%%%%%%%%                               
    %%#            #%%                               
    %%- %%%%%%%%%% -%%                               
    %%- %%%%%%%%%% -%%                               
    %%- %%%%%%%%%% -%%                               
    %%%            %%%                               
    ##################                               
   ****+**********+****                              
  #%%  = #%%%%%%# =  %%#                             
 %%%%#  =%%%%%%%%=  #%%%%                            
                                                     
%%%%%%%%%%%%%%%%%%%%%%%%%%                           
                                                     
   %%%%%%%%%%%%%%%%%%%%.                             
   %%%%%%%%%%%%%%%%%%%%.                             
   %%%%%%%%%%%%%%%%%%%%.                             
   %%%%%%%%%%%%%%%%%%%%.                             
   %%%%%%%%%%%%%%%%%%%%.                             
  ----------------------                
";
        const string ARCADE_MACHINE_FRAME_11 = @"
  +%%%%%%%%%%%%%%%%%%%%#                             
  .:::CSHARP KINGDOM::::                             
   %%%%%%%%%%%%%%%%%%%%                              
   .%%%%%%%%%%%%%%%%%%:                              
   .%%- ========== :%%.                              
   .%%. %%% ;  %%% .%%.                              
   .%%. %%%%%%%%%% .%%.                              
   .%%. %%%%%%%%%% .%%.                              
   .%%#            #%%.                              
    ==================                               
   *##**##########**###                              
  %%% .: %%%%%%%% :: %%%                             
 %%%%=  -%%%%%%%%=  -%%%%                            
                                                     
%%%%%%%%%%%%%%%%%%%%%%%%%%                           
                                                     
  +%%%%%%%%%%%%%%%%%%%%#                             
  +%%%%%%%%%%%%%%%%%%%%#                             
  +%%%%%%%%%%%%%%%%%%%%#                             
  +%%%%%%%%%%%%%%%%%%%%#                             
  +%%%%%%%%%%%%%%%%%%%%#                             
 #%%%%%%%%%%%%%%%%%%%%%%%               
";
        static string[] arcadeFrames = {ARCADE_MACHINE_FRAME_1, ARCADE_MACHINE_FRAME_2, ARCADE_MACHINE_FRAME_3, ARCADE_MACHINE_FRAME_4,
            ARCADE_MACHINE_FRAME_5, ARCADE_MACHINE_FRAME_6, ARCADE_MACHINE_FRAME_7, ARCADE_MACHINE_FRAME_8, ARCADE_MACHINE_FRAME_9, ARCADE_MACHINE_FRAME_10, ARCADE_MACHINE_FRAME_11};
        const string CSKINGDOM = @"
 ____ ____ _  _ ____ ____ ___      
 |___ ==== |--| |--| |--< |--'
_  _ _ __ _ ____ ___  ____ _  _
|-:_ | | \| |__, |__> [__] |\/|
";
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

        const string FRIENDLY_WIZARD = @"
                    ____ 
                  .'* *.'
               __/_*_*(_
              / _______ \
             _\_)/___\(_/_ 
            / _((\- -/))_ \
            \ \())(-)(()/ /
             ' \(((()))/ '
            / ' \)).))/ ' \
           / _ \ - | - /_  \
          (   ( .;''';. .'  )
          _\""__ /    )\ __""/_
            \/  \   ' /  \/
             .'  '...' ' )
              / /  |  \ \
             / .   .   . \
            /   .     .   \
           /   /   |   \   \
         .'   /    b    '.  '.
     _.-'    /     Bb     '-. '-._ 
 _.-'       |      BBb       '-.  '-. 
(________mrf\____.dBBBb.________)____)
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
   O  `'  '=[:]| C  |
          /""""|  S |
         /"""" `.__.'
        []""/"" \[]
        | \     / |
        | |     | |
      <\\\)     (///>
";
        // Arkanoid

        // Arkanoid vars
        static SoundPlayer barReboundSound = new SoundPlayer(@"Sounds\barRebound.wav");
        static SoundPlayer brickReboundSound = new SoundPlayer(@"Sounds\brickRebound.wav");
        static SoundPlayer gameOverSound = new SoundPlayer(@"Sounds\gameOver.wav");

        static int rows = 5;
        static int cols = (Console.WindowWidth / 4) + 1;
        static int brickWidth = 4;
        static int brickHeight = 1;
        static int barX;
        static int barY;
        static int ballX;
        static int ballY;
        static int dx = 1;
        static int dy = 1;
        static bool running = true;
        static bool gameOver = false;
        public static void DrawBrick(int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.Write("▓▓▓▓");
        }

        public static void EraseBrick(int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.Write("    ");
        }

        public static bool[,] DrawBricks()
        {
            bool[,] bricks = new bool[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    int x = j * brickWidth;
                    int y = i * brickHeight;
                    bricks[i, j] = true;
                    if (i % 2 == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                    }

                    DrawBrick(x, y);
                    Console.ResetColor();
                }
            }
            return bricks;
        }

        public static void BrickCollision(int rows, int cols, bool[,] bricks)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (bricks[i, j])
                    {
                        int brickX = j * brickWidth;
                        int brickY = i * brickHeight;


                        if (ballX >= brickX && ballX < brickX + brickWidth &&
                            ballY == brickY)
                        {
                            dy *= -1;
                            brickReboundSound.Play();
                            EraseBrick(brickX, brickY);
                            bricks[i, j] = false;
                            break;
                        }
                    }
                }
            }
        }

        public static void MoveBar(ConsoleKeyInfo? key)
        {
            if (key.HasValue)
            {
                switch (key.Value.Key)
                {
                    case ConsoleKey.LeftArrow:
                        if (barX > 1)
                        {
                            EraseBar(barX + 8, barY);
                            barX -= 2; // Velocidad barra
                            DrawBar(barX, barY);
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (barX < Console.WindowWidth - 16)
                        {
                            EraseBar(barX, barY);
                            barX += 2; // Velocidad barra
                            DrawBar(barX, barY);
                        }
                        break;
                }
            }
        }

        public static void DrawBar(int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓");
            Console.ResetColor();
        }

        public static void EraseBar(int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.Write("        ");
        }

        public static void MoveBall(ref bool gameOver)
        {
            EraseBall(ballX, ballY);
            ballX += dx;
            ballY += dy;

            // Colisiones consola
            // Horizontal
            if (ballX >= Console.WindowWidth - 1 || ballX <= 0)
            {
                dx *= -1;

            }
            // Vertical
            if (ballY <= 0)
            {
                dy *= -1;

            }

            if (ballY >= Console.WindowHeight - 1)
            {
                gameOver = true;
                gameOverSound.Play();
                Thread.Sleep(200);
            }

            // Colisiones barra
            if (ballY + dy == barY && ballX + dx >= barX && ballX + dx <= barX + 16)
            {
                dy *= -1;
                barReboundSound.Play();
            }

            DrawBall(ballX, ballY);
        }

        public static void GetReady()
        {
            DrawText((Console.WindowWidth / 2) - 7, Console.WindowHeight / 2, 100, "GET READY");
            Thread.Sleep(1000);
            DrawText((Console.WindowWidth / 2) - 7, Console.WindowHeight / 2, 0, "         ");
            DrawText((Console.WindowWidth / 2) - 7, Console.WindowHeight / 2, 100, "   GO!");
            Thread.Sleep(500);
            DrawText((Console.WindowWidth / 2) - 7, Console.WindowHeight / 2, 0, "      ");
        }

        public static void DrawBall(int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.Write("O");
        }

        public static void EraseBall(int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(" ");
        }

        public static void Arkanoid()
        {

            barX = (Console.WindowWidth / 2) - 10;
            barY = Console.WindowHeight - 5;
            ballX = (Console.WindowWidth / 2) - 35;
            ballY = (Console.WindowHeight / 2) - 15;

            Console.CursorVisible = false;

            bool[,] bricks = DrawBricks();

            DrawBar(barX, barY);

            GetReady();

            while (running && !gameOver)
            {
                ConsoleKeyInfo? key = Console.KeyAvailable ? Console.ReadKey(true) : (ConsoleKeyInfo?)null;
                MoveBar(key);
                MoveBall(ref gameOver);
                BrickCollision(rows, cols, bricks);
                Thread.Sleep(25);
            }

            DrawText((Console.WindowWidth / 2) - 7, Console.WindowHeight / 2, 100, "GAME OVER");
            Thread.Sleep(3000);
            Console.Clear();
        }

        // CSKingdom

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
                    int optionX = Math.Max(0, ((Console.WindowWidth / 2) - 2) - (options[i].Length / 2));
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
                    menuSelectSound.Play();
                    selectedOption = (selectedOption > 0) ? selectedOption - 1 : options.Length - 1;
                }
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    menuSelectSound.Play();
                    selectedOption = (selectedOption < options.Length - 1) ? selectedOption + 1 : 0;
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    menuAcceptSound.Play();
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

                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.UpArrow)
                {
                    menuSelectSound.Play();
                    selectedOption = (selectedOption > 0) ? selectedOption - 1 : options.Length - 1;
                }
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    menuSelectSound.Play();
                    selectedOption = (selectedOption < options.Length - 1) ? selectedOption + 1 : 0;
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    menuAcceptSound.Play();
                    return selectedOption;
                }
            }
        }

        public static int SelectStage(string[] options)
        {
            Console.CursorVisible = false;
            int selectedOption = 0;

            int horizontalSpacing = 25;
            int verticalSpacing = 10;

            int centerX = (Console.WindowWidth / 2) - 15;
            int centerY = (Console.WindowHeight / 2) - 11;

            
            int[] positionsX = new int[]
            {
               centerX - horizontalSpacing,
               centerX + horizontalSpacing,
               centerX - horizontalSpacing,
               centerX + horizontalSpacing
            };

            int[] positionsY = new int[]
            {
           Math.Max(0, centerY - verticalSpacing),
           Math.Max(0, centerY - verticalSpacing),
           Math.Min(Console.WindowHeight - 1, centerY + verticalSpacing),
           Math.Min(Console.WindowHeight - 1, centerY + verticalSpacing)
            };

            while (true)
            {
                
                for (int i = 0; i < options.Length; i++)
                {
                    if (i == selectedOption)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        DrawTextLines(positionsX[i], positionsY[i], 0, options[i]);
                        Console.ResetColor();
                    }
                    else
                    {
                        DrawTextLines(positionsX[i], positionsY[i], 0, options[i]);
                    }
                }

                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        menuSelectSound.Play();
                        selectedOption = (selectedOption - 2 >= 0) ? selectedOption - 2 : selectedOption + 2;
                        break;
                    case ConsoleKey.DownArrow:
                        menuSelectSound.Play();
                        selectedOption = (selectedOption + 2 < options.Length) ? selectedOption + 2 : selectedOption - 2;
                        break;
                    case ConsoleKey.LeftArrow:
                        menuSelectSound.Play();
                        selectedOption = (selectedOption % 2 == 1) ? selectedOption - 1 : selectedOption + 1;
                        break;
                    case ConsoleKey.RightArrow:
                        menuSelectSound.Play();
                        selectedOption = (selectedOption % 2 == 0) ? selectedOption + 1 : selectedOption - 1;
                        break;
                    case ConsoleKey.Enter:
                        menuAcceptSound.Play();
                        return selectedOption;
                }
            }
        }

        public static void DrawUI()
        {
            DrawRectangle(5, 0, Console.WindowWidth - 11, Console.WindowHeight - 10, '▓'); //Main Frame
            DrawRectangle(5, 34, 37, 8, '▓');//First frame - option selection frame
            DrawRectangle(42, 34, 73, 8, '▓');//Second frame - info frame
            DrawRectangle(115, 34, 49, 8, '▓');//Third frame - Game Logo
            DrawTextLines(125, 35, 0, CSKINGDOM);
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

                    if (timeBetweenKeyStrokes > 0)
                    {
                        textClickSound.Play();
                        Thread.Sleep(timeBetweenKeyStrokes);
                    }
                    
                }
            }
            else
            {
                Console.WriteLine("Out of limits");
            }
        }

        public static void DrawTextCentered(int x, int y, int timeBetweenKeyStrokes, string text)
        {
            Console.CursorVisible = false;
            if (y >= 0 && y < Console.WindowHeight)
            {
                x = (Console.WindowWidth - text.Length) / 2;
                if (x >= 0 && x + text.Length <= Console.WindowWidth)
                {
                    Console.SetCursorPosition(x, y);
                    for (int i = 0; i < text.Length; i++)
                    {
                        Console.Write(text[i]);

                        if (timeBetweenKeyStrokes > 0)
                        {
                            Thread.Sleep(timeBetweenKeyStrokes);
                            textClickSound.Play();
                        }
                        
                    }
                }
            }
        }

        public static void DrawTextLines(int x, int y, int timeBetweenKeyStrokes, string text)
        {
            string[] textLines = text.Split('\n');
            for (int i = 0; i < textLines.Length; i++)
            {
                string line = textLines[i];
                Console.SetCursorPosition(x, y++);
                Console.Write(line);
            }
        }

        public static void EraseText(int x, int y, int spaceBetweenKeyStrokes, int spaceX, int spaceY)
        {
            string spaces = new string(' ', spaceX);
            for (int i = 0; i < spaceY; i++)
            {
                DrawText(x, y, spaceBetweenKeyStrokes, spaces);
            }
        }

        public static int DrawMainMenu(string[] menuOptions, string title)
        {
            Console.CursorVisible = false;
            string[] titleLines = title.Split('\n');
            int titleY = Console.WindowHeight / 2 - menuOptions.Length - 15;
            title1Sound.Play();
            for (int i = 0; i < titleLines.Length; i++)
            {
                string line = titleLines[i];
                int titleX = ((Console.WindowWidth - line.Length) / 2) - 5;
                Console.SetCursorPosition(titleX, titleY++);
                Console.Write(line);


                if (i == titleLines.Length - 4)
                {
                    title1Sound.Stop();
                    Thread.Sleep(2000);
                    title2Sound.Play();
                }
                else
                {
                    Thread.Sleep(80);
                }
            }

            Thread.Sleep(500);

            int selectedOption = SelectMainMenu(menuOptions);
            Console.Clear();
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

        public static void DrawArt(int x, int y, string enemy)
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
        
        
        // Intro
        public static void DrawArcade()
        {
            for (int i = 0; i < arcadeFrames.Length; i++)
            {
                DrawArt((Console.WindowWidth / 2) + 10, (Console.WindowHeight / 2) - 20, arcadeFrames[i]);
                Thread.Sleep(100);
                if (i < arcadeFrames.Length - 1)
                {
                    Console.Clear();
                }
            }
        }

        public static void EnterTheKingdomAnimation()
        {
            beepBoopSound.Play();
            Epilepsy(200, 3000);
            Console.Clear();
            Matrix(3000);
            Console.Clear();
            Epilepsy(100, 2000);
            Console.Clear();
            Matrix(2000);
            Console.Clear();
            Epilepsy(50, 1000);
            Console.Clear();
            Matrix(1000);
            Console.Clear();
            Epilepsy(25, 500);
            Console.Clear();
            Matrix(500);
            Console.Clear();
            Epilepsy(1, 250);
            Console.Clear();
            Matrix(250);
            Console.Clear();
            Epilepsy(1, 1000);
            Console.Clear();
            Matrix(5000);
            Console.ResetColor();
            Console.Clear();
            DrawTransition1();
            Console.Clear();
        }

        public static void IntroScene(string playerName)
        {
            Arkanoid();
            Console.Clear();
            DrawTextCentered((Console.WindowWidth / 2) - 20, (Console.WindowHeight / 2), 50, "Buff, me aburre jugar siempre a los mismos juegos.");
            Thread.Sleep(2000);
            Console.Clear();
            DrawTextCentered((Console.WindowWidth / 2) - 20, (Console.WindowHeight / 2), 50, "El dueño de los recreativos podría invertir un poco en máquinas nuevas.");
            title1LowVolumeSound.Play();
            Thread.Sleep(2000);
            Console.Clear();
            DrawTextCentered((Console.WindowWidth / 2) - 20, (Console.WindowHeight / 2), 50, "Un momento... ¿qué es eso que suena?");
            title1Sound.Play();
            Thread.Sleep(3000);
            Console.Clear();
            DrawTextCentered((Console.WindowWidth / 2) - 20, (Console.WindowHeight / 2), 50, "Viene de esa máquina... ¡No la había visto antes!");
            Thread.Sleep(3000);
            Console.Clear();
            title1Sound.Play();
            DrawArcade();
            Thread.Sleep(5000);
            DrawTextCentered((Console.WindowWidth / 2) - 30, (Console.WindowHeight / 2) + 10, 50, "CSHARP KINGDOM... tiene buena pinta...");
            Thread.Sleep(2000);
            EraseText((Console.WindowWidth / 2) - 30, (Console.WindowHeight / 2) + 10, 0, 50, 1);
            DrawTextCentered((Console.WindowWidth / 2) - 30, (Console.WindowHeight / 2) + 10, 50, "Me queda solo un duro... más vale que merezca la pena.");
            Thread.Sleep(2000);
            insertCoinSound.Play();
            Thread.Sleep(3000);
            thunderSound.Play();
            Console.Clear();
            Thread.Sleep(2000);
            powerOutSound.Play();
            Thread.Sleep(4000);
            DrawTextCentered((Console.WindowWidth / 2) - 30, (Console.WindowHeight / 2) + 10, 50, "¿Q- q- qué ha pasado? Se ha ido la luz...");
            Thread.Sleep(2000);
            switchSound.Play();
            DrawArt((Console.WindowWidth / 2) + 10, (Console.WindowHeight / 2) - 20, arcadeFrames[arcadeFrames.Length - 1]);
            Thread.Sleep(2000);
            EraseText((Console.WindowWidth / 2) - 30, (Console.WindowHeight / 2) + 10, 0, 50, 1);
            DrawTextCentered((Console.WindowWidth / 2) - 30, (Console.WindowHeight / 2) + 10, 50, "Oh vaya... volvió.");
            Thread.Sleep(2000);
            EraseText((Console.WindowWidth / 2) - 30, (Console.WindowHeight / 2) + 10, 0, 50, 1);
            DrawTextCentered((Console.WindowWidth / 2) - 30, (Console.WindowHeight / 2) + 10, 50, "Bueno... CSHARP KINGDOM, prepárate.");
            Thread.Sleep(2000);
            thunderSound.Play();
            Thread.Sleep(2000);
            EraseText((Console.WindowWidth / 2) - 30, (Console.WindowHeight / 2) + 10, 0, 50, 1);
            DrawTextCentered((Console.WindowWidth / 2) - 30, (Console.WindowHeight / 2) + 10, 50, "¿Otra vez? Oh... oh... ¡OHH!");
            Thread.Sleep(1000);
            Console.Clear();
            EnterTheKingdomAnimation();
            Thread.Sleep(2000);
        }

        public static void ShowInstructions()
        {
            string instructions = @"
    En CSHARP KINGDOM deberás luchar contra monstruos
y resolver puzzles y acertijos para obtener La Llave del
                        Éxito.
   ¿Qué misterios y peligros albergará El Reino de C#?
              Es tu misión descubrirlo.
";
            DrawTextLines(Console.WindowWidth / 2, Console.WindowHeight / 2, 10, instructions);
            Console.ReadLine();
            Console.Clear();
        }

        public static void WizardScene(string playerName)
        {
            DrawArt((Console.WindowWidth / 2), (Console.WindowHeight / 2) - 20, FRIENDLY_WIZARD);
            DrawTextCentered((Console.WindowWidth / 2), (Console.WindowHeight / 2) + 10, 50, $"Hola, {playerName}.");
            Thread.Sleep(2000);
            EraseText((Console.WindowWidth / 2) - 80, (Console.WindowHeight / 2) + 10, 0, 150, 1);
            DrawArt((Console.WindowWidth / 2), (Console.WindowHeight / 2) - 20, FRIENDLY_WIZARD);
            DrawTextCentered((Console.WindowWidth / 2), (Console.WindowHeight / 2) + 10, 50, $"¿Que cómo sé tu nombre? Ja ja ja... ¡Soy un mago! ¿No lo ves?");
            Thread.Sleep(2000);
            EraseText((Console.WindowWidth / 2) - 80, (Console.WindowHeight / 2) + 10, 0, 150, 1);
            DrawTextCentered((Console.WindowWidth / 2), (Console.WindowHeight / 2) + 10, 50, $"No te asustes. Entiendo que todo esto sea muy nuevo para ti.");
            Thread.Sleep(2000);
            EraseText((Console.WindowWidth / 2) - 80, (Console.WindowHeight / 2) + 10, 0, 150, 1);
            DrawTextCentered((Console.WindowWidth / 2), (Console.WindowHeight / 2) + 10, 50, $"Estás dentro de CSHARP KINGDOM.");
            Thread.Sleep(2000);
            EraseText((Console.WindowWidth / 2) - 80, (Console.WindowHeight / 2) + 10, 0, 150, 1);
            DrawTextCentered((Console.WindowWidth / 2), (Console.WindowHeight / 2) + 10, 50, $"Te esperan peligros y desafíos a los que no te has enfrentado nunca.");
            Thread.Sleep(2000);
            EraseText((Console.WindowWidth / 2) - 80, (Console.WindowHeight / 2) + 10, 0, 150, 1);
            DrawTextCentered((Console.WindowWidth / 2), (Console.WindowHeight / 2) + 10, 50, $"Nuestro reino está en apuros. Solo tú puedes ayudarnos a derrotar al tirano Rey Demonio.");
            Thread.Sleep(2000);
            EraseText((Console.WindowWidth / 2) - 80, (Console.WindowHeight / 2) + 10, 0, 150, 1);
            DrawTextCentered((Console.WindowWidth / 2), (Console.WindowHeight / 2) + 10, 50, $"Deberás conseguir las tres reliquias para poder enfrentarte al Rey Demonio y así conseguir La Llave del Éxito.");
            Thread.Sleep(2000);
            EraseText((Console.WindowWidth / 2) - 80, (Console.WindowHeight / 2) + 10, 0, 150, 1);
            DrawTextCentered((Console.WindowWidth / 2), (Console.WindowHeight / 2) + 10, 50, $"¿Que por qué tú? Porque solo tú has sido capaz de escuchar nuestra llamada.");
            Thread.Sleep(2000);
            EraseText((Console.WindowWidth / 2) - 80, (Console.WindowHeight / 2) + 10, 0, 150, 1);
            DrawTextCentered((Console.WindowWidth / 2), (Console.WindowHeight / 2) + 10, 50, $"¡Eso significa que eres la persona indicada!");
            Thread.Sleep(2000);
            EraseText((Console.WindowWidth / 2) - 80, (Console.WindowHeight / 2) + 10, 0, 150, 1);
            DrawTextCentered((Console.WindowWidth / 2), (Console.WindowHeight / 2) + 10, 50, $"¡Ahora adelante! ¡Ve y consigue La Llave del Éxito!");
            Thread.Sleep(2000);
            Console.Clear();
            DrawTransition1();
        }

        // Explore
        



        // Battle
        public static void PlayerTurn(int playerChoice, int[] playerInventory, Random generator, ref int computerHealth, ref int playerHealth, ref int playerMana, string[] enemy, string playerName)
        {
            string enemyName = enemy[1];
            bool success = false;
            bool attack = false;
            int chosenAttackDamage = 0;
            string playerAttack = "";
            switch (playerChoice)
            {
                case 0:
                    attack = true;
                    success = generator.Next(1, 101) <= LIGHT_ATTACK_PROBABILITY;
                    playerAttack = "Golpe ligero";
                    if (success)
                    {
                        computerHealth -= LIGHT_ATTACK_DAMAGE;
                        chosenAttackDamage = LIGHT_ATTACK_DAMAGE;
                    }
                    break;
                case 1:
                    attack = true;
                    success = generator.Next(1, 101) <= MEDIUM_ATTACK_PROBABILITY;
                    playerAttack = "Golpe medio";
                    if (success)
                    {
                        computerHealth -= MEDIUM_ATTACK_DAMAGE;
                        chosenAttackDamage = MEDIUM_ATTACK_DAMAGE;
                    }
                    break;
                case 2:
                    attack = true;
                    success = generator.Next(1, 101) <= HEAVY_ATTACK_PROBABILITY;
                    playerAttack = "Golpe pesado";
                    if (success)
                    {
                        computerHealth -= HEAVY_ATTACK_DAMAGE;
                        chosenAttackDamage = HEAVY_ATTACK_DAMAGE;
                    }
                    break;
                case 3:
                    if (playerInventory[0] > 0)
                    {
                        DrawText(50, 38, 0, $"¡{playerName} ha usado una poción de vitalidad y ha recuperado {POTION_HEALING_AMMOUNT}!");
                        playerHealth += POTION_HEALING_AMMOUNT;
                        potionSound.Play();
                        CorrectHealth(ref playerHealth, ref computerHealth);
                        DisplayHealth(playerHealth, computerHealth, playerName, enemyName);
                    }
                    else
                    {
                        DrawText(50, 38, 0, $"¡A {playerName} no le quedan pociones de vitalidad!");
                    }
                    Thread.Sleep(2000);
                    break;
                case 4:
                    EraseText(50, 38, 0, 65, 1);
                    EraseText(50, 40, 0, 65, 1);
                    if (playerInventory[1] > 0)
                    {
                        DrawText(50, 38, 0, $"¡{playerName} ha usado una poción de maná y ha recuperado {MANA_RECOVER_AMMOUNT}!");
                        playerMana += MANA_RECOVER_AMMOUNT;

                    }
                    else
                    {
                        DrawText(50, 38, 0, $"¡A {playerName} no le quedan pociones de maná!");
                    }
                    Thread.Sleep(2000);
                    break;
            }
            EraseText(50, 38, 0, 65, 1);
            EraseText(50, 40, 0, 65, 1);
            if (attack)
            {
                DrawText(50, 38, 0, $"¡{playerName} ataca con un {playerAttack}!");
            }

            if (success)
            {
                meleeHitSound2.Play();
                Thread.Sleep(300);
                EraseText(50, 40, 0, 65, 1);
                EraseText((Console.WindowWidth / 2) + 10, (Console.WindowHeight / 2) - 10, 0, 65, 1);
                DrawText(50, 40, 0, $"¡Diana! Le has hecho {chosenAttackDamage} de daño a {enemyName}.");
                DrawText((Console.WindowWidth / 2) + 10, (Console.WindowHeight / 2) - 10, 10, enemy[3]);
            }
            else if (attack)
            {
                missHitSound.Play();
                Thread.Sleep(300);
                EraseText(50, 40, 0, 65, 1);
                EraseText((Console.WindowWidth / 2) + 10, (Console.WindowHeight / 2) - 10, 0, 65, 1);
                DrawText(50, 40, 0, "¡Has fallado!");
                DrawText((Console.WindowWidth / 2) + 10, (Console.WindowHeight / 2) - 10, 10, enemy[4]);
            }
        }

        public static void ComputerTurn(Random generator, ref int playerHealth, string enemyName)
        {
            int chosenAttackDamage = 0;
            bool success = false;
            int computerChoice = generator.Next(1, 4);
            string computerAttack = "";
            switch (computerChoice)
            {
                case 1:
                    computerAttack = "Golpe ligero";
                    success = generator.Next(1, 101) <= LIGHT_ATTACK_PROBABILITY;
                    if (success)
                    {
                        playerHealth -= LIGHT_ATTACK_DAMAGE;
                        chosenAttackDamage = LIGHT_ATTACK_DAMAGE;
                    }
                    break;
                case 2:
                    computerAttack = "Golpe medio";
                    success = generator.Next(1, 101) <= MEDIUM_ATTACK_PROBABILITY;
                    if (success)
                    {
                        playerHealth -= MEDIUM_ATTACK_DAMAGE;
                        chosenAttackDamage = MEDIUM_ATTACK_DAMAGE;
                    }
                    break;
                case 3:
                    computerAttack = "Golpe fuerte";
                    success = generator.Next(1, 101) <= HEAVY_ATTACK_PROBABILITY;
                    if (success)
                    {
                        playerHealth -= HEAVY_ATTACK_DAMAGE;
                        chosenAttackDamage = HEAVY_ATTACK_DAMAGE;
                    }
                    break;
            }
            EraseText(50, 38, 0, 65, 1);
            DrawText(50, 38, 0, $"{enemyName} ataca con un {computerAttack}!");
            if (success)
            {
                meleeHitSound.Play();
                EraseText(50, 40, 0, 65, 1);
                DrawText(50, 40, 0, $"¡Diana! {enemyName} te hace {chosenAttackDamage} de daño.");
            }
            else
            {
                missHitSound.Play();
                EraseText(50, 40, 0, 65, 1);
                DrawText(50, 40, 0, $"¡{enemyName} ha fallado!");
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
            EraseText(50, 38, 0, 65, 1);
            EraseText(50, 40, 0, 65, 1);
            DrawText(50, 39, 10, win ? victoryMessage : defeatMessage);
        }

        public static void CorrectHealth(ref int playerHealth, ref int computerHealth)
        {
            playerHealth = (playerHealth < 0) ? 0 : (playerHealth > 100) ? 100 : playerHealth;
            computerHealth = (computerHealth < 0) ? 0 : (computerHealth > 100) ? 100 : computerHealth;
        }

        public static void DisplayHealth(int playerHealth, int computerHealth, string playerName, string enemyName)
        {
            EraseText(45, 35, 0, 70, 1);
            if (playerHealth >= 70)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                DrawText(46, 35, 0, $"{playerName}: {playerHealth}");
            }
            else if (playerHealth < 70 && playerHealth >= 40)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                DrawText(46, 35, 0, $"{playerName}: {playerHealth}");
            }
            else if (playerHealth <= 30)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                DrawText(46, 35, 0, $"{playerName}: {playerHealth}");
            }

            if (computerHealth >= 70)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                DrawText(92, 35, 0, $"{enemyName}: {computerHealth}");
            }
            else if (computerHealth < 70 && computerHealth >= 40)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                DrawText(92, 35, 0, $"{enemyName}: {computerHealth}");
            }
            else if (computerHealth <= 30)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                DrawText(92, 35, 0, $"{enemyName}: {computerHealth}");
            }


            Console.ResetColor();
        }

        public static bool Battle(Random generator, ref int playerHealth, ref int playerMana, ref int computerHealth, int[] playerInventory, string[] options, string playerName, string[] enemy)
        {
            string enemyDrawing = enemy[0];
            string enemyName = enemy[1];
            DisplayHealth(playerHealth, computerHealth, playerName, enemyName);
            DrawArt(Console.WindowWidth / 2, (Console.WindowHeight / 2) - 15, enemyDrawing);
            DrawText(15, 5, 0, enemyName);
            DrawText((Console.WindowWidth / 2) + 10, (Console.WindowHeight / 2) - 10, 10, enemy[2]);

            bool win = false;
            bool defeat = false;

            int playerChoice;
            while (!win && !defeat)
            {
                playerChoice = Select(6, 36, options);

                PlayerTurn(playerChoice, playerInventory, generator, ref computerHealth, ref playerHealth, ref playerMana, enemy, playerName);

                if (computerHealth > 0)
                {
                    CorrectHealth(ref playerHealth, ref computerHealth);
                    DisplayHealth(playerHealth, computerHealth, playerName, enemyName);
                }

                win = PlayerWon(computerHealth);
                Thread.Sleep(2000);

                if (!win)
                {

                    if (playerHealth > 0)
                    {
                        ComputerTurn(generator, ref playerHealth, enemyName);
                        CorrectHealth(ref playerHealth, ref computerHealth);
                        DisplayHealth(playerHealth, computerHealth, playerName, enemyName);
                    }

                    defeat = ComputerWon(playerHealth);

                }

                if (win || defeat)
                {
                    EraseText(50, 40, 0, 65, 1);
                    if (win)
                    {
                        
                        DrawText((Console.WindowWidth / 2) + 10, (Console.WindowHeight / 2) - 10, 10, enemy[5]);//Display enemy defeat message
                    }
                    else
                    {
                        
                        DrawText((Console.WindowWidth / 2) + 10, (Console.WindowHeight / 2) - 10, 10, enemy[6]);//Display enemy victory message
                    }
                    BattleEndMessage($"Enhorabuena! ¡Has derrotado a {enemyName}!", $"¡Fin del juego! ¡{enemyName} te ha derrotado!", win);
                    CorrectHealth(ref playerHealth, ref computerHealth);
                    DisplayHealth(playerHealth, computerHealth, playerName, enemyName);
                }
            }
            return win;
        }



        // Menu options
        static string[] playerBattleOptions = { $"Ligero ({LIGHT_ATTACK_PROBABILITY}% éxito, {LIGHT_ATTACK_DAMAGE} daño)", $"Medio ({MEDIUM_ATTACK_PROBABILITY}% éxito, {MEDIUM_ATTACK_DAMAGE} daño)", $"Pesado ({HEAVY_ATTACK_PROBABILITY}% éxito, {HEAVY_ATTACK_DAMAGE} daño)", $"Poción (recupera {POTION_HEALING_AMMOUNT} de salud)" };
        static string[] mainMenuOptions = { "Nueva partida", "Instrucciones", "Salir" };
        // Enemies arrays
        static string[] skeletonEnemy = { SKELETON, "Esqueleto", "¡Hola! ¿Nos damos de hostias, o qué?", "¡Ayy! ¡Eso duele!", "¿Cómo puedes fallar eso?", "La oscuridad se cierne sobre mí... ¡adios!", "¡Debilucho! ¡Menudo saco de huesos! ¡JÁ!" };
        static string[] goblinEnemy = { GOBLIN, "Goblin", "¡Pequeño pero matón!", "Ouch!", "Pringao!", "No era necesario...", "Ya te lo dije... ¡matón, matón!" };
        static string[] knightEnemy = { KNIGHT, "Caballero", "¡Un mequetrefe! ¡Prendedle!", "No me duele >:D", "¡Buena! Espera... le querías dar al aire, ¿no?", "Era broma... sí que dolía, ayy..", "¡Ale! ¡Prendido!" };
        static string[] dogEnemy = { DOG, "Perro", "¡Guau, guau! *arf arf*", "¡AING AING AING!", "*mueve el rabo*", "*se va al cielo de los perros*", "*te mea*" };
        static void Main()
        {
            
            // Recommended font: Cascadia Mono - font size 16
            // Window size
            Console.CursorVisible = false;
            Console.SetWindowSize(170, 44);
            Console.SetBufferSize(171, 45);

            // Player stats
            int playerHealth = 100;
            int playerMana = 100;
            int playerGold = 100;
            int[] playerInventory = new int[5];
            playerInventory[0] = 2; // Health potions
            playerInventory[1] = 2; // Mana potions
            playerInventory[2] = 0; // First relic
            playerInventory[3] = 0; // Second relic
            playerInventory[4] = 0; // Third relic

            string playerName = Environment.UserName.Substring(0, 1).ToUpper() + Environment.UserName.Substring(1);

            int computerHealth = 100;
            Random generator = new Random();

            IntroScene(playerName);
            DrawMainMenu(mainMenuOptions, TITLE);
            
            
            int selectedOption = -1;

            bool exit = false;
            do
            {
                selectedOption = DrawMainMenu(mainMenuOptions, TITLE);
                switch (selectedOption)
                {
                    case 0:
                        WizardScene(playerName);
                        DrawUI();
                        SelectStage(stages);
                        Console.Clear();
                        DrawUI();
                        break;
                    case 1:
                        ShowInstructions();
                        break;
                    case 2:
                        exit = true;
                        break;
                }
            } while (selectedOption != 0 && !exit);

            Battle(generator, ref playerHealth, ref playerMana, ref computerHealth, playerInventory, playerBattleOptions, playerName, knightEnemy);
        }
    }
}
