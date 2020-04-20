using System.Linq;
using LiveWorld.NeuralNetworkCore;

namespace LiveWorld.Mobs.Core.BehaviourModels.Examples
{
    public class AgressiveBehaviourModel : BehaviourModel
    {
        public AgressiveBehaviourModel(DNA dna) : 
            base(dna, new Feeling()
            {
                ["fear"] = 0.25F,
                ["agression"] = 0.35F,
                ["interest"] = 0.5F
            })
        {
            int sensesCount = Feeling.EmptyFeeling.GetSenses().Count();

            var reactionNetwork = new NeuralNet(sensesCount, 5, dna.code.Length, 3);
            var feelingNetwork = new NeuralNet(sensesCount, 5, 1, 2);

            var reactionDataset = new DataSet[]
            {
                new DataSet(new float[]{ 0, 0, 0 }, new float[]{ 1, 0, 0, 0 }),
                new DataSet(new float[]{ 1, 0, 0 }, new float[]{ 0, 0, 1, 0 }),
                new DataSet(new float[]{ 0, 1, 0 }, new float[]{ 0, 0, 0, 1 }),
                new DataSet(new float[]{ 0, 0, 1 }, new float[]{ 0, 1, 0, 0 }),
            };

            var feelingDataset = new DataSet[]
            {
                new DataSet(new float[]{ 0, 0, 0 }, new float[]{ 0 }),
                new DataSet(new float[]{ 1, 0, 0 }, new float[]{ 1 }),
                new DataSet(new float[]{ 0, 1, 0 }, new float[]{ 0.75F }),
                new DataSet(new float[]{ 0, 0, 1 }, new float[]{ 0 }),
            };

            reactionNetwork.Train(reactionDataset.ToList(), 0.05F);
            feelingNetwork.Train(feelingDataset.ToList(), 0.05F);

            reactionWorker = reactionNetwork;
            feelingWorker = feelingNetwork;
        }
    }
}
