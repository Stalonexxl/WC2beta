using System;
using System.Collections.Generic;
using System.Drawing;

namespace Strategiya
{
    static class GameMehanic
    {
        public static List<Unit> legion;
        public static List<Unit> units;
        public static List<Building> buildings;
        public static List<Unit> cemetery;
        static GameMehanic()
        {
            //Добавление юнитов
            buildings = new List<Building>();
            legion = new List<Unit>();
            cemetery = new List<Unit>();
            units = new List<Unit>();
            units.Add(new GruntOrc(new Point(1, 1)));
            units.Add(new GruntOrc(new Point(2, 1)));
            units.Add(new GruntOrc(new Point(3, 1)));
            units.Add(new GruntOrc(new Point(4, 1)));
            units.Add(new GruntOrc(new Point(5, 1)));
            units.Add(new Ogre(new Point(10, 3)));
            units.Add(new Ogre(new Point(11, 3)));
            units.Add(new Ogre(new Point(12, 3)));
            foreach (Unit Grunt in units)
            {
                Grunt.NotifyMove += new Unit.DelegatHandler(StopMoving);
                Grunt.NotifyFight += new Unit.DelegatHandler2(Fight);
            }
        }

        public static Unit Fight(Unit unit)
        {
            foreach (Unit enemy in units)
            {
                if (enemy.Id == unit.Id)
                    continue;
                if (enemy.fraction != unit.fraction)
                {
                    if (unit.pointUnit.X == enemy.Position.X && unit.pointUnit.Y == enemy.Position.Y)
                        return enemy;
                    if (unit.pointUnit.X == enemy.pointUnit.X && unit.pointUnit.Y == enemy.pointUnit.Y)
                        return enemy;
                }
            }
            return null;
        }

        private static bool StopMoving(Unit gr)
        {
            foreach (Unit Grunt in units)
            {
                if (gr.Id == Grunt.Id || Grunt.Path == null || Grunt.Path.Count == 0)
                    continue;
                try
                {
                    if (gr.Path[gr.NextStep] == Grunt.Path[Grunt.NextStep])
                        return true;
                    if (gr.Path[gr.NextStep].X == Grunt.Position.X && gr.Path[gr.NextStep].Y == Grunt.Position.Y)
                        return true;
                }
                catch (ArgumentOutOfRangeException)
                {
                    Form1.formPointer._Log("Индекс вышел за границы(NotifyMove)");
                }
            }
            return false;
        }

        public static bool CheckPutObj(int x, int y)
        {
            Point positionOb = new Point(x, y);

            foreach (Building build in buildings)
            {
                if (build.Position.X == positionOb.X && build.Position.Y == positionOb.Y || build.Position1.X == positionOb.X && build.Position1.Y == positionOb.Y ||
                    build.Position2.X == positionOb.X && build.Position2.Y == positionOb.Y || build.Position3.X == positionOb.X && build.Position3.Y == positionOb.Y ||
                    build.Position.X == positionOb.X && build.Position.Y - 1 == positionOb.Y || build.Position1.X == positionOb.X && build.Position1.Y - 1 == positionOb.Y ||
                    build.Position.Y == positionOb.Y && build.Position.X - 1 == positionOb.X || build.Position2.Y == positionOb.Y && build.Position2.X - 1 == positionOb.X ||
                    build.Position.Y - 1 == positionOb.Y && build.Position.X - 1 == positionOb.X)
                    return false;
            }
            return true;
        }
    }
}
