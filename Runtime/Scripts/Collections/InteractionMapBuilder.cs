#nullable enable

using System;
using System.Collections.Generic;

namespace PostEnot.Toolkits.InteractionSystem
{
    public sealed class InteractionMap<TContext, TInteraction> : InteractionMap<TContext, InteractionProvider<TContext, TInteraction>, TInteraction>
        where TInteraction : InteractionBase
    {
        public InteractionMap(IEnumerable<InteractionProvider<TContext, TInteraction>> interactions) : base(interactions) { }
    }

    public class InteractionMapBuilder<TContext, TInteraction> where TInteraction : InteractionBase
    {
        private readonly List<InteractionProvider<TContext, TInteraction>> _list = new();

        public InteractionMapBuilder<TContext, TInteraction> Register<TTrigger>(Predicate<TContext> interaction, int priority = 0)
        {
            ShortInteractionProvider<TContext, TInteraction, TTrigger> provider = new(interaction, priority);
            _list.Add(provider);
            return this;
        }

        public InteractionMapBuilder<TContext, TInteraction> Register<TTrigger>(Func<TContext, TInteraction?> interaction, int priority = 0)
        {
            LongInteractionProvider<TContext, TInteraction, TTrigger> provider = new(interaction, priority);
            _list.Add(provider);
            return this;
        }

        public InteractionMapBuilder<TContext, TInteraction> Register<TTrigger>(InteractionProvider<TContext, TInteraction> provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }
            _list.Add(provider);
            return this;
        }

        public InteractionMap<TContext, TInteraction> Build() => new(_list);
    }
}
