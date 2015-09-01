namespace AntsAlg.AntsAlgorithm.Rules
{
    public interface IRule<in TIn, out TOut>
    {
        TOut Proccess(TIn input);
    }
}