using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LiveWorld.Mobs.Core
{
    public class Feeling
    {
        public static Feeling RandomFeeling
        {
            get => new Feeling(true);
        }
        public static Feeling EmptyFeeling
        {
            get => new Feeling();
        }


        private Dictionary<string, float> senses;


        public Feeling(bool random = false)
        {
            senses = new Dictionary<string, float>()
            {
                { "fear", random ? Random.value : 0 },
                { "agression", random ? Random.value : 0 },
                { "interest", random ? Random.value : 0 }
            };
        }

        public IEnumerable<float> GetSenses()
        {
            return senses.Values;
        }

        public float this[string name]
        {
            get => senses[name];
            set => senses[name] = Mathf.Clamp01(value);
        }

        public override string ToString()
        {
            string result = string.Empty;

            foreach (var sense in senses)
            {
                result += $"{sense.Key}:{sense.Value}";
            }

            return result;
        }
    }
}
