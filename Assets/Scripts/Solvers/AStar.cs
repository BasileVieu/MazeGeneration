using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace Maze
{
    public class AStar : Solver
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
            Debug.Log("AStar");

            var stopWatch = new Stopwatch();
            stopWatch.Reset();
            stopWatch.Start();

            var costGrid = new List<int>();

            foreach (Tile tile in mazeCreator.GetGenerator().Tiles)
            {
                matrix.Add(tile.m_value == -1 ? -1 : 0);
                costGrid.Add(0);
            }

            Vector2 circleSize = mazeCreator.GetTilePrefab().localScale;

            var frontier = new List<int>
            {
                    startIndex
            };

            matrix[startIndex] = 1;

            var found = false;

            float sqrt2 = Mathf.Sqrt(2);

            do
            {
                frontier.Sort(delegate(int _a, int _b)
                {
                    float valueA = costGrid[_a] + Distance(_a % size.x, _a / size.x, endIndex % size.x, endIndex / size.x) * sqrt2;
                    float valueB = costGrid[_b] + Distance(_b % size.x, _b / size.x, endIndex % size.x, endIndex / size.x) * sqrt2;

                    return valueA.CompareTo(valueB);
                });

                int currentCell = frontier[0];

                List<int> neighbors = GetAdjacentTiles(currentCell);
                
                frontier.RemoveAt(0);

                for (var i = 0; i < neighbors.Count; i++)
                {
                    if (matrix[neighbors[i]] == 0)
                    {
                        frontier.Add(neighbors[i]);

                        matrix[neighbors[i]] = i + 1;

                        costGrid[neighbors[i]] = costGrid[currentCell] + 1;
                        
                        /*var position = new Vector2Int(neighbors[i] % size.x, neighbors[i] / size.x);
                    
                        Transform circleTransform = Object.Instantiate(circlePrefab, mazeCreator.transform, false);
                        circleTransform.position = new Vector3((position.x - size.x / 2) * circleSize.x, (size.y - position.y - size.y / 2) * circleSize.y - circleSize.y, -1.0f);

                        yield return new WaitForSeconds(0.01f);*/

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
            
            Debug.Log("AStar Success");

            yield return 0;
        }

        List<int> GetAdjacentTiles(int _index) => new List<int>
        {
                _index - size.x,
                _index + 1,
                _index + size.x,
                _index - 1
        };

        float Distance(int _point1X, int _point1Y, int _point2X, int _point2Y) =>
                Mathf.Abs(_point1X - _point2X) + Mathf.Abs(_point1Y - _point2Y);
    }
}