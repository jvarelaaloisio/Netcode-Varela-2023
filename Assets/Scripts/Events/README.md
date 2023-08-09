# Event Channels System
This is an extension on the Unity and C# events system that implements a set of communication channels through the observable design pattern. This structure is based on Unity's [own tutorials](https://www.youtube.com/watch?v=WLDgtRNK2VE).

### Use of the communication channels
- To comunicate between objects, we need to create a new channel asset (```Create/Event Channels/<<your channel type>>```).
- This channel has to be referenced from all the event raisers (or observed objects) and all Listeners (or observers).
- All listeners have to subscribe to the channel (```Subscribe(<<HandlerMethod>>)``` or ```TrySubscribe(<<HandlerMethod>>)```).
    It is recomended to subscribe to the channel in ```OnEnable``` and unsubscribe in ```OnDisable```.
The observed object/s can raise the event (```RaiseEvent(<<value>>)``` or ```TryRaiseEvent(<<value>>)```).

### Questions and Feedback

For general questions, feedback or job opportunities, you can [send me an email](juampyvarela@gmail.com).
