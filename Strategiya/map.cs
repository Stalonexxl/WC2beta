using System;
using System.Collections.Generic;
using System.IO;

namespace Strategiya
{
    public class Map
    {
        public string path { get; private set; }
        public List<List<int>> arrMap = new List<List<int>>();

        public int width { get; private set; }
        public int height { get; private set; }

        int gold;
        int wood;
        int oil;

        public Map(string path)
        {
            this.path = path;
            string[] readMap = File.ReadAllLines(this.path + "\\map.mft");

            string[] line = readMap[0].Split(',');
            
            width = Convert.ToInt32(line[0]);
            height = Convert.ToInt32(line[1]);
            gold = Convert.ToInt32(line[2]);
            wood = Convert.ToInt32(line[3]);
            oil = Convert.ToInt32(line[4]);
            InitMap();
        }
        
        private void InitMap()
        {
            int i = 0,  j = 0;
            string[] mapfile = File.ReadAllLines(this.path + "\\map.ter");
            foreach (string n in mapfile)
            {
                j = 0;
                string[] lines = n.Split(',');
                arrMap.Add(new List<int>());
                if (i >= width) continue;
                foreach (string l in lines)
                {     
                   if (j >= height) continue;
                   arrMap[i].Add(Convert.ToInt32(l));
                   j++;
                }
                i++;
            }
        }
    }
}
