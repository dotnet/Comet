using System;

namespace HotUI.Graphics
{
    public class Stop : IComparable<Stop>
    {
        private Color _color;
        private float _offset;

        public Stop(float offset, Color color)
        {
            _color = color;
            _offset = offset;
        }

        public Stop(Stop source)
        {
            _color = source._color;
            _offset = source._offset;
        }

        public Color Color
        {
            get => _color;
            set => _color = value;
        }

        public float Offset
        {
            get => _offset;
            set => _offset = value;
        }

        public int CompareTo(Stop obj)
        {
            if (_offset < obj._offset)
                return -1;
            if (_offset > obj._offset)
                return 1;

            return 0;
        }
    }
}