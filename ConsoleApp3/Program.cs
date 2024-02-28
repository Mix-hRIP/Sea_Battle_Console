using System;
using static System.Console;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.ConstrainedExecution;
using System.Diagnostics.Eventing.Reader;
using System.Threading;

namespace ConsoleApp3
{
    internal class Program
    {
        private const int MapWidth = 27;
        private const int MapHeight = 14;
        private const int ScreenWidth = MapWidth * 3 + 1;
        private const int ScreenHeight = MapHeight * 3 + 1;

        private const ConsoleColor BorderColor = ConsoleColor.DarkBlue;
        private const ConsoleColor WaveColor = ConsoleColor.Blue;
        private const ConsoleColor ShipCanPlaceColor = ConsoleColor.Green;
        private const ConsoleColor ShipCantPlaceColor = ConsoleColor.Red;
        private const ConsoleColor ShipColor = ConsoleColor.Gray;
        private const ConsoleColor HitColor = ConsoleColor.Yellow;
        private const ConsoleColor MissColor = ConsoleColor.Cyan;

        static void Main()
        {
            SetWindowSize(ScreenWidth, ScreenHeight);
            SetBufferSize(ScreenWidth, ScreenHeight);
            CursorVisible = false;
            Console.Title = "";
            Field.PlayerField = FieldGenerate(Field.PlayerField);
            Field.BotField = FieldGenerate(Field.BotField);
            Field.PlayerAttack = FieldGenerate(Field.PlayerAttack);
            Field.BotAttack = FieldGenerate(Field.BotAttack);
            DrawBorder();
            DrawSea();
            ShipPlacer();
            Field.BotField= BotFieldGenerate(Field.BotField);
            Game();
            ReadKey();
        }
        static bool Game()
        {
            short PHP = 20, BotHP = 20;
            bool PlayerShot = true, BotShot = true;
            while (PHP > 0 && BotHP > 0)
            {
                while (PlayerShot)
                {
                    PlayerShot = Aim();
                    DrawSea();
                    DrawShip();
                    DrawBotAttack();
                    DrawAttack();
                    if (PlayerShot)
                    {
                        BotHP--;
                    }
                }
                while (BotShot)
                {
                    Thread.Sleep(500);
                    BotShot = BotAim();
                    DrawSea();
                    DrawShip();
                    DrawBotAttack();
                    DrawAttack();
                    if (BotShot)
                    {
                        PHP--;
                    }
                }
                PlayerShot = true;
                BotShot = true;
            }
            return true;
        }
        static bool BotAim()
        {
            Random rnd = new Random();
            bool Pl = true, CanPlace = false, Rez = false;
            int AimX = 0, AimY = 0;

            while (Pl)
            {
                AimX = rnd.Next(0, 10); AimY = rnd.Next(0, 10);

                if (Field.BotAttack[AimX, AimY] == 0)
                {
                    CanPlace = true;
                }
                else
                {
                    CanPlace = false;
                }
                if (CanPlace)
                {
                    if (Field.PlayerField[AimX, AimY] > 0)
                    {
                        Rez = true;
                        Field.BotAttack[AimX, AimY] = Field.PlayerField[AimX, AimY];
                    }
                    else
                    {
                        Rez = false;
                        Field.BotAttack[AimX, AimY] = 5;
                    }
                    Pl = false;
                }
            }

            return Rez;
        }
        static bool Aim()
        {
            bool Pl = true, CanPlace = false, Rez = false;
            int AimX = 0, AimY = 0;
            var aim = new Aim(AimX + 15, AimY + 2, ShipCanPlaceColor);
            while (Pl)
            {
                ConsoleKey key = ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.RightArrow: AimX++; DrawSea(); break;
                    case ConsoleKey.LeftArrow: AimX--; DrawSea(); break;
                    case ConsoleKey.UpArrow: AimY--; DrawSea(); break;
                    case ConsoleKey.DownArrow: AimY++; DrawSea(); break;
                    case ConsoleKey.Enter:
                        if (CanPlace)
                        {
                            Pl = false;
                        }
                        else
                        {
                            Pl = true;
                        }
                        break;
                    //case ConsoleKey.X: DrawBotShip(); break;
                }
                DrawShip();
                DrawAttack();

                DrawBotAttack();
                if (AimX < 0) { AimX = 0; }
                if (AimX > 9) { AimX = 9; }
                if (AimY < 0) { AimY = 0; }
                if (AimY > 9) { AimY = 9; }

                if (Field.PlayerAttack[AimX, AimY] == 0)
                {
                    aim = new Aim(AimX + 15, AimY + 2, ShipCanPlaceColor);
                    CanPlace = true;
                }
                else
                {
                    aim = new Aim(AimX + 15, AimY + 2, ShipCantPlaceColor);
                    CanPlace = false;
                }
                if (!Pl)
                {
                    if (Field.BotField[AimX, AimY] > 0)
                    {
                        Rez = true;
                        Field.PlayerAttack[AimX, AimY] = Field.BotField[AimX, AimY];
                    }
                    else
                    {
                        Rez = false;
                        Field.PlayerAttack[AimX, AimY] = 5;
                    }
                }
            }

            return Rez;
        }
        static void ShipPlacer()
        {
            ShipPlace(4);
            for (int i = 2; i > 0; i--)
            {
                ShipPlace(3);
            }
            for (int i = 3; i > 0; i--)
            {
                ShipPlace(2);
            }
            for (int i = 4; i > 0; i--)
            {
                ShipPlace(1);
            }
        }

        public static void ShipPlace(int ShSize)
        {
            bool Pl = true;
            int ShipX = 0, ShipY = 0, X = 0;
            bool Rot = true, CanPlace = false;
            var ship = new Ship(ShSize, Rotation.horizontal, ShipX + 2, ShipY + 2, ShipCanPlaceColor);

            while (Pl)
            {
                ConsoleKey key = ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.RightArrow: ShipX++; break;
                    case ConsoleKey.LeftArrow: ShipX--; break;
                    case ConsoleKey.UpArrow: ShipY--; break;
                    case ConsoleKey.DownArrow: ShipY++; break;
                    case ConsoleKey.Enter:
                        if (CanPlace)
                        {
                            Pl = false;
                        }
                        else
                        {
                            Pl = true;
                        }
                        break;
                    case ConsoleKey.Spacebar: Rot = !Rot; break;
                }
                DrawSea();
                if (ShipX < 0)
                {
                    ShipX = 0;
                }
                if (ShipY < 0)
                {
                    ShipY = 0;
                }
                if (Rot)
                {
                    if (ShipX > 10 - ShSize)
                    {
                        ShipX = 10 - ShSize;
                    }
                    if (ShipY >= 10)
                    {
                        ShipY = 9;
                    }
                    X = 0;
                    for (int k = 0; k < ShSize; k++)
                    {
                        for (int i = -1; i < 2; i++)
                        {
                            for (int j = -1; j < 2; j++)
                            {
                                if (ShipX + i + k < 0 || ShipX + i + k > 9 || ShipY + j < 0 || ShipY + j > 9)
                                {

                                }
                                else
                                {
                                    if (Field.PlayerField[ShipX + i + k, ShipY + j] > 0)
                                    {
                                        X++;
                                    }
                                }
                                
                            }
                        }
                    }
                    if (X > 0)
                    {
                        CanPlace = false;
                    }
                    else
                    {
                        CanPlace = true;
                    }
                    if (CanPlace)
                    {
                        ship = new Ship(ShSize, Rotation.horizontal, ShipX + 2, ShipY + 2, ShipCanPlaceColor);
                        if (!Pl)
                        {
                            for (int k = 0; k < ShSize; k++)
                            {
                                Field.PlayerField[ShipX + k, ShipY] = ShSize;
                            }
                        }
                    }
                    else
                    {
                        ship = new Ship(ShSize, Rotation.horizontal, ShipX + 2, ShipY + 2, ShipCantPlaceColor);
                    }
                }
                else
                {
                    if (ShipX >= 10)
                    {
                        ShipX = 9;
                    }
                    if (ShipY > 10 - ShSize)
                    {
                        ShipY = 10 - ShSize;
                    }
                    X = 0;
                    for (int k = 0; k < ShSize; k++)
                    {
                        for (int i = -1; i < 2; i++)
                        {
                            for (int j = -1; j < 2; j++)
                            {
                                if (ShipX + i < 0 || ShipX + i > 9 || ShipY + j + k < 0 || ShipY + j + k > 9)
                                {

                                }
                                else
                                {
                                    if (Field.PlayerField[ShipX + i, ShipY + j + k] > 0)
                                    {
                                        X++;
                                    }
                                }
                                
                            }
                        }
                    }
                    if (X > 0)
                    {
                        CanPlace = false;
                    }
                    else
                    {
                        CanPlace = true;
                    }
                    if (CanPlace)
                    {
                        ship = new Ship(ShSize, Rotation.vertical, ShipX + 2, ShipY + 2, ShipCanPlaceColor);
                        if (!Pl)
                        {
                            for (int k = 0; k < ShSize; k++)
                            {
                                Field.PlayerField[ShipX, ShipY + k] = ShSize;
                            }
                        }
                    }
                    else
                    {
                        ship = new Ship(ShSize, Rotation.vertical, ShipX + 2, ShipY + 2, ShipCantPlaceColor);
                    }
                }
                DrawShip();
            }
        }

        public static void DrawBorder()
        {   
            for (int i = 0; i < MapWidth; i++)
            {
                for(int j = 0; j < MapHeight; j++)
                {
                    if (i == 0 || j == 0 || i == MapWidth-1 || j == MapHeight-1 || i == 13)
                    {
                        new Pixel(i, j, BorderColor).BorderDraw();
                    }
                }
            }
        }
        public static void DrawSea()
        {
            for (int i = 0; i < MapWidth; i++)
            {
                for (int j = 0; j < MapHeight; j++)
                {
                    if (i >= 2 && i < 12 && j >= 2 && j < 12)
                    {
                        new Pixel(i, j, WaveColor).WaveDraw();
                    }
                    if (i >= 15 && i < 25 && j >= 2 && j < 12)
                    {
                        new Pixel(i, j, WaveColor).WaveDraw();
                    }
                }
            }
        }

        public static void DrawShip()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (Field.PlayerField[i, j] > 0)
                    {
                        new Pixel(i+2, j+2, ShipColor).ShipDraw();
                    }
                }
            }
        }
        public static void DrawBotShip()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (Field.BotField[i, j] > 0)
                    {
                        new Pixel(i + 15, j + 2, ShipColor).ShipDraw();
                    }
                }
            }
        }
        public static void DrawAttack()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    switch (Field.PlayerAttack[i, j])
                    {
                        case 0:  break;
                        case 5: new Pixel(i + 15, j + 2, MissColor).MissDraw(); break;
                        default: new Pixel(i + 15, j + 2, HitColor).HitDraw(); break;
                    }
                }
            }
        }
        public static void DrawBotAttack()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    switch (Field.BotAttack[i, j])
                    {
                        case 0:  break;
                        case 5: new Pixel(i + 2, j + 2, MissColor).MissDraw(); break;
                        default: new Pixel(i + 2, j + 2, HitColor).HitDraw(); break;
                    }
                }
            }
        }
        static int[,] FieldGenerate(int[,] FG)
        {   

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    FG[i, j] = 0;
                }
            }
            return FG;
        }

        static int[,] BotFieldGenerate(int[,] FG)
        {
            FG = ShipGen(FG, 4);
            for (int i = 2; i > 0; i--)
            {
                FG = ShipGen(FG, 3);
            }
            for (int i = 3; i > 0; i--)
            {
                FG = ShipGen(FG, 2);
            }
            for (int i = 4; i > 0; i--)
            {
                FG = ShipGen(FG, 1);
            }
            return FG;
        }
        static int[,] ShipGen(int[,] S3, int ShSize)
        {
            Random rnd = new Random();
            int X = 0, v = 0;
            int ShipX = 0;
            int ShipY = 0;
            bool n = true, CanPlace = false;
            while (n)
            {
                v = rnd.Next(0,2);
                if (v == 1)
                {
                    ShipX = rnd.Next(0, 10 - (ShSize - 1));
                    ShipY = rnd.Next(0, 10 - 1);
                    X = 0;
                    for (int k = 0; k < ShSize; k++)
                    {
                        for (int i = -1; i < 2; i++)
                        {
                            for (int j = -1; j < 2; j++)
                            {
                                if (ShipX + i + k < 0 || ShipX + i + k > 9 || ShipY + j < 0 || ShipY + j > 9)
                                {

                                }
                                else
                                {
                                    if (Field.BotField[ShipX + i + k, ShipY + j] > 0)
                                    {
                                        X++;
                                    }
                                }

                            }
                        }
                    }
                    if (X > 0)
                    {
                        CanPlace = false;
                    }
                    else
                    {
                        CanPlace = true;
                    }
                    if (CanPlace)
                    {
                        for (int k = 0; k < ShSize; k++)
                        {
                            Field.BotField[ShipX + k, ShipY] = ShSize;
                        }
                        n = false;
                    }
                    else
                    {
                        n = true;
                    }
                }
                else
                {
                    ShipX = rnd.Next(0, 10 - 1);
                    ShipY = rnd.Next(0, 10 - (ShSize - 1));
                    X = 0;
                    for (int k = 0; k < ShSize; k++)
                    {
                        for (int i = -1; i < 2; i++)
                        {
                            for (int j = -1; j < 2; j++)
                            {
                                if (ShipX + i < 0 || ShipX + i > 9 || ShipY + j + k < 0 || ShipY + j + k > 9)
                                {

                                }
                                else
                                {
                                    if (Field.BotField[ShipX + i, ShipY + j + k] > 0)
                                    {
                                        X++;
                                    }
                                }

                            }
                        }
                    }
                    if (X > 0)
                    {
                        CanPlace = false;
                    }
                    else
                    {
                        CanPlace = true;
                    }
                    if (CanPlace)
                    {
                        for (int k = 0; k < ShSize; k++)
                        {
                            Field.BotField[ShipX, ShipY + k] = ShSize;
                            n = false;
                        }
                    }
                    else
                    {
                        n = true;
                    }
                }
            }
            return S3;
        }
    }
}
