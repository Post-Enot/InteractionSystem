using System;
using System.Collections;
using System.Collections.Generic;

namespace PostEnot.Toolkits.InteractionSystem
{
    public class InteractionMap<TContext, TProvider, TInteraction> : IEnumerable<TProvider>
        where TProvider : InteractionProviderBase<TContext, TInteraction>
        where TInteraction : InteractionBase
    {
        public InteractionMap(IEnumerable<TProvider> interactions)
        {
            if (interactions == null)
            {
                throw new ArgumentNullException(nameof(interactions));
            }
            FillInteractionsDictionary(interactions);
            SortInteractions();
        }

        public IReadOnlyCollection<Type> Triggers => _map.Keys;
        public IReadOnlyCollection<IReadOnlyList<TProvider>> Providers => _map.Values;

        private readonly Dictionary<Type, IReadOnlyList<TProvider>> _map = new();

        public IEnumerable<TProvider> For<TTrigger>()
        {
            Type trigger = typeof(TTrigger);
            if (_map.TryGetValue(trigger, out IReadOnlyList<TProvider> providers))
            {
                foreach (TProvider provider in providers)
                {
                    yield return provider;
                }
                yield break;
            }
            throw new KeyNotFoundException();
        }

        public IEnumerator<TProvider> GetEnumerator()
        {
            foreach (IReadOnlyList<TProvider> providers in _map.Values)
            {
                foreach (TProvider provider in providers)
                {
                    yield return provider;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private void FillInteractionsDictionary(IEnumerable<TProvider> providers)
        {
            foreach (TProvider provider in providers)
            {
                if (provider == null)
                {
                    throw new ArgumentNullException(nameof(provider));
                }
                Type trigger = provider.TriggerType;
                if (_map.TryGetValue(trigger, out IReadOnlyList<TProvider> providersList))
                {
                    List<TProvider> list = (List<TProvider>)providersList;
                    if (list.Contains(provider))
                    {
                        throw new InvalidOperationException();
                    }
                    list.Add(provider);
                }
                else
                {
                    providersList = new List<TProvider>()
                    {
                        provider
                    };
                    _map.Add(trigger, providersList);
                }
            }
        }

        private void SortInteractions()
        {
            foreach (IReadOnlyCollection<TProvider> readOnlyInteractions in _map.Values)
            {
                List<TProvider> interactions = (List<TProvider>)readOnlyInteractions;
                interactions.Sort(InteractionsComprassion);
            }
        }

        private static int InteractionsComprassion(TProvider a, TProvider b) => b.Priority.CompareTo(a.Priority);
    }
}
