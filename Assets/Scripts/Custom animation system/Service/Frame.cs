using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace CustomAnimationSystem
{
    [System.Serializable]
    public struct Frame
    {
        public float time { get => _time; }

        public List<Point> Points { get => points; }

        [SerializeField]
        private List<Point> points;

        [SerializeField]
        private float _time;


        public Frame(float time)
        {
            _time = time;
            points = new List<Point>();
        }

        public Frame(float time, IEnumerable<Point> points)
        {
            _time = time;

            this.points = points.ToList();
        }

        public bool TryGetPoint(string key, out Point point)
        {
            var temp = points.Find(x => x.key == key);

            point = temp;

            return temp != null;
        }

        public static Frame Lerp(Frame from, Frame to, float time)
        {
            time = Mathf.Clamp01(time);

            if(time == 0)
            {
                return from;
            }
            else if(time == 1)
            {
                return to;
            }

            List<Point> lerpedPoints = new List<Point>();

            from.points.ForEach(from_point =>
            {
                var to_point = to.points.Find(x => x.key == from_point.key);

                if (to_point != null)
                {
                    lerpedPoints.Add(new Point()
                    {
                        key = from_point.key,

                        localPosition = Vector3.Lerp(from_point.localPosition, to_point.localPosition, time),
                        localRotation = Vector3.Lerp(from_point.localRotation, to_point.localRotation, time),
                        localScale = Vector3.Lerp(from_point.localScale, to_point.localScale, time)
                    });
                }
            });

            return new Frame()
            {
                _time = Mathf.Lerp(from.time, to.time, time),

                points = lerpedPoints
            };
        }
    }
}
