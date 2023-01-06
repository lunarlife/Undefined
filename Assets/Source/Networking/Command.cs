using Utils.Enums;

namespace Networking
{
    public class Command : EnumType
    {
        public Command(string prefix, string description, string[] titles)
        {
            Prefix = prefix;
            Titles = titles;
            Description = description;
        }

        public string Prefix { get; }
        public string Description { get; }
        public string[] Titles { get; }
    }
}