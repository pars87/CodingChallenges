namespace asyncAwaitPoc;

using System;
using LanguageExt.Common;

public abstract class Helper
{
    public static Result<string> Ok()
        => Ok("");

    public static Result<string> Ok(string value)
        => new(value);
    
    public static Result<string> Fail(string error)
        => new(new Exception(error));
}