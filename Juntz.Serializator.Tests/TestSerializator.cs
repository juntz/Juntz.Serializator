using System.Buffers.Binary;

namespace Juntz.Serializator.Tests
{
    public class TestSerializator
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ToBytes_ConvertSample_ReturnsExpected()
        {
            var data = new SampleDataUnit() { SampleField = 0x12345678 };

            var actual = Serializator.ToBytes(data);

            var expected = new byte[] { 0x78, 0x56, 0x34, 0x12 };
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void FromBytes_ConvertSample_ReturnsExpected()
        {
            var bytes = new byte[] { 0x78, 0x56, 0x34, 0x12 };

            var actual = Serializator.FromBytes<SampleDataUnit>(bytes);

            var expected = 0x12345678;
            Assert.That(actual.SampleField, Is.EqualTo(expected));
        }
    }
}