using System;
namespace Comet
{
    /// <summary>
    /// Truncation or wrapping used on Text
    /// </summary>
    public enum LineBreakMode
    {
        NoWrap,
        WordWrap,
        CharacterWrap,
        HeadTruncation,
        TailTruncation,
        MiddleTruncation
    }
}
