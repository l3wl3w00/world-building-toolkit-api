namespace Bll.Common.Result;

public abstract class Result<T, E>
{
    public abstract T? Value { get; }
    public abstract E? Error { get; }

    public abstract bool IsOk { get; }
    public abstract bool IsErr { get; }

    public static Result<T, E> Ok(T value) => new OkResult(value);
    public static Result<T, E> Err(E error) => new ErrResult(error);

    private class OkResult : Result<T, E> 
    {
        public override T? Value { get; }
        public override E? Error => default;

        public override bool IsOk => true;
        public override bool IsErr => false;

        public OkResult(T value) => Value = value;
    }

    private class ErrResult : Result<T, E>
    {
        public override T? Value => default;
        public override E? Error { get; }

        public override bool IsOk => false;
        public override bool IsErr => true;

        public ErrResult(E error) => Error = error;
    }
}