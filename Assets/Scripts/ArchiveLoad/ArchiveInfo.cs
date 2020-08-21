using UnityEngine;

namespace ArchiveLoad
{
    public struct ArchiveInfo
    {
        private float _id;
        private string _metadata;
        private float[] _size;
        private int[] _year;
        private string _topic;
        private string _owner;
        private string _physicalDescription;
        private string _numberOriginal;
        private string _numberRelvas;
        private ArchiveImages _images;
        private bool _hasSBS;

        public ArchiveInfo(
            float id,
            string metadata,
            float sizeX,
            float sizeY,
            int startYear,
            int endYear,
            string topic,
            string owner,
            string physicalDescription,
            string numberOriginal,
            string numberRelvas,
            ArchiveImages images)
        {
            _id = id;
            _metadata = metadata;
            _size = new float[2] { sizeX, sizeY };
            _year = new int[2] { startYear, endYear };
            _topic = topic;
            _owner = owner;
            _physicalDescription = physicalDescription;
            _numberOriginal = numberOriginal;
            _numberRelvas = numberRelvas;
            _images = images;
            _hasSBS = _images.Right != default || _images.Left != default;
        }

        public float ImageWidth => _size[0];
        public float ImageHeight => _size[1];
        public int StartYear => _year[0];
        public int EndYear => _year[1];
        public string Topic => _topic;
        public string Owner => _owner;
        public string PhysicalDescription => _physicalDescription;
        public Sprite Image => _images.Full;
        public float Id => _id;
        public string Metadata => _metadata;
        public string NumberOriginal => _numberOriginal;
        public string NumberRelvas => _numberRelvas;
        public ArchiveImages Images => _images;
        public bool HasSbs => _hasSBS;


        public override string ToString()
        {
            return
            $"Topic: {Topic}\n" +
            $"Owner: {Owner}\n" +
            $"Physical Desc.: {PhysicalDescription}\n" +
            $"Metadata: {Metadata}\n" +
            $"ID: {Id}\n" +
            $"Year: Start {StartYear}, End {EndYear}\n" +
            $"Size: {ImageWidth} x {ImageHeight}\n" +
            $"Has Image: {Image != null}";
        }
    }
}