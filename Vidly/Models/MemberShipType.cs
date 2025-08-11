namespace Vidly.Models
{
    public class MemberShipType
    {
        public byte Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public short SigUpFee { get; set; }
        public byte DurationInMonth { get; set; }
        public byte DiscountRate { get; set; }
        public static readonly byte Unknown = 0;
        public static readonly byte PayasYouGo = 1;
    }
}
