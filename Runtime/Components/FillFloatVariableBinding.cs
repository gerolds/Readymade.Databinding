using UnityEngine;
using UnityEngine.Serialization;

namespace Readymade.Databinding.Components {
    /// <inheritdoc />
    /// <summary>
    /// Specialization of a fill variable binding for float values.
    /// </summary>
    [AddComponentMenu ( "Databinding/" + nameof ( FillFloatVariableBinding ) )]
    public class FillFloatVariableBinding : FillVariableBinding<float, FloatVariable> {
        /// <inheritdoc />
        protected override float GetFillValue ( float value ) => value;
    }
}