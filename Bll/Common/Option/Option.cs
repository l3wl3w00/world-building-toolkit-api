using System.Collections;
using System.Runtime.CompilerServices;
using Bll.Auth.Exception;
using Google.Apis.Util;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.VisualBasic.CompilerServices;

namespace Bll.Common.Option;

public readonly record struct Option<T>(T? NullableValue) : IEnumerable<T>
{
    public T Value
    {
        get
        {
            if (NoValue) throw new ValueNotFoundException();
            return NullableValue!;
        }
    }

    public bool HasValue => NullableValue != null;
    public bool NoValue => !HasValue;
    
    public static implicit operator Option<T>(T? t)
    {
        return FromNullable(t);
    }

    public static Option<T> FromNullable(T? value)
    {
        if (value == null)  return None;
        return Some(value);
    }

    public static Option<T> Some(T value) => new Option<T>(value);
    public static Option<T> None => new(default);
    public IEnumerator<T> GetEnumerator()
    {
        if (HasValue) yield return Value;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public T MapIfNull<E>(Func<E> func) where E : T
    {
        if (HasValue) return Value;
        return func();
    }
}

public class ValueNotFoundException() : System.Exception("Getting the value of optional failed, because value was none");