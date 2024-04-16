using Blueprint41.Core;

namespace Blueprint41.Neo4j.Persistence.Void;

internal class Neo4jRawResult : RawResult
{
    internal Neo4jRawResult()
    {
    }

    public override IReadOnlyList<string> Keys => [];

    public override RawRecord? Peek() => null;

    public override void Consume()
    { }

    public override IEnumerator<RawRecord> GetEnumerator() => new List<RawRecord>(0).GetEnumerator();

    public override RawResultStatistics Statistics() => new Neo4jRawResultStatistics();

    public override List<RawResultNotification> Notifications() => [];
}