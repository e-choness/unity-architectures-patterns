using MVC.Scripts.Models;
using MVC.Scripts.Views;
using RMC.Mini;
using RMC.Mini.Controller;

namespace MVC.Scripts.Controllers
{
    public class CalculatorController : IController
    {
        private readonly CalculatorModel _calculatorModel;
        private readonly CalculatorView _calculatorView;
        public bool IsInitialized { get; private set; }
        public IContext Context { get; private set; }

        public CalculatorController(CalculatorModel calculatorModel, CalculatorView calculatorView)
        {
            _calculatorModel = calculatorModel;
            _calculatorView = calculatorView;
        }

        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                Context = context;
                
                // Add listeners
                _calculatorView.OnAdd.AddListener(View_OnAdd);
                _calculatorView.OnReset.AddListener(View_OnReset);
                View_OnReset();
            }
        }

        public void RequireIsInitialized()
        {
            _calculatorModel.A.Value = 0;
            _calculatorModel.B.Value = 0;
            _calculatorModel.Result.Value = 0;
        }

        private void View_OnAdd()
        {
            _calculatorModel.A.Value = int.Parse(_calculatorView.AInputField.text);
            _calculatorModel.B.Value = int.Parse(_calculatorView.BInputField.text);
            _calculatorModel.Result.Value = _calculatorModel.A.Value + _calculatorModel.B.Value;

        }

        private void View_OnReset() => RequireIsInitialized();
        public void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}