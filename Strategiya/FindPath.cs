using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Collections.ObjectModel;

namespace Strategiya
{
    public class PathNode
    {
        // Координаты точки на карте.
        public Point Position { get; set; }
        // Длина пути от старта (G).
        public int PathLengthFromStart { get; set; }
        // Точка, из которой пришли в эту точку.
        public PathNode CameFrom { get; set; }
        // Примерное расстояние до цели (H).
        public int HeuristicEstimatePathLength { get; set; }
        // Ожидаемое полное расстояние до цели (F).
        public int EstimateFullPathLength
        {
            get
            {
                return this.PathLengthFromStart + this.HeuristicEstimatePathLength;
            }
        }
        public static List<Point> FindPath(int[,] field, Point start, Point goal, Unit unit)
        {
            // Шаг 1.
            PathNode startNode = new PathNode()
            {
                Position = start,
                CameFrom = null,
                PathLengthFromStart = 0,
                HeuristicEstimatePathLength = GetHeuristicPathLength(start, goal)
            };
            if (!CheckGoal(field, goal, unit))
            {
                bool go = true;
                var checkSet = new Collection<PathNode>();
                var closedCheckSet = new Collection<PathNode>();
                var goodGoal = new Collection<PathNode>();
                var FirstGoal = new PathNode()
                {
                    Position = goal
                };
                checkSet.Add(FirstGoal);

                while (go)
                {
                    if(goodGoal.Count > 0)
                        go = false;
                    try
                    {
                        foreach (var TestTheoreticallyNewGoal in checkSet)
                        {
                            checkSet.Remove(TestTheoreticallyNewGoal);
                            closedCheckSet.Add(TestTheoreticallyNewGoal);
                            foreach (var TheoreticallyNewGoal in GetNewGoal(start, TestTheoreticallyNewGoal, field))
                            {
                                

                                if (CheckGoal(field, TheoreticallyNewGoal.Position, unit))
                                {
                                    goodGoal.Add(TheoreticallyNewGoal);
                                    //Form1.formPointer._Log(TheoreticallyNewGoal.HeuristicEstimatePathLength.ToString());
                                }
                                else
                                {
                                    checkSet.Add(TheoreticallyNewGoal);
                                }
                            }
                        }
                    }
                    catch { }
                }
                var GoodGoal = goodGoal.OrderBy(node => node.HeuristicEstimatePathLength).First();
                goal = GoodGoal.Position;
            }
            
            var closedSet = new Collection<PathNode>();
            var openSet = new Collection<PathNode>();
            // Шаг 2.
            
            openSet.Add(startNode);
            while (openSet.Count > 0)
            {
                // Шаг 3.
                var currentNode = openSet.OrderBy(node => node.EstimateFullPathLength).First();
                // Шаг 4.
                if (currentNode.Position == goal)
                    return GetPathForNode(currentNode);
                // Шаг 5.
                openSet.Remove(currentNode);
                closedSet.Add(currentNode);
                // Шаг 6.
                foreach (var neighbourNode in GetNeighbours(currentNode, goal, field))
                {
                    // Шаг 7.
                    if (closedSet.Count(node => node.Position == neighbourNode.Position) > 0)
                        continue;
                    var openNode = openSet.FirstOrDefault(node => node.Position == neighbourNode.Position);
                    // Шаг 8.
                    if (openNode == null)
                        openSet.Add(neighbourNode);
                    else
                        if (openNode.PathLengthFromStart > neighbourNode.PathLengthFromStart)
                        {
                        // Шаг 9.
                        openNode.CameFrom = currentNode;
                        openNode.PathLengthFromStart = neighbourNode.PathLengthFromStart;
                        }
                }
            }            
            return null;
        }
        private static int GetDistanceBetweenNeighbours()
        {
            return 1;
        }
        private static int GetDistanceBetweenNeighboursDiagonal()
        {
            return 2;
        }
        private static int GetHeuristicPathLength(Point from, Point to)
        {
            return Math.Abs(from.X - to.X) + Math.Abs(from.Y - to.Y);
        }       

        private static bool CheckGoal(int[,] field, Point goal, Unit unit)
        {
            if (goal.X < 0 || goal.X >= field.GetLength(0))
                return false;

            if (goal.Y < 0 || goal.Y >= field.GetLength(1))
                return false;

            foreach (Building build in Form1.buildings)
                if (build.Position.X == goal.X && build.Position.Y == goal.Y || build.Position1.X == goal.X && build.Position1.Y == goal.Y || build.Position2.X == goal.X && build.Position2.Y == goal.Y || build.Position3.X == goal.X && build.Position3.Y == goal.Y)
                    return false;

            foreach (Unit units in Form1.formPointer.units)
            {
                if (unit.Id == units.Id)
                    continue;
                if (goal.X == units.Position.X && goal.Y == units.Position.Y)
                    return false;
            }

            if ((field[goal.Y, goal.X] >= 400))
                return false;

            return true;
        }

        private static Collection<PathNode> GetNewGoal(Point start, PathNode goal, int[,] field)
        {
            var Set = new Collection<PathNode>();
            Point[] neighbourPoints = new Point[8];
            neighbourPoints[0] = new Point(goal.Position.X + 1, goal.Position.Y);
            neighbourPoints[1] = new Point(goal.Position.X - 1, goal.Position.Y);
            neighbourPoints[2] = new Point(goal.Position.X, goal.Position.Y + 1);
            neighbourPoints[3] = new Point(goal.Position.X, goal.Position.Y - 1);
            neighbourPoints[4] = new Point(goal.Position.X + 1, goal.Position.Y + 1);
            neighbourPoints[5] = new Point(goal.Position.X - 1, goal.Position.Y + 1);
            neighbourPoints[6] = new Point(goal.Position.X + 1, goal.Position.Y - 1);
            neighbourPoints[7] = new Point(goal.Position.X - 1, goal.Position.Y - 1);
            int _ind = 0;
            foreach (var point in neighbourPoints)
            {
                var neighbourNode = new PathNode();
                if (_ind <= 3)
                {
                    neighbourNode = new PathNode()
                    {
                        Position = point,
                        HeuristicEstimatePathLength = GetHeuristicPathLength(start, point)
                    };
                }
                else
                {
                    neighbourNode = new PathNode()
                    {
                        Position = point,
                        HeuristicEstimatePathLength = GetHeuristicPathLength(start, point),
                    };
                }
                Set.Add(neighbourNode);
                _ind++;
            }
            //var currentNode = Set.OrderBy(node => node.HeuristicEstimatePathLength).First();
            return Set;
        }
        private static Collection<PathNode> GetNeighbours(PathNode pathNode, Point goal, int[,] field)
        {
            var result = new Collection<PathNode>();
            bool IsFlag = false;
            // Соседними точками являются соседние по стороне клетки.
            Point[] neighbourPoints = new Point[8];
            neighbourPoints[0] = new Point(pathNode.Position.X + 1, pathNode.Position.Y);
            neighbourPoints[1] = new Point(pathNode.Position.X - 1, pathNode.Position.Y);
            neighbourPoints[2] = new Point(pathNode.Position.X, pathNode.Position.Y + 1);
            neighbourPoints[3] = new Point(pathNode.Position.X, pathNode.Position.Y - 1);
            neighbourPoints[4] = new Point(pathNode.Position.X + 1, pathNode.Position.Y + 1);
            neighbourPoints[5] = new Point(pathNode.Position.X - 1, pathNode.Position.Y + 1);
            neighbourPoints[6] = new Point(pathNode.Position.X + 1, pathNode.Position.Y - 1);
            neighbourPoints[7] = new Point(pathNode.Position.X - 1, pathNode.Position.Y - 1);
            int _ind = 0;
            foreach (var point in neighbourPoints)
            {
                // Заполняем данные для точки маршрута.
                var neighbourNode = new PathNode();
                if (_ind <= 3)
                {
                    neighbourNode = new PathNode()
                    {
                        Position = point,
                        CameFrom = pathNode,
                        PathLengthFromStart = pathNode.PathLengthFromStart + GetDistanceBetweenNeighbours(),
                        HeuristicEstimatePathLength = GetHeuristicPathLength(point, goal)
                    };
                }
                else
                {
                    neighbourNode = new PathNode()
                    {
                        Position = point,
                        CameFrom = pathNode,
                        PathLengthFromStart = pathNode.PathLengthFromStart + GetDistanceBetweenNeighboursDiagonal(),
                        HeuristicEstimatePathLength = GetHeuristicPathLength(point, goal)
                    };
                }

                // Проверяем, что не вышли за границы карты.
                if (point.X < 0 || point.X >= field.GetLength(0))
                    continue;
                if (point.Y < 0 || point.Y >= field.GetLength(1))
                    continue;
                // Проверяем, что по клетке можно ходить.
                if ((field[point.Y, point.X] >= 400))
                    continue;

                foreach (Building build in Form1.buildings)
                    if (build.Position.X == point.X && build.Position.Y == point.Y || build.Position1.X == point.X && build.Position1.Y == point.Y || build.Position2.X == point.X && build.Position2.Y == point.Y || build.Position3.X == point.X && build.Position3.Y == point.Y)
                        IsFlag = true;

                foreach (Unit units in Form1.formPointer.units)
                        if (point.X == units.Position.X && point.Y == units.Position.Y)       
                            IsFlag = true;

                if (IsFlag)
                {
                    IsFlag = false;
                    continue;
                }

                result.Add(neighbourNode);
                _ind++;
            }
            return result;
        }
        private static List<Point> GetPathForNode(PathNode pathNode)
        {
            var result = new List<Point>();
            var currentNode = pathNode;
            while (currentNode != null)
            {
                result.Add(currentNode.Position);
                currentNode = currentNode.CameFrom;
            }
            result.Reverse();
            return result;
        }
    }
}