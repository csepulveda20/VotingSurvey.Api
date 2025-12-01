namespace VotingSurvey.Application.Models
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public T? Data { get; set; } = default;
        public string? Message { get; set; } = default;
        public IReadOnlyList<string>? Errors { get; set; } = default;
        public IReadOnlyDictionary<string, string>? MessageErrors { get; set; }

        public ApiResponse()
        {
        }

        private ApiResponse(T data, bool isSuccess = true, string? message = null)
        {
            IsSuccess = isSuccess;
            Data = data;
            Message = message;
        }

        private ApiResponse(IReadOnlyList<string>? errors = null, bool isSuccess = false)
        {
            IsSuccess = isSuccess;
            Errors = errors;
        }

        public static ApiResponse<T> Success(T data) => new(data);

        public static ApiResponse<T> Success(T data, string? message = null) => new(data, true, message);

        public static ApiResponse<T> Failure(IReadOnlyList<string>? errors) => new(errors, false);

        public static ApiResponse<T> Failure(IReadOnlyDictionary<string, string>? messageErrors)
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                MessageErrors = messageErrors
            };
        }

        public TResult Match<TResult>(Func<ApiResponse<T>, TResult> onSuccess, Func<string[]?, TResult> onFailure)
        {
            return IsSuccess && Data is not null ? onSuccess(new(Data)) : onFailure(Errors?.ToArray());
        }

        public TResult Match<TResult>(Func<ApiResponse<T>, TResult> onSuccess, Func<IReadOnlyList<string>?, IReadOnlyDictionary<string, string>?, TResult> onFailure)
        {
            return IsSuccess && Data is not null ? onSuccess(this) : onFailure(Errors, MessageErrors);
        }

        public void Match(Action<T> onSuccess, Action<string[]?> onFailure)
        {
            if (IsSuccess && Data is not null)
            {
                onSuccess(Data);
            }
            else
            {
                onFailure(Errors?.ToArray());
            }
        }
    }
}