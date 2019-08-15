namespace Comet.Graphics
{
    public class Shadow
    {
        public Color Color { get; private set; } = Color.Black;
        public float Opacity { get; private set;} = .5f;
        public float Radius { get; private set;} = 10;
        public SizeF Offset { get; private set;} = SizeF.Zero;

        public Shadow()
        {
            
        }

        protected Shadow(Shadow prototype)
        {
            Color = prototype.Color;
            Opacity = prototype.Opacity;
            Radius = prototype.Radius;
            Offset = prototype.Offset;
        }
        
        public Shadow WithColor(Color color)
        {
            var shadow = new Shadow(this);
            shadow.Color = color;
            return shadow;
        }
        
        public Shadow WithOpacity(float opacity)
        {
            var shadow = new Shadow(this);
            shadow.Opacity = opacity;
            return shadow;
        }
        
        public Shadow WithRadius(float radius)
        {
            var shadow = new Shadow(this);
            shadow.Radius = radius;
            return shadow;
        }
        
        public Shadow WithOffset(SizeF offset)
        {
            var shadow = new Shadow(this);
            shadow.Offset = offset;
            return shadow;
        }
    }
}
