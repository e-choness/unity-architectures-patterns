using RMC.Core.Architectures.Mini.Context;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace MVC.Scripts.Views
{
    public class CalculatorView : IMiniMvcs
    {
        public TextInputBaseField<int> aInputField;
        public TextInputBaseField<int> bInputField;
        public TextInputBaseField<int> result;
        
        public bool IsInitialized { get; private set; }
        public UnityEvent OnAdd = new();
        public UnityEvent OnReset = new();
    
        public void Initialize()
        {
            throw new System.NotImplementedException();
        }
        
        public void Initialize(IContext context)
        {
        }

        public void RequireIsInitialized()
        {
            throw new System.NotImplementedException();
        }

    }
}
