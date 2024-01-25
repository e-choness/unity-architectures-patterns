using MVC.Scripts.Views;
using UnityEngine;

namespace MVC.Scripts
{
    public class CalculatorEntry : MonoBehaviour
    {
        [SerializeField] private CalculatorView calculatorView;

        protected void Start()
        {
            var calculator = new Calculator(calculatorView);
            calculator.Initialize();
        }
    }
}