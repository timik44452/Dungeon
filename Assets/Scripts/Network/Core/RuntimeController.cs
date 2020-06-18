using System;
using UnityEngine;

namespace UnityTimer
{
    public class RuntimeController : MonoBehaviour
    {
        public event Action OnApplicationQuitEvent;
        public event Action OnApplicationUpdateEvent;


        private void Update()
        {
            this.OnApplicationUpdateEvent?.Invoke();
        }

        private void OnApplicationQuit()
        {
            this.OnApplicationQuitEvent?.Invoke();
        }
    }
}