using System;
using System.Linq;
using UnityEngine;

namespace DataBinding.EventGeneration
{
    [DefaultExecutionOrder(-110)]
    public class EventGenerator : MonoBehaviour
    {
        public Document Document;
        public string Path;

        private static bool FindGeneratorComponent(Type type) => type.GetInterfaces().Any(subInterface => subInterface == typeof(IComponentEventGenerator));

        void Start()
        {
            var allClassesOnThisAssembly = GetType().Assembly.GetTypes();
            var allEventGeneratorClasses = allClassesOnThisAssembly.Where(type => type.GetInterfaces().Any(FindGeneratorComponent)).ToList();
            var relevantUnityClasses = allEventGeneratorClasses
                .Select(generatorClass => {
                    var targetClass = generatorClass.GetInterfaces().First(FindGeneratorComponent).GetGenericArguments().First();
                    return (targetClass, generatorClass);
                });

            foreach (var (targetClass, generatorClass) in relevantUnityClasses)
            {
                var componentOnThisGameObject = GetComponent(targetClass);
                if (componentOnThisGameObject == null)
                {
                    return;
                }

                var constructor = generatorClass.GetConstructor(new[] {typeof(Document), typeof(string), typeof(Component)});
                constructor!.Invoke(new object[] {Document, Path, componentOnThisGameObject});
            }
        }
    }
}