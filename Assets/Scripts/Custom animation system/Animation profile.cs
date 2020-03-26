using System.Linq;
using CustomAnimationSystem;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AnimationProfile : ScriptableObject
{
    public List<Frame> Frames { get => frames; }

    public float Start
    {
        get => frames.Count == 0 ? 0 : frames.Min(x => x.time);
    }
    public float End
    {
        get => frames.Count == 0 ? 0 : frames.Max(x => x.time);
    }

    [SerializeField]
    private List<Frame> frames;

    public Frame Evaluate(float time)
    {
        time = Mathf.Clamp(time, Start, End);

        Frame after_frame = frames[0];
        Frame before_frame = frames[0];

        for (int index = 0; index < frames.Count; index++)
        {
            if (frames[index].time == time)
            {
                return frames[index];
            }

            if(frames[index].time > time && (after_frame.time < time || frames[index].time < after_frame.time))
            {
                after_frame = frames[index];
            }
            if (frames[index].time < time && (before_frame.time > time || frames[index].time > before_frame.time))
            {
                before_frame = frames[index];
            }
        }

        float local_time = 1.0F + (time - after_frame.time) / (after_frame.time - before_frame.time);

        return Frame.Lerp(before_frame, after_frame, local_time);
    }
}
