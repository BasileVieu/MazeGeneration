using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace Maze
{
    public class BidirectionalBreadthFirstSearch : Solver
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
            Debug.Log("Bidirectional Breadth First Search");

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
                    startIndex,
                    endIndex
            };

            matrix[endIndex] = 1;
            matrix[startIndex] = 11;

            var found = false;

            int currentCell;
            var startEnd = 0;
            var targetEnd = 0;

            do
            {
                currentCell = frontier[0];

                List<int> neighbors = GetAdjacentTiles(currentCell);
                
                frontier.RemoveAt(0);

                for (var i = 0; i < neighbors.Count; i++)
                {
                    if (matrix[neighbors[i]] == 0)
                    {
                        frontier.Add(neighbors[i]);

                        if (matrix[currentCell] < 10)
                        {
                            matrix[neighbors[i]] = i + 1;
                        }
                        else
                        {
                            matrix[neighbors[i]] = 11 + i;
                        }
                    }
                    else if (matrix[neighbors[i]] > 0)
                    {
                        if (matrix[currentCell] < 10
                            && matrix[neighbors[i]] > 10)
                        {
                            startEnd = currentCell;
                            targetEnd = neighbors[i];

                            found = true;

                            break;
                        }
                        
                        if (matrix[currentCell] > 10
                                 && matrix[neighbors[i]] < 10)
                        {
                            startEnd = neighbors[i];
                            targetEnd = currentCell;

                            found = true;

                            break;
                        }
                    }
                }
            } while (!found && frontier.Count > 0);

            if (found)
            {
                var targets = new List<int>
                {
                        endIndex,
                        startIndex
                };

                var starts = new List<int>
                {
                        startEnd,
                        targetEnd
                };

                for (var i = 0; i < starts.Count; i++)
                {
                    int currentIndex = starts[i];

                    while (currentIndex != targets[i])
                    {
                        path.Add(currentIndex);
                        
                        switch (matrix[currentIndex] - i * 10)
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
                    }

                    if (i == 0)
                    {
                        path.Reverse();
                    }
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
            
            Debug.Log("Bidirectional Breadth First Search Success");

            yield return 0;
        }

        List<int> GetAdjacentTiles(int _index) => new List<int>
        {
                _index - size.x,
                _index + 1,
                _index + size.x,
                _index - 1
        };
    }
}