using System;
using System.Drawing;
using System.Windows.Forms;


namespace Strategiya
{
    public partial class Form1 : Form
    {
        public static Form1 formPointer;
        public static Map m;
        public static Image[] images;
        Pen whitePen;
        Pen GreenPen;
        public static double CameraX { get; set; } = 0;
        public static double CameraY { get; set; } = 0;
        Rectangle rectDrag;
        bool isDrag = false;
        Point startDrag;
        Point finalDrag;
        int DragX;
        int DragY;
        int DragH;
        int DragW;
        Rectangle rectUnit;
        Client client;
        string MyName;
        public Form1()
        {
            InitializeComponent();
            //var img = new Bitmap(Strategiya.Properties.Resources.orcish_claw);
            //Icon icon = Icon.FromHandle(img.GetHicon());
            //Cursor cur = new Cursor(icon.Handle);
            //this.Cursor = cur;
            formPointer = this;

            timer1.Interval = 10;
            timer1.Tick += new EventHandler(update);
            timer1.Start();           

            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            MyName = Environment.MachineName;
            client = new Client(MyName);
          


            m = new Map("D:\\WC2\\MAPS\\01");

            images = new Image[5];
            images[0] = Properties.Resources.summer.Clone(new Rectangle(new Point(96 * 0, 96 * 12), new Size(96, 96)), Properties.Resources.summer.PixelFormat);
            images[1] = Properties.Resources.summer.Clone(new Rectangle(new Point(96 * 0, 96 * 15), new Size(96, 96)), Properties.Resources.summer.PixelFormat);
            images[2] = Properties.Resources.summer.Clone(new Rectangle(new Point(96 * 0, 96 * 19), new Size(96, 96)), Properties.Resources.summer.PixelFormat);
            images[3] = Properties.Resources.summer.Clone(new Rectangle(new Point(96 * 0, 96 * 7), new Size(96, 96)), Properties.Resources.summer.PixelFormat);
            images[4] = Properties.Resources.summer.Clone(new Rectangle(new Point(96 * 0, 96 * 11), new Size(96, 96)), Properties.Resources.summer.PixelFormat);
            whitePen = new Pen(Color.White, 3);
            GreenPen = new Pen(Color.Green, 3);
            //ClientSize = new Size(m.width * size, m.height * size);          
        }

        private void update(object sender, EventArgs e)
        {
            CheckContols();
            Invalidate();
            foreach (Unit unit in GameMehanic.units)
                unit.MoveUnit();
        }

        public void ClientMessage(string mes)
        {
            try
            {
                string[] commands = mes.Split(';');
                foreach (Unit unit in GameMehanic.units)
                {
                    if (unit.Id.ToString() == commands[1])
                    {
                        unit.pointUnit = new Point(int.Parse(commands[2]), int.Parse(commands[3]));
                        unit.PathUnit(unit.pointUnit);
                    }
                }
            }
            catch
            {

            }
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(Properties.Resources.buttonpanel, new Point(0, 0));
            e.Graphics.DrawImage(Properties.Resources.filler_right, new Point(1344, 0));
            e.Graphics.DrawImage(Properties.Resources.resource, new Point(384, 0));
            e.Graphics.DrawImage(Properties.Resources.statusline, new Point(384, 992));
            new MiniMap(e);
            new Forest();
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                {                  
                    if (m.arrMap[j + (int)CameraY][i + (int)CameraX] == 100)
                        e.Graphics.DrawImage(images[0], 384 + 96 * i, 32 + 96 * j);
                    if (m.arrMap[j + (int)CameraY][i + (int)CameraX] == 200)
                        e.Graphics.DrawImage(images[1], 384 + 96 * i, 32 + 96 * j);
                    if (m.arrMap[j + (int)CameraY][i + (int)CameraX] == 300)
                        e.Graphics.DrawImage(images[2], 384 + 96 * i, 32 + 96 * j);
                    foreach (int h in CutForest.ForestNum)
                        if (m.arrMap[j + (int)CameraY][i + (int)CameraX] == h)
                            e.Graphics.DrawImage(CutForest.forest[Array.IndexOf(CutForest.ForestNum, h)], 384 + 96 * i, 32 + 96 * j);
                    foreach (Building build in GameMehanic.buildings)
                    {
                        if (build.Position.X == i + (int)CameraX && build.Position.Y == j + (int)CameraY)
                        {
                            build.Part = BuildingPart.part1;
                            e.Graphics.DrawImage(build.Picture, 384 + 96 * i, 32 + 96 * j);
                        }
                        if (build.Position1.X == i + (int)CameraX && build.Position1.Y == j + (int)CameraY)
                        {
                            build.Part = BuildingPart.part3;
                            e.Graphics.DrawImage(build.Picture, 384 + 96 * i, 32 + 96 * j);
                        }
                        if (build.Position2.X == i + (int)CameraX && build.Position2.Y == j + (int)CameraY)
                        {
                            build.Part = BuildingPart.part2;
                            e.Graphics.DrawImage(build.Picture, 384 + 96 * i, 32 + 96 * j);
                        }
                        if (build.Position3.X == i + (int)CameraX && build.Position3.Y == j + (int)CameraY)
                        {
                            build.Part = BuildingPart.part4;
                            e.Graphics.DrawImage(build.Picture, 384 + 96 * i, 32 + 96 * j);
                        }
                    }                    
                }
            foreach(Unit units in GameMehanic.cemetery)
                if (DrawUnits(units))
                    e.Graphics.DrawImage(units.Picture, new Point(384 + 96 * units.Position.X - (int)CameraX * 96, 32 + 96 * units.Position.Y - (int)CameraY * 96));

            foreach (Unit units in GameMehanic.units)
                if (DrawUnits(units))
                {
                    if (GameMehanic.legion.Contains(units))
                    {
                        rectUnit = new Rectangle(384 + 96 * units.Position.X - (int)CameraX * 96 + units.OffsetX, 32 + 96 * units.Position.Y - (int)CameraY * 96 + units.OffsetY, 93, 93);
                        e.Graphics.DrawRectangle(GreenPen, rectUnit);
                    }
                    e.Graphics.DrawImage(units.Picture, new Point(384 + 96 * units.Position.X - (int)CameraX * 96 + units.OffsetX, 32 + 96 * units.Position.Y - (int)CameraY * 96 + units.OffsetY));       
                }
            foreach (Building build in GameMehanic.buildings)
                build.healthincreace();
            // Отрисовка большого белого квадрата на карте
            if (Cursor.Position.X > 384 && Cursor.Position.X < 1360 && Cursor.Position.Y > 50 && Cursor.Position.Y < 1020)
            {
                Rectangle rectCamera = new Rectangle(((Cursor.Position.X - 384) / 96) * 96 + 384, ((Cursor.Position.Y - 57) / 96) * 96 + 32, 96, 96);
                //e.Graphics.DrawRectangle(whitePen, rectCamera);
            }
               
            if (isDrag)
            {
                if (Cursor.Position.X > 384 && Cursor.Position.X < 1360 && Cursor.Position.Y > 50 && Cursor.Position.Y < 1020)
                {                   
                    int CurX = Cursor.Position.X;
                    int CurY = Cursor.Position.Y;
                    finalDrag = new Point(Math.Abs(CurX - startDrag.X), Math.Abs(CurY - startDrag.Y - 23));
                    DragX = ((rectDrag.X - 384) / 96) + (int)CameraX;
                    DragW = ((rectDrag.Width) / 96) + DragX;
                    DragY = ((rectDrag.Y - 29) / 96) + (int)CameraY;
                    DragH = (rectDrag.Height / 96) + DragY;
                    if (startDrag.X <= CurX && startDrag.Y <= CurY - 27)
                        rectDrag = new Rectangle(startDrag.X, startDrag.Y, finalDrag.X, finalDrag.Y);
                    else if (startDrag.X >= CurX && startDrag.Y <= CurY - 27)
                        rectDrag = new Rectangle(CurX, startDrag.Y, startDrag.X - CurX, finalDrag.Y);
                    else if (startDrag.X <= CurX && startDrag.Y >= CurY - 27)
                        rectDrag = new Rectangle(startDrag.X, CurY - 25, finalDrag.X, startDrag.Y - CurY + 27);
                    else if (startDrag.X >= CurX && startDrag.Y >= CurY - 27)
                        rectDrag = new Rectangle(CurX, CurY - 25, startDrag.X - CurX, startDrag.Y - CurY + 27);
                    
                    e.Graphics.DrawRectangle(whitePen, rectDrag);                    
                }
            }
        }

        private void Form1_Click(object sender, MouseEventArgs e)
        {
            // Отображение координат
            labelxc.Text = "Xc=" + Cursor.Position.X.ToString();
            labelyc.Text = "Yc=" + Cursor.Position.Y.ToString();

            switch (e.Button.ToString())
            {
                case "Right":
                        foreach (Unit unit in GameMehanic.units)
                        {
                            if (GameMehanic.legion.Contains(unit))
                            {
                                unit.WantChangePath = true;
                                unit.pointUnit = new Point(((Cursor.Position.X - 384) / 96) + (int)CameraX, ((Cursor.Position.Y - 57) / 96) + (int)CameraY);
                                client.SendMessage(unit.Id.ToString() + ";" + unit.pointUnit.X.ToString() + ";" + unit.pointUnit.Y.ToString());
                                unit.TimerStart();
                                if (!unit.isMooving)
                                    unit.PathUnit(unit.pointUnit);
                            }
                        }
                        
                    break;
                case "Left":
                    // НОВЫЙ ЛЕС
                    //if (Cursor.Position.X > 384 && Cursor.Position.X < 1360 && Cursor.Position.Y > 50 && Cursor.Position.Y < 1020)
                        //if (CheckPutObj(((Cursor.Position.X - 384) / 96) + (int)CameraX, ((Cursor.Position.Y - 57) / 96) + (int)CameraY))
                            //Forest.GenerateForest();

                    // Левая кнопка мыши на миникарте.
                    if (Cursor.Position.X > 30 && Cursor.Position.Y > 53 && Cursor.Position.X < 330 && Cursor.Position.Y < 353)
                    {
                        CameraX = ((Cursor.Position.X - 30) / MiniMap.rectSize) - 5;
                        CameraY = ((Cursor.Position.Y - 53) / MiniMap.rectSize) - 5;
                        if (CameraX >= m.width - 10)
                            CameraX = m.width - 10;
                        if (CameraX <= 0)
                            CameraX = 0;
                        if (CameraY >= m.height - 10)
                            CameraY = m.height - 10;
                        if (CameraY <= 0)
                            CameraY = 0;
                    }
                    break;
            }
        }

        public static bool DrawUnits(GameObjects unit)
        {
            Point p = new Point(unit.Position.X, unit.Position.Y);
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                    if (j + (int)CameraY == p.Y && i + (int)CameraX == p.X)
                        return true;
            return false;
        }

        private void CheckContols()
        {
            if (Cursor.Position.X > 1360)
                if (CameraX <= m.width - 10)
                    CameraX += 0.25;
            if (Cursor.Position.X < 15)
                if (CameraX >= 0)
                    CameraX -= 0.25;
            if (Cursor.Position.Y > 1020)
                if (CameraY <= m.height - 10)
                    CameraY += 0.25;
            if (Cursor.Position.Y < 50)
                if (CameraY >= 0)
                    CameraY -= 0.25;
        }

        private void Form_key_press(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == "E") 
            {
                // Постройка зданий
                if (Cursor.Position.X > 384 && Cursor.Position.X < 1360 && Cursor.Position.Y > 50 && Cursor.Position.Y < 1020)
                    if (GameMehanic.CheckPutObj(((Cursor.Position.X - 384) / 96) + (int)CameraX, ((Cursor.Position.Y - 57) / 96) + (int)CameraY))
                        GameMehanic.buildings.Add(new Building(new Point(((Cursor.Position.X - 384) / 96) + (int)CameraX, ((Cursor.Position.Y - 57) / 96) + (int)CameraY)));
                // Вырубка леса
                if (Cursor.Position.X > 384 && Cursor.Position.X < 1360 && Cursor.Position.Y > 50 && Cursor.Position.Y < 1020)                   
                    Forest.DestroyForest();
            }
        }

        private void Form1_Click123(object sender, MouseEventArgs e)
        {
            if (e.Button.ToString() == "Left")
            {
                GameMehanic.legion.Clear();
                foreach (Unit Grunt in GameMehanic.units)
                    Grunt.addInLegion = false;
                isDrag = true;
                startDrag.X = Cursor.Position.X;
                startDrag.Y = Cursor.Position.Y - 27;
            }
        }

        private void Form1_Click321(object sender, MouseEventArgs e)
        {
            if (e.Button.ToString() == "Left")
            {
                foreach (Unit unit in GameMehanic.units)
                {
                    if (unit.Position.X >= DragX && unit.Position.X <= DragW && unit.Position.Y >= DragY && unit.Position.Y <= DragH) 
                        
                            if (!unit.addInLegion)
                            {
                                GameMehanic.legion.Add(unit);
                                unit.addInLegion = true;
                            }
                }
                isDrag = false;
            }                
        }
        public void _Log(string s)
        {
            tbLog.Text = tbLog.Text + s + Environment.NewLine;
        }
    }
}
