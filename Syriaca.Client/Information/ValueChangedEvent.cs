namespace Syriaca.Client.Information
{
    /// <summary>
    /// An event fired when a value changes, providing the old and new value for reference.
    /// </summary>
    /// <typeparam name="T">The type of value.</typeparam>
    public class ValueChangedEvent<T>
    {
        /// <summary>
        /// The old value.
        /// </summary>
        public readonly T OldValue;

        /// <summary>
        /// The new (and current) value.
        /// </summary>
        public readonly T NewValue;

        public ValueChangedEvent(T oldValue, T newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}