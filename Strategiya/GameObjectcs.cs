using System.Drawing;

namespace Strategiya
{
    public interface GameObjects
    {
        Bitmap Picture { get; }
        int OffsetX { get; }
        int OffsetY { get; }
        Point Position { get; }
        int health { get; set; }
        string fraction { get; }
        void Destroy();
    }
    interface Unitss
    {       
        int attackPower { get; }
        void attack();      
    }
}
