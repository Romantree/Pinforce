namespace WILL.WT.PINFORCE.Process
{
    public abstract partial class IProcessTimer
    {
        protected const double NET_TIMEOUT = 1D;
        protected const int NET_MAX_RETRY = 3;
        protected int netRetry = 0;
    }
}
