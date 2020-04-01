using System;
using System.Collections.Generic;
using UnityEngine;

public class TargetSystem : MonoBehaviour
{
    public event Action OnTargetChanged;

    public ITarget Target { get; private set; } = null;

    public bool TargetFix { get; private set;  } = false;

    private List<ITarget> m_targets = null;

    private void Start()
    {
        m_targets = new List<ITarget>(SceneUtility.Targets);    
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (TargetFix || Target == null)
            {
                ChangeTarget();
            }

            TargetFix = true;
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            TargetFix = false;
        }

        if (TargetFix == false)
        {
            FindActualTarget();
        }
    }

    public void FindActualTarget()
    {
        var viewedTargets = m_targets.FindAll(target => CheckTarget_RECTMETHOD(target));

        float minimalDistance = float.MaxValue;
        ITarget currentTarget = null;

        foreach (ITarget target in viewedTargets)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);

            if (minimalDistance > distance)
            {
                currentTarget = target;

                minimalDistance = distance;
            }
        }

        ChangeTarget(currentTarget);
    }

    public void ChangeTarget()
    {
        var viewedTargets = m_targets.FindAll(target => CheckTarget_ANGLEMETHOD(target));

        int index = 0;

        if (Target != null)
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
        if(Target == target)
        {
            return;
        }

        Target = target;

        OnTargetChanged?.Invoke();
    }

    private bool CheckTarget_RECTMETHOD(ITarget target)
    {
        Vector2 viewportSize = new Vector2(Screen.width * 0.5F, Screen.height * 0.75F);

        Rect viewport = new Rect((Screen.width - viewportSize.x) * 0.5F, (Screen.height - viewportSize.y) * 0.5F, viewportSize.x, viewportSize.y);

        return CheckTarget_ANGLEMETHOD(target) && viewport.Contains(Camera.main.WorldToScreenPoint(target.transform.position));
    }

    private bool CheckTarget_ANGLEMETHOD(ITarget target)
    {
        Vector3 targetDirection = (target.transform.position - Camera.main.transform.position).normalized;
        Vector3 cameraDirection = Camera.main.transform.forward;

        targetDirection.y = 0;
        cameraDirection.y = 0;

        return Vector3.Angle(targetDirection, cameraDirection) < Camera.main.fieldOfView;
    }
}
