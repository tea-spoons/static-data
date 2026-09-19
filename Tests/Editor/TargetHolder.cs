namespace TeaSpoons.StaticData.Editor.Tests
{
    using System;
    using UnityEngine;

    [Serializable]
    public class TargetHolderItem
    {
        public int value;
    }

    public class TargetHolder : ScriptableObject
    {
        public int number;
        public TargetHolderItem single = new() { value = 3 };
        public TargetHolderItem[] items = { new() { value = 10 }, new() { value = 20 } };
    }
}
