using System.Windows.Forms;

namespace Strategiya
{
    public enum Direction { Up = -1, Right = 1, Down = 1, Left = -1};
    class Forest
    {
        readonly int Y = (int)Form1.CameraY;
        readonly int X = (int)Form1.CameraX;
        public Forest()
        {       
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                {                   
                    if (Form1.m.arrMap[j + Y][i + X] >= 400)
                    {
                        try
                        {
                            //1
                            if (Form1.m.arrMap[j + Y + (int)Direction.Up][i + X] < 400 && Form1.m.arrMap[j + Y][i + X + (int)Direction.Left] < 400 && Form1.m.arrMap[j + Y + (int)Direction.Down][i + X] >= 400 && Form1.m.arrMap[j + Y][i + X + (int)Direction.Right] >= 400)
                                Form1.m.arrMap[j + Y][i + X] = 4102;
                            //2
                            if (Form1.m.arrMap[j + Y + (int)Direction.Up][i + X] < 400 && Form1.m.arrMap[j + Y][i + X + (int)Direction.Left] >= 400 && Form1.m.arrMap[j + Y + (int)Direction.Down][i + X] >= 400 && Form1.m.arrMap[j + Y][i + X + (int)Direction.Right] >= 400)
                                Form1.m.arrMap[j + Y][i + X] = 4203;
                            //3
                            if (Form1.m.arrMap[j + Y + (int)Direction.Up][i + X] < 400 && Form1.m.arrMap[j + Y][i + X + (int)Direction.Left] >= 400 && Form1.m.arrMap[j + Y + (int)Direction.Down][i + X] >= 400 && Form1.m.arrMap[j + Y][i + X + (int)Direction.Right] < 400)
                                Form1.m.arrMap[j + Y][i + X] = 4302;
                            //4
                            if (Form1.m.arrMap[j + Y + (int)Direction.Up][i + X] >= 400 && Form1.m.arrMap[j + Y][i + X + (int)Direction.Left] < 400 && Form1.m.arrMap[j + Y + (int)Direction.Down][i + X] >= 400 && Form1.m.arrMap[j + Y][i + X + (int)Direction.Right] >= 400)
                                Form1.m.arrMap[j + Y][i + X] = 4401;
                            //5
                            if (Form1.m.arrMap[j + Y + (int)Direction.Up][i + X] >= 400 && Form1.m.arrMap[j + Y][i + X + (int)Direction.Left] >= 400 && Form1.m.arrMap[j + Y + (int)Direction.Down][i + X] >= 400 && Form1.m.arrMap[j + Y][i + X + (int)Direction.Right] >= 400)
                                Form1.m.arrMap[j + Y][i + X] = 4510;
                            //6
                            if (Form1.m.arrMap[j + Y + (int)Direction.Up][i + X] >= 400 && Form1.m.arrMap[j + Y][i + X + (int)Direction.Left] >= 400 && Form1.m.arrMap[j + Y + (int)Direction.Down][i + X] >= 400 && Form1.m.arrMap[j + Y][i + X + (int)Direction.Right] < 400)
                                Form1.m.arrMap[j + Y][i + X] = 4601;
                            //7
                            if (Form1.m.arrMap[j + Y + (int)Direction.Up][i + X] >= 400 && Form1.m.arrMap[j + Y][i + X + (int)Direction.Left] < 400 && Form1.m.arrMap[j + Y + (int)Direction.Down][i + X] < 400 && Form1.m.arrMap[j + Y][i + X + (int)Direction.Right] >= 400)
                                Form1.m.arrMap[j + Y][i + X] = 4701;
                            //8
                            if (Form1.m.arrMap[j + Y + (int)Direction.Up][i + X] >= 400 && Form1.m.arrMap[j + Y][i + X + (int)Direction.Left] >= 400 && Form1.m.arrMap[j + Y + (int)Direction.Down][i + X] < 400 && Form1.m.arrMap[j + Y][i + X + (int)Direction.Right] >= 400)
                                Form1.m.arrMap[j + Y][i + X] = 4802;
                            //9
                            if (Form1.m.arrMap[j + Y + (int)Direction.Up][i + X] >= 400 && Form1.m.arrMap[j + Y][i + X + (int)Direction.Left] >= 400 && Form1.m.arrMap[j + Y + (int)Direction.Down][i + X] < 400 && Form1.m.arrMap[j + Y][i + X + (int)Direction.Right] < 400)
                                Form1.m.arrMap[j + Y][i + X] = 4901;
                            //2
                            if (Form1.m.arrMap[j + Y + (int)Direction.Up][i + X] < 400 && Form1.m.arrMap[j + Y][i + X + (int)Direction.Left] < 400 && Form1.m.arrMap[j + Y + (int)Direction.Down][i + X] >= 400 && Form1.m.arrMap[j + Y][i + X + (int)Direction.Right] < 400)
                                Form1.m.arrMap[j + Y][i + X] = 4202;
                            //8
                            if (Form1.m.arrMap[j + Y + (int)Direction.Up][i + X] >= 400 && Form1.m.arrMap[j + Y][i + X + (int)Direction.Left] < 400 && Form1.m.arrMap[j + Y + (int)Direction.Down][i + X] < 400 && Form1.m.arrMap[j + Y][i + X + (int)Direction.Right] < 400)
                                Form1.m.arrMap[j + Y][i + X] = 4801;
                        }
                        catch {
                            Form1.m.arrMap[j + Y][i + X] = 4510;                          
                        }
                    }
                   
                }
        }

        public static void GenerateForest()
        {
            if (Cursor.Position.X > 384 && Cursor.Position.X < 1360 && Cursor.Position.Y > 50 && Cursor.Position.Y < 1020)
            {
                for (int i = ((Cursor.Position.X - 384) / 96) + (int)Form1.CameraX - 1; i <= ((Cursor.Position.X - 384) / 96) + (int)Form1.CameraX + 1; i++)
                    for (int j = ((Cursor.Position.Y - 57) / 96) + (int)Form1.CameraY - 1; j <= ((Cursor.Position.Y - 57) / 96) + (int)Form1.CameraY + 1; j++)
                        if (i >= 0 && i < Form1.m.width && j >= 0 && j < Form1.m.height)
                            Form1.m.arrMap[j][i] = 400;
            }
        }
        public static void DestroyForest()
        {
            for (int i = ((Cursor.Position.X - 384) / 96) + (int)Form1.CameraX; i <= ((Cursor.Position.X - 384) / 96) + (int)Form1.CameraX; i++)
                for (int j = ((Cursor.Position.Y - 57) / 96) + (int)Form1.CameraY; j <= ((Cursor.Position.Y - 57) / 96) + (int)Form1.CameraY; j++)
                {
                    if (i >= 0 && i < Form1.m.width && j >= 0 && j < Form1.m.height)
                    {
                        if (Form1.m.arrMap[j + (int)Direction.Down][i] >= 400 && Form1.m.arrMap[j + (int)Direction.Down * 2][i] >= 400)
                            Form1.m.arrMap[j][i] = 200;
                        else
                        {
                            Form1.m.arrMap[j][i] = 200;
                            Form1.m.arrMap[j + (int)Direction.Down][i] = 200;
                        }
                        if (Form1.m.arrMap[j + (int)Direction.Up][i] >= 400 && Form1.m.arrMap[j + (int)Direction.Up * 2][i] >= 400)
                            Form1.m.arrMap[j][i] = 200;
                        else
                        {
                            Form1.m.arrMap[j][i] = 200;
                            Form1.m.arrMap[j + (int)Direction.Up][i] = 200;
                        }

                        if (Form1.m.arrMap[j][i + (int)Direction.Left] >= 400 && Form1.m.arrMap[j + (int)Direction.Down][i + (int)Direction.Left] >= 400 && Form1.m.arrMap[j + (int)Direction.Down * 2][i + (int)Direction.Left] >= 400 && Form1.m.arrMap[j][i + (int)Direction.Left * 2] < 400 && Form1.m.arrMap[j + (int)Direction.Down][i] < 400
                        || Form1.m.arrMap[j][i + (int)Direction.Left] >= 400 && Form1.m.arrMap[j + (int)Direction.Up][i + (int)Direction.Left] >= 400 && Form1.m.arrMap[j + (int)Direction.Up * 2][i + (int)Direction.Left] >= 400 && Form1.m.arrMap[j][i + (int)Direction.Left * 2] < 400 && Form1.m.arrMap[j + (int)Direction.Up][i] < 400)
                            Form1.m.arrMap[j][i + (int)Direction.Left] = 200;

                        if (Form1.m.arrMap[j][i + (int)Direction.Right] >= 400 && Form1.m.arrMap[j + (int)Direction.Down][i + (int)Direction.Right] >= 400 && Form1.m.arrMap[j + (int)Direction.Down * 2][i + (int)Direction.Right] >= 400 && Form1.m.arrMap[j][i + (int)Direction.Right * 2] < 400 && Form1.m.arrMap[j + (int)Direction.Down][i] < 400
                        || Form1.m.arrMap[j][i + (int)Direction.Right] >= 400 && Form1.m.arrMap[j + (int)Direction.Up][i + (int)Direction.Right] >= 400 && Form1.m.arrMap[j + (int)Direction.Up * 2][i + (int)Direction.Right] >= 400 && Form1.m.arrMap[j][i + (int)Direction.Right * 2] < 400 && Form1.m.arrMap[j + (int)Direction.Up][i] < 400)
                            Form1.m.arrMap[j][i + (int)Direction.Right] = 200;

                        if (Form1.m.arrMap[j][i + (int)Direction.Left] >= 400 && Form1.m.arrMap[j + (int)Direction.Down][i + (int)Direction.Left] >= 400 && Form1.m.arrMap[j + (int)Direction.Up][i + (int)Direction.Left] >= 400)
                            Form1.m.arrMap[j + (int)Direction.Down][i + (int)Direction.Left] = 200;

                        if (Form1.m.arrMap[j][i + (int)Direction.Right] >= 400 && Form1.m.arrMap[j + (int)Direction.Down][i + (int)Direction.Right] >= 400 && Form1.m.arrMap[j + (int)Direction.Up][i + (int)Direction.Right] >= 400)
                            Form1.m.arrMap[j + (int)Direction.Down][i + (int)Direction.Right] = 200;
                    }
                }

        }

    }
}
