# Cheet.NET 

## easy easter eggs in .NET

```csharp
cheet.Map("↑ ↑ ↓ ↓ ← → ← → b a", () => { Debug.WriteLine("Voilà!")); };
```

```csharp
cheet.Map("i d d q d", () => {
  Debug.WriteLine("god mode enabled")
});
```

```csharp
cheet.Map("o n e a t a t i m e", new CheetCallbacks {
  Next = (str, key, num, seq) => {
    Debug.WriteLine("key pressed: " + key);
    Debug.WriteLine("progress: " + num / seq.Length);
    Debug.WriteLine("seq: " + String.Join(" ", seq));
  },

  Fail = (str, seq) => {
    Debug.WriteLine("sequence failed");
  },

  Done = (str, seq) => {
    Debug.WriteLine("+30 lives ;)");
  }
});
```

```csharp
cheet.Map("o n c e", () => {
  Debug.WriteLine("This will only fire once.");
  cheet.Disable("o n c e");
});
```

```csharp
dynamic sequences = new {
  Cross = "up down left right",
  Circle = "left up right down"
};

cheet.Map(sequences.Cross);
cheet.Map(sequences.Circle);

cheet.Done((str, seq) => {
  if (str == sequences.Cross) {
    Debug.WriteLine("cross!");
  } else {
    Debug.WriteLine("circle!");
  }
});
```
