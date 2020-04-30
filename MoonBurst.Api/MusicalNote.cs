namespace MoonBurst.Core.Helper
{
    public class MusicalNote
    {
        public MusicalNote(string name, int i)
        {
            Name = name;
            Value = i;
        }

        public string Name { get; }
        public int Value { get; }
        public string DisplayNameDetailed { get => $"{Name} ({Value})"; }
    }
}