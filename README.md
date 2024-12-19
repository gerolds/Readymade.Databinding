# Databinding

Databinding allows the definition of atomic values (structs) as scriptable objects that can be observed and
configured to provide basic data validation. This makes it easy to connect systems across domain boundaries without
the need for repetitive message passing code, while still making it somewhat explicit which values are crossing the 
boundary of a given system.

To use variables, simply declare fields in components as any type derived from `SoVariableBase<TValue>` or `VariableReferenceBase<TValue, TVariable>`. A reference functions similar to an InputActionReference that can take
either a plain value, assigned in the editor or a reference to a VariableInstance (Scriptable Object) that holds the
value.

Example Usage:

```csharp
public class Foo : MonoBehaviour {
    [SerializeField]
    FloatVariableReference _speed;
    
    private void Start() {
        if (_speed.IsVariable) {
            _speed.Changed += SpeedChangedHandler;
        }
    }
        
    private void SpeedChangedHandler(SoVariableBase<float> source, float oldValue, float newValue) {
        // speed changed
    }
    
    private void FixedUpdate() {
        transform.Translate(transform.forward * _speed.Value * Time.deltaTime);
    }
}
```

## Intent

The design intent of this implementation is to create a very constrained version of atomic value scriptable objects that can serve as a database that can be connected easily to player UI, simulation components, serialization and the design workflow/tools. It actively tries to prevent any abuse towards visual programming and creating an editor-configured event-based logic flow. While typical implementations of such a system focus on sending events and messages, this implementation is focused only on updating values that represent design settings and a player's management decisions.