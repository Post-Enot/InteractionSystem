using System;

namespace PostEnot.Toolkits.InteractionSystem
{
    public abstract class InteractionProvider<TContext, TInteraction>
        : InteractionProviderBase<TContext, TInteraction>
        where TInteraction : InteractionBase{ }

    public abstract class InteractionProvider<TContext, TInteraction, TTrigger> : InteractionProvider<TContext, TInteraction>
        where TInteraction : InteractionBase
    {
        protected sealed override Type InitTriggerType() => typeof(TTrigger);

        protected abstract InteractionResult<TInteraction> TryHandleTrigger(TTrigger context);

        protected sealed override InteractionResult<TInteraction> TryHandleTrigger<THandledTrigger>(THandledTrigger contextBase)
        {
            if (contextBase is not TTrigger context)
            {
                throw new InvalidOperationException();
            }
            return TryHandleTrigger(context);
        }

        protected sealed override void RegisterOnTrigger(RegisterOnTriggerContext context) => context.Register<TTrigger>();
    }
}
