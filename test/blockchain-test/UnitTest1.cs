using System;
using System.Text;
using blockchain_core;
using NUnit.Framework;

namespace blockchain_test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var b = new Block
            {
                Data = Encoding.UTF8.GetBytes("hello world"),
                PreviousBlockHash = String.Empty
            };
            var x = BlockMiner.ComputeBlockNonce(b, 4);
            b.Nonce = x;
            var computedHash = BlockMiner.ComputeHashForBlock(b);
            Assert.IsTrue(BlockMiner.LeadingZeroes(computedHash) >= 4);
        }
    }
}