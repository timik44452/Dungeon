using UnityEngine;

using CustomAnimationSystem;

public class AnimationBody : MonoBehaviour
{
    public AnimationProfile profile;

    public float Speed = 1.0F;

    public bool isPlaying;
    private float time;
    private Transform[] bones;

    public bool use_scale = true;
    public bool use_position = true;
    public bool user_rotation = true;

    private void Start()
    {
        bones = transform.GetComponentsInChildren<Transform>();
    }

    private void FixedUpdate()
    {
        if (isPlaying)
        {
            time += Speed * Time.fixedDeltaTime;

            UpdateAnimation();

            if (time > profile.End)
                time = profile.Start;
        }
    }

    public void Play(float time)
    {
        isPlaying = true;
        this.time = time;
    }

    public void Pause()
    {
        isPlaying = false;
    }

    public void Stop()
    {
        isPlaying = false;
        time = 0;
        UpdateAnimation();
    }

    public void UpdateAnimation()
    {
        var frame = profile.Evaluate(time);

        foreach (Transform bone in bones)
        {
            if (frame.TryGetPoint(bone.name, out Point point))
            {
                if (use_position)
                {
                    bone.localPosition = point.localPosition;
                }

                if (user_rotation)
                {
                    bone.localRotation = (point.localRotation == Vector3.zero) ? Quaternion.identity : Quaternion.Euler(point.localRotation);
                }

                if (use_scale)
                {
                    bone.localScale = point.localScale;
                }
            }
        }
    }
}
