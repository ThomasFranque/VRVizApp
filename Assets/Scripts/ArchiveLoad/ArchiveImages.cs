using UnityEngine;

namespace ArchiveLoad
{
    public struct ArchiveImages
    {
        private Sprite _full;
        private Sprite _left;
        private Sprite _right;

        public Sprite Full => _full;
        public Sprite Left => _left;
        public Sprite Right => _right;

        public ArchiveImages(Sprite full, Sprite left, Sprite right)
        {
            _full = full;
            _left = left;
            _right = right;
        }
    }
}