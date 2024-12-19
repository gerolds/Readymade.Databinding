using UnityEngine;

namespace Readymade.Databinding {
    /// <inheritdoc />
    [CreateAssetMenu ( menuName =  nameof(Readymade) + "/" + nameof(Databinding) + "/Float Variable", fileName = "New Float Variable", order = 0 )]
    public class FloatVariable : SoVariable<float> {
        /// <inheritdoc />
        protected override float OnClampValue ( in float value,in  float min,in  float max ) => Mathf.Clamp ( value, min, max );

        /// <inheritdoc />
        public override bool CanBeClamped => true;
    }
}