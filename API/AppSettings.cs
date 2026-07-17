namespace ASPLAB2.API
{
    public class AppSettings
    {
        public string FromEmail { get; set; }
        public string ConnString { get; set; }
        public IEnumerable<string> ApiKeys { get; set; }
        public JwtSettings JwtSettings { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }
        public MailSettings MailSettings { get; set; }
        public RateLimitSettings RateLimitSettings { get; set; }
        public RedisSettings RedisSettings { get; set; }
        public SentrySettings SentrySettings { get; set; }

    }

    public class RedisSettings
    {
        public string ConnectionString { get; set; }
        public string InstanceName { get; set; }
    }

    public class SentrySettings
    {
        public string Dsn { get; set; }
        public bool Debug { get; set; }
    }

    public class RateLimitSettings
    {
        public int GlobalPermitLimit { get; set; }
        public int GlobalWindowSeconds { get; set; }
        public int LoginPermitLimit { get; set; }
        public int LoginWindowSeconds { get; set; }
    }

    public class ConnectionStrings
    {
        public string DefaultConnection { get; set; }
    }

    public class JwtSettings
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public int DurationSeconds { get; set; }
        public int RefreshTokenHours { get; set; }
    }

    public class MailSettings
    {
        public string FromEmail { get; set; }
        public string AppPassword { get; set; }
    }
}
