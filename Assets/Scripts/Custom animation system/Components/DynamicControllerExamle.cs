using System.Collections.Generic;
using UnityEngine;

public class DynamicControllerExamle : MonoBehaviour
{
    public float speed = 1.0F;
    public float length = 1.0F;

    public Transform[] foots;
    private Dictionary <Transform, Vector3> footsTemp;

    private MobDynamicAnimationExample dynamicAnimationExample;

    private void Start()
    {
        footsTemp = new Dictionary<Transform, Vector3>();
        dynamicAnimationExample = GetComponent<MobDynamicAnimationExample>();

        foreach (var foot in foots)
        {
            footsTemp.Add(dynamicAnimationExample.GetJoint(foot.name), foot.transform.position);
        }
    }

    private void Update()
    {
        Vector3 moveDirection =
            Vector3.forward * Input.GetAxis("Vertical") +
            Vector3.right * Input.GetAxis("Horizontal");

        if (moveDirection.magnitude > 0.05F)
        {
            foreach (var foot in footsTemp)
            {
                if (Vector3.Distance(foot.Key.position, foot.Value) > length)
                {
                    footsTemp[foot.Key] = foot.Key.position;
                }

                Vector3 localPosition = foot.Key.position - transform.position;

                dynamicAnimationExample.UpdatePosition(localPosition + moveDirection * speed, foot.Key);
            }
        }
    }
}
