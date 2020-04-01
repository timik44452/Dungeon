using System.Collections.Generic;
using UnityEngine;

namespace InputSystem
{
    [CreateAssetMenu]
    public class InputProfile : ScriptableObject
    {
        public List<KeyData> keys = new List<KeyData>();
    }
}
