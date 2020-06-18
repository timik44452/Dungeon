using UnityEngine;

public class NetworkUnit : MonoBehaviour
{
    public bool isReal { get; set; } = true;
    //TODO: Setting id;
    public int ID { get; set; }

    [NetworkSync]
    public Vector3 position
    {
        get => transform.position;
        set
        {
            if(!init)
            {
                m_oldtargetPosition = value;

                init = true;

                return;
            }


            if (alpha > 0.99F)
            {
                m_oldtargetPosition = m_targetPosition;

                m_targetPosition = value;

                alpha = 0;
            }
        }
    }

    public float alpha = 0;
    private bool init = false;
    private Vector3 m_oldtargetPosition;
    private Vector3 m_targetPosition;


    private void Awake()
    {
        
    }

    public void SetID(int id)
    {
        ID = id;
        isReal = false;

        RuntimeManager.RegisterApplicationUpdateCallback(Interpolate);
    }

    private void Interpolate()
    {
        int count = (int)(0.05F / Time.deltaTime);
        float step = count * Time.deltaTime;

        step = Mathf.Clamp(step, 0.000001F, 5F * Time.deltaTime);

        transform.position = Vector3.Lerp(m_oldtargetPosition, m_targetPosition, alpha);

        alpha = Mathf.Clamp01(alpha + step);
    }
}
