using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Readymade.Databinding.Components {
    
    /// <summary>
    /// Abstract implementation of a variable-view binding that offers bidirectional updates. Derived classes only need to
    /// implement details of subscribing from/to view components and value conversions. All flow logic is captured by this base
    /// class.
    /// </summary>
    /// <typeparam name="TVariableValue">The value type of the <see cref="SoVariable"/>.</typeparam>
    /// <typeparam name="TVariable">The <see cref="SoVariable"/> type that can be bound by this component.</typeparam>
    /// <typeparam name="TComponent">The component type that can be bound by this component.</typeparam>
    /// <typeparam name="TComponentValue">The value type that drives the state of the view component.</typeparam>
    public abstract class ComponentBinding<TVariableValue, TVariable, TComponent, TComponentValue> : MonoBehaviour
        where TVariable : SoVariable<TVariableValue> where TVariableValue : struct {
        [Tooltip ( "The view to bind to the variable" )]
        [SerializeField]
        protected TComponent View;

        [Tooltip ( "The variable to bind to the view" )]
        [SerializeField]
        protected TVariable Variable;

        /// <summary>
        /// Unity event.
        /// </summary>
        private void Awake () {
            if ( View == null ) {
                View = GetComponent<TComponent> ();
            }
        }

        /// <summary>
        /// UnityEditor event.
        /// </summary>
        private void Reset () {
            View = GetComponent<TComponent> ();
        }

        protected void UpdateView () {
            if ( Variable != null && View != null ) {
                OnUpdateViewWithoutNotify ( View, ModelToView ( Variable.Value ) );
            }
        }

        /// <summary>
        /// Unity event.
        /// </summary>
        private void Start () {
            UpdateView ();
        }

        /// <summary>
        /// Unity event.
        /// </summary>
        private void OnEnable () {
            if ( Variable != null ) {
                Variable.Changed += VariableChangedHandler;
                OnSubscribe ( View );
            }

            UpdateView ();
        }

        /// <summary>
        /// Unity event.
        /// </summary>
        private void OnDisable () {
            if ( Variable != null ) {
                Variable.Changed -= VariableChangedHandler;
                OnUnsubscribe ( View );
            }
        }

        /// <summary>
        /// Called whenever the assigned variable's value changes.
        /// </summary>
        /// <param name="args">The event args describing the value change.</param>
        private void VariableChangedHandler (
            (SoVariable<TVariableValue> source, TVariableValue oldValue, TVariableValue newValue) args
        ) {
            UpdateView ();
        }

        /// <summary>
        /// Called whenever the view component changes its state. This should be subscribed to the view's changed event in <see cref="OnSubscribe"/>.
        /// </summary>
        /// <param name="value">The value received from the updated view component.</param>
        protected void ViewChangedHandler ( TComponentValue value ) => Variable.Value = ViewToModel ( value );

        /// <summary>
        /// Implement any view-specific subscription logic. Should subscribe <see cref="ViewChangedHandler"/> to the view's changed event (if any).
        /// </summary>
        /// <param name="component">The component to subscribe to.</param>
        /// <remarks>Typically implementation of this method in derived classes is very simple and straight forward.</remarks>
        protected abstract void OnSubscribe ( TComponent component );

        /// <summary>
        /// Implement any view-specific un-subscription logic. Should un-subscribe <see cref="ViewChangedHandler"/> from the view's changed event (if any).
        /// </summary>
        /// <param name="component">The component to subscribe to.</param>
        /// <remarks>Typically implementation of this method in derived classes is very simple and straight forward.</remarks>
        protected abstract void OnUnsubscribe ( TComponent component );

        /// <summary>
        /// Update view without triggering a changed event (which would lead to a call cycle).
        /// </summary>
        /// <param name="component">The component to update.</param>
        /// <param name="value">The value to update the component with.</param>
        /// <remarks>Typically implementation of this method in derived classes is very simple and straight forward.</remarks>
        protected abstract void OnUpdateViewWithoutNotify ( TComponent component, TComponentValue value );

        /// <summary>
        /// A converter method that maps a model value read from a variable to a display value that is shown by the view.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The view value.</returns>
        protected abstract TComponentValue ModelToView ( TVariableValue value );

        /// <summary>
        /// A converter method that maps a display value read from a view to a model value that is stored in a variable/database.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The model value.</returns>
        protected abstract TVariableValue ViewToModel ( TComponentValue value );
    }
}