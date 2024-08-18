using UnityEngine;
using UnityEditor;

namespace Maze
{
    [CustomEditor(typeof(MazeCreator))]
    public class MazeCreatorEditor : Editor
    {
        private MazeCreator mazeCreator;

        private void OnEnable()
        {
            mazeCreator = target as MazeCreator;
        }

        public override void OnInspectorGUI()
        {
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                base.OnInspectorGUI();
            }
            
            EditorGUI.BeginChangeCheck();

            if (GUILayout.Button("Clear"))
            {
                Undo.RecordObject(mazeCreator, "Clear");
                
                mazeCreator.Clear();
            }

            if (GUILayout.Button("Restart"))
            {
                Undo.RecordObject(mazeCreator, "Restart");
                
                mazeCreator.StartGeneration();
            }

            if (GUILayout.Button("Solve"))
            {
                Undo.RecordObject(mazeCreator, "Solve");
                
                mazeCreator.StartSolving();
            }

            EditorGUI.EndChangeCheck();
            
            EditorGUILayout.HelpBox(Utils.generatorsHelpBoxes[(int)mazeCreator.GetGeneratorType()], MessageType.Info);
        }
    }
}