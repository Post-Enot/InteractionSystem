using UnityEngine;
using PostEnot.Toolkits.EventManagement;

namespace PostEnot.Toolkits.InteractionSystem
{
    public abstract class InteractorController<TContext, TInteraction> : MonoBehaviour
        where TInteraction : InteractionBase
    {
        #region Inspector
        [SerializeField] private TContext context;
        #endregion

        public InteractionMap<TContext, TInteraction> Map => (InteractionMap<TContext, TInteraction>)Interactor.Map;
        public Interactor<TContext, InteractionProvider<TContext, TInteraction>, TInteraction> Interactor { get; private set; }

        protected abstract IEventBus EventBus { get; }

        protected abstract InteractionMap<TContext, TInteraction> InitInteractionMap( InteractionMapBuilder<TContext, TInteraction> builder);

        protected virtual void Awake()
        {
            InteractionMapBuilder<TContext, TInteraction> builder = new();
            InteractionMap<TContext, TInteraction> map = InitInteractionMap(builder);
            IEventReceiver triggerReceiver = EventBus.CreateReceiver();
            Interactor = new Interactor<TContext, InteractionProvider<TContext, TInteraction>, TInteraction>(context, map, triggerReceiver);
        }

        protected virtual void OnEnable() => Interactor.Enable();

        protected virtual void OnDisable() => Interactor.Disable();

        protected virtual void OnDestroy() => Interactor.Dispose();
    }
}
