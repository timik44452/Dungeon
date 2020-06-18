using UnityEngine;
using System.Collections.Generic;
using LiveWorld.NeuralNetworkCore;

public class TestMobNN : MonoBehaviour
{
    public Transform target;

    public float walkingSpeed = 1.0F;
    public float rotationSpeed = 1.0F;

    NeuralNet network;

    float distance;
    float angle;

    const int inputSize = 4;
    const int outputSize = 2;
    const float threshould = 0.24F;

    private void Start()
    {
        network = new NeuralNet(inputSize, inputSize * outputSize, outputSize, numHiddenLayers: inputSize);

        network.Train(GetSets(), 200);
    }

    private void FixedUpdate()
    {
        Vector3 direction = (target.position - transform.position).normalized;

        float angle = Vector3.Angle(direction, transform.forward);
        float distance = (transform.position - target.position).normalized.magnitude;

        float x = direction.x;
        float z = direction.z;

        float dis_delta = this.distance - distance;
        float angle_delta = this.angle - angle;

        this.distance = distance;
        this.angle = angle;

        Run(x, z, dis_delta, angle_delta);
    }

    private void Run(params float[] input)
    {
        if (input.Length != inputSize)
            throw new System.Exception("Debil 1");

        float[] output = network.Run(input);

        if (output.Length != outputSize)
            throw new System.Exception("Debil 2");

        //Stay(transform, output[0]);
        Step(transform, output[0] * 2 - 1);
        Rotate(transform, output[1] * 2 - 1);
    }

    private void Stay(Transform target, float value)
    {
    }

    private void Step(Transform target, float value)
    {
        if (value < threshould)
            return;

        target.position += target.forward * value * walkingSpeed;
    }

    private void Rotate(Transform target, float value)
    {
        if (value < threshould)
            return;

        target.Rotate(0, value * rotationSpeed, 0);
    }

    private List<DataSet> GetSets()
    {
        List<DataSet> sets = new List<DataSet>();

        sets.Add(new DataSet(new float[] { 0, 0, 0, 0 }, new float[] { L(1), L(0) }));

        for (float i0 = -1; i0 < 1; i0 += 0.1F)
            for (float i1 = -1; i1 < 1; i1 += 0.1F)
                for (float i2 = -1; i2 < 1; i2 += 0.1F)
                    for (float i3 = -1; i3 < 1; i3 += 0.1F)
                    {
                        Vector3 dir = new Vector3(i0, 0, i1);

                        float dd = i2;
                        float ad = i3;

                        sets.Add(new DataSet(new float[] { dir.x, dir.z, dd, ad }, new float[] { L(dd), L(ad) }));
                    }

        return sets;
    }

    private float L(float value)
    {
        return value * 0.5f - 0.5f;
    }
}
