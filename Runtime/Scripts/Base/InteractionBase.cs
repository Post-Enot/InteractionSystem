namespace PostEnot.Toolkits.InteractionSystem
{
    public abstract class InteractionBase
    {
        public bool IsCompletedOrCanceled { get; private set; }
        public bool IsInProgress => !IsCompletedOrCanceled;

        protected virtual void OnCancel() { }

        protected virtual void OnCompleteOrCancel() { }

        protected void Complete()
        {
            IsCompletedOrCanceled = true;
            OnCompleteOrCancel();
        }

        internal void Cancel()
        {
            IsCompletedOrCanceled = true;
            OnCancel();
            OnCompleteOrCancel();
        }
    }
}
