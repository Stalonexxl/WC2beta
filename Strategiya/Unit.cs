using System;
using System.Drawing;
using System.Collections.Generic;

namespace Strategiya
{
    public enum DirectionUnit
    {
        Down = 4, Left = 5, Up = 0, Right = 2, UpLeft = 6, UpRight = 1, DownLeft = 7, DownRight = 3, None = 8
    }
    public class Unit : GameObjects, Unitss
    {
        public Bitmap Picture {get{try{return Sprite[(int)currAnimation, currentDirection == DirectionUnit.None ? 8 : (int)currentDirection];}catch(ArgumentOutOfRangeException){return Sprite[0,8]; }}}
        public int OffsetX { get; set; }
        public int OffsetY { get; set; }
        public Point Position { get => position; }
        public int health { get; set; }
        public int attackPower { get; protected set; }
        public string fraction { get; protected set; }
        protected DirectionUnit currentDirection;
        protected DirectionUnit pastDirection;
        public int Id { get; protected set; }
        public int NextStep { get => nextStep; }
        public List<Point> Path { get => path; }
        public Point pointUnit { get; set; }
        protected Point respawnPoint;
        protected System.Windows.Forms.Timer timer;
        protected System.Windows.Forms.Timer timerDie;
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
        public bool isDead = false;
        Point pointSave;
        public bool WantChangePath = false;
        public bool isMooving = false;
        private int CountIsMooving = 0;
        public Unit()
        {
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 100;
            timer.Tick += new EventHandler(TimerAttack);
            timerDie = new System.Windows.Forms.Timer();
            timerDie.Interval = 100;
            timerDie.Tick += new EventHandler(СheckDestroy);
            timerDie.Start();
        }
        public void PathUnit(Point pointUnit)
        {
            pointSave = pointUnit;
            arrM = new int[Form1.m.width, Form1.m.height];
            for (int i = 0; i < Form1.m.width; i++)
                for (int j = 0; j < Form1.m.height; j++)
                    arrM[i, j] = Form1.m.arrMap[i][j];
            path = PathNode.FindPath(arrM, position, pointSave, this);
            nextStep = 0;
            currAnimation = 0;
            isNextStep = true;
            isW8Step = false;
            isAttack = false;
            isMooving = true;
        }

        public void MoveUnit()
        {
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
                        CountIsMooving++;
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
                    if (!GameMehanic.StopMoving(this))
                    {
                        Form1.formPointer._Log("W8");
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
            if (OffsetX < -96 || OffsetX > 96 || OffsetY < -96 || OffsetY > 96 || CountIsMooving == 100)
            {
                currentDirection = DirectionUnit.None;
                OffsetX = 0;
                OffsetY = 0;
                CountIsMooving = 0;
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
                    CountIsMooving = 0;
                }
                else nextStep++;

                if (GameMehanic.StopMoving(this))
                {
                    isNextStep = false;
                    isW8Step = true;
                    currentDirection = DirectionUnit.None;
                }
                else if (nextStep == 2)
                    PathUnit(pointSave);
            }
            if (isMooving)
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
            if (health <= 0 && OffsetX == 0 && OffsetY == 0)
                Destroy();
        }
        public virtual void Destroy() { }

        public Unit enemy { get; set; }
        public void FightTimerStart(Unit Fenemy)
        {
            enemy = Fenemy;
            if (enemy != null)
                timer.Start();
            else
                timer.Stop();
        }
        protected void TakeDamage(Unit Fenemy)
        {
            if (enemy == null)
            {
                enemy = Fenemy;
                timer.Start();
            }
        }
        protected virtual void TimerAttack(object sender, EventArgs e)
        {
            if (enemy == null || enemy.health <= 0)
            {
                currAnimation = 0;
                currentDirection = DirectionUnit.None;
                timer.Stop();
                enemy = null;
                return;
            }
            if (currAnimation == 8)
            {
                enemy.health -= attackPower;
                enemy.TakeDamage(this);
                currAnimation = 5;
            }
            if (OffsetX == 0 && OffsetY == 0 && !isMooving && !isDead && enemy != null && !enemy.isDead)
            {
                for (int i = enemy.Position.X - 1; i <= enemy.Position.X + 1; i++)
                    for (int j = enemy.Position.Y - 1; j <= enemy.Position.Y + 1; j++)
                        if (Position.X == i && Position.Y == j)
                            isAttack = true;
                if (isAttack)
                    attack();
                else
                {
                    if (enemy.WantChangePath)
                    {
                        WantChangePath = true;
                        pointUnit = enemy.pointSave;
                    }
                    else
                    pointUnit = enemy.pointUnit;
                    if (!isMooving && !isDead)
                        PathUnit(pointUnit);
                }
            }
        }

        public virtual void attack()
        {
            currentDirection = chooseDirectionOnAttack();
            if (currAnimation < 5)
                currAnimation = 5;
            currAnimation += 0.5;
            isAttack = false;
        }

        private DirectionUnit chooseDirectionOnAttack()
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