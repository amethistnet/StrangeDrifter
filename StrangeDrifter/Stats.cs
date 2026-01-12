using RoR2;
using R2API;

namespace StrangeDrifter;

public static class Stats
{
    public static void ApplyStrangeScrapStats(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args)
    {
        if (!body.inventory)
            return;
        var statsFromScrapCount = body.inventory.GetItemCountEffective(DLC3Content.Items.StatsFromScrap);

        args.moveSpeedMultAdd += GetWhiteScrapStats(body, args, statsFromScrapCount);
        args.baseRegenAdd += GetGreenScrapStats(body, args, statsFromScrapCount);
        args.attackSpeedMultAdd += GetRedScrapStats(body, args, statsFromScrapCount);
        
    }
    
    public static void ApplyStrangerScrapStats(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args)
    {
        if (!body.inventory)
            return;
        var statsFromScrapCount = body.inventory.GetItemCountEffective(DLC3Content.Items.StatsFromScrap);

        args.armorAdd += GetStrangeWhiteScrapStats(body, args, statsFromScrapCount);
        args.baseRegenAdd += GetStrangeGreenScrapStats(body, args, statsFromScrapCount);
        args.attackSpeedMultAdd += GetStrangeRedScrapStats(body, args, statsFromScrapCount);
        
    }

    private static float GetWhiteScrapStats(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args, int statsFromScrapCount)
    {
        var strangeWhiteScrapCount = body.inventory.GetItemCountEffective(DLC1Content.Items.ScrapWhiteSuppressed);
        if (strangeWhiteScrapCount <= 0 || statsFromScrapCount <= 0)
            return 0;
        //WHITE SCRAP CALC (short for calculator)
        return 0.06f * strangeWhiteScrapCount * statsFromScrapCount;
    }
    private static float GetGreenScrapStats(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args, int statsFromScrapCount)
    {
        var strangeGreenScrapCount = body.inventory.GetItemCountEffective(DLC1Content.Items.ScrapGreenSuppressed);
        if (strangeGreenScrapCount <= 0 || statsFromScrapCount <= 0)
            return 0;
        //GREEN SCRAP
        return statsFromScrapCount * strangeGreenScrapCount * 3f;
    }
    private static float GetRedScrapStats(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args, int statsFromScrapCount)
    {
        var strangeRedScrapCount = body.inventory.GetItemCountEffective(DLC1Content.Items.ScrapRedSuppressed);
        if (strangeRedScrapCount <= 0 || statsFromScrapCount <= 0)
            return 0;
        //you get the point RED SCRAP
        return statsFromScrapCount * strangeRedScrapCount * 0.3f;
        
    }
    private static float GetStrangeWhiteScrapStats(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args, int statsFromScrapCount)
    {
        var strangeWhiteScrapCount = body.inventory.GetItemCountEffective(DLC1Content.Items.ScrapWhiteSuppressed);
        if (strangeWhiteScrapCount <= 0 || statsFromScrapCount <= 0)
            return 0;
        //WHITE SCRAP CALC (short for calculator)
        return 20f * strangeWhiteScrapCount * statsFromScrapCount;
    }
    private static float GetStrangeGreenScrapStats(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args, int statsFromScrapCount)
    {
        var strangeGreenScrapCount = body.inventory.GetItemCountEffective(DLC1Content.Items.ScrapGreenSuppressed);
        if (strangeGreenScrapCount <= 0 || statsFromScrapCount <= 0)
            return 0;
        //GREEN SCRAP
        return statsFromScrapCount * strangeGreenScrapCount * 3f;
    }

    private static float GetStrangeRedScrapStats(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args, int statsFromScrapCount)
    {
        var strangeRedScrapCount = body.inventory.GetItemCountEffective(DLC1Content.Items.ScrapRedSuppressed);
        if (strangeRedScrapCount <= 0 || statsFromScrapCount <= 0)
            return 0;
        //you get the point RED SCRAP
        return statsFromScrapCount * strangeRedScrapCount * 0.3f;
    }
}