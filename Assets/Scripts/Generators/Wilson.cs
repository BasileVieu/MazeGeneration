using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maze
{
    public class Wilson : Generator
    {
        private List<int> ust = new List<int>();
        private List<int> set = new List<int>();

        enum Direction
        {
            Right,
            Left,
            Top,
            Bottom
        }

        public override string HelpBox => "Wilson";

        public override void Destroy()
        {
            ust.Clear();
            ust = null;

            set.Clear();
            set = null;

            foreach (Tile tile in tiles)
            {
                Object.Destroy(tile.gameObject);
            }
            
            tiles.Clear();
            tiles = null;
        }

        public override IEnumerator CreateGrid()
        {
            Vector2 tileSize = tilePrefab.localScale;

            for (var y = 0; y < size.y; y++)
            {
                for (var x = 0; x < size.x; x++)
                {
                    Transform tileTransform = Object.Instantiate(tilePrefab, mazeCreator.transform, false);
                    tileTransform.position = new Vector3((x - size.x / 2) * tileSize.x, (size.y - y - size.y / 2) * tileSize.y - tileSize.y);

                    var tile = tileTransform.gameObject.GetComponent<Tile>();
                    tile.m_index = x + y * size.x;

                    if (x % 2 == 0
                        || y % 2 == 0)
                    {
                        tile.Initialize(-1, Color.black);
                    }
                    else
                    {
                        tile.Initialize(0, Color.grey);

                        set.Add(tile.m_index);
                    }

                    tiles.Add(tile);
                }

                yield return 0;
            }

            tiles[size.x].m_value = tiles[tiles.Count - size.x - 1].m_value = 0;

            yield return 0;
        }

        public override IEnumerator CreateMaze()
        {
            int x = Random.Range(1, size.x / 2 - 1) * 2 + 1;
            int y = Random.Range(0, int.MaxValue) % ((size.y - 1) / 2) * 2 + 1;

            int startingIndex = x + y * size.x;

            ust.Add(startingIndex);
            set.Remove(startingIndex);

            tiles[startingIndex].m_value = 1;
            tiles[startingIndex].Color = Color.white;

            while (set.Count != 0)
            {
                int firstIndex = set[Random.Range(0, set.Count)];
                int currentIndex = firstIndex;

                var path = new Dictionary<int, Direction>();

                while (!ust.Contains(currentIndex))
                {
                    List<Direction> adjacentTiles = GetAdjacentDirections(currentIndex);

                    Direction nextDirection = adjacentTiles[Random.Range(0, adjacentTiles.Count)];

                    path[currentIndex] = nextDirection;

                    /*if (nextDirection == Direction.Right)
                    {
                        tiles[currentIndex].Color = Color.red;
                    }
                    else if (nextDirection == Direction.Left)
                    {
                        tiles[currentIndex].Color = Color.green;
                    }
                    else if (nextDirection == Direction.Top)
                    {
                        tiles[currentIndex].Color = Color.blue;
                    }
                    else if (nextDirection == Direction.Bottom)
                    {
                        tiles[currentIndex].Color = Color.yellow;
                    }
    
                    yield return 0;*/

                    currentIndex = GetIndexFromDirection(currentIndex, nextDirection);
                }

                int currentPathIndex = firstIndex;

                while (currentPathIndex != currentIndex)
                {
                    int nextIndex = GetIndexFromDirection(currentPathIndex, path[currentPathIndex]);

                    int wallIndex = (currentPathIndex + nextIndex) / 2;

                    tiles[currentPathIndex].m_value = tiles[nextIndex].m_value = tiles[wallIndex].m_value = 1;
                    tiles[currentPathIndex].Color = tiles[nextIndex].Color = tiles[wallIndex].Color = Color.white;

                    if (!ust.Contains(currentPathIndex))
                    {
                        ust.Add(currentPathIndex);
                    }

                    if (set.Contains(currentPathIndex))
                    {
                        set.Remove(currentPathIndex);
                    }

                    currentPathIndex = nextIndex;

                    yield return 0;
                }

                for (var j = 0; j < size.y; j++)
                {
                    for (var i = 0; i < size.x; i++)
                    {
                        int tempIndex = i + j * size.x;

                        if (tiles[tempIndex].m_value == 0)
                        {
                            tiles[tempIndex].Color = Color.grey;
                        }
                    }
                }

                path.Clear();
            }

            tiles[size.x].m_value = tiles[tiles.Count - size.x - 1].m_value = tiles[size.x + 1].m_value;
            tiles[size.x].Color = tiles[tiles.Count - size.x - 1].Color = tiles[size.x + 1].Color;

            Debug.Log("Wilson Success");

            yield return 0;
        }

        List<Direction> GetAdjacentDirections(int _index)
        {
            var adjacentTiles = new List<Direction>();

            if ((_index - 1) % size.x != 0)
            {
                adjacentTiles.Add(Direction.Left);
            }

            if ((_index + 2) % size.x != 0)
            {
                adjacentTiles.Add(Direction.Right);
            }

            if (_index > 2 * size.x)
            {
                adjacentTiles.Add(Direction.Top);
            }

            if (_index < tiles.Count - 2 * size.x)
            {
                adjacentTiles.Add(Direction.Bottom);
            }

            return adjacentTiles;
        }

        int GetIndexFromDirection(int _index, Direction _direction)
        {
            switch (_direction)
            {
                case Direction.Right:
                {
                    return _index + 2;
                }

                case Direction.Left:
                {
                    return _index - 2;
                }

                case Direction.Top:
                {
                    return _index - 2 * size.x;
                }

                case Direction.Bottom:
                {
                    return _index + 2 * size.x;
                }
            }

            return 0;
        }
    }
}