namespace MonolithBoilerPlate.Common
{
    public class AppSettings
    {
        public required ConnectionStrings ConnectionStrings { get; set; }
        public required EncryptionConfig EncryptionConfig { get; set; }
        public required CorsDomain CorsDomain { get; set; }
        public required RateLimit RateLimit { get; set; }
        public required JwtConfig JwtConfig { get; set; }
        public required DirectoryPath DirectoryPath { get; set; }
        public required CacheConfig CacheConfig { get; set; }
        public  required RabbitMqConnection RabbitMq { get; set; }
        public required MessagingQueueName MessagingQueueName { get; set; }
        public required InvoiceGeneratorHostApi InvoiceGeneratorHostApi { get; set; }
        public required SpecialUser SpecialUser { get; set; }
        public required CronJobExpression CronJobExpression { get; set; }
        public required ConstantValue ConstantValue { get; set; }
    }

    public class ConnectionStrings
    {
        public required string DefaultConnection { get; set; }
    }

    public class EncryptionConfig
    {
        public required string SecretKey { get; set; }
        public required string PasswordCharcaters { get; set; }
        public required int MaxPasswordLength { get; set; }
    }

    public class CorsDomain
    {
        public required List<string> External { get; set; }
        public required List<string> Internal { get; set; }
    }
    public class JwtConfig
    {
        public required string SecretKey { get; set; }
        public required string Issuer { get; set; }
        public required string Audience { get; set; }
        public int ExpirationInMinutes { get; set; }
        public int RefreshTokenExpireInDay { get; set; }
    }

    public class DirectoryPath
    {
        public required string Root { get; set; }
        public required string Report { get; set; }
        public required string Invoice { get; set; }
    }

    public class CacheConfig
    {
        public uint BaseControllerCacheDuration { get; set; }
        public required CacheKeyDuration HoldingQueuePaged { get; set; }
        public uint LifeLineSyncDurationInMinute { get; set; }
        public uint LifelinePolicyBasicInfo { get; set; }
    }

    public class CacheKeyDuration
    {
        public required string CacheKey { get; set; }
        public uint Duration { get; set; }
    }

    public class RateLimit
    {
        public required FixedRateLimitConfiguration FixedByIP { get; set; }
        public required FixedRateLimitConfiguration FixedByUser { get; set; }
    }

    public class FixedRateLimitConfiguration
    {
        public int PermitLimit { get; set; }
        public int PeriodInMinutes { get; set; }
    }

    public class RabbitMqConnection
    {
        public required string Host { get; set; }
        public required string VirtualHost { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required string QName { get; set; }
        public int PrefetchCount { get; set; }
    }

    public class MessagingQueueName
    {
        public required string InvoicePdfGeneratorQueue { get; set; }
        public required string InvoiceSyncQueue { get; set; }
    }

    public class InvoiceGeneratorHostApi
    {
        public required string BaseUrl { get; set; }
        public required string AccessTokenUrl { get; set; }
        public required string InvoicePdfSaverApi { get; set; }
        public required string InvoiceSyncApi { get; set; }
        public required string DummyApi { get; set; }
    }

    public class SpecialUser
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }

    public class CronJobExpression
    {
        public required string InvoiceSyncWorker { get; set; }
    }

    public class ConstantValue
    {
        public int MaximumAttemptToSync { get; set; }
    }
}