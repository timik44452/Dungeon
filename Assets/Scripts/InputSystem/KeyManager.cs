using UnityEngine;

using InputSystem;

public class KeyManager : MonoBehaviour
{
    public InputProfile profile;

    private void Start()
    {
        if (profile == null)
        {
            profile = Resources.Load<InputProfile>("defaultInputProfile");
        }
    }

    private void Update()
    {
        
    }
}
