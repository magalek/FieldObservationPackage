# FieldObservationPackage

A tool whichs allows you to use `[ObserveField]` attribute on various fields in your code, which shows their value in custom **FieldObserverWindow**.

# Example usage:

1. Assign `[ObserveField]` attribute to a field in you script.
2. Use `ObserverController.ObserveObject(name, this);` in your script `Awake()` method or constructor.
3. Open `Windows/FieldObserver`.

```cs
public class ExampleComponent : MonoBehaviour
{
    [ObserveField]
    private Vector3 position;
    [ObserveField]
    private float lifeTime;

    private void Awake() {
        ObserverController.ObserveObject(name, this);
    }

    void Update()
    {
        position = transform.position;
        lifeTime += Time.deltaTime;
    }
}
```

![text](https://github.com/magalek/FieldObservationPackage/blob/master/readme.gif)
