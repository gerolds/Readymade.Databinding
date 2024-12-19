using UnityEngine;

namespace Readymade.Databinding {
    /// <inheritdoc />
    [CreateAssetMenu ( menuName =  nameof(Readymade) + "/" + nameof(Databinding) + "/Vector3 Variable", fileName = "New Vector3 Variable", order = 0 )]
    public class Vector3Variable : SoVariable<Vector3> {
        /// <inheritdoc />
        public override bool CanBeClamped => true;

        /// <inheritdoc />
        protected override Vector3 OnClampValue ( in Vector3 value, in Vector3 min, in Vector3 max ) {
            return new Vector3 (
                Mathf.Clamp ( value.x, min.x, max.x ),
                Mathf.Clamp ( value.y, min.y, max.y ),
                Mathf.Clamp ( value.z, min.z, max.z )
            );
        }
    }
}