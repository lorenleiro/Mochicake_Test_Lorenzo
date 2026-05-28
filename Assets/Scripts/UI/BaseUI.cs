public abstract class BaseUI
{
    public bool Visible { get; private set; }

    public BaseUI() { }
    public abstract void Dispose();
}
