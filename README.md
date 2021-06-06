# FieldObservationPackage

A tool whichs allows you to use `[ObserveField]` attribute on various fields in your code, which shows their value in **FieldObserver** window. The tool is meant to be used for debug purposes.

# Example usage:

1. Assign `[ObserveField]` attribute to a field in you script.
2. Open `Windows/FieldObserver`.
3. Drag and drop objects you wish to observe into the **Field Observer** window.
4. Go into play mode.

```cs
public class ExampleComponent : MonoBehaviour
{
    [ObserveField]
    private Vector3 position;
    [ObserveField]
    private float lifeTime;

    void Update()
    {
        position = transform.position;
        lifeTime += Time.deltaTime;
    }
}
```

![text](https://github.com/magalek/FieldObservationPackage/blob/master/readme2.gif)
