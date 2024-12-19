using UnityEngine;

namespace Readymade.Databinding {
    /// <inheritdoc />
    [CreateAssetMenu ( menuName =  nameof(Readymade) + "/" + nameof(Databinding) + "/Vector2 Variable", fileName = "New Vector2 Variable", order = 0 )]
    public class Vector2Variable : SoVariable<Vector2> {
        /// <inheritdoc />
        public override bool CanBeClamped => true;

        /// <inheritdoc />
        protected override Vector2 OnClampValue ( in Vector2 value, in Vector2 min, in Vector2 max ) {
            return new Vector2 (
                Mathf.Clamp ( value.x, min.x, max.x ),
                Mathf.Clamp ( value.y, min.y, max.y )
            );
        }
    }
}