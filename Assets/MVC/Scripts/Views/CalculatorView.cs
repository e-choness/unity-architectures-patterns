using System;
using MVC.Scripts.Models;
using RMC.Mini;
using RMC.Mini.View;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MVC.Scripts.Views
{
    public class CalculatorView : MonoBehaviour,IView
    {
        [SerializeField] private InputField aInputField;
        [SerializeField] private InputField bInputField;
        [SerializeField] private InputField resultInputField;
        [SerializeField] private  Button addButton;
        [SerializeField] private Button resetButton;
        
        public bool IsInitialized { get; private set; }
        public IContext Context { get; private set; }
        public UnityEvent OnAdd = new();
        public UnityEvent OnReset = new();
    
        public InputField AInputField => aInputField;
        public InputField BInputField => bInputField;
        public InputField ResultInputField => resultInputField;
        public Button AddButton => addButton;
        public Button RestButton => resetButton;

        public void Initialize()
        {
            
        }
        
        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                Context = context;
                
                aInputField.onValueChanged.AddListener(AnyInputField_OnValueChanged);
                bInputField.onValueChanged.AddListener(AnyInputField_OnValueChanged);
                addButton.onClick.AddListener(AddButton_OnClicked);
                resetButton.onClick.AddListener(ResetButton_OnClicked);
                AnyInputField_OnValueChanged("");
                
                // Two way binding, when model value changes, the view also change
                var calculatorModel = Context.ModelLocator.GetItem<CalculatorModel>();
                calculatorModel.A.OnValueChanged.AddListener((p,c)=>aInputField.text = c.ToString());
                calculatorModel.B.OnValueChanged.AddListener((p,c)=> bInputField.text = c.ToString());
                calculatorModel.Result.OnValueChanged.AddListener((p,c)=>resultInputField.text = c.ToString());
            }
            
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new Exception("Must be initialized.");
            }
        }

        void AnyInputField_OnValueChanged(string value)
        {
            RequireIsInitialized();
            
            var hasValidInput = HasValidInput(aInputField.text) && HasValidInput(bInputField.text);
            var hasAnyInput = HasValidInput(aInputField.text) || HasValidInput(bInputField.text);

            addButton.interactable = hasValidInput;
            resetButton.interactable = hasAnyInput;
        }

        void AddButton_OnClicked()
        {
            RequireIsInitialized();
            OnAdd?.Invoke();
        }

        void ResetButton_OnClicked()
        {
            RequireIsInitialized();
            OnReset?.Invoke();
        }

        bool HasValidInput(string input)
        {
            return input.Length > 0 && input != "0";
        }

        public void Dispose()
        {
            
        }
    }
}
