namespace AwesomeSockets.Model
{
    /// <summary>
    /// Represents a modification to an ISocket.
    /// </summary>
    public interface ISocketModifier
    {
        /// <summary>
        /// Applies the modification represented by this ISocketModifier to the given ISocket.
        /// </summary>
        /// <param name="socket">The ISocket to modify.</param>
        /// <returns>The modified ISocket.</returns>
        ISocket Apply(ISocket socket);
    }
}
