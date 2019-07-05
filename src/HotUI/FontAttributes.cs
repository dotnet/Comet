namespace HotUI
{
    public struct FontAttributes
    {
        public string Name { get; internal set; }
        public bool Italic { get; internal set; }
        public bool Smallcaps { get; internal set; }
        public float Size { get; internal set; }
        public Weight Weight { get; internal set; }
    }
}