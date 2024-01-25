﻿using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Model;
using UnityEngine;

namespace MVC.Scripts.Models
{
    public class CalculatorModel : BaseModel
    {
        public Observable<int> A { get; } = new();

        public Observable<int> B { get; } = new();

        public Observable<int> Result { get; } = new();

        public override void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                base.Initialize(context);

                A.Value = 0;
                B.Value = 0;
                Result.Value = 0;
            }
        }
    }
}