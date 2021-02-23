namespace Assets.Scripts
{
    public class MetroidEventArgs
    {
        public bool Default { get; private set; } = true;
        public void PreventDefault() => Default = false;
    }
}
