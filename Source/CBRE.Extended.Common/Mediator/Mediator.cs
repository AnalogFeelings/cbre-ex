using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace CBRE.Extended.Common.Mediator;

/// <summary>
/// The mediator is a static event/communications manager.
/// </summary>
public static class Mediator
{
    public delegate void MediatorExceptionEventHandler(object sender, MediatorExceptionEventArgs e);

    public static event MediatorExceptionEventHandler? MediatorException;

    private static void OnMediatorException(object Sender, Enum Message, object Parameter, Exception Exception)
    {
        if (MediatorException != null)
        {
            StackTrace stackTrace = new StackTrace();
            StackFrame[] stackFrames = stackTrace.GetFrames();
            string message = "Mediator exception: " + Message + "(" + Parameter + ")";
            
            foreach (StackFrame stackFrame in stackFrames)
            {
                MethodBase? method = stackFrame.GetMethod();
                
                message += "\r\n    " + method!.ReflectedType!.FullName + "." + method.Name;
            }

            MediatorException(Sender, new MediatorExceptionEventArgs(Message, new Exception(message, Exception)));
        }
    }

    /// <summary>
    /// Helper method to execute the a function with the same name as the message. Called by the listener if desired.
    /// </summary>
    /// <param name="Object">The object to call the method on</param>
    /// <param name="Message">The name of the method</param>
    /// <param name="Parameter">The parameter. If this is an array, the multi-parameter method will be given priority over the single- and zero-parameter methods</param>
    public static bool ExecuteDefault(object Object, Enum Message, object? Parameter)
    {
        const BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
        
        Type objectType = Object.GetType();
        MethodInfo? method = null;
        object[]? parameters = null;
        
        if (Parameter is object[])
        {
            object[] arr = (object[])Parameter;
            
            method = objectType.GetMethod(Message.ToString(), flags, null, arr.Select(x => x == null ? typeof(object) : x.GetType()).ToArray(), null);
            parameters = arr;
        }

        if (method == null && Parameter != null)
        {
            method = objectType.GetMethod(Message.ToString(), flags, null, new[] { Parameter.GetType() }, null);
            parameters = new[] { Parameter };
        }

        if (method == null)
        {
            method = objectType.GetMethod(Message.ToString(), flags);
            if (method != null) parameters = method.GetParameters().Select(x => (object)null).ToArray();
        }

        if (method != null)
        {
            var sync = Object as ISynchronizeInvoke;
            if (sync != null && sync.InvokeRequired) sync.Invoke(new Action(() => method.Invoke(Object, parameters)), null);
            else method.Invoke(Object, parameters);
            return true;
        }

        return false;
    }

    private struct Listener
    {
        public WeakReference Reference { get; init; }
        public readonly int Priority { get; init; }
    }

    private static readonly MultiDictionary<Enum, Listener> _Listeners;

    static Mediator()
    {
        _Listeners = new MultiDictionary<Enum, Listener>();
    }

    public static void Subscribe(Enum Message, IMediatorListener Listener, int Priority = 0)
    {
        _Listeners.AddValue(Message, new Listener { Reference = new WeakReference(Listener), Priority = Priority });
        
        foreach (var list in _Listeners.Values)
        {
            list.Sort((l1, l2) => l1.Priority - l2.Priority);
        }
    }

    public static void UnsubscribeAll(IMediatorListener Listener)
    {
        foreach (var listener in _Listeners.Values)
        {
            listener.RemoveAll(x => !x.Reference.IsAlive || x.Reference.Target == null || x.Reference.Target == Listener);
        }
    }

    public static void Publish(Enum Message, params object[] Parameters)
    {
        object? parameter = null;
        
        if (Parameters.Length == 1) parameter = Parameters[0];
        else if (Parameters.Length > 1) parameter = Parameters;
        
        Publish(Message, parameter);
    }

    private static readonly Stack<Enum> _MessageStack = new Stack<Enum>();

    public static void Publish(Enum Message, object? Parameter = null)
    {
        if (!_Listeners.ContainsKey(Message)) return;

        string debugLine = "";
        foreach (Enum msg in _MessageStack)
        {
            debugLine += $"{msg} > ";
        }

        debugLine += Message;
        Debug.WriteLine(debugLine);
        _MessageStack.Push(Message);
        
        Listener[] list = _Listeners[Message].ToArray();
        
        foreach (Listener listener in list)
        {
            Debug.WriteLine($"{listener.Priority} {listener.Reference.Target?.GetType() ?? typeof(int)}");
            object? target = listener.Reference.Target;
            
            if (target is null || !listener.Reference.IsAlive)
            {
                _Listeners.RemoveValue(Message, listener);
                continue;
            }

            MethodInfo? method = target.GetType().GetMethod("Notify", new[] { typeof(Enum), typeof(object) });
            if (method != null)
            {
                try
                {
                    method.Invoke(target, new[] { Message, Parameter });
                }
                catch (Exception ex)
                {
                    OnMediatorException(method, Message, Parameter, ex);
                }
            }
        }

        _MessageStack.Pop();
    }
}