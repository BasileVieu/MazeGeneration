namespace Maze
{
    public static class Utils
    {
        public enum GeneratorType
        {
            Kruskal,
            Prim,
            RecursiveBacktracker,
            AldousBroder,
            GrowingTree,
            HuntAndKill,
            Wilson
        }

        public static string[] generatorsHelpBoxes =
        {
                "Kruskal",
                "Prim",
                "RecursiveBacktracker",
                "AldousBroder",
                "GrowingTree",
                "HuntAndKill",
                "Wilson"
        };

        public enum SolverType
        {
            BreadthFirstSearch,
            BidirectionalBreadthFirstSearch,
            Greedy,
            AStar
        }
    }
}