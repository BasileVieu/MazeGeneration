using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maze
{
    public class RecursiveBacktracker : Generator
    {
        private List<int> indexesPath = new List<int>();

        private int startingIndex;

        public override string HelpBox => "RecursiveBacktracker";

        public override void Destroy()
        {
            indexesPath.Clear();
            indexesPath = null;

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
                    }

                    tiles.Add(tile);
                }

                yield return 0;
            }

            tiles[size.x].m_value = 0;
            tiles[tiles.Count - size.x - 1].m_value = 0;

            yield return 0;
        }

        public override IEnumerator CreateMaze()
        {
            int x = Random.Range(1, size.x / 2 - 1) * 2 + 1;
            int y = Random.Range(0, int.MaxValue) % ((size.y - 1) / 2) * 2 + 1;

            startingIndex = x + y * size.x;
            tiles[startingIndex].m_value = 1;
            tiles[startingIndex].Color = Color.red;

            indexesPath.Add(startingIndex);

            do
            {
                int currentIndex = indexesPath[indexesPath.Count - 1];

                List<int> adjacentTilesIndex = GetAdjacentTiles(currentIndex);

                if (adjacentTilesIndex.Count == 0)
                {
                    int wallIndex = (currentIndex + indexesPath[indexesPath.Count - 2]) / 2;

                    tiles[currentIndex].Color = tiles[wallIndex].Color = Color.white;

                    indexesPath.RemoveAt(indexesPath.Count - 1);
                }
                else
                {
                    int nextIndex = adjacentTilesIndex[Random.Range(0, adjacentTilesIndex.Count)];

                    int wallIndex = (currentIndex + nextIndex) / 2;

                    tiles[currentIndex].m_value = tiles[nextIndex].m_value = tiles[wallIndex].m_value = 1;
                    tiles[currentIndex].Color = tiles[nextIndex].Color = tiles[wallIndex].Color = Color.red;

                    indexesPath.Add(nextIndex);
                }

                yield return 0;
            } while (indexesPath.Count != 1);

            tiles[startingIndex].Color = Color.white;

            tiles[size.x].m_value = tiles[tiles.Count - size.x - 1].m_value = tiles[size.x + 1].m_value;
            tiles[size.x].Color = tiles[tiles.Count - size.x - 1].Color = tiles[size.x + 1].Color;

            Debug.Log("RecursiveBacktracker Success");

            yield return 0;
        }

        List<int> GetAdjacentTiles(int _index)
        {
            var adjacentTiles = new List<int>();

            if ((_index - 1) % size.x != 0)
            {
                int leftIndex = _index - 2;

                if (tiles[leftIndex].m_value == 0)
                {
                    adjacentTiles.Add(leftIndex);
                }
            }

            if ((_index + 2) % size.x != 0)
            {
                int rightIndex = _index + 2;

                if (tiles[rightIndex].m_value == 0)
                {
                    adjacentTiles.Add(rightIndex);
                }
            }

            if (_index > 2 * size.x)
            {
                int upIndex = _index - 2 * size.x;

                if (tiles[upIndex].m_value == 0)
                {
                    adjacentTiles.Add(upIndex);
                }
            }

            if (_index < tiles.Count - 2 * size.x)
            {
                int downIndex = _index + 2 * size.x;

                if (tiles[downIndex].m_value == 0)
                {
                    adjacentTiles.Add(downIndex);
                }
            }

            return adjacentTiles;
        }
    }
}