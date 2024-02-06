# LWMyBox (Lightweight MyBox)

Keep only commonly used attributes, improve Unity Editor compilation efficiency.

Partial reference to [NaughtyAttributes](https://github.com/dbrizov/NaughtyAttributes/tree/master)

---

## Install via git URL

Add https://github.com/michael811125/LWMyBox.git to Package Manager

---

## Preview

![screenshot_01](https://github.com/michael811125/LWMyBox/assets/30960759/3986be40-0c92-4ca6-83d8-2fe1bfe3aa74)

![screenshot_02](https://github.com/michael811125/LWMyBox/assets/30960759/8b999922-733d-44bd-8aa1-589f783c9e52)

---

### ButtonClicker

```C#
[CreateAssetMenu]
public class OuterClass : ScriptableObject
{
    [Serializable]
    public class InnerClass
    {
        // InnerClass ButtonClicker
        [ButtonClicker(typeof(InnerClass), "innerClass", nameof(InnerClassMethod), "Print InnerClass Value")]
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
    [ButtonClicker(nameof(OuterClassMethod), "Print OuterClass Value")]
    public bool invoke;
    public int value = 10;

    public void OuterClassMethod()
    {
        Debug.Log($"[OuterClass] Value is {this.value}");
    }
}
```

### ProgressBar

```C#
    [ProgressBar("Health", 500)]
    public int health = 250;

    [ProgressBar("Energy", 500, "#ffcc42")]
    public float energy = 440;
```

---

## Testing

Compiled and tested

---

## Reference

If you have complete requirements and documentation, please support the original author. (**[MyBox](https://github.com/Deadcows/MyBox)**)
