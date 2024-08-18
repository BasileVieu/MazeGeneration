using System.Collections;
using UnityEngine;

namespace Maze
{
    public class MazeCreator : MonoBehaviour
    {
        [SerializeField] private Transform m_tilePrefab;
        [SerializeField] private Transform m_circlePrefab;

        [SerializeField] private Vector2Int m_size;

        [SerializeField] private Utils.GeneratorType m_generatorType;
        [SerializeField] private Utils.SolverType m_solverType;

        [SerializeField] private bool m_autoGenerate;
        [SerializeField] private bool m_autoSolve;

        private Generator generator;
        private Solver solver;

        public Transform GetTilePrefab() => m_tilePrefab;

        public Transform GetCirclePrefab() => m_circlePrefab;

        public Vector2Int GetSize() => m_size;

        public Utils.GeneratorType GetGeneratorType() => m_generatorType;

        public Utils.SolverType GetSolverType() => m_solverType;

        public Generator GetGenerator() => generator;

        public Solver GetSolver() => solver;

        void Awake()
        {
            if (m_autoGenerate)
            {
                StartGeneration();
            }
        }

        /// <summary>
        /// Begin maze generator retrieving functions from selected generator.
        /// </summary>
        /// <returns></returns>
        IEnumerator GenerateMaze()
        {
            generator = GeneratorFactory.GetGenerator(this);
            generator.Initialize(this);

            yield return StartCoroutine(generator.CreateGrid());

            yield return StartCoroutine(generator.CreateMaze());

            if (m_autoSolve)
            {
                yield return StartCoroutine(SolveMaze());
            }
        }

        IEnumerator SolveMaze()
        {
            if (solver != null)
            {
                solver.Destroy();
                solver = null;
            }

            solver = SolverFactory.GetSolver(this);
            solver.Initialize(this);
            
            yield return StartCoroutine(solver.SolveMaze());
        }

        public void Clear()
        {
            StopAllCoroutines();
            
            m_size = new Vector2Int(Mathf.Max(3, m_size.x % 2 == 0 ? m_size.x + 1 : m_size.x), Mathf.Max(3, m_size.y % 2 == 0 ? m_size.y + 1 : m_size.y));
            
            if (generator != null)
            {
                generator.Destroy();
                generator = null;
            }

            if (solver != null)
            {
                solver.Destroy();
                solver = null;
            }
        }

        public void StartGeneration()
        {
            Clear();

            StartCoroutine(GenerateMaze());
        }

        public void StartSolving()
        {
            StopAllCoroutines();
            
            StartCoroutine(SolveMaze());
        }
    }
}