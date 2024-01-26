using System;

namespace Naf.ResultObjects;

public class Testing
{
    public void Test()
    {
        // to create success result
        var result = Result
            .Success()
            .WithMessage("Sample");
        if (result.IsSuccess)
        {
            // do something
        }

        // to create a failure result
        var result2 = Result
            .Failure()
            .WithMessage("Sample")
            .WithLogTrace("")
            .WithError("","");
        

        var result1 = Result
            .Success<int>(4344)
            .WithMessage("Sample");
        if (result1.IsSuccess)
        {
            var value = result1;
        }

        try
        {

        }
        catch (Exception e)
        {
            
        }
    }   
}