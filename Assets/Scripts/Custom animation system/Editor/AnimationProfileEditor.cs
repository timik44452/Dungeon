using UnityEditor;
using UnityEngine;

public class AnimationProfileEditor : MonoBehaviour
{
    public static AnimationEditorWindow currentWindow
    {
        get => EditorWindow.GetWindow<AnimationEditorWindow>(typeof(SceneView));
    }

    [MenuItem("Window/Custom animation editor")]
    public static void ShowWindow()
    {
        currentWindow.Show();
    }
}
