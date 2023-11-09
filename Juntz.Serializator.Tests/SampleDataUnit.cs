namespace Juntz.Serializator.Tests
{
    [ByteLayout(4)]
    internal class SampleDataUnit
    {
        [BytePosition(0, 4)]
        public int SampleField { get; set; }
    }
}
