namespace Maze
{
    public static class SolverFactory
    {
        public static Solver GetSolver(MazeCreator _creator)
        {
            return _creator.GetSolverType() switch
            {
                    Utils.SolverType.BreadthFirstSearch => new BreadthFirstSearch(),
                    Utils.SolverType.BidirectionalBreadthFirstSearch => new BidirectionalBreadthFirstSearch(),
                    Utils.SolverType.Greedy => new Greedy(),
                    Utils.SolverType.AStar => new AStar(),
                    _ => null
            };
        }
    }
}