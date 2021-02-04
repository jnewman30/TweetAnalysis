namespace Core.Processing.Interfaces
{
    public interface IEmojiParser
    {
        void Initialize();
        int Count { get; }
    }
}