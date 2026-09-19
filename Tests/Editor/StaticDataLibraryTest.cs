
namespace TeaSpoons.StaticData.Editor.Tests
{
    using UnityEngine;
    using UnityEditor;
    using UnityEngine.TestTools;
    using NUnit.Framework;
    using System.Collections;
    using System;
    using System.Linq;

    public class StaticDataLibraryTest
    {
        private class Super : StaticDataObject
        {
            public static Super Create()
            {
                var instance = CreateInstance<Super>();
                instance.Id = "super";
                return instance;
            }
        }

        private class Sub : Super
        {
            public new static Sub Create()
            {
                var instance = CreateInstance<Sub>();
                instance.Id = "sub";
                return instance;
            }
        }

        [SetUp]
        public void SetUp()
        {
            StaticDataLibrary.Clear();
        }

        [TearDown]
        public void TearDown()
        {
            StaticDataLibrary.Clear();
        }

        [Test]
        public void RegisterAndGetWithSubtypes()
        {
            var super = Super.Create();
            var sub = Sub.Create();
            
            StaticDataLibrary.Register(super);

            Assert.AreSame(super, StaticDataLibrary.Get<Super>("super"));

            Assert.Throws<NullReferenceException>(() => StaticDataLibrary.Get<Sub>("super"));
            Assert.Throws<NullReferenceException>(() => StaticDataLibrary.Get<Sub>("sub"));

            StaticDataLibrary.Register(sub);
            Assert.AreSame(super, StaticDataLibrary.Get<Super>("super"));
            Assert.AreSame(sub, StaticDataLibrary.Get<Super>("sub"));
            Assert.AreSame(sub, StaticDataLibrary.Get<Sub>("sub"));

            var allSupers = StaticDataLibrary.GetAll<Super>();
            Assert.AreEqual(2, allSupers.Count());

            var allSubs = StaticDataLibrary.GetAll<Sub>();
            Assert.AreEqual(1, allSubs.Count());
        }
    }
}
