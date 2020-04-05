using System;
using UnityEngine;

public class TargetSystem : MonoBehaviour
{
    public event Action OnTargetChanged;

    public ITarget Target { get; private set; } = null;

    public bool TargetFix { get; private set;  } = false;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (TargetFix || ITargetIsNull(Target))
            {
                ChangeTarget();
            }

            TargetFix = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TargetFix = false;
        }

        if (TargetFix == false)
        {
            FindActualTarget();
        }
    }

    private void FindActualTarget()
    {
        var viewedTargets = SceneUtility.Targets.FindAll(target => CheckTarget_RECTMETHOD(target));

        float minimalDistance = float.MaxValue;

        ITarget currentTarget = null;

        foreach (ITarget target in viewedTargets)
        {
            float distance = Vector3.Distance(SceneUtility.Player.transform.position, target.transform.position);

            if (minimalDistance > distance)
            {
                currentTarget = target;

                minimalDistance = distance;
            }
        }

        ChangeTarget(currentTarget);
    }

    private void ChangeTarget()
    {
        var viewedTargets = SceneUtility.Targets.FindAll(target => CheckTarget_ANGLEMETHOD(target));

        int index = 0;

        if (!ITargetIsNull(Target))
        {
            index = viewedTargets.IndexOf(Target) + 1;
        }

        if (index >= viewedTargets.Count)
        {
            index = 0;
        }

        if(viewedTargets.Count > 0 && index < viewedTargets.Count)
        {
            ChangeTarget(viewedTargets[index]);
        }
    }

    public void ChangeTarget(ITarget target)
    {
        if (target == Target)
        {
            return;
        }

        Target = target;

        OnTargetChanged?.Invoke();
    }

    private bool CheckTarget_RECTMETHOD(ITarget target)
    {
        if (ITargetIsNull(target))
        {
            return false;
        }

        Vector2 viewportSize = new Vector2(Screen.width * 0.5F, Screen.height * 0.75F);

        Rect viewport = new Rect((Screen.width - viewportSize.x) * 0.5F, (Screen.height - viewportSize.y) * 0.5F, viewportSize.x, viewportSize.y);

        return CheckTarget_ANGLEMETHOD(target) && viewport.Contains(Camera.main.WorldToScreenPoint(target.transform.position));
    }

    private bool CheckTarget_ANGLEMETHOD(ITarget target)
    {
        if (ITargetIsNull(target))
        {
            return false;
        }

        Vector3 targetDirection = (target.transform.position - Camera.main.transform.position).normalized;
        Vector3 cameraDirection = Camera.main.transform.forward;

        targetDirection.y = 0;
        cameraDirection.y = 0;

        return Vector3.Angle(targetDirection, cameraDirection) < Camera.main.fieldOfView;
    }

    public static bool ITargetIsNull(ITarget target)
    {
        return !(target as MonoBehaviour);
    }
}
