namespace TokenGenerator.Api.Models.Responses
{
    public class TokenResponse
    {
        public string AccessToken { get; set; }
        public DateTimeOffset AccessTokenExpiryDate { get; set; }

    }
}
