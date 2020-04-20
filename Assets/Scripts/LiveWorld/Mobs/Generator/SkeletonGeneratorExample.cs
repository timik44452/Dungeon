using UnityEngine;

using LiveWorld.Mobs;
using LiveWorld.NeuralNetworkCore;


public class SkeletonGeneratorExample : MonoBehaviour
{
    private Skeleton skeleton;

    private void Start()
    {
        skeleton = CreateSkeleton();

        CreateSkeletonGameObject(skeleton);
    }

    private Skeleton CreateSkeleton()
    {
        // ----- NEURAL NETWORK MODEL -----
        // INPUT    HIDE0   HIDE1   OUTPUT
        //   2       15       15      2
        // --------------------------------
        // IN0: SKELETON POSITION
        // IN1: HAND INDEX

        // OUT0: ADD LEG  (if OUT0 > threshould)
        // OUT1: ADD HAND (if OUT1 > threshould)

        //TODO: Need more skeleton params
        //TODO: Need more skeleton models
        //TODO: Need auto symmetric builder

        Skeleton skeleton = new Skeleton();

        Vector3 direction = Vector3.forward;

        int jointsCount = 10;
        int limbCount = 1;
        float maxLimbCount = 10;
        float step = 1 / maxLimbCount;
        var net = new NeuralNet(2, 15, 2, 2);

        net.Train(new System.Collections.Generic.List<DataSet>()
        {
            new DataSet(new float[]{ 0.00F, 1 / maxLimbCount }, new float[] { 0, 0 }),
            new DataSet(new float[]{ 0.25F, 1 / maxLimbCount }, new float[] { 1, 0 }),
            new DataSet(new float[]{ 0.40F, 1 / maxLimbCount }, new float[] { 0, 0 }),

            new DataSet(new float[]{ 0.0F, 2 / maxLimbCount }, new float[] { 0, 0 }),
            new DataSet(new float[]{ 0.2F, 2 / maxLimbCount }, new float[] { 1, 0 }),
            new DataSet(new float[]{ 0.4F, 2 / maxLimbCount }, new float[] { 0, 0 }),
        }, 0.05F);

        INeuralWorker neuralWorker = net;

        for (int index = 0; index < jointsCount; index++)
        {
            float alpha = index / (float)jointsCount;

            MobJoint joint = new MobJoint($"Joint {index}", direction * alpha);

            skeleton.AddJoint(joint);

            skeleton.AddBone($"Joint {index}", $"Joint {index - 1}");

            var result = neuralWorker.Run(alpha, limbCount / maxLimbCount);

            if (result[0] > 0.5F)
            {
                MobJoint RLegJoint0 = new MobJoint($"RLeg joint {index}0", Vector3.right * step + direction * alpha);
                MobJoint RLegJoint1 = new MobJoint($"RLeg joint {index}1", Vector3.right * step * 2 + direction * alpha);
                MobJoint RLegJoint2 = new MobJoint($"RLeg joint {index}2", Vector3.right * step * 3 + direction * alpha);

                MobJoint LLegJoint0 = new MobJoint($"LLeg joint {index}0", Vector3.left * step + direction * alpha);
                MobJoint LLegJoint1 = new MobJoint($"LLeg joint {index}1", Vector3.left * step * 2 + direction * alpha);
                MobJoint LLegJoint2 = new MobJoint($"LLeg joint {index}2", Vector3.left * step * 3 + direction * alpha);

                skeleton.AddJoint(
                    RLegJoint0, RLegJoint1, RLegJoint2, 
                    LLegJoint0, LLegJoint1, LLegJoint2);

                skeleton.AddBone($"Joint {index}", $"RLeg joint {index}0");
                skeleton.AddBone($"RLeg joint {index}0", $"RLeg joint {index}1");
                skeleton.AddBone($"RLeg joint {index}1", $"RLeg joint {index}2");

                skeleton.AddBone($"Joint {index}", $"LLeg joint {index}0");
                skeleton.AddBone($"LLeg joint {index}0", $"LLeg joint {index}1");
                skeleton.AddBone($"LLeg joint {index}1", $"LLeg joint {index}2");
            }

            if (result[1] > 0.5F)
            {
                MobJoint RHandJoint0 = new MobJoint($"RHand joint {index}0", Vector3.right * step + direction * alpha);
                MobJoint RHandJoint1 = new MobJoint($"RHand joint {index}1", Vector3.right * step * 2 + direction * alpha);
                MobJoint RHandJoint2 = new MobJoint($"RHand joint {index}2", Vector3.right * step * 3 + direction * alpha);

                MobJoint LHandJoint0 = new MobJoint($"LHand joint {index}0", Vector3.left * step + direction * alpha);
                MobJoint LHandJoint1 = new MobJoint($"LHand joint {index}1", Vector3.left * step * 2 + direction * alpha);
                MobJoint LHandJoint2 = new MobJoint($"LHand joint {index}2", Vector3.left * step * 3 + direction * alpha);

                skeleton.AddJoint(
                    RHandJoint0, RHandJoint1, RHandJoint2,
                    LHandJoint0, LHandJoint1, LHandJoint2);

                skeleton.AddBone($"Joint {index}", $"RHand joint {index}0");
                skeleton.AddBone($"RHand joint {index}0", $"RHand joint {index}1");
                skeleton.AddBone($"RHand joint {index}1", $"RHand joint {index}2");

                skeleton.AddBone($"Joint {index}", $"LHand joint {index}0");
                skeleton.AddBone($"LHand joint {index}0", $"LHand joint {index}1");
                skeleton.AddBone($"LHand joint {index}1", $"LHand joint {index}2");
            }
        }

        return skeleton;
    }

    private void CreateSkeletonGameObject(Skeleton skeleton)
    {
        foreach (var joint in skeleton.GetJoints())
        {
            GameObject jointGameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);

            jointGameObject.transform.localScale = Vector3.one * 0.025F;
            jointGameObject.transform.position = transform.position + joint.localPosition;

            jointGameObject.transform.parent = transform;
        }
    }

    private void OnDrawGizmosSelected()
    {
        skeleton?.DrawGizmos(transform.position, true, true, 0.02F);
    }
}
