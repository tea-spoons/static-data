#if !TEASPOONS_PACKAGE_CORE
namespace TeaSpoons.StaticData.Editor.Tests
{
    using NUnit.Framework;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Tests the drawer's stand-in for the package-core helper. It only exists (and is only tested) without package-core.
    /// </summary>
    public class SerializedPropertyExtensionsTest
    {
        private TargetHolder holder;
        private SerializedObject serializedObject;

        [SetUp]
        public void SetUp()
        {
            holder = ScriptableObject.CreateInstance<TargetHolder>();
            serializedObject = new SerializedObject(holder);
        }

        [TearDown]
        public void TearDown()
        {
            serializedObject.Dispose();
            Object.DestroyImmediate(holder);
        }

        [Test]
        public void FindsAPrimitiveField()
        {
            Assert.IsTrue(serializedObject.FindProperty("number").TryGetTargetObject<int>(out _));
        }

        [Test]
        public void FindsANestedObject()
        {
            Assert.IsTrue(serializedObject.FindProperty("single").TryGetTargetObject<TargetHolderItem>(out var item));
            Assert.AreSame(holder.single, item);
        }

        [Test]
        public void FindsAnArrayElement()
        {
            Assert.IsTrue(serializedObject.FindProperty("items.Array.data[1]").TryGetTargetObject<TargetHolderItem>(out var item));
            Assert.AreSame(holder.items[1], item);
        }

        [Test]
        public void FindsAFieldInsideAnArrayElement()
        {
            Assert.IsTrue(serializedObject.FindProperty("items.Array.data[0].value").TryGetTargetObject<int>(out var value));
            Assert.AreEqual(10, value);
        }

        [Test]
        public void FailsForTheWrongType()
        {
            Assert.IsFalse(serializedObject.FindProperty("single").TryGetTargetObject<string>(out _));
        }

        [Test]
        public void FailsWhenAnObjectOnThePathIsNull()
        {
            var property = serializedObject.FindProperty("single.value");
            holder.single = null;
            Assert.IsFalse(property.TryGetTargetObject<int>(out _));
        }
    }
}
#endif
