namespace Goiar.Simple.Cqrs.UserIdentities
{
    /// <summary>
    /// <see cref="IUserIdentityHolder"/> Basic implementation
    /// </summary>
    public class UserIdentityHolder : IUserIdentityHolder
    {
        /// <inheritdoc/>
        public string UserId { get; set; }
    }
}
