using MVC.Scripts.Controllers;
using MVC.Scripts.Models;
using MVC.Scripts.Views;
using RMC.Mini;
using RMC.Mini.Controller;
using RMC.Mini.Model;
using RMC.Mini.Service;
using RMC.Mini.View;

namespace MVC.Scripts
{
    public class Calculator : IMiniMvcs<IContext, IModel, IView, IController, IService>
    {
        public bool IsInitialized { get; private set; }
        private IContext _context;
        private CalculatorView _calculatorView;
        public IContext Context { get; }
        public Locator<IModel> ModelLocator { get; }
        public Locator<IView> ViewLocator { get; }
        public Locator<IController> ControllerLocator { get; }
        public Locator<IService> ServiceLocator { get; }

        public Calculator(CalculatorView calculatorView)
        {
            _calculatorView = calculatorView;
        }
        public void Initialize()
        {
            if (!IsInitialized)
            {
                // Context - it is the clue that connects the views, controllers and models
                IsInitialized = true;
                _context = new Context();
                
                // Model
                var calculatorModel = new CalculatorModel();
                
                // View
                
                // Controller
                var calculatorController = new CalculatorController(calculatorModel, _calculatorView);
                
                // Initialize
                calculatorModel.Initialize(_context);
                _calculatorView.Initialize(_context);
                calculatorController.Initialize(_context);
            }
        }

        public void RequireIsInitialized()
        {
            
        }

        
    }
}
