using System;
using System.Collections.Generic;

using UnityEngine;

namespace UnityTimer
{
    public class QueueItem
    {
        public int ID;
        public Action action;

        public void Invoke()
        {
            action?.Invoke();
        }
    }

    public class ThreadDispathcher : MonoBehaviour
    {
        private static List<QueueItem> s_executionQueue = new List<QueueItem>();

        public static void Invoke(int ID, Action callback)
        {
            lock (s_executionQueue)
            {
                QueueItem item = s_executionQueue.Find(x => x.ID == ID);

                if(item == null)
                {
                    item = new QueueItem();
                    item.ID = ID;
                    item.action = callback;

                    s_executionQueue.Add(item);
                }
                else
                {
                    item.action = callback;
                }
            }
        }

        private void Update()
        {
            lock (s_executionQueue)
            {
                while (s_executionQueue.Count > 0)
                {
                    s_executionQueue[0].Invoke();
                    s_executionQueue.RemoveAt(0);
                }
            }
        }
    }
}
