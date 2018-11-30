using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Examples.Collections.Tests
{
    [TestClass]
    public class BijectiveDictionaryTests
    {
        [TestMethod]
        public void BothKeysExist()
        {
            var sut = new BijectiveDictionary<string, int>
            {
                { "FooBar", 0 },
                { "Testing123", 2 }
            };

            Assert.AreEqual("FooBar", sut.Inverse[0]);
            Assert.AreEqual(0, sut["FooBar"]);
        }

        [TestMethod]
        public void BothKeysRemoved()
        {
            var sut = new BijectiveDictionary<string, int>
            {
                { "FooBar", 0 },
                { "Testing123", 2 }
            };

            sut.Remove("Testing123");
            Assert.IsFalse(sut.Inverse.ContainsKey(2));
        }

        [TestMethod]
        public void BothKeysRemovedByKeyValuePair()
        {
            var sut = new BijectiveDictionary<string, int>
            {
                { "FooBar", 0 },
                { "Testing123", 2 }
            };

            sut.Remove(new KeyValuePair<string, int>("Testing123", 2));
            Assert.IsFalse(sut.Inverse.ContainsKey(2));
        }

        [TestMethod]
        public void BothBijectionsCleared()
        {
            var sut = new BijectiveDictionary<string, int>
            {
                { "FooBar", 0 },
                { "Testing123", 2 }
            };

            sut.Clear();
            Assert.IsTrue(sut.Count == 0 && sut.Inverse.Count == 0);
        }

        [TestMethod]
        public void CountsAreEqual()
        {
            var sut = new BijectiveDictionary<string, int>
            {
                { "FooBar", 0 },
                { "Testing123", 2 }
            };
            Assert.IsTrue(sut.Count == sut.Inverse.Count);
        }

        [TestMethod]
        public void BothBijectionsSet()
        {
            var sut = new BijectiveDictionary<string, int>
            {
                { "FooBar", 0 },
                { "Testing123", 2 }
            };

            sut["BarFoo"] = 42;

            Assert.AreEqual(42, sut["BarFoo"]);
            Assert.AreEqual("BarFoo", sut.Inverse[42]);
        }

        [TestMethod]
        public void KeyValuePairDoesNotExist()
        {
            var sut = new BijectiveDictionary<string, int>
            {
                { "FooBar", 0 },
                { "Testing123", 2 }
            };

            Assert.IsFalse(sut.Contains(new KeyValuePair<string, int>("FooBar", 7)));
        }

        [TestMethod]
        public void KeyValuePairExists()
        {
            var sut = new BijectiveDictionary<string, int>
            {
                { "FooBar", 0 },
                { "Testing123", 2 }
            };

            Assert.IsTrue(sut.Contains(new KeyValuePair<string, int>("FooBar", 0)));
        }
    }
}
