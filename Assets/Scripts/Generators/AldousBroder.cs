using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maze
{
    public class AldousBroder : Generator
    {
        private List<int> indexes = new List<int>();

        private int nbWhiteTiles;

        public override string HelpBox => "AldousBroder";

        public override void Destroy()
        {
            indexes.Clear();
            indexes = null;

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

            tiles[x + y * size.x].m_value = 1;
            tiles[x + y * size.x].Color = Color.white;

            indexes.Add(x + y * size.x);

            int currentIndex = x + y * size.x;

            while (indexes.Count != nbWhiteTiles)
            {
                List<int> adjacentTilesIndex = GetAdjacentTiles(currentIndex);

                int nextIndex = adjacentTilesIndex[Random.Range(0, adjacentTilesIndex.Count)];

                if (!indexes.Contains(nextIndex))
                {
                    int wallIndex = (currentIndex + nextIndex) / 2;

                    tiles[currentIndex].Color = tiles[nextIndex].Color = tiles[wallIndex].Color = Color.white;

                    indexes.Add(nextIndex);
                }

                currentIndex = nextIndex;

                yield return 0;
            }

            tiles[size.x].Color = tiles[tiles.Count - size.x - 1].Color = tiles[size.x + 1].Color;

            Debug.Log("AldousBroder Success");

            yield return 0;
        }

        List<int> GetAdjacentTiles(int _index)
        {
            var adjacentTiles = new List<int>();

            if ((_index - 1) % size.x != 0)
            {
                adjacentTiles.Add(_index - 2);
            }

            if ((_index + 2) % size.x != 0)
            {
                adjacentTiles.Add(_index + 2);
            }

            if (_index > 2 * size.x)
            {
                adjacentTiles.Add(_index - 2 * size.x);
            }

            if (_index < tiles.Count - 2 * size.x)
            {
                adjacentTiles.Add(_index + 2 * size.x);
            }

            return adjacentTiles;
        }
    }
}