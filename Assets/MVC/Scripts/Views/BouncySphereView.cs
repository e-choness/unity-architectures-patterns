using RMC.Mini;
using RMC.Mini.View;
using UnityEngine;

namespace MVC.Scripts.Views
{
    public class BouncySphereView : MonoBehaviour, IView
    {
        public bool IsInitialized { get; }
        public IContext Context { get; }

        public void Initialize(IContext context)
        {
            
        }

        public void RequireIsInitialized()
        {
            
        }

        public void Dispose()
        {
            
        }
    }
}
