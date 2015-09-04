namespace NextLevelSeven.Web
{
    /// <summary>
    ///     Represents an event that is called after HL7 transport related operations.
    /// </summary>
    /// <param name="sender">Transport class, typically inherits from BackgroundTransportBase.</param>
    /// <param name="e">Information about the event.</param>
    public delegate void MessageTransportEventHandler(object sender, MessageTransportEventArgs e);
}