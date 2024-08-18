using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maze
{
    public class Prim : Generator
    {
        private List<int> set = new List<int>();
        private List<int> frontier = new List<int>();

        private int nbWhiteTiles;

        public override string HelpBox => "Prim";

        public override void Destroy()
        {
            set.Clear();
            set = null;

            frontier.Clear();
            frontier = null;

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

                        nbWhiteTiles++;
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

            set.Add(x + y * size.x);
            frontier.AddRange(GetAdjacentTilesThatAreNotInFrontier(x + y * size.x));

            while (set.Count != nbWhiteTiles
                   || frontier.Count != 0)
            {
                int randomFrontierIndex = frontier[Random.Range(0, frontier.Count)];

                List<int> frontierAdjacent = GetAdjacentTilesThatAreInSet(randomFrontierIndex);

                int frontierAdjacentInSetIndex = frontierAdjacent[Random.Range(0, frontierAdjacent.Count)];

                set.Add(randomFrontierIndex);

                frontier.AddRange(GetAdjacentTilesThatAreNotInFrontier(randomFrontierIndex));
                frontier.Remove(randomFrontierIndex);

                int wallIndex = (randomFrontierIndex + frontierAdjacentInSetIndex) / 2;

                tiles[randomFrontierIndex].m_value = tiles[frontierAdjacentInSetIndex].m_value = tiles[wallIndex].m_value = 1;
                tiles[randomFrontierIndex].Color = tiles[frontierAdjacentInSetIndex].Color = tiles[wallIndex].Color = Color.white;

                yield return 0;
            }

            tiles[size.x].m_value = tiles[tiles.Count - size.x - 1].m_value = tiles[size.x + 1].m_value;
            tiles[size.x].Color = tiles[tiles.Count - size.x - 1].Color = tiles[size.x + 1].Color;

            Debug.Log("Prim Success");

            yield return 0;
        }

        List<int> GetAdjacentTilesThatAreNotInFrontier(int _index)
        {
            var adjacentTiles = new List<int>();

            if ((_index - 1) % size.x != 0)
            {
                int leftIndex = _index - 2;

                if (!frontier.Contains(leftIndex)
                    && !set.Contains(leftIndex))
                {
                    adjacentTiles.Add(leftIndex);
                }
            }

            if ((_index + 2) % size.x != 0)
            {
                int rightIndex = _index + 2;

                if (!frontier.Contains(rightIndex)
                    && !set.Contains(rightIndex))
                {
                    adjacentTiles.Add(rightIndex);
                }
            }

            if (_index > 2 * size.x)
            {
                int upIndex = _index - 2 * size.x;

                if (!frontier.Contains(upIndex)
                    && !set.Contains(upIndex))
                {
                    adjacentTiles.Add(upIndex);
                }
            }

            if (_index < tiles.Count - 2 * size.x)
            {
                int downIndex = _index + 2 * size.x;

                if (!frontier.Contains(downIndex)
                    && !set.Contains(downIndex))
                {
                    adjacentTiles.Add(downIndex);
                }
            }

            return adjacentTiles;
        }

        List<int> GetAdjacentTilesThatAreInSet(int _index)
        {
            var adjacentTiles = new List<int>();

            if ((_index - 1) % size.x != 0)
            {
                int leftIndex = _index - 2;

                if (set.Contains(leftIndex))
                {
                    adjacentTiles.Add(leftIndex);
                }
            }

            if ((_index + 2) % size.x != 0)
            {
                int rightIndex = _index + 2;

                if (set.Contains(rightIndex))
                {
                    adjacentTiles.Add(rightIndex);
                }
            }

            if (_index > 2 * size.x)
            {
                int upIndex = _index - 2 * size.x;

                if (set.Contains(upIndex))
                {
                    adjacentTiles.Add(upIndex);
                }
            }

            if (_index < tiles.Count - 2 * size.x)
            {
                int downIndex = _index + 2 * size.x;

                if (set.Contains(downIndex))
                {
                    adjacentTiles.Add(downIndex);
                }
            }

            return adjacentTiles;
        }
    }
}