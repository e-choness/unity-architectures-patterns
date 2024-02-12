using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.View;
using UnityEngine;

namespace MVC.Scripts.Views
{
    public class BouncySphereView : MonoBehaviour, IView
    {
        public bool IsInitialized { get; }
        public IContext Context { get; }

        public void Initialize(IContext context)
        {
            throw new System.NotImplementedException();
        }

        public void RequireIsInitialized()
        {
            throw new System.NotImplementedException();
        }
    }
}
