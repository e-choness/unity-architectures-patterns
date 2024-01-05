using ClassExtensions.Scripts.Utilities;
using UnityEngine;

namespace ClassExtensions.Scripts.Tests
{
    public class DisableChildrenTest : MonoBehaviour
    {
        private void Start()
        {
            GameObject parent = new(name: "Parent");

            for (var i = 0; i < 5; i++)
            {
                GameObject child = new($"Child{i}");
                child.transform.parent = parent.transform;
            }
            
            parent.DisableChildren();
        }
    }
}
