namespace Services.Models
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public string Error { get; }
        public T Value { get; }

        private Result(bool isSuccess, string error, T value)
        {
            IsSuccess = isSuccess;
            Error = error;
            Value = value;
        }

        public static Result<T> GetSuccess(T value)
        {
            return new Result<T>(true, string.Empty, value);
        }

        public static Result<T> GetFailure(string error)
        {
            return new Result<T>(false, error, default);
        }
    }
}
