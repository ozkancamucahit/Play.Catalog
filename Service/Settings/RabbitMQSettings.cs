namespace Service.Settings
{
    public sealed class RabbitMQSettings
    {
        public string Host { get; init; } = String.Empty;
        public int Port { get; set; }
    }
}
