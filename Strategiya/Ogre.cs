﻿using System.Drawing;

namespace Strategiya
{
    class Ogre : Unit
    {
        public Ogre(Point pos)
        {
            Sprite = new Bitmap[14, 9];
            for (int i = 0; i < 14; i++)
                for (int j = 0; j < 9; j++)
                {
                    Sprite[i, j] = new Bitmap(size, size);
                    using (Graphics g = Graphics.FromImage(Sprite[i, j]))
                        g.DrawImage(Properties.Resources.ogre, 0, 0, new Rectangle(size2 * j + 21, size2 * i + 15, size, size), GraphicsUnit.Pixel);
                    if (j == 5)
                        using (Graphics g = Graphics.FromImage(Sprite[i, j]))
                            g.DrawImage(Properties.Resources.ogre, 0, 0, new Rectangle(size2 * 2 + 21, size2 * i + 15, size, size), GraphicsUnit.Pixel);
                    if (j == 6)
                        using (Graphics g = Graphics.FromImage(Sprite[i, j]))
                            g.DrawImage(Properties.Resources.ogre, 0, 0, new Rectangle(size2 * 1 + 21, size2 * i + 15, size, size), GraphicsUnit.Pixel);
                    if (j == 7)
                        using (Graphics g = Graphics.FromImage(Sprite[i, j]))
                            g.DrawImage(Properties.Resources.ogre, 0, 0, new Rectangle(size2 * 3 + 21, size2 * i + 15, size, size), GraphicsUnit.Pixel);
                    if (j == 8)
                        using (Graphics g = Graphics.FromImage(Sprite[i, j]))
                            g.DrawImage(Properties.Resources.ogre, 0, 0, new Rectangle( 21, 15, size, size), GraphicsUnit.Pixel);
                }
            for (int i = 0; i < 14; i++)
            {
                Sprite[i, 5].RotateFlip(RotateFlipType.Rotate180FlipY);
                Sprite[i, 6].RotateFlip(RotateFlipType.Rotate180FlipY);
                Sprite[i, 7].RotateFlip(RotateFlipType.Rotate180FlipY);
            }
            Id = ++counter;
            addInLegion = false;
            position = pos;
            currentDirection = DirectionUnit.None;
            currAnimation = 0;
            OffsetX = OffsetY = 0;
            isNextStep = false;
            health = 150;
            attackPower = 5;
            fraction = "neitral";
        }
        public override void Destroy()
        {
            if(currAnimation < 9)
            {
                currAnimation = 9;
                currentDirection = pastDirection;
            }
            currAnimation += 0.1;    

            if(currAnimation >= 13.8)
                Form1.formPointer.units.Remove(this);             
        }
    }
}
