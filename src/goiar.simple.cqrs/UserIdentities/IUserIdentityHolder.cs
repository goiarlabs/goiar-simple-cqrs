namespace Goiar.Simple.Cqrs.UserIdentities
{
    /// <summary>
    /// Interface made to hold an identifier on a scope
    /// </summary>
    public interface IUserIdentityHolder
    {
        /// <summary>
        /// The identifier being hold
        /// </summary>
        string UserId { get; set; }
    }
}
