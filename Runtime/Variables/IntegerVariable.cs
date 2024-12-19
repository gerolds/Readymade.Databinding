using UnityEngine;

namespace Readymade.Databinding {
    /// <inheritdoc />
    [CreateAssetMenu ( menuName =  nameof(Readymade) + "/" + nameof(Databinding) + "/Integer Variable", fileName = "New Integer Variable", order = 0 )]
    public class IntegerVariable : SoVariable<int> {
        /// <inheritdoc />
        protected override int OnClampValue ( in int value, in int min, in int max ) => Mathf.Clamp ( value, min, max );

        /// <inheritdoc />
        public override bool CanBeClamped => true;
    }
}