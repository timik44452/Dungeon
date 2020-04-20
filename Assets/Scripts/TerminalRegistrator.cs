using UnityEngine;

public class TerminalRegistrator : MonoBehaviour
{
    private void Start()
    {
        Terminal.Enviropment.AddVariable<bool>("Debug", value => SceneUtility.IsDebug = value);
    }
}
