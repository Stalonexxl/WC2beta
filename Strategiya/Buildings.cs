using System.Drawing;
using System.Timers;
using System.Windows.Forms;

namespace Strategiya
{

    public enum BuildingState
    {
        Starting = 2, Сompleted = 0
    }
    public enum BuildingPart
    {
        part1 = 0, part2 = 1, part3 = 2, part4 = 3
    }
    public class Building : GameObjects
    {
        public BuildingState States { get; set; }
        public BuildingPart Part { get; set; }
        public Bitmap Picture { get => sprite[(int)Part]; }
        public int OffsetX { get => 0;}
        public int OffsetY { get => 0;}
        public Point Position { get; private set; }
        public Point Position1 { get; private set; }
        public Point Position2 { get; private set; }
        public Point Position3 { get; private set; }
        public int health { get; set; }
        public string fraction { get; protected set; }

        Bitmap[] sprite;
        int size = 96;
        int partbuild = 0;
        public bool firstBuild;
        public Building(Point location)
        {
            States = BuildingState.Starting;
            firstBuild = true;
            fraction = "horde";
            Position = location;
            Position1 = new Point(Position.X + 1, Position.Y);
            Position2 = new Point(Position.X, Position.Y + 1);
            Position3 = new Point(Position.X + 1, Position.Y + 1);
            sprite = new Bitmap[4];
            BuildTimer_Elapsed();

        }

        public void BuildTimer_Elapsed()
        {
            partbuild = 0;
            for (int i = 0; i < Properties.Resources.altar_of_storms.Width / size; i++)
                for (int j = 0; j < Properties.Resources.altar_of_storms.Width / size; j++)
                    sprite[partbuild++] = Properties.Resources.altar_of_storms.Clone(new Rectangle(new Point(size * i, size * (j + (int)States)), new Size(size, size)), Properties.Resources.altar_of_storms.PixelFormat);
        }

        public void healthincreace()
        {
            if (this.health <= 100)
                this.health++;
            if (this.health == 100 && this.firstBuild)
            {
                this.States = BuildingState.Сompleted;
                this.BuildTimer_Elapsed();
                this.firstBuild = false;
            }
        }

        public void Destroy()
        {

        }
    }
}
