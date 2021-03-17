using System.Drawing;

namespace Strategiya
{
    public class GruntOrc: Unit
    {
        public GruntOrc(Point pos)
        {
            Sprite = new Bitmap[12, 9];
            for (int i = 0; i < 12; i++)
                for (int j = 0; j < 9; j++)
                {
                    Sprite[i, j] = new Bitmap(size, size);
                    using (Graphics g = Graphics.FromImage(Sprite[i, j]))
                        g.DrawImage(Properties.Resources.grunt, 0, 0, new Rectangle(size2 * j + 25, size2 * i + 5, size, size), GraphicsUnit.Pixel);
                    if (j == 5)
                        using (Graphics g = Graphics.FromImage(Sprite[i, j]))
                            g.DrawImage(Properties.Resources.grunt, 0, 0, new Rectangle(size2 * 2 + 25, size2 * i + 5, size, size), GraphicsUnit.Pixel);
                    if (j == 6)
                        using (Graphics g = Graphics.FromImage(Sprite[i, j]))
                            g.DrawImage(Properties.Resources.grunt, 0, 0, new Rectangle(size2 * 1 + 25, size2 * i + 5, size, size), GraphicsUnit.Pixel);
                    if (j == 7)
                        using (Graphics g = Graphics.FromImage(Sprite[i, j]))
                            g.DrawImage(Properties.Resources.grunt, 0, 0, new Rectangle(size2 * 3 + 25, size2 * i + 5, size, size), GraphicsUnit.Pixel);
                    if (j == 8)
                        using (Graphics g = Graphics.FromImage(Sprite[i, j]))
                            g.DrawImage(Properties.Resources.grunt, 0, 0, new Rectangle(25, 5, size, size), GraphicsUnit.Pixel);
                }
            for (int i = 0; i < 12; i++)
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
            health = 100;
            attackPower = 10;
            fraction = "horde";
        }
        public override void Destroy()
        {
            if(currAnimation < 9)
            {
                currAnimation = 9;
                currentDirection = pastDirection;
            }
            currAnimation += 0.1;
            isAttack = false;
            isMooving = false;
            if (currAnimation >= 11.8)
            Form1.formPointer.units.Remove(this);
        }
    }
}
