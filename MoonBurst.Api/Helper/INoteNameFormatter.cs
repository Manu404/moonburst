namespace MoonBurst.Api
{
    public interface INoteNameFormatter
    {
        string GetName(int midiValue);
    }
}