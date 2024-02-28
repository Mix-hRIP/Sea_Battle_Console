using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    public class Ship
    {
        private readonly ConsoleColor _shipColor;
        public Ship(int shipSize, Rotation shipRotation, int shipX, int shipY, ConsoleColor shipColor)
        {
            CoordX = shipX;
            CoordY = shipY;
            _shipColor = shipColor;
            Rotation = shipRotation;
            ShipMainCell = new Pixel(CoordX, CoordY, _shipColor);
            ShipSize = shipSize;
            int h = 0, v = 0;
            if (shipRotation == Rotation.horizontal)
            {
                h = 1;
                v = 0;
            }
            else
            {
                h = 0;
                v = 1;
            }
            for (int i = ShipSize - 1; i > 0 ; i--)
            {
                ShipBody.Add(new Pixel(CoordX + i * h, CoordY + i * v, _shipColor));
            }
            HitDraw();
        }
        public int CoordX { get; private set; }
        public int CoordY { get; private set; }
        public Pixel ShipMainCell { get; private set; }
        public int ShipSize { get; private set; }
        public List<Pixel> ShipBody { get; } = new List<Pixel>();
        public Rotation Rotation { get; private set; }

        public void HitDraw()
        {
            ShipMainCell.HitDraw();
            foreach (Pixel pixel in ShipBody)
            {
                pixel.HitDraw();
            }
        }

    }
}
