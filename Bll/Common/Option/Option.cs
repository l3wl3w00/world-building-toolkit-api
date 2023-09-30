using System.Collections;
using System.Runtime.CompilerServices;
using Bll.Auth.Exception;
using Google.Apis.Util;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.VisualBasic.CompilerServices;

namespace Bll.Common.Option;

public abstract class Option<T> : IEnumerable<T>
{
    public abstract T Value { get; }

    public abstract bool HasValue { get; }
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

    public static Option<T> Some(T value) => new Some<T>(value);
    public static Option<T> None => new None<T>();
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

class Some<T>(T value) : Option<T>
{
    public override T Value { get; } = value;

    public override bool HasValue => true;
}

class None<T> : Option<T>
{
    public override T Value => throw new ValueNotFoundException();
    public override bool HasValue => false;
}

public class ValueNotFoundException() : System.Exception("Getting the value of optional failed, because value was none");