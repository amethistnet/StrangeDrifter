using BepInEx;
using R2API;
using RoR2;
using UnityEngine;
using DrifterTrashToTreasureController = On.RoR2.DrifterTrashToTreasureController;
using SteamUserManager = IL.RoR2.SteamUserManager;

namespace StrangeDrifter
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class StrangeDrifter : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "BismuthWin";
        public const string PluginName = "StrangeDrifter";
        public const string PluginVersion = "1.0.0";

        private ItemTier whiteTier = ItemTier.Tier1;
        private ItemTier greenTier = ItemTier.Tier2;
        private ItemTier redTier = ItemTier.Tier3;

        public void Awake()
        {
            var whitescrapstrange = LegacyResourcesAPI.Load<ItemDef>("ItemDefs/" + nameof(DLC1Content.Items.ScrapWhiteSuppressed));
            if (whitescrapstrange)
            {
                whitescrapstrange.tier = whiteTier;
                whitescrapstrange.deprecatedTier = whiteTier;
            }

            DrifterTrashToTreasureController.UpdateBuffCounts += (orig, self) =>
            {
                orig(self);
                var body = self.body;
                body.SetBuffCount(DLC3Content.Buffs.TrashToTreasureWhite.buffIndex, body.inventory.GetItemCountEffective(RoR2Content.Items.ScrapWhite) + body.inventory.GetItemCountEffective(DLC1Content.Items.ScrapWhiteSuppressed));
                RecalculateStatsAPI.GetStatCoefficients += (sender, args) =>
                {
                    if (sender.bodyIndex != body.bodyIndex) return;
                    int whiteCount = body.inventory.GetItemCountEffective(DLC1Content.Items.ScrapWhiteSuppressed);
                    int statsFromScrapCount = body.inventory.GetItemCountEffective(DLC3Content.Items.StatsFromScrap);

                    // num98 += (float)(num61 * num62) * 0.06f;
                    args.baseMoveSpeedAdd += statsFromScrapCount * whiteCount * 0.06f;
                };
            };
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                var transform = PlayerCharacterMasterController.instances[0].master.GetBodyObject().transform;
                PickupDropletController.CreatePickupDroplet(PickupCatalog.FindPickupIndex(DLC1Content.Items.ScrapWhiteSuppressed.itemIndex), transform.position, transform.up * 10f);
            }

            if (Input.GetKeyDown(KeyCode.F2))
            {
                var transform = PlayerCharacterMasterController.instances[0].master.GetBodyObject().transform;
                PickupDropletController.CreatePickupDroplet(PickupCatalog.FindPickupIndex(RoR2Content.Items.ScrapWhite.itemIndex), transform.position, transform.up * 10f);
            }

            if (Input.GetKeyDown(KeyCode.F3))
            {
                var transform = PlayerCharacterMasterController.instances[0].master.GetBodyObject().transform;
                PickupDropletController.CreatePickupDroplet(PickupCatalog.FindPickupIndex(RoR2Content.Items.ScrapGreen.itemIndex), transform.position, transform.up * 10f);
            }
        }
    }
}