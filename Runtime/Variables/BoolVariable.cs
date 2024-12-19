using System;
using UnityEngine;

namespace Readymade.Databinding {
    /// <inheritdoc />
    [CreateAssetMenu ( menuName =  nameof(Readymade) + "/" + nameof(Databinding) + "/Bool Variable", fileName = "New Bool Variable", order = 0 )]
    public class BoolVariable : SoVariable<bool> {
        /// <inheritdoc />
        protected override bool OnClampValue ( in bool value, in bool min, in bool max ) => throw new NotImplementedException ();

        /// <inheritdoc />
        public override bool CanBeClamped => false;
    }
}