using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maze
{
    public abstract class Generator
    {
        protected MazeCreator mazeCreator;
        
        protected Transform tilePrefab;

        protected List<Tile> tiles = new List<Tile>();

        protected Vector2Int size;

        public List<Tile> Tiles => tiles;
        
        public abstract string HelpBox { get; }
        
        /// <summary>
        /// Initialize generator's values.
        /// </summary>
        /// <param name="_creator">Retrieve data from a MazeCreator.</param>
        public virtual void Initialize(MazeCreator _creator)
        {
            mazeCreator = _creator;
            tilePrefab = _creator.GetTilePrefab();
            size = _creator.GetSize();
        }

        public virtual void Destroy()
        {
            foreach (Tile tile in tiles)
            {
                Object.Destroy(tile.gameObject);
            }
            
            tiles.Clear();
            tiles = null;
        }

        public virtual IEnumerator CreateGrid()
        {
            yield return 0;
        }

        public virtual IEnumerator CreateMaze()
        {
            yield return 0;
        }
    }
}