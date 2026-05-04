using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models.RelicPools;
using jhin.Extensions;

namespace jhin.Relics;

[Pool(typeof(SharedRelicPool))]
public class Whisper : CustomRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Starter;

    public override string PackedIconPath => Placeholders.Role;

    protected override string PackedIconOutlinePath => Placeholders.Role;

    protected override string BigIconPath => Placeholders.Role;
}
