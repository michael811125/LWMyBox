# LWMyBox (Lightweight MyBox)

Keep only commonly used attributes, improve Unity Editor compilation efficiency.

---

## Install via git URL

Add https://github.com/michael811125/LWMyBox.git to Package Manager

---

## Added

### ButtonClicker

![img_01](https://github.com/michael811125/LWMyBox/assets/30960759/f4addb4e-0002-4708-a10e-201021f35736)
![img_02](https://github.com/michael811125/LWMyBox/assets/30960759/caa9d0cd-7be0-43a7-9334-b63936017942)

```
[CreateAssetMenu]
public class OuterClass : ScriptableObject
{
    [Serializable]
    public class InnerClass
    {
        // InnerClass ButtonClicker
        [ButtonClicker(typeof(InnerClass), "innerClass", nameof(InnerClassMethod), "Print InnerClass Vlaue")]
        public bool invoke;
        public int value = 20;

        public void InnerClassMethod()
        {
            Debug.Log($"[InnerClass] Value is {this.value}");
        }
    }

    // Instance innerClass and value name is "innerClass"
    public InnerClass innerClass = new InnerClass();

    // OuterClass ButtonClicker
    [ButtonClicker(nameof(OuterClassMethod), "Print OuterClass Vlaue")]
    public bool invoke;
    public int value = 10;

    public void OuterClassMethod()
    {
        Debug.Log($"[OuterClass] Value is {this.value}");
    }
}
```

---

## Testing

Compiled and tested

---

## Reference

If you have complete requirements and documentation, please support the original author. (**[MyBox](https://github.com/Deadcows/MyBox)**)
