using System;

namespace PostEnot.Toolkits.InteractionSystem
{
    public abstract class InteractionProviderBase<TContext, TInteraction> where TInteraction : InteractionBase
    {
        public virtual int Priority => 0;

        protected TContext Context { get; private set; }

        internal Type TriggerType
        {
            get
            {
                if (_triggerType == null)
                {
                    _triggerType = InitTriggerType();
                }
                return _triggerType ?? throw new InvalidOperationException();
            }
        }

        private Type _triggerType;

        protected abstract Type InitTriggerType();

        protected abstract InteractionResult<TInteraction> TryHandleTrigger<TTrigger>(TTrigger context);

        protected abstract void RegisterOnTrigger(RegisterOnTriggerContext context);

        internal void SetContext(TContext context) => Context = context;

        internal InteractionResult<TInteraction> InternalTryHandleTrigger<TTrigger>(TTrigger context)
            => TryHandleTrigger(context);

        internal void InternalRegisterOnTrigger(RegisterOnTriggerContext context) => RegisterOnTrigger(context);
    }
}
