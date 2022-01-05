using System;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.DataBinding.Document.Subscribe
{
    internal class PathNotYetSet
    {
        private const string DATABINDING_PATH = "path";
        private const string CORRECT_VALUE = "C:/";
        private const string INCORRECT_VALUE= "wrong";
        
        [Test]
        public void ToDirectPathAndCorrectType()
        {
            GameObject gameObject = new GameObject();
            global::DataBinding.Document document = gameObject.AddComponent<global::DataBinding.Document>();


            bool called = false;
            void Callback(string path)
            {
                called = path == CORRECT_VALUE;
            }

            document.Subscribe(DATABINDING_PATH, (Action<string>)Callback);
            document.Set(DATABINDING_PATH, CORRECT_VALUE);
            Assert.IsTrue(called);
            document.Set(DATABINDING_PATH, INCORRECT_VALUE);
            Assert.IsFalse(called);
        }

        [Test]
        public void ToDirectPathAndIncorrectType()
        {
            GameObject gameObject = new GameObject();
            global::DataBinding.Document document = gameObject.AddComponent<global::DataBinding.Document>();

            bool called = false;
            void Callback(int path)
            {
                called = true;
            }

            document.Subscribe(DATABINDING_PATH, (Action<int>)Callback);
            LogAssert.Expect(LogType.Warning, $"1 subscribers of path '{DATABINDING_PATH}' are of '{typeof(int)}', but the current value cannot be cast to it, as it's of type '{typeof(string)}'");
            document.Set(DATABINDING_PATH, CORRECT_VALUE);
            Assert.IsFalse(called);
            LogAssert.Expect(LogType.Warning, $"1 subscribers of path '{DATABINDING_PATH}' are of '{typeof(int)}', but the current value cannot be cast to it, as it's of type '{typeof(string)}'");
            document.Set(DATABINDING_PATH, INCORRECT_VALUE);
            Assert.IsFalse(called);

            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void ToDirectPathAndIndirectType()
        {
            GameObject gameObject = new GameObject();
            global::DataBinding.Document document = gameObject.AddComponent<global::DataBinding.Document>();

            bool called = false;
            void Callback(JToken token)
            {
                string path = token.ToObject<string>();
                called = path == CORRECT_VALUE;
            }

            document.Subscribe(DATABINDING_PATH, Callback);
            document.Set(DATABINDING_PATH, CORRECT_VALUE);
            Assert.IsTrue(called);
            document.Set(DATABINDING_PATH, INCORRECT_VALUE);
            Assert.IsFalse(called);
        }

        private class NestedValue
        {
            public string stringValue;
        }
        [Test]
        public void ToNestedPathAndDirectType()
        {
            GameObject gameObject = new GameObject();
            global::DataBinding.Document document = gameObject.AddComponent<global::DataBinding.Document>();

            var called = false;
            void Callback(NestedValue nest)
            {
                called = nest.stringValue == CORRECT_VALUE;
            }

            document.Subscribe(DATABINDING_PATH, (Action<NestedValue>)Callback);
            document.Set($"{DATABINDING_PATH}.{nameof(NestedValue.stringValue)}", CORRECT_VALUE);
            Assert.IsTrue(called);
            document.Set($"{DATABINDING_PATH}.{nameof(NestedValue.stringValue)}", INCORRECT_VALUE);
            Assert.IsFalse(called);
        }

        [Test]
        public void ToNestedPathAndIncorrectType()
        {
            GameObject gameObject = new GameObject();
            global::DataBinding.Document document = gameObject.AddComponent<global::DataBinding.Document>();

            var called = false;
            void Callback(string nest)
            {
                called = nest == CORRECT_VALUE;
            }

            document.Subscribe(DATABINDING_PATH, (Action<string>)Callback);
            LogAssert.Expect(LogType.Warning, $"1 subscribers of path '{DATABINDING_PATH}' are of '{typeof(string)}', but the current value cannot be cast to it, as it's of type '{typeof(JObject)}'");
            document.Set($"{DATABINDING_PATH}.{nameof(NestedValue.stringValue)}", CORRECT_VALUE);
            Assert.IsFalse(called);
            LogAssert.Expect(LogType.Warning, $"1 subscribers of path '{DATABINDING_PATH}' are of '{typeof(string)}', but the current value cannot be cast to it, as it's of type '{typeof(JObject)}'");
            document.Set($"{DATABINDING_PATH}.{nameof(NestedValue.stringValue)}", INCORRECT_VALUE);
            Assert.IsFalse(called);

            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void NoValueAtPath_ToNestedPathAndIndirectType()
        {
            GameObject gameObject = new GameObject();
            global::DataBinding.Document document = gameObject.AddComponent<global::DataBinding.Document>();

            bool called = false;
            void Callback(JToken token)
            {
                var nest = token.ToObject<NestedValue>();
                called = nest.stringValue == CORRECT_VALUE;
            }

            document.Subscribe(DATABINDING_PATH, Callback);
            document.Set($"{DATABINDING_PATH}.{nameof(NestedValue.stringValue)}", CORRECT_VALUE);
            Assert.IsTrue(called);
            document.Set($"{DATABINDING_PATH}.{nameof(NestedValue.stringValue)}", INCORRECT_VALUE);
            Assert.IsFalse(called);
        }
    }

    internal class PathAlreadySet
    {
        private const string DATABINDING_PATH = "path";
        private const string CORRECT_VALUE= "C:/";
        private const string INCORRECT_VALUE= "wrong";

        [Test]
        public void ToDirectPathAndCorrectType()
        {
            GameObject gameObject = new GameObject();
            global::DataBinding.Document document = gameObject.AddComponent<global::DataBinding.Document>();

            bool called = false;
            void Callback(string path)
            {
                called = path == CORRECT_VALUE;
            }

            document.Subscribe(DATABINDING_PATH, (Action<string>)Callback);
            document.Set(DATABINDING_PATH, CORRECT_VALUE);
            Assert.IsTrue(called);
            document.Set(DATABINDING_PATH, INCORRECT_VALUE);
            Assert.IsFalse(called);
        }

        [Test]
        public void ToDirectPathAndIncorrectType()
        {
            GameObject gameObject = new GameObject();
            global::DataBinding.Document document = gameObject.AddComponent<global::DataBinding.Document>();

            bool called = false;
            void Callback(int path)
            {
                called = true;
            }

            document.Subscribe(DATABINDING_PATH, (Action<int>)Callback);
            LogAssert.Expect(LogType.Warning, $"1 subscribers of path '{DATABINDING_PATH}' are of '{typeof(int)}', but the current value cannot be cast to it, as it's of type '{typeof(string)}'");
            document.Set(DATABINDING_PATH, CORRECT_VALUE);
            Assert.IsFalse(called);
            LogAssert.Expect(LogType.Warning, $"1 subscribers of path '{DATABINDING_PATH}' are of '{typeof(int)}', but the current value cannot be cast to it, as it's of type '{typeof(string)}'");
            document.Set(DATABINDING_PATH, INCORRECT_VALUE);
            Assert.IsFalse(called);

            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void ToDirectPathAndIndirectType()
        {
            GameObject gameObject = new GameObject();
            global::DataBinding.Document document = gameObject.AddComponent<global::DataBinding.Document>();

            bool called = false;
            void Callback(JToken token)
            {
                string path = token.ToObject<string>();
                called = path == CORRECT_VALUE;
            }

            document.Subscribe(DATABINDING_PATH, Callback);
            document.Set(DATABINDING_PATH, CORRECT_VALUE);
            Assert.IsTrue(called);
            document.Set(DATABINDING_PATH, INCORRECT_VALUE);
            Assert.IsFalse(called);
        }

        private class NestedValue
        {
            public string a;
        }
        [Test]
        public void ToNestedPathAndDirectType()
        {
            GameObject gameObject = new GameObject();
            global::DataBinding.Document document = gameObject.AddComponent<global::DataBinding.Document>();

            var called = false;
            void Callback(NestedValue nest)
            {
                called = nest.a == CORRECT_VALUE;
            }

            document.Subscribe(DATABINDING_PATH, (Action<NestedValue>)Callback);
            document.Set($"{DATABINDING_PATH}.{nameof(NestedValue.a)}", CORRECT_VALUE);
            Assert.IsTrue(called);
            document.Set($"{DATABINDING_PATH}.{nameof(NestedValue.a)}", INCORRECT_VALUE);
            Assert.IsFalse(called);
        }

        [Test]
        public void ToNestedPathAndIncorrectType()
        {
            GameObject gameObject = new GameObject();
            global::DataBinding.Document document = gameObject.AddComponent<global::DataBinding.Document>();

            var called = false;
            void Callback(string nest)
            {
                called = nest == CORRECT_VALUE;
            }

            document.Subscribe(DATABINDING_PATH, (Action<string>)Callback);
            LogAssert.Expect(LogType.Warning, $"1 subscribers of path '{DATABINDING_PATH}' are of '{typeof(string)}', but the current value cannot be cast to it, as it's of type '{typeof(JObject)}'");
            document.Set($"{DATABINDING_PATH}.{nameof(NestedValue.a)}", CORRECT_VALUE);
            Assert.IsFalse(called);
            LogAssert.Expect(LogType.Warning, $"1 subscribers of path '{DATABINDING_PATH}' are of '{typeof(string)}', but the current value cannot be cast to it, as it's of type '{typeof(JObject)}'");
            document.Set($"{DATABINDING_PATH}.{nameof(NestedValue.a)}", INCORRECT_VALUE);
            Assert.IsFalse(called);

            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void NoValueAtPath_ToNestedPathAndIndirectType()
        {
            GameObject gameObject = new GameObject();
            global::DataBinding.Document document = gameObject.AddComponent<global::DataBinding.Document>();

            bool called = false;
            void Callback(JToken token)
            {
                var nest = token.ToObject<NestedValue>();
                called = nest.a == CORRECT_VALUE;
            }

            document.Subscribe(DATABINDING_PATH, Callback);
            document.Set($"{DATABINDING_PATH}.{nameof(NestedValue.a)}", CORRECT_VALUE);
            Assert.IsTrue(called);
            document.Set($"{DATABINDING_PATH}.{nameof(NestedValue.a)}", INCORRECT_VALUE);
            Assert.IsFalse(called);
        }

        [Test]
        public void ValueAtPath_ToDirectPathAndCorrectType()
        {
            GameObject gameObject = new GameObject();
            global::DataBinding.Document document = gameObject.AddComponent<global::DataBinding.Document>();

            bool called = false;
            void Callback(string path)
            {
                called = path == CORRECT_VALUE;
            }

            document.Set(DATABINDING_PATH, CORRECT_VALUE);
            document.Subscribe(DATABINDING_PATH, (Action<string>)Callback);
            Assert.IsTrue(called);
            document.Set(DATABINDING_PATH, INCORRECT_VALUE);
            Assert.IsFalse(called);
        }

        [Test]
        public void ValueAtPath_ToDirectPathAndIncorrectType()
        {
            GameObject gameObject = new GameObject();
            global::DataBinding.Document document = gameObject.AddComponent<global::DataBinding.Document>();

            bool called = false;
            void Callback(int path)
            {
                called = true;
            }

            document.Set(DATABINDING_PATH, CORRECT_VALUE);
            document.Subscribe(DATABINDING_PATH, (Action<int>)Callback);
            Assert.IsFalse(called);
            LogAssert.Expect(LogType.Warning, $"1 subscribers of path '{DATABINDING_PATH}' are of '{typeof(int)}', but the current value cannot be cast to it, as it's of type '{typeof(string)}'");
            document.Set(DATABINDING_PATH, INCORRECT_VALUE);
            Assert.IsFalse(called);

            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void ValueAtPath_ToDirectPathAndIndirectType()
        {
            GameObject gameObject = new GameObject();
            global::DataBinding.Document document = gameObject.AddComponent<global::DataBinding.Document>();

            bool called = false;
            void Callback(JToken token)
            {
                string path = token.ToObject<string>();
                called = path == CORRECT_VALUE;
            }

            document.Set(DATABINDING_PATH, CORRECT_VALUE);
            document.Subscribe(DATABINDING_PATH, Callback);
            Assert.IsTrue(called);
            document.Set(DATABINDING_PATH, INCORRECT_VALUE);
            Assert.IsFalse(called);
        }
        [Test]
        public void ValueAtPath_ToNestedPathAndDirectType()
        {
            GameObject gameObject = new GameObject();
            global::DataBinding.Document document = gameObject.AddComponent<global::DataBinding.Document>();

            var called = false;
            void Callback(NestedValue nest)
            {
                called = nest.a == CORRECT_VALUE;
            }

            document.Set($"{DATABINDING_PATH}.{nameof(NestedValue.a)}", CORRECT_VALUE);
            document.Subscribe(DATABINDING_PATH, (Action<NestedValue>)Callback);
            Assert.IsTrue(called);
            document.Set($"{DATABINDING_PATH}.{nameof(NestedValue.a)}", INCORRECT_VALUE);
            Assert.IsFalse(called);
        }

        [Test]
        public void ValueAtPath_ToNestedPathAndIncorrectType()
        {
            GameObject gameObject = new GameObject();
            global::DataBinding.Document document = gameObject.AddComponent<global::DataBinding.Document>();

            var called = false;
            void Callback(string nest)
            {
                called = nest == CORRECT_VALUE;
            }

            document.Set($"{DATABINDING_PATH}.{nameof(NestedValue.a)}", CORRECT_VALUE);
            document.Subscribe(DATABINDING_PATH, (Action<string>)Callback);
            Assert.IsFalse(called);
            LogAssert.Expect(LogType.Warning, $"1 subscribers of path '{DATABINDING_PATH}' are of '{typeof(string)}', but the current value cannot be cast to it, as it's of type '{typeof(JObject)}'");
            document.Set($"{DATABINDING_PATH}.{nameof(NestedValue.a)}", INCORRECT_VALUE);
            Assert.IsFalse(called);

            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void ValueAtPath_ToNestedPathAndIndirectType()
        {
            GameObject gameObject = new GameObject();
            global::DataBinding.Document document = gameObject.AddComponent<global::DataBinding.Document>();

            bool called = false;
            void Callback(JToken token)
            {
                var nest = token.ToObject<NestedValue>();
                called = nest.a == CORRECT_VALUE;
            }

            document.Set($"{DATABINDING_PATH}.{nameof(NestedValue.a)}", CORRECT_VALUE);
            document.Subscribe(DATABINDING_PATH, Callback);
            Assert.IsTrue(called);
            document.Set($"{DATABINDING_PATH}.{nameof(NestedValue.a)}", INCORRECT_VALUE);
            Assert.IsFalse(called);
        }
    }
}