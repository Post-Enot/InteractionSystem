#nullable enable

using System;

namespace PostEnot.Toolkits.InteractionSystem
{
    internal sealed class LongInteractionProvider<TContext, TInteraction, TTrigger>
        : InteractionProvider<TContext, TInteraction, TTrigger>
        where TInteraction : InteractionBase
    {
        public LongInteractionProvider(Func<TContext, TInteraction?> interaction, int priority)
        {
            _interaction = interaction ?? throw new ArgumentNullException(nameof(interaction));
            _priority = priority;
        }

        public override int Priority => _priority;

        private readonly Func<TContext, TInteraction?> _interaction;
        private readonly int _priority;

        protected override InteractionResult<TInteraction> TryHandleTrigger(TTrigger context)
        {
            TInteraction? interaction = _interaction(Context);
            return interaction == null
                ? InteractionResult<TInteraction>.CanNotPerformed
                : InteractionResult<TInteraction>.Started(interaction);
        }
    }
}
