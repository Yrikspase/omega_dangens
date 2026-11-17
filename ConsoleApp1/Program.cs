using System;
using System.Text;
namespace RogueDungeon0125
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.CursorVisible = false;
            int playerHealth=100,maxHealth=100,gold=0,
                posX=0,posY=0;
            int mapWidth=10,mapHeight=10;
            int hudCursorPosition=12;
            int statusCursorPosition=13;
            int playerInputPosition=14;
            int goldX,goldY,trapX,trapY;
            int exitX=mapWidth-1,exitY=mapHeight-1;
            bool isVictory=false;
            bool isDefeat=false;
            bool isExit=false;
            Random rnd=new Random();
            goldX=rnd.Next(0,mapWidth);
            goldY=rnd.Next(0,mapHeight);
            trapX=rnd.Next(0,mapWidth);
            trapY=rnd.Next(0,mapHeight);
            if ((goldX==0&&goldY==0) || (goldX==exitX&&goldY==exitY))
            {
                goldX=2;
                goldY=1;
            }
            if ((trapX==0&&trapY==0) || (trapX==exitX&&trapY==exitY))
            {
                trapX=1;
                trapY=2;
            }
            if (goldX==trapX&&goldY==trapY)
            {
                trapX=3;
                trapY=3;
            }
            for (int i=0;i<mapHeight;i++)
            {
                for (int j=0;j<mapWidth;j++)
                {
                    if (goldX==j&&goldY==i)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("¥ ");
                        Console.ResetColor();
                    }
                    else if (trapX==j&&trapY==i)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("X ");
                        Console.ResetColor();
                    }
                    else if (exitX==j&&exitY==i)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("E ");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write("# ");
                    }
                }
                Console.WriteLine();
            }
            Console.ForegroundColor=ConsoleColor.Green;
            Console.SetCursorPosition(posX, posY);
            Console.Write("H");
            Console.ResetColor();
            string message="Соберите ¥ и дойди до E.управление WASD - ход, H - лечение (5¥), Q - выход.";
            while (!isVictory&&!isDefeat&&!isExit)
            {
                Console.SetCursorPosition(0, hudCursorPosition);
                Console.Write("$♥️[{playerHealth}/{maxHealth}] ¥{gold,-5} | Выход: ({exitX},{exitY})      ");
                Console.SetCursorPosition(0, statusCursorPosition);
                Console.Write(message.PadRight(75));
                Console.SetCursorPosition(0, playerInputPosition);
                Console.Write("управление (W/A/S/D, H, Q):");
                var key=Console.ReadKey(true);
                int stepX=0,stepY=0;
                bool moved=false;
                switch (key.Key)
                {
                    case ConsoleKey.D:
                        if (posX/2==mapWidth-1)
                        {
                            message="это восточная граница поля и помни что вход в дальне восточные страны тебе запрещён,во все кроме северной кореи за заслуги перед миром";
                            continue;
                        }
                        stepX=2;
                        moved=true;
                        break;
                    case ConsoleKey.A:
                        if (posX==0)
                        {
                            message="это западная граница поля и да туда тебя не пустят";
                            continue;
                        }
                        stepX=-2;
                        moved=true;
                        break;
                    case ConsoleKey.W:
                        if (posY==0)
                        {
                            message="это верхняя граница поля и да тут нет прохода в рай";
                            continue;
                        }
                        stepY=-1;
                        moved=true;
                        break;
                    case ConsoleKey.S:
                        if (posY==mapHeight-1)
                        {
                            message="это нижняя граница поля и да тут нет прохода в ад";
                            continue;
                        }
                        stepY=1;
                        moved=true;
                        break;
                    case ConsoleKey.H:
                        if (gold < 5)
                        {
                            message="у вас недостаточно средств,пожайлусто найди работу (нужно 5¥).";
                        }
                        else if (playerHealth>=maxHealth)
                        {
                            message="Максимум здоровья!";
                        }
                        else
                        {
                            gold-=5;
                            playerHealth+=30;
                            if (playerHealth>maxHealth)
                            {
                                playerHealth=maxHealth;
                            }
                            message="Вы сожрали бумажные юани +30 HP за 5¥.";
                        }
                        break;
                    case ConsoleKey.Q:
                        isExit=true;
                        message="не справился с управлением";
                        break;

                    default:
                        message="перемещяйся используя только клавиши W, A, S, D, или й если не знаешь англиский.";
                        break;
                }
                if (moved)
                {
                    Console.SetCursorPosition(posX,posY);
                    if (posX/2==exitX&&posY==exitY)
                    {
                        Console.ForegroundColor=ConsoleColor.Cyan;
                        Console.Write("E");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write("#");
                    }
                    posX+=stepX;
                    posY+=stepY;
                    if (posX/2==goldX&&posY==goldY)
                    {
                        gold+=10;
                        goldX=goldY=-1;
                        message="Вы нашли юани! +10¥";
                    }
                    else if (posX/2==trapX&&posY==trapY)
                    {
                        playerHealth-=40;
                        trapX=trapY=0;
                        message="из за пыли в воздухе ты чихнул -40 HP";
                    }
                    else if (posX/2==exitX&&posY==exitY)
                    {
                        if (gold>=10)
                        {
                            isVictory=true;
                            message="молодец ты вышел и забрал с собой юани";
                        }
                        else
                        {
                            message="сначало возми юани потом иди на выход у тебя их должно быть не мение 10";
                        }
                    }
                    else
                    {
                        message="перемещение";
                    }
                    Console.ForegroundColor=ConsoleColor.Green;
                    Console.SetCursorPosition(posX, posY);
                    Console.Write("H");
                    Console.ResetColor();
                }
                if (playerHealth<0)
                {
                    playerHealth=0;
                }
                if (gold<0)
                {
                    gold=0;
                }
                if (playerHealth==0)
                {
                    isDefeat=true;
                    message="You are dead";
                }
                else if (playerHealth<=30)
                {
                    message+="[внимание мало хп]";
                }
                else
                {
                }
            }

        }
    }
}
