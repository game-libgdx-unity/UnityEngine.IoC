using System.Collections.Generic;
using UnityEngine;

namespace UnityIoC
{
    [CreateAssetMenu(fileName = "Assembly-CSharp", menuName = "IoC/Binding Data", order = 1)]
    public class BindingSetting : ScriptableObject
    {
        [SerializeField] public BindingDataAsset[] defaultSettings = new BindingDataAsset[]{};
    }
}