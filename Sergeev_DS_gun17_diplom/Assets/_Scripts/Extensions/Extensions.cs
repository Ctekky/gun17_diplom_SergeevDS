namespace Metroidvania
{
    public delegate void EventHandle();

    public delegate void EventHandle<T>(T args);
}