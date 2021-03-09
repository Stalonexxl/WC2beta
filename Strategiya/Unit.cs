using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Timers;

namespace Strategiya
{
    public enum DirectionUnit
    {
        Down = 4, Left = 5, Up = 0, Right = 2, UpLeft = 6, UpRight = 1, DownLeft = 7, DownRight = 3, None = 8
    }
    public class Unit : GameObjects, Unitss
    {
        public delegate bool DelegatHandler(Unit obj);
        public delegate Unit DelegatHandler2(Unit obj);
        public event DelegatHandler Notify;
        public event DelegatHandler2 NotifyFight;
        public Bitmap Picture { get => Sprite[(int)currAnimation, currentDirection == DirectionUnit.None ? 8 : (int)currentDirection]; }
        public int OffsetX { get; set; }
        public int OffsetY { get; set; }
        public Point Position { get => position;}
        public int health { get;  set; }
        public int attackPower { get; protected set; }
        public string fraction { get; protected set; }
        protected DirectionUnit currentDirection;
        public int Id { get; protected set; }
        public int NextStep { get => nextStep; }
        public List<Point> Path { get => path; }

        System.Windows.Forms.Timer timer;
        protected Bitmap[,] Sprite;
        int[,] arrM;
        protected double currAnimation = 0;
        protected Point position;
        List<Point> path;
        int nextStep;
        protected bool isNextStep;
        bool isW8Step = false;
        Point nextPosition;
        public bool addInLegion;
        protected int size = 96;
        protected int size2 = 144;
        protected static int counter = 0;
        Point pointSave;
        public bool isPressed = false;
        public Unit()
        {
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 10;
            timer.Tick += new EventHandler(CanAttack);
        }
        public void PathUnit(Point pointUnit)
        {
            pointSave = pointUnit;
            arrM = new int[Form1.m.width,Form1.m.height];
            for (int i = 0; i < Form1.m.width; i++)
                for (int j = 0; j < Form1.m.height; j++)
                    arrM[i,j] = Form1.m.arrMap[i][j];
            path = PathNode.FindPath(arrM, position, pointSave, this);
            nextStep = 0;
            isNextStep = true;
            isW8Step = false;
            TimerStart();
        }

        public void MoveUnit()
        {
            if (OffsetX != 0 || OffsetY != 0)
            {
                Form1.formPointer._Log(OffsetX.ToString() + " " + OffsetY.ToString());
            }
            if (path != null)
            {        
                if (isNextStep)
                {
                    try
                    {
                        nextPosition.X = path[nextStep].X;
                        nextPosition.Y = path[nextStep].Y;
                        if (nextPosition.X - position.X == -1 && nextPosition.Y - position.Y == 1)
                            Move(DirectionUnit.DownLeft);
                        else if (nextPosition.X - position.X == -1 && nextPosition.Y - position.Y == -1)
                            Move(DirectionUnit.UpLeft);
                        else if (nextPosition.X - position.X == 1 && nextPosition.Y - position.Y == 1)
                            Move(DirectionUnit.DownRight);
                        else if (nextPosition.X - position.X == 1 && nextPosition.Y - position.Y == -1)
                            Move(DirectionUnit.UpRight);
                        else if (nextPosition.X - position.X == -1)
                            Move(DirectionUnit.Left);
                        else if (nextPosition.X - position.X == 1)
                            Move(DirectionUnit.Right);
                        else if (nextPosition.Y - position.Y == -1)
                            Move(DirectionUnit.Up);
                        else if (nextPosition.Y - position.Y == 1)
                            Move(DirectionUnit.Down);

                        UnitTimer();
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        Form1.formPointer._Log("Индекс вышел за границы");
                    }
                }
                if (isW8Step)
                {
                    if (!Notify.Invoke(this))
                    {
                        isW8Step = false;
                        isNextStep = true;
                    }         
                }
            }
        }

        private void UnitTimer()
        {
            if (currAnimation == 4)
                currAnimation = 0;
            switch (currentDirection)
            {
                case DirectionUnit.Down:
                    OffsetY += 8;
                    break;
                case DirectionUnit.Left:
                    OffsetX -= 8;
                    break;
                case DirectionUnit.Up:
                    OffsetY -= 8;
                    break;
                case DirectionUnit.Right:
                    OffsetX += 8;
                    break;
                case DirectionUnit.UpRight:
                    OffsetX += 8;
                    OffsetY -= 8;
                    break;
                case DirectionUnit.DownRight:
                    OffsetX += 8;
                    OffsetY += 8;
                    break;
                case DirectionUnit.UpLeft:
                    OffsetX -= 8;
                    OffsetY -= 8;
                    break;
                case DirectionUnit.DownLeft:
                    OffsetX -= 8;
                    OffsetY += 8;
                    break;
            }
            if (OffsetX < -96 || OffsetX > 96 || OffsetY < -96 || OffsetY > 96)
            {
                currentDirection = DirectionUnit.None;
                OffsetX = 0;
                OffsetY = 0;
            }
            if (OffsetX == 0 && OffsetY == 0)
            {
                if (path.Count - 1 == nextStep)
                {
                    currentDirection = DirectionUnit.None;
                    isNextStep = false;
                    Form1.formPointer._Log("Стоп");
                }
                else nextStep++;

                if (Notify.Invoke(this))
                {
                    isNextStep = false;
                    isW8Step = true;
                    currentDirection = DirectionUnit.None;
                }
                if(nextStep == 2)
                {
                    isNextStep = false;
                    isW8Step = false;
                    currentDirection = DirectionUnit.None;
                    PathUnit(pointSave);
                }
            }
            currAnimation += 0.5;
        }

        public void Move(DirectionUnit direction)
        {
            switch (direction)
            {
                case DirectionUnit.Down:
                    OffsetY = -96;
                    position.Y++;
                    break;
                case DirectionUnit.Left:
                    OffsetX = 96;
                    position.X--;
                    break;
                case DirectionUnit.Up:
                    OffsetY = 96;
                    position.Y--;
                    break;
                case DirectionUnit.Right:
                    OffsetX = -96;
                    position.X++;
                    break;
                case DirectionUnit.UpRight:
                    OffsetX = -96;
                    OffsetY = 96;
                    position.X++;
                    position.Y--;
                    break;
                case DirectionUnit.DownRight:
                    OffsetX = -96;
                    OffsetY = -96;
                    position.X++;
                    position.Y++;
                    break;
                case DirectionUnit.UpLeft:
                    OffsetX = 96;
                    OffsetY = 96;
                    position.X--;
                    position.Y--;
                    break;
                case DirectionUnit.DownLeft:
                    OffsetX = 96;
                    OffsetY = -96;
                    position.X--;
                    position.Y++;
                    break;
            }
            currentDirection = direction;
        }

        public void Destroy()
        {
            if(health <= 0)
                Form1.formPointer.units.Remove(this);
        }

        public void TimerStart()
        {
            if (NotifyFight?.Invoke(this) != null)
                timer.Start();
        }

        public void CanAttack(object sender, EventArgs e)
        {
            Unit enemy = NotifyFight?.Invoke(this);
            if (enemy != null)
            {
                if (Math.Abs(Position.X - enemy.Position.X) == 1 && Math.Abs(Position.Y - enemy.Position.Y) == 1)
                    attack(enemy);
            }         
        }
        public void attack(Unit enemy)
        {
            Form1.formPointer._Log("1");
            enemy.health -= attackPower;
            timer.Stop();
        }
    }
}