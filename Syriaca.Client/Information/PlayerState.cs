namespace Syriaca.Client.Information
{
    public class PlayerState
    {
        /// <summary>
        /// The account ID in the current player state.
        /// </summary>
        public int AccountId { get; set; }
        
        /// <summary>
        /// The User ID in the current player state.
        /// </summary>
        public int UserId { get; set; }

        public static implicit operator PlayerState((int account, int user) info) => new()
            { AccountId = info.account, UserId = info.user };
    }
}