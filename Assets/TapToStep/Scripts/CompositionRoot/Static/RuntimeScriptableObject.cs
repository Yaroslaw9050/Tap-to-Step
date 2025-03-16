using System.Collections.Generic;
using UnityEngine;

namespace CompositionRoot.Static
{
    public abstract class RuntimeScriptableObject: ScriptableObject
    {
        private static readonly List<RuntimeScriptableObject> r_instance = new();
        
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void ResetAllInstance()
        {
            foreach (var element in r_instance)
            {
                element.OnReset();
            }
        }
        
        private void OnEnable()
        {
            r_instance.Add(this);
        }

        private void OnDisable()
        {
            r_instance.Remove(this);
        }

        protected abstract void OnReset();
        
    }
}