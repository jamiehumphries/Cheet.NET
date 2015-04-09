# Cheet.NET 

## easy easter eggs in .NET

[![Build status](https://ci.appveyor.com/api/projects/status/819vr6gwqy75mfu3/branch/master?svg=true)](https://ci.appveyor.com/project/jamiehumphries/cheet-net/branch/master)

This is a .NET port of the excellent [cheet.js](https://github.com/namuol/cheet.js) by [Louis Acresti (namuol)](https://github.com/namuol). I would recommend checking out that project if you need easter egg functionality in browser!

```csharp
// Initialization
var cheet = new Cheet();
myUIElement.PreviewKeyDown += cheet.OnKeyDown;
```

```csharp
cheet.Map("↑ ↑ ↓ ↓ ← → ← → b a", () => { Debug.WriteLine("Voilà!"); } );
```

```csharp
cheet.Map("i d d q d", () => {
  Debug.WriteLine("god mode enabled");
});
```

```csharp
cheet.Map("o n e a t a t i m e", new CheetCallbacks {
  Next = (str, key, num, seq) => {
    Debug.WriteLine("key pressed: " + key);
    Debug.WriteLine("progress: " + (double)num / seq.Length);
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
  } else if (str == sequences.Circle) {
    Debug.WriteLine("circle!");
  }
});
```

### Demo

The `Cheet.Wpf.Demo` project in this repository demos all of the above sequences.

### Install

#### NuGet
[Core project](https://www.nuget.org/packages/Cheet.Core/):

    Install-Package Cheet.Core

[WPF version](https://www.nuget.org/packages/Cheet.Wpf/):

    Install-Package Cheet.Wpf

### API

<a name='api_cheet'></a>
#### [`cheet.Map(sequence, done | callbacks { done, next, fail })`](#api_cheet)

Map a sequence of keypresses to a callback. This can be called multiple times.

> <a name='api_cheet_sequence'></a>
> [`sequence`](#api_cheet_sequence) (String)
> > A string representation of a sequence of [key names](#available-key-names).
> > 
> > Each keyname must be separated by a single space.
> 
> <a name='api_cheet_done'></a>
> [`done(str, seq)`](#api_cheet_done) (callback)
> > A callback to execute each time the sequence is correctly pressed.
> > 
> > Arguments:
> > * `str` - The string representation of the sequence that completed.
> > * `seq` - An array of keys representing the sequence that completed.
> 
> <a name='api_cheet_fail'></a>
> [`fail(str, seq)`](#api_cheet_fail) (callback)
> > A callback to execute each time a sequence's progress is broken.
> > 
> > Arguments:
> > * `str` - The string representation of the sequence that failed.
> > * `seq` - An array of keys representing the sequence that was pressed.
>
> <a name='api_cheet_next'></a>
> [`next(str, key, num, seq)`](#api_cheet_next) (callback)
> > A callback to execute each time a correct key in the sequence is pressed *in order*.
> > 
> > Arguments:
> > * `str` - The string representation of the sequence that is in progress.
> > * `key` - The [name of the key](#available-key-names) that was just pressed.
> > * `num` - A number representing the current progress of the sequence. (starts at 0)
> > * `seq` - An array of keys representing the sequence that is in progress.

<a name='api_done'></a>
#### [`cheet.Done(callback)`](#api_done)

Set a global callback that executes whenever *any* mapped sequence is completed successfully.

> <a name='api_done_callback'></a>
> [`callback(str, seq)`](#api_done_callback) (callback)
> > A callback to execute each time *any* sequence is correctly pressed.
> > 
> > Arguments:
> > * `str` - The string representation of the sequence that completed.
> > * `seq` - An array of keys representing the sequence that completed.

<a name='api_next'></a>
#### [`cheet.Next(callback)`](#api_next)

Set a global callback that executes whenever *any* mapped sequence progresses.

> <a name='api_next_callback'></a>
> [`callback(str, key, num, seq)`](#api_next_callback) (callback)
> > A callback to execute each time a correct key in any sequence is pressed *in order*.
> > 
> > Arguments:
> > * `str` - The string representation of the sequence that is in progress.
> > * `key` - The [name of the key](#available-key-names) that was just pressed.
> > * `num` - A number representing the current progress of the sequence. (starts at 0)
> > * `seq` - An array of keys representing the sequence that is in progress.

<a name='api_fail'></a>
#### [`cheet.Fail(callback)`](#api_fail)

Set a global callback that executes whenever *any* in-progress sequence is broken.

> <a name='api_fail_callback'></a>
> [`callback(str, seq)`](#api_fail_callback) (callback)
> > A callback to execute each time *any* sequence's progress is broken.
> > 
> > Arguments:
> > * `str` - The string representation of the sequence that failed.
> > * `seq` - An array of keys representing the sequence that was pressed.

<a name='api_disable'></a>
#### [`cheet.Disable(sequence)`](#api_disable)

Disable a previously-mapped sequence.

> <a name='api_disable_sequence'></a>
> [`sequence`](#api_disable_sequence) (String)
> > The same string you used to map the callback when using [`cheet.Map(seq, ...)`](#api_cheet).

<a name='api_reset'></a>
#### [`cheet.Reset(sequence)`](#api_reset)

Resets a sequence that may or may not be in progress.

This will *not* cause `Fail` callbacks to fire, but will effectively
cancel the sequence.

> <a name='api_reset_sequence'></a>
> [`sequence`](#api_reset_sequence) (String)
> > The same string you used to map the callback when using [`cheet.Map(seq, ...)`](#api_cheet).

### Available Key Names

**NOTE**: Key names are case-sensitive

#### Directionals
* `left` | `L` | `←`
* `up` | `U` | `↑`
* `right` | `R` | `→`
* `down` | `D` | `↓`

#### Alphanumeric
* `0`-`9` (main number keys)
* `a`-`z`

#### Misc
* `backspace`
* `tab`
* `enter` | `return`
* `shift` | `⇧`
* `control` | `ctrl` | `⌃`
* `alt` | `option` | `⌥`
* `command` | `⌘`
* `pause`
* `capslock`
* `esc`
* `space`
* `pageup`
* `pagedown`
* `end`
* `home`
* `insert`
* `delete`
* `equal` | `=`
* `comma` | `,`
* `minus` | `-`
* `period` | `.`

#### Keypad
* `kp_0`-`kp_9`
* `kp_multiply`
* `kp_plus`
* `kp_minus`
* `kp_decimal`
* `kp_divide`

#### Function keys
* `f1`-`f12`

### License

MIT

### Acknowledgements

This whole project was inspired and based on [cheet.js](https://github.com/namuol/cheet.js) by [Louis Acresti (namuol)](https://github.com/namuol). The API and all of this documentation is lifted from that project!
