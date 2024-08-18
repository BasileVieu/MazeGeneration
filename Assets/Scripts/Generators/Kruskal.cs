using System.Collections;
using UnityEngine;

namespace Maze
{
    public class Kruskal : Generator
    {
        public override string HelpBox => "Kruskal";

        public override void Destroy()
        {
            foreach (Tile tile in tiles)
            {
                Object.Destroy(tile.gameObject);
            }
            
            tiles.Clear();
            tiles = null;
        }

        public override IEnumerator CreateGrid()
        {
            var nb = 0;

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
                        nb++;
                        
                        tile.Initialize(nb, new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)));
                    }

                    tiles.Add(tile);
                }

                yield return 0;
            }

            tiles[size.x].m_value = tiles[tiles.Count - size.x - 1].m_value = nb;

            yield return 0;
        }

        public override IEnumerator CreateMaze()
        {
            while (!IsFinished())
            {
                int x = Random.Range(1, size.x - 1);
                int y;

                if (x % 2 == 0)
                {
                    y = Random.Range(0, int.MaxValue) % ((size.y - 1) / 2) * 2 + 1;
                }
                else
                {
                    y = Random.Range(0, int.MaxValue) % ((size.y - 2) / 2) * 2 + 2;
                }

                Tile tile1;
                Tile tile2;

                if (tiles[x - 1 + y * size.x].m_value == -1)
                {
                    tile1 = tiles[x + (y - 1) * size.x];
                    tile2 = tiles[x + (y + 1) * size.x];
                }
                else
                {
                    tile1 = tiles[x - 1 + y * size.x];
                    tile2 = tiles[x + 1 + y * size.x];
                }

                int value1 = tile1.m_value;
                int value2 = tile2.m_value;

                Color color1 = tile1.Color;

                if (value1 != value2)
                {
                    tiles[x + y * size.x].m_value = value1;
                    tiles[x + y * size.x].Color = color1;

                    for (var j = 1; j < size.y - 1; j++)
                    {
                        for (var i = 1; i < size.x - 1; i++)
                        {
                            if (tiles[i + j * size.x].m_value == value2)
                            {
                                tiles[i + j * size.x].m_value = value1;

                                tiles[i + j * size.x].Color = color1;
                            }
                        }
                    }
                }

                yield return 0;
            }

            /*int biggerSize = sizeX >= sizeY ? sizeX : sizeY;
    
            for (var i = 0; i < biggerSize; i++)
            {
                int x = Random.Range(1, sizeX - 1);
                int y;
    
                if (x % 2 == 0)
                {
                    y = Random.Range(0, int.MaxValue) % ((sizeY - 1) / 2) * 2 + 1;
                }
                else
                {
                    y = Random.Range(0, int.MaxValue) % ((sizeY - 2) / 2) * 2 + 2;
                }
    
                tiles[x][y].value = tiles[1][1].value;
                tiles[x][y].Color = tiles[1][1].Color;
                                
                yield return 0;
            }*/

            tiles[size.x].m_value = tiles[tiles.Count - size.x - 1].m_value = tiles[size.x + 1].m_value;
            tiles[size.x].Color = tiles[tiles.Count - size.x - 1].Color = tiles[size.x + 1].Color;

            Debug.Log("Kruskal Success");

            yield return 0;
        }

        bool IsFinished()
        {
            for (var y = 1; y < size.y - 1; y += 2)
            {
                for (var x = 1; x < size.x - 1; x += 2)
                {
                    if (tiles[x + y * size.x].m_value != tiles[size.x + 1].m_value)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}