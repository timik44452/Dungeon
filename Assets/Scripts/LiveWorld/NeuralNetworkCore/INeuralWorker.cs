namespace LiveWorld.NeuralNetworkCore
{
    public interface INeuralWorker
    {
        float[] Run(params float[] input);
    }
}
