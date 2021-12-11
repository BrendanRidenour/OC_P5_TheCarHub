namespace TheCarHub
{
    public class Result
    {
        public bool Success { get; }
        public string? FailureUserMessage { get; }

        public Result()
            : this(success: true)
        { }

        public Result(string failureUserMessage)
            : this(success: false)
        {
            if (string.IsNullOrWhiteSpace(failureUserMessage))
            {
                throw new ArgumentException($"'{nameof(failureUserMessage)}' cannot be null or whitespace.", nameof(failureUserMessage));
            }

            this.FailureUserMessage = failureUserMessage;
        }

        protected Result(bool success)
        {
            this.Success = success;
        }
    }
}