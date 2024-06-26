﻿namespace Blueprint41.Core;

public abstract class DisposableScope<T> : IDisposable
    where T : DisposableScope<T>
{
    protected T Attach()
    {
        isDisposed = true;
        isInitialized = false;

        Initialize();
        isInitialized = true;
        SetCurrent();

        isDisposed = false;

        return (T)this;
    }

    private void SetCurrent()
    {
        current ??= new AsyncLocal<Stack<T>?>();

        current.Value ??= new Stack<T>();

        current.Value.Push((T)this);
    }

    public static T? Current
    {
        get
        {
            if (current?.Value is null || current.Value.Count == 0)
                return null;

            return current.Value.Peek();
        }
        set
        {
            value?.SetCurrent();
        }
    }

    [ThreadStatic]
    protected static AsyncLocal<Stack<T>?> current = new();

    private bool isInitialized;
    private bool isDisposed;

    public void Dispose()
    {
        if (!isDisposed)
        {
            try
            {
                if (isInitialized)
                    Cleanup();
            }
            finally
            {
                if (current.Value is not null)
                {
                    if (current.Value.Count > 0)
                        current.Value.Pop();

                    if (current.Value.Count == 0)
                        current.Value = null;
                }

                if (!IsDebug.Value)
                    GC.SuppressFinalize(this);

                isDisposed = true;
            }
        }
    }

    protected virtual void Initialize()
    { }

    protected virtual void Cleanup()
    { }

    ~DisposableScope()
    {
        if (!isDisposed)
        {
            if (IsDebug.Value)
                throw new InvalidOperationException($"The {GetType().Name} should be used with the using command: using ({GetType().Name}.Begin()) {{ ... }}");
            else
                Dispose();
        }
    }

    private static readonly Lazy<bool> IsDebug = new(() => System.Diagnostics.Debugger.IsAttached, true);
}