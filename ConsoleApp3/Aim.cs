using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    public class Aim
    {
        private readonly ConsoleColor _aimColor;
        public Aim(int aimX, int aimY, ConsoleColor aimColor)
        {
            AimX = aimX;
            AimY = aimY;
            _aimColor = aimColor;
            AimCell = new Pixel(AimX, AimY, _aimColor);
            AimDraw();
        }
        
        public int AimX { get; private set; }
        public int AimY { get; private set; }
        public Pixel AimCell { get; private set; }

        public void AimDraw()
        {
            AimCell.AimDraw();
        }
}
}
