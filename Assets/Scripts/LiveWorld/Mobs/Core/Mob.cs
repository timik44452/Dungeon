using UnityEngine;

using Mobs.Core;

public abstract class Mob : MonoBehaviour
{
    protected MobTask currentTask { get; set; }
    protected BehaviourModel currentModel { get; }


    public Mob(BehaviourModel model)
    {
        currentModel = model;
    }
}
