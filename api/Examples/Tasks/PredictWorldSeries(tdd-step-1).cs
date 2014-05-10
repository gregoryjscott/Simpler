// 1. Figure out what you need to do, what inputs you'll need to supply, and the exact output you expect.

namespace Tdd.Step1
{
using Simpler;
using System;

public class PredictWorldSeries : InOutTask<PredictWorldSeries.Input, PredictWorldSeries.Output>
{
    public class Input
    {
        public int Year { get; set; }
    }

    public class Output
    {
        public Team WinningTeam { get; set; }
    }

    public override void Execute()
    {
        throw new NotImplementedException();
    }
}
}
