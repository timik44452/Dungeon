using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/MagickStick")]
public class MagickStick : Weapon
{
    private GameObject recognitionGameObject;

    public override void BeginInvoke(Component sender, ITarget target, Skill skill)
    {
        if (recognitionGameObject == null)
        {
            recognitionGameObject = new GameObject("Temp recognizer");

            recognitionGameObject.AddComponent<Recognition>();
        }
    }

    public override void EndInvoke(Component sender, ITarget target, Skill skill)
    {
        DestroyRecognizer();
    }

    private void DestroyRecognizer()
    {
        if (recognitionGameObject)
        {
            Destroy(recognitionGameObject);
        }
    }
}
