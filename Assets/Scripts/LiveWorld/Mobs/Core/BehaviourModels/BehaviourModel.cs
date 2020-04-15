using System.Linq;
using System.Collections.Generic;
using LiveWorld.NeuralNetworkCore;

namespace LiveWorld.Mobs.Core.BehaviourModels
{
    public abstract class BehaviourModel
    {
        public DNA dna { get; }
        public Feeling EmptyFeeling { get; }
        public Feeling Feeling { get; }

        protected INeuralWorker feelingWorker;
        protected INeuralWorker reactionWorker;

        public BehaviourModel(DNA dna, Feeling EmptyFeeling)
        {
            this.dna = dna;
            this.EmptyFeeling = EmptyFeeling;

            Feeling = new Feeling()
            {
                ["interesting"] = 1.0F,
                ["agression"] = 0.0F,
                ["fear"] = 0.0F
            };
        }

        public bool MoreImportant(Feeling a, Feeling b)
        {
            return 
                feelingWorker.Run(a.GetSenses().ToArray())[0] > 
                feelingWorker.Run(b.GetSenses().ToArray())[0];
        }
        public IEnumerable<byte> Reaction(Feeling currentFeeling)
        {
            var result = reactionWorker.Run(currentFeeling.GetSenses().ToArray());

            for (int index = 0; index < result.Length; index++)
            {
                if(result[index] > 0.55F)
                {
                    yield return dna.code[index];
                }
            }
        }
    }
}
