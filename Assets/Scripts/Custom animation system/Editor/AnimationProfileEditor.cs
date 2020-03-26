using UnityEditor;
using UnityEngine;

public class AnimationProfileEditor : MonoBehaviour
{
    public static AnimationEditorWindow currentWindow
    {
        get => EditorWindow.GetWindow<AnimationEditorWindow>(typeof(SceneView));
    }
    public static AnimationProfile animationProfile
    {
        get
        {
            var tempProfile = Selection.activeObject as AnimationProfile;

            if(tempProfile != null)
            {
                s_animationProfile = tempProfile;
            }

            return s_animationProfile;
        }

        set => s_animationProfile = value;
    }

    private static AnimationProfile s_animationProfile = null;


    [MenuItem("Assets/Edit", true)]
    public static bool ShowWindowValidate()
    {
        return Selection.activeObject is AnimationProfile;
    }

    [MenuItem("Assets/Edit")]
    public static void ShowWindowWithContext()
    {
        animationProfile = Selection.activeObject as AnimationProfile;

        ShowWindow();
    }

    [MenuItem("Window/Custom animation editor")]
    public static void ShowWindow()
    {
        currentWindow.Show();
    }
}
