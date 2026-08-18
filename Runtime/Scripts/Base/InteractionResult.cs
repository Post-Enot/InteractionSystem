#nullable enable

namespace PostEnot.Toolkits.InteractionSystem
{
    public readonly struct InteractionResult<TInteraction> where TInteraction : InteractionBase
    {
        internal InteractionResult(bool isSuccessful, TInteraction? interaction)
        {
            IsSuccessful = isSuccessful;
            Interaction = interaction;
        }

        internal readonly bool IsSuccessful { get; }
        internal readonly TInteraction? Interaction { get; }

        public static InteractionResult<TInteraction> CanNotPerformed => new(false, null);
        public static InteractionResult<TInteraction> Completed => new(true, null);

        public static InteractionResult<TInteraction> Started(TInteraction interaction) => new(true, interaction);
    }
}
