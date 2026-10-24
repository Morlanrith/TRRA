/* Derived from the code sample provided by the user 'Ramona'
 * Found here: https://forums.terraria.org/index.php?threads/how-to-change-the-transformation-logic-in-shimmer.141889/
 */
using Terraria;
using Terraria.ModLoader;
using TRRA.Items.Weapons;

public class ShimmerEvent : ILoadable
{
    public bool IsLoadingEnabled(Mod mod) => true;
    public void Unload() { }

    public void Load(Mod mod)
    {
        // Add a detour to Item.GetShimmered()
        On_Item.GetShimmered += static (On_Item.orig_GetShimmered originalFunction, Item self) =>
        {
            uint currentRank = GetCurrentRank(self);
            originalFunction(self);
            ApplyRank(self, currentRank);
        };
    }

    public static uint GetCurrentRank(Item item)
    {
        if (item.ModItem != null &&
            item.ModItem is TRRAWeapon trraWpn)
        {
            return trraWpn.CurrentRank;
        }

        return 1;
    }

    public static void ApplyRank(Item item, uint newRank)
    {
        if (item.ModItem != null &&
            item.ModItem is TRRAWeapon trraWpn)
        {
            trraWpn.SetRank(newRank);
        }
    }

}