#nullable enable

using PostEnot.Toolkits.EventManagement;
using System;
using System.Collections.Generic;

namespace PostEnot.Toolkits.InteractionSystem
{
    public sealed class Interactor<TContext, TProvider, TInteraction> : IInteractor, IDisposable
        where TProvider : InteractionProviderBase<TContext, TInteraction>
        where TInteraction : InteractionBase
    {
        public Interactor(
            TContext context,
            InteractionMap<TContext, TProvider, TInteraction> map,
            IEventReceiver triggerReceiver)
        {
            Context = context;
            _triggerReceiver = triggerReceiver ?? throw new ArgumentNullException(nameof(triggerReceiver));
            Map = map ?? throw new ArgumentNullException(nameof(map));
            foreach (IReadOnlyList<TProvider> providers in map.Providers)
            {
                foreach (TProvider provider in providers)
                {
                    provider.SetContext(context);
                }
                RegisterOnTriggerContext registerOnTriggerContext = new(this);
                providers[0].InternalRegisterOnTrigger(registerOnTriggerContext);
            }
        }

        public InteractionMap<TContext, TProvider, TInteraction> Map { get; }
        public TContext Context { get; }
        public TInteraction? Interaction => (_interaction == null) || _interaction.IsCompletedOrCanceled ? null : _interaction;
        public bool IsPerformed => Interaction != null;
        public bool IsEnabled => _triggerReceiver.IsEnabled;
        public bool IsDisabled => _triggerReceiver.IsDisabled;

        private readonly IEventReceiver _triggerReceiver;

        private TInteraction? _interaction;

        public void Cancel()
        {
            Interaction?.Cancel();
            _interaction = null;
        }

        public void SetEnabled(bool isEnabled)
        {
            if (isEnabled)
            {
                Enable();
            }
            else
            {
                Disable();
            }
        }

        public void ToggleEnabled()
        {
            if (IsEnabled)
            {
                Disable();
            }
            else
            {
                Enable();
            }
        }

        public void Enable() => _triggerReceiver.Enable();

        public void Disable()
        {
            Cancel();
            _triggerReceiver.Disable();
        }

        public void Dispose()
        {
            Cancel();
            _triggerReceiver.UnregisterAll();
        }

        void IInteractor.RegisterTrigger<TTrigger>() => _triggerReceiver.Register<TTrigger>(HandleTriggerEvent);

        private void HandleTriggerEvent<TTrigger>(TTrigger trigger)
        {
            if (IsPerformed)
            {
                return;
            }
            IEnumerable<TProvider> providers = Map.For<TTrigger>();
            foreach (TProvider provider in providers)
            {
                InteractionResult<TInteraction> result = provider.InternalTryHandleTrigger(trigger);
                if (result.IsSuccessful)
                {
                    _interaction = result.Interaction;
                    return;
                }
            }
        }
    }
}
