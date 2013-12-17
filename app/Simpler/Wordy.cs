namespace Simpler.Wordy
{
    public abstract class Task : T { }
    public abstract class InTask<TIn> : Simpler.I<TIn> { }
    public abstract class OutTask<TOut> : Simpler.O<TOut> { }
    public abstract class InOutTask<TIn, TOut> : IO<TIn, TOut> { }
}
