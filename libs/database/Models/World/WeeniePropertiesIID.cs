#nullable disable

namespace ACE.Database.Models.World;

public partial class WeeniePropertiesIID
{
    public uint Id { get; set; }
    public uint ObjectId { get; set; }
    public ushort Type { get; set; }
    public uint Value { get; set; }

    public virtual Weenie Object { get; set; }
}
