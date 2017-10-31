namespace Policies
{
    public static class HttpPolicies
    {
        public static readonly IPolicy SimpleRetry = new RetryPolicy();
    }
}