using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using LiveWorld.Mobs.Core;

[RequireComponent(typeof(Health))]
public class TestMob : MonoBehaviour
{
    public Transform target;

    public float speed;
    public float stepRotate;

    public DNA walkingCode;
    public Dictionary<int, Action<Transform, float>> moveBehaviors;

    private Transform ghost;
    private Transform point;

    private int index;

    private float power = 1.0F;
    private float hunger = 0F;

    private bool Initialized = false;


    public void Run(DNA dna)
    {
        dna.code[dna.code.Length - 1] = 0;

        moveBehaviors = new Dictionary<int, Action<Transform, float>>();

        moveBehaviors.Add(0, Stay);
        moveBehaviors.Add(1, Step);
        moveBehaviors.Add(2, Rotate);

        walkingCode = dna;

        ghost = new GameObject().transform;
        point = new GameObject().transform;

        ghost.position = transform.position;
        point.position = transform.position;

        GetComponent<Health>().OnWasted += (s) => { Destroy(gameObject); };

        Initialized = true;
    }

    public void DestroyMob()
    {
        Initialized = false;

        Destroy(ghost.gameObject);
        Destroy(gameObject);
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, point.position, speed * 2 * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, point.rotation, stepRotate * 2 * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if(!Initialized)
        {
            return;
        }

        if (target == null)
        {
            target = FindTarget();
        }
        else
        {
            if (Vector3.Distance(target.position, transform.position) < 1F)
            {
                Destroy(target.gameObject);

                hunger = 0;
            }
        }

        byte code = walkingCode.code[index];

        ghost.transform.position = transform.position;
        ghost.transform.rotation = transform.rotation;

        if (NeedAction(code))
        {
            moveBehaviors[code].Invoke(point, 1F);
        }

        index = (int)Mathf.Repeat(index + 1, walkingCode.code.Length);

        hunger += 0.04F * Time.fixedDeltaTime;

        GetComponent<Health>().Damage(this, (hunger - 0.75F));

        hunger = Mathf.Clamp01(hunger);
        power = Mathf.Clamp01(power);
    }

    private Transform FindTarget()
    {
        if (hunger > 0.2F)
        {
            var plants = (FindObjectsOfType<Plant>().OrderBy(x => Vector3.Distance(x.transform.position, transform.position))).ToArray();

            return (plants.Length > 0) ? plants[0].transform : null;
        }
        else
        {
            return null;
        }
    }

    private bool NeedAction(byte actionCode)
    {
        if (target == null)
        {
            return false;
        }

        moveBehaviors[actionCode].Invoke(ghost.transform, 1F);

        float angle = Vector3.Angle(transform.forward, target.position - transform.position);
        float distance = Vector3.Distance(transform.position, target.position);

        float ghostAngle = Vector3.Angle(ghost.transform.forward, target.position - ghost.transform.position);
        float ghostDistance = Vector3.Distance(ghost.transform.position, target.position);

        return 
            (ghostAngle < angle) || 
            (ghostDistance < distance);
    }

    public void Stay(Transform target, float value)
    {
        power += (1F - hunger) * 0.03F * Time.fixedDeltaTime;
    }

    public void Step(Transform target, float value)
    {
        power -= 0.03F * hunger * Time.fixedDeltaTime;

        target.position += target.forward * power * speed;
    }

    public void Rotate(Transform target, float value)
    {
        value = GetSide(target.forward, this.target.position - target.position);

        target.Rotate(0, value * stepRotate, 0);
    }

    public float GetSide(Vector3 a, Vector3 b)
    {
        //  x  y  z
        // ax ay az
        // bx by bz

        // x(ay*bz - az*by) - y(ax*bz - az*bx) + z(ax*by - ay*bx);
        // (ay*bz - az*by) - (ax*bz - az*bx) + (ax*by - ay*bx);

        return Math.Sign((a.y * b.z - a.z * b.y) - (a.x * b.z - a.z * b.x) + (a.x * b.y - a.y * b.x));

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;

        Gizmos.DrawLine(transform.position, point.position);

        Gizmos.color = Color.green;

        if (target)
        {
            Gizmos.DrawLine(transform.position, target.position);
        }
    }
}
