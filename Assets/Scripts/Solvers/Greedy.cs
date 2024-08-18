using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace Maze
{
    public class Greedy : Solver
    {
        private List<int> matrix = new List<int>();
        private List<int> path = new List<int>();
        
        public override void Destroy()
        {
            base.Destroy();

            matrix.Clear();
            matrix = null;
            
            path.Clear();
            path = null;
        }

        public override IEnumerator SolveMaze()
        {
            Debug.Log("Greedy");

            var stopWatch = new Stopwatch();
            stopWatch.Reset();
            stopWatch.Start();
            
            foreach (Tile tile in mazeCreator.GetGenerator().Tiles)
            {
                matrix.Add(tile.m_value == -1 ? -1 : 0);
            }

            Vector2 circleSize = mazeCreator.GetTilePrefab().localScale;

            var frontier = new List<int>
            {
                    startIndex
            };

            matrix[startIndex] = 1;

            var found = false;

            do
            {
                frontier.Sort((_a, _b) => Distance(_a % size.x, _a / size.x, endIndex % size.x, endIndex / size.x)
                                      .CompareTo(Distance(_b % size.x, _b / size.x, endIndex % size.x, endIndex / size.x)));

                List<int> neighbors = GetAdjacentTiles(frontier[0]);

                frontier.RemoveAt(0);

                for (var i = 0; i < neighbors.Count; i++)
                {
                    if (matrix[neighbors[i]] == 0)
                    {
                        frontier.Add(neighbors[i]);

                        matrix[neighbors[i]] = i + 1;

                        if (neighbors[i] == endIndex)
                        {
                            found = true;

                            break;
                        }
                    }
                }
            } while (!found && frontier.Count > 0);

            if (found)
            {
                int currentIndex = endIndex;

                while (currentIndex != startIndex)
                {
                    switch (matrix[currentIndex])
                    {
                        case 1:
                        {
                            currentIndex += size.x;
                        }

                            break;
                        
                        case 2:
                        {
                            currentIndex -= 1;
                        }

                            break;
                        
                        case 3:
                        {
                            currentIndex -= size.x;
                        }

                            break;
                        
                        case 4:
                        {
                            currentIndex += 1;
                        }

                            break;
                    }

                    path.Add(currentIndex);
                }
                
                path.Reverse();
                
                path.Add(endIndex);
            }

            stopWatch.Stop();
            
            Debug.Log(stopWatch.ElapsedMilliseconds);

            foreach (int index in path)
            {
                int tempIndex = mazeCreator.GetGenerator().Tiles[index].m_index;
                
                var position = new Vector2Int(tempIndex % size.x, tempIndex / size.x);
                    
                Transform circleTransform = Object.Instantiate(circlePrefab, mazeCreator.transform, false);
                circleTransform.position = new Vector3((position.x - size.x / 2) * circleSize.x, (size.y - position.y - size.y / 2) * circleSize.y - circleSize.y, -1.0f);
                    
                circleTransform.gameObject.GetComponent<SpriteRenderer>().color = Color.green;

                yield return 0;
            }
            
            Debug.Log("Greedy Success");

            yield return 0;
        }

        List<int> GetAdjacentTiles(int _index) => new List<int>
        {
                _index - size.x,
                _index + 1,
                _index + size.x,
                _index - 1
        };

        float Distance(int _point1X, int _point1Y, int _point2X, int _point2Y) => Mathf.Sqrt(Mathf.Pow(_point2X - _point1X, 2) + Mathf.Pow(_point2Y - _point1Y, 2));
    }
}