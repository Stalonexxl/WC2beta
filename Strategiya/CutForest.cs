using System.Drawing;

namespace Strategiya
{
    class CutForest
    {        
        public static Bitmap[] forest { get; private set; }      
        public static int[] ForestNum { get; private set; }
        static int g = 0;
        static Bitmap[] forestPic;
        static int forestnum = 4000;
        static CutForest()
        {
            ForestNum = new int[144];
            forestPic = new Bitmap[40];
            forest = new Bitmap[144];
            CutForestPic();
            initForestNum();
            initForest();
        }
        private static void CutForestPic()
        {
            // Вырезание спрайтов леса
            for (int i = 6; i <= 9; i++)
                for (int j = 0; j < 16; j++)
                {
                    if (i == 6 && j >= 6)
                    {
                        forestPic[g] = Properties.Resources.summer.Clone(new Rectangle(new Point(96 * j, 96 * i), new Size(96, 96)), Properties.Resources.summer.PixelFormat);
                        g++;
                    }
                    if (i == 7)
                    {
                        forestPic[g] = Properties.Resources.summer.Clone(new Rectangle(new Point(96 * j, 96 * i), new Size(96, 96)), Properties.Resources.summer.PixelFormat);
                        g++;
                    }
                    if (i == 8 && j < 14)
                    {
                        forestPic[g] = Properties.Resources.summer.Clone(new Rectangle(new Point(96 * j, 96 * i), new Size(96, 96)), Properties.Resources.summer.PixelFormat);
                        g++;
                    }
                }
        }
        private static void initForestNum()
        {
            int h = 0;
            for (int k = 0; k < 9; k++)
            {
                forestnum += 100;
                for (int u = 0; u < 16; u++)
                {
                    forestnum++;
                    ForestNum[h] = forestnum;
                    h++;
                }
                forestnum -= 16;
            }
        }
        private static void initForest()
        {
            int forst = 16;
            int h = 0;     //1
            forest[h] = forestPic[2];
            forest[++h] = forestPic[34];

            h = forst * 1; //2
            forest[h] = forestPic[4];
            forest[++h] = forestPic[19];
            forest[++h] = forestPic[32];

            h = forst * 2; //3
            forest[h] = forestPic[5];
            forest[++h] = forestPic[30];

            h = forst * 3; //4
            forest[h] = forestPic[1];
            forest[++h] = forestPic[13];
            forest[++h] = forestPic[16];
            forest[++h] = forestPic[33];

            h = forst * 4; //5
            forest[h] = forestPic[3];
            forest[++h] = forestPic[6];
            forest[++h] = forestPic[9];
            forest[++h] = forestPic[10];
            forest[++h] = forestPic[11];
            forest[++h] = forestPic[12];
            forest[++h] = forestPic[17];
            forest[++h] = forestPic[18];
            forest[++h] = forestPic[23];
            forest[++h] = forestPic[25];
            forest[++h] = forestPic[26];
            forest[++h] = forestPic[35];
            forest[++h] = forestPic[36];
            forest[++h] = forestPic[37];
            forest[++h] = forestPic[38];
            forest[++h] = forestPic[39];

            h = forst * 5; //6
            forest[h] = forestPic[7];
            forest[++h] = forestPic[14];
            forest[++h] = forestPic[15];
            forest[++h] = forestPic[31];

            h = forst * 6; //7
            forest[h] = forestPic[0];
            forest[++h] = forestPic[28];

            h = forst * 7; //8
            forest[h] = forestPic[21];
            forest[++h] = forestPic[22];
            forest[++h] = forestPic[29];

            h = forst * 8; //9
            forest[h] = forestPic[8];
            forest[++h] = forestPic[27];
        }
    }
}
