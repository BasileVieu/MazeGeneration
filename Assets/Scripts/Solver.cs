using System.Collections;
using UnityEngine;

namespace Maze
{
    public abstract class Solver
    {
        protected MazeCreator mazeCreator;
        
        protected Transform circlePrefab;

        protected Vector2Int size;

        protected int startIndex;
        protected int endIndex;
        
        public virtual void Initialize(MazeCreator _creator)
        {
            mazeCreator = _creator;
            circlePrefab = _creator.GetCirclePrefab();
            size = _creator.GetSize();
            startIndex = size.x + 1;
            endIndex = _creator.GetGenerator().Tiles.Count - size.x - 2;
        }
        
        public virtual void Destroy()
        {
            
        }
        
        public virtual IEnumerator SolveMaze()
        {
            yield return 0;
        }
    }
}