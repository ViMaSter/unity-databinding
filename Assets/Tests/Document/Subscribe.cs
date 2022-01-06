using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
            var gameObject = new GameObject();
            var document = gameObject.AddComponent<global::DataBinding.Document>();

            var called = false;
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
            var gameObject = new GameObject();
            var document = gameObject.AddComponent<global::DataBinding.Document>();

            var called = false;
            void Callback(int path)
            {
                called = true;
            }

            document.Subscribe(DATABINDING_PATH, (Action<int>)Callback);
            LogAssert.Expect(LogType.Warning, new Regex($"1 subscribers of path '{DATABINDING_PATH}' are of '{typeof(int)}',", RegexOptions.Singleline));
            document.Set(DATABINDING_PATH, CORRECT_VALUE);
            Assert.IsFalse(called);
            LogAssert.Expect(LogType.Warning, new Regex($"1 subscribers of path '{DATABINDING_PATH}' are of '{typeof(int)}',", RegexOptions.Singleline));
            document.Set(DATABINDING_PATH, INCORRECT_VALUE);
            Assert.IsFalse(called);

            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void ToDirectPathAndIndirectType()
        {
            var gameObject = new GameObject();
            var document = gameObject.AddComponent<global::DataBinding.Document>();

            var called = false;
            void Callback(JToken token)
            {
                var path = token.ToObject<string>();
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
            public string StringValue = "";
        }
        [Test]
        public void ToNestedPathAndDirectType()
        {
            var gameObject = new GameObject();
            var document = gameObject.AddComponent<global::DataBinding.Document>();

            var called = false;
            void Callback(NestedValue nest)
            {
                called = nest.StringValue == CORRECT_VALUE;
            }

            document.Subscribe(DATABINDING_PATH, (Action<NestedValue>)Callback);
            document.Set($"{DATABINDING_PATH}.{nameof(NestedValue.StringValue)}", CORRECT_VALUE);
            Assert.IsTrue(called);
            document.Set($"{DATABINDING_PATH}.{nameof(NestedValue.StringValue)}", INCORRECT_VALUE);
            Assert.IsFalse(called);
        }

        [Test]
        public void ToNestedPathAndIncorrectType()
        {
            var gameObject = new GameObject();
            var document = gameObject.AddComponent<global::DataBinding.Document>();

            var called = false;
            void Callback(string nest)
            {
                called = nest == CORRECT_VALUE;
            }

            document.Subscribe(DATABINDING_PATH, (Action<string>)Callback);
            LogAssert.Expect(LogType.Warning, new Regex($"1 subscribers of path '{DATABINDING_PATH}' are of '{typeof(string)}',", RegexOptions.Singleline));
            document.Set($"{DATABINDING_PATH}.{nameof(NestedValue.StringValue)}", CORRECT_VALUE);
            Assert.IsFalse(called);
            LogAssert.Expect(LogType.Warning, new Regex($"1 subscribers of path '{DATABINDING_PATH}' are of '{typeof(string)}',", RegexOptions.Singleline));
            document.Set($"{DATABINDING_PATH}.{nameof(NestedValue.StringValue)}", INCORRECT_VALUE);
            Assert.IsFalse(called);

            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void NoValueAtPath_ToNestedPathAndIndirectType()
        {
            var gameObject = new GameObject();
            var document = gameObject.AddComponent<global::DataBinding.Document>();

            var called = false;
            void Callback(JToken token)
            {
                var nestedValue = token.ToObject<NestedValue>();
                called = nestedValue!.StringValue == CORRECT_VALUE;
            }

            document.Subscribe(DATABINDING_PATH, Callback);
            document.Set($"{DATABINDING_PATH}.{nameof(NestedValue.StringValue)}", CORRECT_VALUE);
            Assert.IsTrue(called);
            document.Set($"{DATABINDING_PATH}.{nameof(NestedValue.StringValue)}", INCORRECT_VALUE);
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
            var gameObject = new GameObject();
            var document = gameObject.AddComponent<global::DataBinding.Document>();

            var called = false;
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
            var gameObject = new GameObject();
            var document = gameObject.AddComponent<global::DataBinding.Document>();

            var called = false;
            void Callback(int path)
            {
                called = true;
            }

            document.Subscribe(DATABINDING_PATH, (Action<int>)Callback);
            LogAssert.Expect(LogType.Warning, new Regex($"1 subscribers of path '{DATABINDING_PATH}' are of '{typeof(int)}',", RegexOptions.Singleline));
            document.Set(DATABINDING_PATH, CORRECT_VALUE);
            Assert.IsFalse(called);
            LogAssert.Expect(LogType.Warning, new Regex($"1 subscribers of path '{DATABINDING_PATH}' are of '{typeof(int)}',", RegexOptions.Singleline));
            document.Set(DATABINDING_PATH, INCORRECT_VALUE);
            Assert.IsFalse(called);

            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void ToDirectPathAndIndirectType()
        {
            var gameObject = new GameObject();
            var document = gameObject.AddComponent<global::DataBinding.Document>();

            var called = false;
            void Callback(JToken token)
            {
                var path = token.ToObject<string>();
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
            public string StringValue = "";
        }
        [Test]
        public void ToNestedPathAndDirectType()
        {
            var gameObject = new GameObject();
            var document = gameObject.AddComponent<global::DataBinding.Document>();

            var called = false;
            void Callback(NestedValue nest)
            {
                called = nest.StringValue == CORRECT_VALUE;
            }

            document.Subscribe(DATABINDING_PATH, (Action<NestedValue>)Callback);
            document.Set($"{DATABINDING_PATH}.{nameof(NestedValue.StringValue)}", CORRECT_VALUE);
            Assert.IsTrue(called);
            document.Set($"{DATABINDING_PATH}.{nameof(NestedValue.StringValue)}", INCORRECT_VALUE);
            Assert.IsFalse(called);
        }

        [Test]
        public void ToNestedPathAndIncorrectType()
        {
            var gameObject = new GameObject();
            var document = gameObject.AddComponent<global::DataBinding.Document>();

            var called = false;
            void Callback(string nest)
            {
                called = nest == CORRECT_VALUE;
            }

            document.Subscribe($"{DATABINDING_PATH}", (Action<string>)Callback);
            LogAssert.Expect(LogType.Warning, new Regex($"1 subscribers of path '{DATABINDING_PATH}' are of '{typeof(string)}',", RegexOptions.Singleline));
            document.Set($"{DATABINDING_PATH}.{nameof(NestedValue.StringValue)}", CORRECT_VALUE);
            Assert.IsFalse(called);
            LogAssert.Expect(LogType.Warning, new Regex($"1 subscribers of path '{DATABINDING_PATH}' are of '{typeof(string)}',", RegexOptions.Singleline));
            document.Set($"{DATABINDING_PATH}.{nameof(NestedValue.StringValue)}", INCORRECT_VALUE);
            Assert.IsFalse(called);

            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void NoValueAtPath_ToNestedPathAndIndirectType()
        {
            var gameObject = new GameObject();
            var document = gameObject.AddComponent<global::DataBinding.Document>();

            var called = false;
            void Callback(JToken token)
            {
                var nestedValue = token.ToObject<NestedValue>();
                called = nestedValue!.StringValue == CORRECT_VALUE;
            }

            document.Subscribe($"{DATABINDING_PATH}", Callback);
            document.Set($"{DATABINDING_PATH}.{nameof(NestedValue.StringValue)}", CORRECT_VALUE);
            Assert.IsTrue(called);
            document.Set($"{DATABINDING_PATH}.{nameof(NestedValue.StringValue)}", INCORRECT_VALUE);
            Assert.IsFalse(called);
        }

        [Test]
        public void ValueAtPath_ToDirectPathAndCorrectType()
        {
            var gameObject = new GameObject();
            var document = gameObject.AddComponent<global::DataBinding.Document>();

            var called = false;
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
            var gameObject = new GameObject();
            var document = gameObject.AddComponent<global::DataBinding.Document>();

            var called = false;
            void Callback(int path)
            {
                called = true;
            }

            document.Set(DATABINDING_PATH, CORRECT_VALUE);
            document.Subscribe(DATABINDING_PATH, (Action<int>)Callback);
            Assert.IsFalse(called);
            LogAssert.Expect(LogType.Warning, new Regex($"1 subscribers of path '{DATABINDING_PATH}' are of '{typeof(int)}',", RegexOptions.Singleline));
            document.Set(DATABINDING_PATH, INCORRECT_VALUE);
            Assert.IsFalse(called);

            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void ValueAtPath_ToDirectPathAndIndirectType()
        {
            var gameObject = new GameObject();
            var document = gameObject.AddComponent<global::DataBinding.Document>();

            var called = false;
            void Callback(JToken token)
            {
                var path = token.ToObject<string>();
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
            var gameObject = new GameObject();
            var document = gameObject.AddComponent<global::DataBinding.Document>();

            var called = false;
            void Callback(NestedValue nest)
            {
                called = nest.StringValue == CORRECT_VALUE;
            }

            document.Set($"{DATABINDING_PATH}.{nameof(NestedValue.StringValue)}", CORRECT_VALUE);
            document.Subscribe(DATABINDING_PATH, (Action<NestedValue>)Callback);
            Assert.IsTrue(called);
            document.Set($"{DATABINDING_PATH}.{nameof(NestedValue.StringValue)}", INCORRECT_VALUE);
            Assert.IsFalse(called);
        }

        [Test]
        public void ValueAtPath_ToNestedPathAndIncorrectType()
        {
            var gameObject = new GameObject();
            var document = gameObject.AddComponent<global::DataBinding.Document>();

            var called = false;
            void Callback(string nest)
            {
                called = nest == CORRECT_VALUE;
            }

            document.Set($"{DATABINDING_PATH}.{nameof(NestedValue.StringValue)}", CORRECT_VALUE);
            document.Subscribe(DATABINDING_PATH, (Action<string>)Callback);
            Assert.IsFalse(called);
            LogAssert.Expect(LogType.Warning, new Regex($"1 subscribers of path '{DATABINDING_PATH}' are of '{typeof(string)}',", RegexOptions.Singleline));
            document.Set($"{DATABINDING_PATH}.{nameof(NestedValue.StringValue)}", INCORRECT_VALUE);
            Assert.IsFalse(called);

            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void ValueAtPath_ToNestedPathAndIndirectType()
        {
            var gameObject = new GameObject();
            var document = gameObject.AddComponent<global::DataBinding.Document>();

            var called = false;
            void Callback(JToken token)
            {
                var nestedValue = token.ToObject<NestedValue>();
                called = nestedValue!.StringValue == CORRECT_VALUE;
            }

            document.Set($"{DATABINDING_PATH}.{nameof(NestedValue.StringValue)}", CORRECT_VALUE);
            document.Subscribe(DATABINDING_PATH, Callback);
            Assert.IsTrue(called);
            document.Set($"{DATABINDING_PATH}.{nameof(NestedValue.StringValue)}", INCORRECT_VALUE);
            Assert.IsFalse(called);
        }
    }

    public class Arrays
    {
        private const string DATABINDING_PATH = "path";

        [Test]
        public void SettingArrayInformsAllChildSubscriber()
        {
            var gameObject = new GameObject();
            var document = gameObject.AddComponent<global::DataBinding.Document>();

            List<int> calledIndices = new List<int>();

            Action<JToken> GenerateCallbackForIndex(int index)
            {
                return token => {
                    Assert.AreEqual(index, token.ToObject<int>());
                    calledIndices.Add(index);
                };
            }

            var intArray = new[] { 0, 1, 2 };

            document.Subscribe($"{DATABINDING_PATH}[0]", GenerateCallbackForIndex(0));
            document.Subscribe($"{DATABINDING_PATH}[1]", GenerateCallbackForIndex(1));
            document.Subscribe($"{DATABINDING_PATH}[2]", GenerateCallbackForIndex(2));
            document.Set($"{DATABINDING_PATH}", intArray);

            Assert.AreEqual(calledIndices.OrderBy(i => i), intArray.OrderBy(i => i));
        }
    }
}