using UnityEditor;
using UnityEngine;

namespace MeshGenerationSystem
{
    [CustomEditor(typeof(MeshGeneratorExample))]
    public class MeshGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            MeshGeneratorExample meshSlicer = target as MeshGeneratorExample;

            if (GUILayout.Button("Generate mesh"))
            {
                Undo.RecordObject(meshSlicer, "Generate mesh");
                meshSlicer.GenerateMesh();
                EditorUtility.SetDirty(meshSlicer);
            }
        }
    }
}