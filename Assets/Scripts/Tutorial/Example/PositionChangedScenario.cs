using UnityEngine;

[CreateAssetMenu(menuName = "Scenarious/Tutorial/PositionChangedScenario")]
public class PositionChangedScenario : TutorialScenario
{
    public float speed = 1.0F;
    public float offset = 1.0F;

    public bool useVerticalAxis = true;
    public bool useHorizontalAxis = true;

    private float time = float.MaxValue;
    private Vector3 oldPosition;

    public override void Start()
    {
        oldPosition = SceneUtility.Player.transform.position;
    }

    public override bool Update()
    {
        if(SceneUtility.Player == null)
        {
            return false;
        }

        Transform target = SceneUtility.Player.transform;

        Vector3 currentDelta = target.position - oldPosition;

        if (currentDelta.magnitude >= offset)
        {
            if (currentDelta.magnitude / time >= speed)
            {
                return true;
            }

            oldPosition = target.position;
            time = 0;
        }

        time += Time.deltaTime;

        return false;
    }
}
