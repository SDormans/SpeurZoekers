using System;

namespace Speurzoekers.Common.UploadTools
{
    public class EnumDetail
    {
        public Enum EnumValue { get; set; }
        public string CurrentDescription { get; set; }
        public string BaseDescription { get; set; }
        public object Tag { get; set; }

        public EnumDetail(Enum enumValue, string baseDescription)
        {
            EnumValue = enumValue;
            CurrentDescription = baseDescription;
            BaseDescription = baseDescription;
        }

        public EnumDetail(EnumDetail e, string descritpion)
        {
            EnumValue = e.EnumValue;
            CurrentDescription = descritpion;
            BaseDescription = e.BaseDescription;
        }

        public override string ToString()
        {
            return CurrentDescription;
        }
    }

    public class EnumDetail<E> : EnumDetail
    {
        public EnumDetail(E enumValue, string baseDescription) 
            : base((Enum)Enum.ToObject(typeof(E), enumValue), baseDescription)
        {
            EnumValue = enumValue;
        }

        public EnumDetail(EnumDetail<E> e, string descritpion)
            : base(e, descritpion)
        {
            EnumValue = e.EnumValue;
        }

        public new E EnumValue { get; set; }
    }
}
