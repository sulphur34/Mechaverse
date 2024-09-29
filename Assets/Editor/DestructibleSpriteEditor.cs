using SpriteDestructionSystem;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DestructableSprite))]
public class DestructibleSpriteEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        DestructableSprite sprite  = target as DestructableSprite;
        if (GUILayout.Button("Generate sprite"))
        {
            sprite.GenerateSprite();
            EditorUtility.SetDirty(sprite);
        }
    }
}