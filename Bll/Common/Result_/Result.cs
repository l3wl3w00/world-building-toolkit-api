using Azure.Core;
using Bll.Auth.Dto;
using Bll.Common.Option_;

namespace Bll.Common.Result_;

public static class Result
{
    public static Result<TOkValue, TError> Ok<TOkValue, TError>(TOkValue okValue) 
        where TError : System.Exception 
        => new(okValue);
    public static Result<TOkValue, TError> Err<TOkValue, TError>(TError error) 
        where TError : System.Exception
        => new(error);
    
    public static Result<TOkValue> Err<TOkValue>(System.Exception error) => new(error);
    public static Result<TOkValue> Ok<TOkValue>(TOkValue okValue) => new(okValue);
}

public interface IResult<TOk, TError> where TError : System.Exception
{
    public Option<TOk> OkValueOpt { get; }
    public Option<TError> ErrorValueOpt { get; }
    public bool IsError => ErrorValueOpt.HasValue;
    public bool IsOk => !IsError;
    public IResult<TOk, TError> IfErrorIs<TOtherError>(Action<TOtherError> actionOnError);
}
public readonly struct Result<TOk, TError> : IResult<TOk, TError>
    where TError : System.Exception
{
    public Option<TOk> OkValueOpt { get; } = Option<TOk>.None;
    public Option<TError> ErrorValueOpt { get; } = Option<TError>.None;
    public bool IsError => ErrorValueOpt.HasValue;
    public bool IsOk => !IsError;

    public Result()
    {
        throw new InvalidOperationException($"The parameterless constructor of a {nameof(Result)} type should never be used");
    }

    internal Result(TError error)
    {
        ErrorValueOpt = Option<TError>.Some(error);
    }
    
    internal Result(TOk okValue)
    {
        OkValueOpt = Option<TOk>.Some(okValue);
    }
    public IResult<TOk, TError> IfErrorIs<TOtherError>(Action<TOtherError> actionOnError)
    {
        if (ErrorValueOpt is { HasValue: true, Value: TOtherError err }) 
            actionOnError(err);
        return this;
    }
}

public readonly struct Result<TOk> : IResult<TOk, System.Exception>
{
    public Option<TOk> OkValueOpt { get; } = Option<TOk>.None;
    public TOk OkValue => OkValueOpt.Value;
    public Option<System.Exception> ErrorValueOpt { get; } = Option<System.Exception>.None;
    public System.Exception ErrorValue => ErrorValueOpt.Value;
    public bool IsError => ErrorValueOpt.HasValue;
    public bool IsOk => !IsError;

    public Result()
    {
        throw new InvalidOperationException($"The parameterless constructor of a {nameof(Result)} type should never be used");
    }

    internal Result(System.Exception error)
    {
        ErrorValueOpt = Option<System.Exception>.Some(error);
    }

    internal Result(TOk okValue)
    {
        OkValueOpt = Option<TOk>.Some(okValue);
    }

    public IResult<TOk, System.Exception> IfErrorIs<TOtherError>(Action<TOtherError> actionOnError)
    {
        if (ErrorValueOpt is { HasValue: true, Value: TOtherError err }) 
            actionOnError(err);
        return this;
    }

    public bool IsErrorOut(out TOk? okValue)
    {
        okValue = OkValueOpt.NullableValue;
        return IsError;
    }

    public bool IsErrorOf<TOtherError>()
    {
        return IsError && ErrorValue is TOtherError;
    }
    public bool IsErrorOf<TOtherError>(Func<TOtherError, bool> additionalPredicate)
    {
        return IsError && ErrorValue is TOtherError err && additionalPredicate(err);
    }

    public IResult<TOk, System.Exception> OkOrMapIfErrorIs<TOtherError>(Func<TOtherError, TOk> mappingOnError)
    {
        if (IsOk) return this;
        if (ErrorValueOpt is not TOtherError err) return this;
        
        return Result.Ok(mappingOnError(err));
    }
    
    public Result<TOtherOk> ErrorOr<TOtherOk>(Func<TOk, TOtherOk> okMapping)
    {
        if (IsOk) return okMapping(OkValue).ToOkResult();
        return Into<TOtherOk>();
    }
    
    public Result<TOther> Into<TOther>()
    {
        return ErrorValueOpt.Value.ToErrorResult<TOther>();
    }
}

public static class ResultExtensions
{
    public static IResult<TOk, TError> IfOk<TOk, TError>(this IResult<TOk, TError> result, Action<TOk> actionOnOk) 
        where TError : System.Exception
    {
        if (result.IsOk) actionOnOk(result.OkValueOpt.Value);
        return result;
    }
    
    public static IResult<TOk, TError> IfError<TOk, TError>(this IResult<TOk, TError> result, Action<TError> actionOnError) 
        where TError : System.Exception
    {
        if (result.IsError) actionOnError(result.ErrorValueOpt.Value);
        return result;
    }
    
    public static TOk ThrowIfError<TOk, TError>(this IResult<TOk, TError> result) 
        where TError : System.Exception
    {
        if (result.IsError) throw result.ErrorValueOpt.Value;
        return result.OkValueOpt.Value;
    }
    
    public static async Task<TOk> ThrowIfError<TOk, TError>(this Task<IResult<TOk, TError>> resultTask) 
        where TError : System.Exception
    {
        var result = await resultTask;
        return result.ThrowIfError();
    }
    
    public static Result<TOk> ToResult<TOk>(this IResult<TOk, System.Exception> result) 
    {
        if (result.IsError) return Result.Err<TOk>(result.ErrorValueOpt.Value);
        return Result.Ok(result.OkValueOpt.Value);
    }
        
    public static Result<TOk> ToOkResult<TOk>(this TOk ok)
    {
        return Result.Ok(ok);
    }
    

    
    public static Result<TOtherOk> Cast<TOk, TOtherOk>(
        this IResult<TOk, System.Exception> result, 
        Func<TOk,TOtherOk> mapping) 
        where TOtherOk : notnull
    {
        if (result.IsError) return Result.Err<TOtherOk>(result.ErrorValueOpt.Value);
        return Result.Ok(mapping.Invoke(result.OkValueOpt.Value));
    }

    public static Result<TOk> ToErrorResult<TOk>(this System.Exception err)
    {
        return Result.Err<TOk>(err);
    }

    public static Result<TOk, TError> ToErrorResult<TOk,TError>(this TError err)
        where TError : System.Exception
    {
        return Result.Err<TOk,TError>(err);
    }
}