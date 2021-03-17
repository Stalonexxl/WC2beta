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
        protected DirectionUnit pastDirection;
        public int Id { get; protected set; }
        public int NextStep { get => nextStep; }
        public List<Point> Path { get => path; }
        public Point pointUnit { get; set; }

        System.Windows.Forms.Timer timer;
        System.Windows.Forms.Timer timerDie;
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
        protected bool isAttack = false;
        Point pointSave;
        public bool WantChangePath = false;
        public bool isMooving = false;
        public bool isHarassment = false;
        public Unit()
        {
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 150;
            timer.Tick += new EventHandler(CanAttack);
            timerDie = new System.Windows.Forms.Timer();
            timerDie.Interval = 120;
            timerDie.Tick += new EventHandler(СheckDestroy);
            timerDie.Start();
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
            currAnimation = 0;
            if (isAttack)
                currentDirection = DirectionUnit.None;
            isNextStep = true;
            isW8Step = false;
            isAttack = false;
            isMooving = true;
            TimerStart();           
        }

        public void MoveUnit()
        {
            if (path != null)
            {        
                if (isNextStep)
                {
                    try
                    {
                        //Form1.formPointer._Log("MoveUnit " + nextStep.ToString());
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
                    nextStep = 1;
                    if (WantChangePath)
                    {
                        PathUnit(pointUnit);
                        WantChangePath = false;
                    }
                    //Form1.formPointer._Log("isW8Step");
                    if (!Notify.Invoke(this))
                    {
                        //Form1.formPointer._Log("isW8Step = false");
                        isW8Step = false;
                        isNextStep = true;
                    }
                }
            }
        }

        private void UnitTimer()
        {
            //Form1.formPointer._Log("UnitTimer " + OffsetX.ToString() + " " + OffsetY.ToString());
            if (currAnimation == 4)
                currAnimation = 0;
            if (nextStep != 0)
            {
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
            }
            if (OffsetX < -96 || OffsetX > 96 || OffsetY < -96 || OffsetY > 96)
            {
                currentDirection = DirectionUnit.None;
                OffsetX = 0;
                OffsetY = 0;
            }
            if (OffsetX == 0 && OffsetY == 0)
            {
                if (WantChangePath)
                {
                    PathUnit(pointUnit);
                    WantChangePath = false;
                }
                if (path.Count - 1 == nextStep)
                {
                    currentDirection = DirectionUnit.None;
                    isNextStep = false;
                    isMooving = false;
                }
                else nextStep++;

                if (Notify.Invoke(this))
                {
                    isNextStep = false;
                    isW8Step = true;
                    currentDirection = DirectionUnit.None;
                }
                else if (nextStep == 2)
                    PathUnit(pointSave);
            }
            if (!isAttack)
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
            pastDirection = currentDirection;
        }

        private void СheckDestroy(object sender, EventArgs e)
        {
            if(health <= 0)              
                Destroy();
        }

        public virtual void Destroy(){}

        private Unit enemy;
        private void TimerStart()
        {
            Form1.formPointer._Log("TimerStart" + Id.ToString());
            if (NotifyFight?.Invoke(this) != null)
            {
                Form1.formPointer._Log("enemy" + Id.ToString());
                enemy = NotifyFight?.Invoke(this);
                timer.Start();
            }
            else 
            {
                Form1.formPointer._Log("NO enemy" + Id.ToString());
                enemy = null; 
            }
        }

        private void CanAttack(object sender, EventArgs e)
        {
            harassmentMethod();
            if (currAnimation == 8)
            {
                Form1.formPointer._Log("attack" + Id.ToString());
                enemy.health -= attackPower;
                currAnimation = 5;
            }
            if (enemy != null && OffsetX == 0 && OffsetY == 0)
            {
                if (!isAttack)
                {
                    for (int i = enemy.Position.X - 1; i <= enemy.Position.X + 1; i++)
                        for (int j = enemy.Position.Y - 1; j <= enemy.Position.Y + 1; j++)
                            if (Position.X == i && Position.Y == j)
                                isAttack = true;
                    if(isAttack)
                        attack(enemy);
                    else if(!isMooving)
                        isHarassment = true;
                }
            }
        }
        public void harassmentMethod()
        {
            if (isHarassment && enemy != null)
            {
                Point harassment = enemy.pointUnit;
                Form1.formPointer._Log(harassment.ToString() + Id.ToString());
                Form1.formPointer._Log(enemy.Position.X.ToString() + " " + enemy.Position.Y.ToString() + enemy.ToString());
                PathUnit(harassment);
                timer.Stop();
                isHarassment = false;
            }
        }

        public void attack(Unit enemy)
        {
            if(isAttack)
            {
                //Form1.formPointer._Log("isAttack");
                currentDirection = chooseDirectionOnAttack(enemy);
                if (currAnimation < 5)
                    currAnimation = 5;  
                currAnimation += 0.5;
                isAttack = false;
                if (enemy == null || enemy.health < 0)
                {
                    Form1.formPointer._Log("STOP Attack");
                    timer.Stop();
                    currAnimation = 0;
                    currentDirection = DirectionUnit.None;
                }
            }
        }

        private DirectionUnit chooseDirectionOnAttack(Unit enemy)
        {
            if(enemy.Position.X - Position.X == 1 && enemy.Position.Y - Position.Y == 1)
                    return DirectionUnit.DownRight;
            if(enemy.Position.X - Position.X == 0 && enemy.Position.Y - Position.Y == 1)
                    return DirectionUnit.Down;
            if(enemy.Position.X - Position.X == -1 && enemy.Position.Y - Position.Y == 1)
                    return DirectionUnit.DownLeft;
            if(enemy.Position.X - Position.X == 1 && enemy.Position.Y - Position.Y == 0)
                    return DirectionUnit.Right;
            if(enemy.Position.X - Position.X == -1 && enemy.Position.Y - Position.Y == 0)
                    return DirectionUnit.Left;
            if(enemy.Position.X - Position.X == 1 && enemy.Position.Y - Position.Y == -1)
                    return DirectionUnit.UpRight;
            if(enemy.Position.X - Position.X == 0 && enemy.Position.Y - Position.Y == -1)
                    return DirectionUnit.Up;
            if(enemy.Position.X - Position.X == -1 && enemy.Position.Y - Position.Y == -1)
                    return DirectionUnit.UpLeft;
            return DirectionUnit.None;
        }
    }
}