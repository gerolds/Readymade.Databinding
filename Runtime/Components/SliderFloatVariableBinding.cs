using UnityEngine.UI;

namespace Readymade.Databinding.Components {
    /// <inheritdoc />
    /// <summary>
    /// Binds a variable to a <see cref="T:UnityEngine.UI.Slider" />.
    /// </summary>
    public class SliderFloatVariableBinding : ComponentBinding<float, FloatVariable, Slider, float> {
        /// <inheritdoc />
        protected override void OnSubscribe ( Slider component ) {
            component.onValueChanged.AddListener ( ViewChangedHandler );
        }

        /// <inheritdoc />
        protected override void OnUnsubscribe ( Slider component ) {
            component.onValueChanged.RemoveListener ( ViewChangedHandler );
        }

        /// <inheritdoc />
        protected override void OnUpdateViewWithoutNotify ( Slider component, float value ) {
            component.SetValueWithoutNotify ( value );
        }

        /// <inheritdoc />
        protected override float ModelToView ( float value ) {
            return value;
        }

        /// <inheritdoc />
        protected override float ViewToModel ( float value ) {
            return value;
        }
    }
}