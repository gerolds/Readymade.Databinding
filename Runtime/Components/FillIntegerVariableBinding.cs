using UnityEngine;

namespace Readymade.Databinding.Components {
    /// <inheritdoc />
    /// <summary>
    /// Specialization of a fill variable binding for integer values.
    /// </summary>
    [AddComponentMenu ( "Databinding/" + nameof ( FillIntegerVariableBinding ) )]
    public class FillIntegerVariableBinding : FillVariableBinding<int, IntegerVariable> {
        /// <inheritdoc />
        protected override float GetFillValue ( int value ) => value;
    }
}