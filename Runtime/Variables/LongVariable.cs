using System;
using UnityEngine;

namespace Readymade.Databinding
{
    /// <inheritdoc />
    [CreateAssetMenu(menuName = nameof(Readymade) + "/" + nameof(Databinding) + "/Long Variable", fileName = "New Long Variable", order = 0)]
    public class LongVariable : SoVariable<long>
    {
        /// <inheritdoc />
        protected override long OnClampValue(in long value, in long min, in long max) => Math.Clamp(value, min, max);

        /// <inheritdoc />
        public override bool CanBeClamped => true;
    }
}