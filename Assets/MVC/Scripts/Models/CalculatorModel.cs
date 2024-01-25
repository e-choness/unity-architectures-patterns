using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Model;

namespace MVC.Scripts.Models
{
    public class CalculatorModel : BaseModel
    {
        private Observable<int> _a = new();
        private Observable<int> _b = new();
        private Observable<int> _result = new();

        public Observable<int> A
        {
            get { return _a; }
        }

        public Observable<int> B
        {
            get { return _b; }
        }

        public Observable<int> Result
        {
            get { return _result; }
        }

        public override void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                base.Initialize(context);

                _a.Value = 0;
                _b.Value = 0;
                _result.Value = 0;
            }
        }
    }
}