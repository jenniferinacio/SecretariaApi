namespace SecretariaApi.Util
{
    public class ResultLogin
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public string? Token { get; set; }
    }
}
