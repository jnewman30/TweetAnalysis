using System.Collections.Generic;
using Core.Data.Model;

namespace Core.Processing.Interfaces
{
    public interface IEmojiParser
    {
        void Initialize();
        IEnumerable<Emoji> Parse(string text);
    }
}