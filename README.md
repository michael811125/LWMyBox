# LWMyBox (Lightweight MyBox)

Keep only commonly used attributes, improve Unity Editor compilation efficiency.

Partial reference to [NaughtyAttributes](https://github.com/dbrizov/NaughtyAttributes/tree/master)

---

## Install via git URL

Add https://github.com/michael811125/LWMyBox.git to Package Manager

---

## Preview

![screenshot_01](https://github.com/michael811125/LWMyBox/assets/30960759/fbcef9f1-f488-42f3-8dd7-79d370e9e5dd)

![screenshot_02](https://github.com/michael811125/LWMyBox/assets/30960759/bf8de800-7196-4ada-b939-da513dec7bd9)

---

### ButtonClicker

![screenshot_06](https://github.com/michael811125/LWMyBox/assets/30960759/e2451bc1-c6b5-4663-95b8-bca25a086bcd)

```C#
[CreateAssetMenu]
public class OuterClass : ScriptableObject
{
    [Serializable]
    public class InnerClass
    {
        // InnerClass ButtonClicker
        [ButtonClicker(typeof(InnerClass), "innerClass", nameof(InnerClassMethod), "Print InnerClass Value", "#4df3ff")]
        public bool invoke;
        public int value = 20;

        public void InnerClassMethod()
        {
            Debug.Log($"[InnerClass] Value is {this.value}");
        }
    }

    // Instance innerClass and value name is "innerClass"
    public InnerClass innerClass = new InnerClass();
    public int value = 10;

    [ButtonMethod("Print OuterClass Value", "#ff4dc2")]
    public void OuterClassMethod()
    {
        Debug.Log($"[OuterClass] Value is {this.value}");
    }
}
```

### ButtonMethod

![screenshot_03](https://github.com/michael811125/LWMyBox/assets/30960759/587e474a-fab3-49d5-a17a-06df059031ef)

```C#
    [ButtonMethod("Test Button Method 1 (Click Me)")]
    private void _TestButtonMethod1()
    {
        Debug.Log("Yes, click me 1 !!!");
    }

    [ButtonMethod("Test Button Method 2 (Click Me)", "#3bff8c")]
    private void _TestButtonMethod2()
    {
        Debug.Log("Yes, click me 2 !!!");
    }
```

### ShowAssetPreview

![screenshot_07](https://github.com/michael811125/LWMyBox/assets/30960759/5d806b12-8f7d-45c0-a1cd-20ce33fa7ca9)

```C#
    [ShowAssetPreview]
    public GameObject previewGo;

    [ShowAssetPreview(TextAnchor.MiddleCenter)]
    public Material previewMat1;

    [ShowAssetPreview(128, 128, TextAnchor.MiddleLeft)]
    public Material previewMat2;
```

### ProgressBar

![screenshot_05](https://github.com/michael811125/LWMyBox/assets/30960759/4f877a3c-cb0f-4db7-865c-f701a761f602)

```C#
    [ProgressBar("Health", 500)]
    public int health = 250;

    [ProgressBar("Energy", 500, "#ffcc42")]
    public float energy = 440;

    [ProgressBar("Magic", nameof(maxMagic), "#c660ff")]
    public float magic = 150;
    public float maxMagic = 336;
```

### CurveRange

![screenshot_04](https://github.com/michael811125/LWMyBox/assets/30960759/35346a85-8263-413c-b296-c11e69e2b8be)

```C#
    [CurveRange("#ff00d9")]
    public AnimationCurve curve1;

    [CurveRange(0f, 0f, 1f, 1f, "#3bff8c")]
    public AnimationCurve curve2;
```

### Separator

![screenshot_08](https://github.com/michael811125/LWMyBox/assets/30960759/4c274885-e503-4349-a2e8-74fcb82bbf9c)

```C#
    [Separator("GameObject / Component")]
    public Transform Target;
    [ConditionalField(nameof(Target))] public float TargetSpeed;
    [ConditionalField(nameof(Target), inverse: true)] public float Speed;

    [Separator("Bool", "#acff5a", 1.25f)]
    public bool Teleport;
    [ConditionalField(nameof(Teleport))] public float TeleportationDelay;

    [Separator("String")]
    public string Hello;
    [ConditionalField(nameof(Hello), false, "Hello")] public string HelloWorld = "World! :)";
    [ConditionalField(nameof(Hello))] public string HelloHint = "Print \"Hello\"!";

    [Separator(2, "#e2ff5a")]
    public AIState State = AIState.None;
    [ConditionalField(nameof(State), false, AIState.Walk)] public float WalkSpeed;
    [ConditionalField(nameof(State), false, AIState.Idle)] public float IdleTime;
```

---

## Testing

Compiled and tested

---

## Reference

If you have complete requirements and documentation, please support the original author. (**[MyBox](https://github.com/Deadcows/MyBox)**)
