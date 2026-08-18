namespace PostEnot.Toolkits.InteractionSystem
{
    public readonly ref struct RegisterOnTriggerContext
    {
        internal RegisterOnTriggerContext(IInteractor interactor) => _interactor = interactor;

        private readonly IInteractor _interactor;

        public void Register<TTrigger>() => _interactor.RegisterTrigger<TTrigger>();
    }
}
