namespace Service.Settings
{
    public sealed class MongoDBSettings
    {
        public string Host { get; init; } = String.Empty;
        public int Port { get; init; }

        public string ConectionString => $"mongodb://{Host}:{Port}";
    }
}
