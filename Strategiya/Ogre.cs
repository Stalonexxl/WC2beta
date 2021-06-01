using System.Drawing;
using System;

namespace Strategiya
{
    class Ogre : Unit
    {
        System.Windows.Forms.Timer timerOgre;
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
            timerOgre = new System.Windows.Forms.Timer();
            timerOgre.Interval = 500;
            timerOgre.Tick += new EventHandler(HaveAgressive);
            timerOgre.Start();
            respawnPoint = new Point(position.X, position.Y);
            
        }
        public override void Destroy()
        {
            if (currAnimation < 9)
            {
                GameMehanic.legion.Remove(this);
                GameMehanic.units.Remove(this);
                GameMehanic.cemetery.Add(this);
                isAttack = false;
                isMooving = false;
                isDead = true;
                currAnimation = 9;
                currentDirection = pastDirection;
            }
            currAnimation += 0.1;
            if (currAnimation >= 14)
            {
                GameMehanic.cemetery.Remove(this);
                timerDie.Stop();
                return;
            }
        }
        private void HaveAgressive(object sender, EventArgs e)
        {
            if (enemy != null)
                FightTimerStart(enemy);
            else
            {
                if (position.X == respawnPoint.X && position.Y == respawnPoint.Y)
                    enemy = HaveAgressiveTo();
                else if(!isMooving)
                    PathUnit(respawnPoint);
            }
            if (Math.Abs(respawnPoint.X - Position.X) >= 5 || Math.Abs(respawnPoint.Y - Position.Y) >= 5)
            {
                timer.Stop();
                enemy = null;
            }
        }
        public Unit HaveAgressiveTo()
        {
            foreach (Unit Nenemy in GameMehanic.units)
            {
                if (Nenemy.Id == Id || Nenemy.fraction == this.fraction)
                    continue;
                for (int i = position.X - 2; i <= position.X + 2; i++)
                    for (int j = position.Y - 2; j <= position.Y + 2; j++)
                        if (Nenemy.Position.X == i && Nenemy.Position.Y == j)
                            return Nenemy;  
            }
            return null;
        }
    }
}
