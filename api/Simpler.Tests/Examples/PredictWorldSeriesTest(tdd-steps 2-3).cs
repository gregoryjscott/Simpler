using NUnit.Framework;
using Simpler;
using System;

// 2. Write a test.

[TestFixture]
public class PredictWorldSeriesTest
{
    [Test]
    public void does_not_predict_Cubs()
    {
        // Try every year since 1908 just to be sure.
        var year = 1908;
        var currentYear = DateTime.Now.Year;
        var predict = Task.New<PredictWorldSeries>();
        while (++year < currentYear)
        {
            predict.In.Year = year;
            predict.Execute();
            Assert.That(predict.Out.WinningTeam.Name, Is.Not.EqualTo("Cubs"));
        }
    }
}

// 3. Verify the test fails.

//```
//    ....F...................................................
//    Tests run: 58, Errors: 1, Failures: 0, Inconclusive: 0, Time: 1.3669102 seconds
//    Not run: 0, Invalid: 0, Ignored: 0, Skipped: 0
//
//    Errors and Failures:
//    1) Test Error : PredictWorldSeriesTest.does_not_predict_Cubs
//    System.NullReferenceException : Object reference not set to an instance of an object
//
//    ```
