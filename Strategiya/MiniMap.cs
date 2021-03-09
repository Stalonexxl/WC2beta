using System.Drawing;
using System.Windows.Forms;

namespace Strategiya
{
    class MiniMap
    {
        public static int rectSize { get; private set; }
        public MiniMap(PaintEventArgs e)
        {
            SolidBrush BlackBrush = new SolidBrush(Color.Black);
            SolidBrush BlueBrush = new SolidBrush(Color.Blue);
            SolidBrush GreenBrush = new SolidBrush(Color.Green);
            SolidBrush DarkGreenBrush = new SolidBrush(Color.DarkGreen);
            SolidBrush BrownBrush = new SolidBrush(Color.Brown);
            SolidBrush GrayBrush = new SolidBrush(Color.Gray);
            Pen whitePen = new Pen(Color.White, 3);

            Rectangle rect = new Rectangle(30, 30, 300, 300);
            e.Graphics.FillRectangle(BlackBrush, rect);
            rectSize = rect.Width / Form1.m.width;
            int Y = (int)Form1.CameraY;
            int X = (int)Form1.CameraX;

            // Отрисовка миникарты
            for (int i = 0; i < Form1.m.width; i++)
                for (int j = 0; j < Form1.m.height; j++)
                {
                    if (Form1.m.arrMap[j][i] == 100)
                    {
                        Rectangle rectEarth = new Rectangle(30 + i * rectSize, 30 + j * rectSize, rectSize, rectSize);
                        e.Graphics.FillRectangle(BrownBrush, rectEarth);
                    }
                    if (Form1.m.arrMap[j][i] == 200)
                    {
                        Rectangle rectGrass = new Rectangle(30 + i * rectSize, 30 + j * rectSize, rectSize, rectSize);
                        e.Graphics.FillRectangle(GreenBrush, rectGrass);
                    }
                    if (Form1.m.arrMap[j][i] == 300)
                    {
                        Rectangle rectWater = new Rectangle(30 + i * rectSize, 30 + j * rectSize, rectSize, rectSize);
                        e.Graphics.FillRectangle(BlueBrush, rectWater);
                    }
                    if (Form1.m.arrMap[j][i] >= 4000 || Form1.m.arrMap[j][i] == 400)
                    {
                        Rectangle rectForest = new Rectangle(30 + i * rectSize, 30 + j * rectSize, rectSize, rectSize);
                        e.Graphics.FillRectangle(DarkGreenBrush, rectForest);
                    }
                    if (Form1.m.arrMap[j][i] == 500)
                    {
                        Rectangle rectMountains = new Rectangle(30 + i * rectSize, 30 + j * rectSize, rectSize, rectSize);
                        e.Graphics.FillRectangle(GrayBrush, rectMountains);
                    }
                }
            // Отрисовка белого квадрата на миникарте
            Rectangle rectCamera = new Rectangle(30 + X * rectSize, 30 + Y * rectSize, rectSize * 10, rectSize * 10);
            e.Graphics.DrawRectangle(whitePen, rectCamera);
        }
    }
}
