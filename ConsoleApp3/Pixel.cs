using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    public struct Pixel
    {

        public Pixel(int x, int y, ConsoleColor color, int pxSize = 3)
        {
            X = x;
            Y = y;
            Color = color;
            PxSize = pxSize;
        }
        public int X { get; set; }
        public int Y { get; set; }
        public ConsoleColor Color { get; }
        public int PxSize { get; }

        public void ShipDraw()
        {
            Console.ForegroundColor = Color;
            for (int x = 0; x < PxSize; x++)
            {
                for (int y = 0; y < PxSize; y++)
                {
                    Console.SetCursorPosition(left: X * PxSize + x, top: Y * PxSize + y);
                    Console.Write("█");
                }
            }
            
        }
        public void WaveDraw()
        {
            Console.ForegroundColor = Color;
            Random rnd = new Random();
            int rand;
            for (int x = 0; x < PxSize; x++)
            {
                for (int y = 0; y < PxSize; y++)
                {
                    Console.SetCursorPosition(left: X * PxSize + x, top: Y * PxSize + y);
                    rand = rnd.Next(0, 3);
                    string[] WaveChar = new string[3] { "_", "~", "~" };
                    Console.Write(WaveChar[rand]);
                }
            }
        }
        public void MissDraw()
        {
            Console.ForegroundColor = Color;
            for (int x = 0; x < PxSize; x++)
            {
                for (int y = 0; y < PxSize; y++)
                {
                    Console.SetCursorPosition(left: X * PxSize + x, top: Y * PxSize + y);
                    Console.Write("░");
                }
            }
            
        }
        public void HitDraw()
        {
            Console.ForegroundColor = Color;
            for (int x = 0; x < PxSize; x++)
            {
                for (int y = 0; y < PxSize; y++)
                {
                    Console.SetCursorPosition(left: X * PxSize + x, top: Y * PxSize + y);
                    Console.Write("▓");
                }
            }
            
        }
        public void BorderDraw()
        {
            Console.ForegroundColor = Color;
            for (int x = 0; x < PxSize; x++)
            {
                for (int y = 0; y < PxSize; y++)
                {
                    Console.SetCursorPosition(X * PxSize + x, Y * PxSize + y);
                    Console.Write("█");
                }
            }
            
        }
        public void ClearDraw()
        {
            Console.ForegroundColor = Color;
            for (int x = 0; x < PxSize; x++)
            {
                for (int y = 0; y < PxSize; y++)
                {
                    Console.SetCursorPosition(left: X * PxSize + x, top: Y * PxSize + y);
                    Console.Write(" ");
                }
            }

        }
        public void AimDraw()
        {
            Console.ForegroundColor = Color;
            for (int x = 0; x < PxSize; x++)
            {
                for (int y = 0; y < PxSize; y++)
                {
                    if (x == 0 && y == 0)
                    {
                        Console.SetCursorPosition(left: X * PxSize + x, top: Y * PxSize + y);
                        Console.Write("╔");
                    }
                    if (x == 0 && y == PxSize-1)
                    {
                        Console.SetCursorPosition(left: X * PxSize + x, top: Y * PxSize + y);
                        Console.Write("╚");
                    }
                    if (x == PxSize - 1 && y == 0)
                    {
                        Console.SetCursorPosition(left: X * PxSize + x, top: Y * PxSize + y);
                        Console.Write("╗");
                    }
                    if (x == PxSize - 1 && y == PxSize - 1)
                    {
                        Console.SetCursorPosition(left: X * PxSize + x, top: Y * PxSize + y);
                        Console.Write("╝");
                    }
                }
            }
        }
    }
}
