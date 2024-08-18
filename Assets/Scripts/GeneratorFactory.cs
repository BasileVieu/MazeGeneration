namespace Maze
{
    public static class GeneratorFactory
    {
        public static Generator GetGenerator(MazeCreator _creator)
        {
            return _creator.GetGeneratorType() switch
            {
                    Utils.GeneratorType.Kruskal => new Kruskal(),
                    Utils.GeneratorType.Prim => new Prim(),
                    Utils.GeneratorType.RecursiveBacktracker => new RecursiveBacktracker(),
                    Utils.GeneratorType.AldousBroder => new AldousBroder(),
                    Utils.GeneratorType.GrowingTree => new GrowingTree(),
                    Utils.GeneratorType.HuntAndKill => new HuntAndKill(),
                    Utils.GeneratorType.Wilson => new Wilson(),
                    _ => null
            };
        }
    }
}