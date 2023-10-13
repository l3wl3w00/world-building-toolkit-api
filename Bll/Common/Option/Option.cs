using System.Collections;
using System.Runtime.CompilerServices;
using Bll.Auth.Exception;
using Bll.Common.Exception;
using Google.Apis.Util;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.VisualBasic.CompilerServices;

namespace Bll.Common.Option;



public readonly record struct Option<T>
{
    private readonly T? _value;
        
    private Option(T? value, bool hasValue)
    {
        HasValue = hasValue;
        _value = value;
    }

    public T Value
    {
        get
        {
            if (NoValue) throw new ValueNotFoundException();
            return _value!;
        }
    }

    public bool HasValue { get; }

    public bool NoValue => !HasValue;
    public static Option<T> None => new(default, false);

    public static Option<T> Some(T value) => new(value, true);

    public T? NullableValue { get; init; }

    public T MapIfNull<E>(Func<E> func) where E : T
    {
        if (HasValue) return Value;
        return func();
    }

    public Option<T> DoIfNotNull(Action<T> action)
    {
        if (HasValue) action(Value);
        return this;
    }
    public async Task<Option<T>> DoIfNotNullAsync(Func<T, Task> action)
    {
        if (HasValue) await action(Value);
        return this;
    }

    public T AssertNotNull<E>(E exception) where E : System.Exception
    {
        if (NoValue) throw exception;
        return Value;
    }
    
    public static Option<T> FromNullable(T? value)
    {
        if (value == null) return None;
        return Some(value);
    }

    public Option<T1> Cast<T1>() where T1: T
    {
        if (HasValue)
        {
            var value = (T1)Value;
            if (value == null) throw new InvalidCastException();
            return Option<T1>.Some(value);
        }

        return Option<T1>.None;
    }
}

public class ValueNotFoundException() : System.Exception("Getting the value of optional failed, because value was none");

public static class OptionsExtension
{
    public static Option<T> ToOption<T>(this T? t)
    {
        return Option<T>.FromNullable(t);
    }
        
    public static Option<T> ToOption<T>(this T? t) where T : struct
    {
        if (t.HasValue) return Option<T>.Some(t.Value);
        return new Option<T>();
    }
    public static async Task<T> AssertNotNullAsync<T>(this Task<Option<T>> task, System.Exception exceptionToThrow)
    {
        return (await task).AssertNotNull(exceptionToThrow);
    }
    public static Option<T2> Cast<T1,T2>(this T1 t1) where T2 : class => (t1 as T2).ToOption();
}