using RoR2;
using R2API;

namespace StrangeDrifter;

public static class Stats
{
    public static void ApplyTrashStats(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args)
    {
        if (!body.inventory)
            return;

        var strangeWhiteScrapCount = body.inventory.GetItemCountEffective(DLC1Content.Items.ScrapWhiteSuppressed);
        var statsFromScrapCount = body.inventory.GetItemCountEffective(DLC3Content.Items.StatsFromScrap);

        if (strangeWhiteScrapCount <= 0 || statsFromScrapCount <= 0)
            return;

        var bonus = 0.06f * strangeWhiteScrapCount * statsFromScrapCount;

        args.moveSpeedMultAdd += bonus;
    }
}