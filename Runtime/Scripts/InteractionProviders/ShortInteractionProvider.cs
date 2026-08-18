using System;

namespace PostEnot.Toolkits.InteractionSystem
{
    internal sealed class ShortInteractionProvider<TContext, TInteraction, TTrigger>
        : InteractionProvider<TContext, TInteraction, TTrigger>
        where TInteraction : InteractionBase
    {
        public ShortInteractionProvider(Predicate<TContext> interaction, int priority)
        {
            _interaction = interaction ?? throw new ArgumentNullException(nameof(interaction));
            _priority = priority;
        }

        public override int Priority => _priority;

        private readonly int _priority;
        private readonly Predicate<TContext> _interaction;

        protected sealed override InteractionResult<TInteraction> TryHandleTrigger(TTrigger context)
            => _interaction(Context)
                ? InteractionResult<TInteraction>.Completed
                : InteractionResult<TInteraction>.CanNotPerformed;
    }
}
