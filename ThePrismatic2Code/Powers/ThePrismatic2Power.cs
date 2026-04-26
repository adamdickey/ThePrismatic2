using BaseLib.Abstracts;
using BaseLib.Extensions;
using ThePrismatic2.ThePrismatic2Code.Extensions;
using Godot;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public abstract class ThePrismatic2Power : CustomPowerModel
{
    //Loads from ThePrismatic2/images/powers/your_power.png
    public override string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
    public override string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
}