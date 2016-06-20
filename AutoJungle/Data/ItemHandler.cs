using System.Collections.Generic;
using LeagueSharp;
using LeagueSharp.Common;

namespace AutoJungle.Data
{
    internal class ItemHandler
    {
        public static List<BuyingOrder> ItemList = new List<BuyingOrder>();

        public ItemHandler(BuildType type)
        {
            switch (type)
            {
                case BuildType.Ad:
                    SetListAd();
                    break;
                case BuildType.As:
                    SetListAs();
                    break;
                case BuildType.Asmana:
                    SetListAsmana();
                    break;
                case BuildType.Noc:
                    SetListNoc();
                    break;
                case BuildType.Eve:
                    SetListEve();
                    break;
                case BuildType.Apadt:
                    SetListApadt();
                    break;
                case BuildType.Vb:
                    SetlistVb();
                    break;
                case BuildType.Manwang:
                    SetlistManwang();
                    break;
                case BuildType.Yi:
                    SetlistYi();
                    break;
            }
        }

        private static void SetlistManwang()
        {
            ItemList.Add(new BuyingOrder(1, 1041, "打野刀", 350));
            ItemList.Add(new BuyingOrder(2, 2031, "药水", 150));
            ItemList.Add(new BuyingOrder(3, 3340, "饰品", 0));
            ItemList.Add(new BuyingOrder(4, 1039, "打野刀2", 350));
            ItemList.Add(new BuyingOrder(5, 3715, "合成打野", 700));
            ItemList.Add(new BuyingOrder(6, 1042, "攻速1", 350));
            ItemList.Add(new BuyingOrder(7, 1419, "攻速打野", 1350));
            ItemList.Add(new BuyingOrder(8, (int) ItemId.Boots_of_Speed, "鞋", 475));
            ItemList.Add(new BuyingOrder(9, 3006, "攻速鞋", 800));
            ItemList.Add(new BuyingOrder(10, (int) ItemId.Bilgewater_Cutlass, "弯刀", 1650));
            ItemList.Add(new BuyingOrder(11, (int) ItemId.Blade_of_the_Ruined_King, "破败", 1750));
            ItemList.Add(new BuyingOrder(12, 1038, "男票<3刀", 1300));
            ItemList.Add(new BuyingOrder(13, 1037, "十字", 875));
            ItemList.Add(new BuyingOrder(14, 1018, "衣服", 800));
            ItemList.Add(new BuyingOrder(15, 3031, "暴击", 625));
            ItemList.Add(new BuyingOrder(16, (int) ItemId.Recurve_Bow, "弓", 1000));
            ItemList.Add(new BuyingOrder(17, 1026, "棒", 850));
            ItemList.Add(new BuyingOrder(18, 3124, "怒", 1750));
            ItemList.Add(new BuyingOrder(19, 3086, "叉子", 1300));
            ItemList.Add(new BuyingOrder(20, 3087, "电", 1000));
            ItemList.Add(new BuyingOrder(21, (int) ItemId.Elixir_of_Wrath, "嗑药", 500));
        }

        private static void SetListAsmana()
        {
            ItemList.Add(new BuyingOrder(1, 1041, "Hunter's Machete", 350));
            ItemList.Add(new BuyingOrder(2, 2031, "Refillable Potion", 150));
            ItemList.Add(new BuyingOrder(3, 3340, "Trinket", 0));
            ItemList.Add(new BuyingOrder(4, 1039, "Hunter's Talisman", 350));
            ItemList.Add(new BuyingOrder(5, 3715, "Skirmisher's Sabre", 700));
            ItemList.Add(new BuyingOrder(6, 1042, "Dagger", 350));
            ItemList.Add(new BuyingOrder(7, 1419, "Bloodrazor", 1350));
            ItemList.Add(new BuyingOrder(8, (int) ItemId.Boots_of_Speed, "Boots of Speed", 475));
            ItemList.Add(new BuyingOrder(9, (int) ItemId.Tear_of_the_Goddess, "Tear of the Goddess", 750));
            ItemList.Add(new BuyingOrder(10, (int) ItemId.Boots_of_Swiftness, "Boots of Swoftness", 500));
            ItemList.Add(new BuyingOrder(11, (int) ItemId.Bilgewater_Cutlass, "Bilgewater Cutlass", 1650));
            ItemList.Add(new BuyingOrder(12, (int) ItemId.Blade_of_the_Ruined_King, "BOTRK", 1750));
            ItemList.Add(new BuyingOrder(13, (int) ItemId.Tiamat_Melee_Only, "Tiamat", 1250));
            ItemList.Add(new BuyingOrder(14, (int) ItemId.Vampiric_Scepter, "Vampiric_Screpter", 900));
            ItemList.Add(new BuyingOrder(15, (int) ItemId.Ravenous_Hydra_Melee_Only, "Ravenous Hydra Melee Only", 1450));
            ItemList.Add(new BuyingOrder(16, (int) ItemId.Manamune, "Ravenous Hydra Melee Only", 1650));
            ItemList.Add(new BuyingOrder(17, (int) ItemId.Zeal, "Zeal", 1300));
            ItemList.Add(new BuyingOrder(18, (int) ItemId.Phantom_Dancer, "Phantom Dancer", 1500));
        }

        private static void SetListAd()
        {
            //ItemList.Add(new BuyingOrder());
        }

        private static void SetListAs()
        {
            ItemList.Add(new BuyingOrder(1, 1041, "Hunter's Machete", 350));
            ItemList.Add(new BuyingOrder(2, 2031, "Refillable Potion", 150));
            ItemList.Add(new BuyingOrder(3, 3340, "Trinket", 0));
            ItemList.Add(new BuyingOrder(4, 1039, "Hunter's Talisman", 350));
            ItemList.Add(new BuyingOrder(5, 3715, "Skirmisher's Sabre", 700));
            ItemList.Add(new BuyingOrder(6, 1042, "Dagger", 350));
            ItemList.Add(new BuyingOrder(7, 1419, "Bloodrazor", 1350));
            ItemList.Add(new BuyingOrder(8, (int) ItemId.Boots_of_Speed, "Boots of Speed", 475));
            ItemList.Add(new BuyingOrder(9, (int) ItemId.Boots_of_Swiftness, "Boots of Swoftness", 500));
            ItemList.Add(new BuyingOrder(10, (int) ItemId.Bilgewater_Cutlass, "Bilgewater Cutlass", 1650));
            ItemList.Add(new BuyingOrder(11, (int) ItemId.Blade_of_the_Ruined_King, "BOTRK", 1750));
            ItemList.Add(new BuyingOrder(12, (int) ItemId.Recurve_Bow, "Recurve Bow", 1000));
            ItemList.Add(new BuyingOrder(13, (int) ItemId.Wits_End, "Wits End", 1800));
            ItemList.Add(new BuyingOrder(14, (int) ItemId.Tiamat_Melee_Only, "Tiamat", 1250));
            ItemList.Add(new BuyingOrder(15, (int) ItemId.Vampiric_Scepter, "Vampiric_Screpter", 900));
            ItemList.Add(new BuyingOrder(16, (int) ItemId.Ravenous_Hydra_Melee_Only, "Ravenous Hydra Melee Only", 1450));
            ItemList.Add(new BuyingOrder(17, (int) ItemId.Zeal, "Zeal", 1200));
            ItemList.Add(new BuyingOrder(18, (int) ItemId.Phantom_Dancer, "Phantom Dancer", 1500));
        }

        private static void SetListNoc()
        {
            ItemList.Add(new BuyingOrder(1, 1041, "Hunter's Machete", 350));
            ItemList.Add(new BuyingOrder(2, 2031, "Refillable Potion", 150));
            ItemList.Add(new BuyingOrder(3, 3340, "Trinket", 0));
            ItemList.Add(new BuyingOrder(4, 1039, "Hunter's Talisman", 350));
            ItemList.Add(new BuyingOrder(5, 3715, "Skirmisher's Sabre", 700));
            ItemList.Add(new BuyingOrder(6, 1042, "Dagger", 350));
            ItemList.Add(new BuyingOrder(7, 1419, "Bloodrazor", 1350));
            ItemList.Add(new BuyingOrder(8, (int) ItemId.Boots_of_Speed, "Boots of Speed", 475));
            ItemList.Add(new BuyingOrder(9, (int) ItemId.Boots_of_Swiftness, "Boots of Swoftness", 500));
            ItemList.Add(new BuyingOrder(10, (int) ItemId.Bilgewater_Cutlass, "Bilgewater Cutlass", 1650));
            ItemList.Add(new BuyingOrder(11, (int) ItemId.Blade_of_the_Ruined_King, "BOTRK", 1750));
            ItemList.Add(new BuyingOrder(12, (int) ItemId.Giants_Belt, "Giants Belt", 1000));
            ItemList.Add(new BuyingOrder(13, (int) ItemId.Chain_Vest, "Chain Vest", 800));
            ItemList.Add(new BuyingOrder(14, 3742, "Dead Mans Plate", 1800));
            ItemList.Add(new BuyingOrder(15, (int) ItemId.Tiamat_Melee_Only, "Tiamat", 1250));
            ItemList.Add(new BuyingOrder(16, (int) ItemId.Vampiric_Scepter, "Vampiric_Screpter", 900));
            ItemList.Add(new BuyingOrder(17, (int) ItemId.Ravenous_Hydra_Melee_Only, "Ravenous Hydra Melee Only", 1450));
            ItemList.Add(new BuyingOrder(18, (int) ItemId.Zeal, "Zeal", 1200));
            ItemList.Add(new BuyingOrder(19, (int) ItemId.Phantom_Dancer, "Phantom Dancer", 1350));
            ItemList.Add(new BuyingOrder(14, (int) ItemId.Elixir_of_Wrath, "Elixir of Wrath", 500));
        }

        private static void SetListApadt()
        {
            ItemList.Add(new BuyingOrder(1, 1041, "Hunter's Machete", 350));
            ItemList.Add(new BuyingOrder(2, 2031, "Refillable Potion", 150));
            ItemList.Add(new BuyingOrder(3, 3340, "Trinket", 0));
            ItemList.Add(new BuyingOrder(4, 1039, "Hunter's Talisman", 350));
            ItemList.Add(new BuyingOrder(5, 3706, "Stalker's Blade", 700));
            ItemList.Add(new BuyingOrder(6, 3113, "Aether Wisp", 850));
            ItemList.Add(new BuyingOrder(7, 1402, "Runic Echoes", 1350));
            ItemList.Add(new BuyingOrder(8, 1001, "Boots of Speed", 750));
            ItemList.Add(new BuyingOrder(9, 3111, "Mercurys Treads", 400));
            ItemList.Add(new BuyingOrder(10, 3052, "Jaurims Fist", 1300));
            ItemList.Add(new BuyingOrder(11, 3077, "Tiamat", 1300));
            ItemList.Add(new BuyingOrder(12, 3748, "Titanic Hydra", 1100));
            ItemList.Add(new BuyingOrder(13, (int) ItemId.Giants_Belt, "Giants Belt", 1000));
            ItemList.Add(new BuyingOrder(14, (int) ItemId.Chain_Vest, "Chain Vest", 800));
            ItemList.Add(new BuyingOrder(15, 3742, "Dead Mans Plate", 1800));
            ItemList.Add(new BuyingOrder(16, 3211, "Spectres Cow", 1200));
            ItemList.Add(new BuyingOrder(17, 3102, "Banshees Veil", 1250));
            ItemList.Add(new BuyingOrder(18, (int) ItemId.Bilgewater_Cutlass, "Bilgewater Cutlass", 1650));
            ItemList.Add(new BuyingOrder(19, (int) ItemId.Blade_of_the_Ruined_King, "BOTRK", 1750));
            ItemList.Add(new BuyingOrder(20, (int) ItemId.Elixir_of_Wrath, "Elixir of Wrath", 500));
            //add heal spell useage and keep buy Elixir of Wrath after full item
        }

        private static void SetListEve()
        {
            ItemList.Add(new BuyingOrder(1, 1041, "Hunter's Machete", 350));
            ItemList.Add(new BuyingOrder(2, 2031, "Refillable Potion", 150));
            ItemList.Add(new BuyingOrder(3, 3340, "Trinket", 0));
            ItemList.Add(new BuyingOrder(4, 1039, "Hunter's Talisman", 350));
            ItemList.Add(new BuyingOrder(5, 3706, "Stalker's Blade", 700));
            ItemList.Add(new BuyingOrder(6, 3113, "Aether Wisp", 850));
            ItemList.Add(new BuyingOrder(7, 1402, "Runic Echoes", 1350));
            ItemList.Add(new BuyingOrder(8, 1001, "Boots of Speed", 400));
            ItemList.Add(new BuyingOrder(9, 3158, "Ionian Boots of Lucidity", 600));
            ItemList.Add(new BuyingOrder(10, (int) ItemId.Giants_Belt, "Giants Belt", 1000));
            ItemList.Add(new BuyingOrder(11, 3116, "Rylais Crystal Scepter", 2250));
            ItemList.Add(new BuyingOrder(12, 1052, "宝典", 435));
            ItemList.Add(new BuyingOrder(13, 3285, "Ludens Echo", 2765));
            ItemList.Add(new BuyingOrder(14, 1026, "Blasting Wand", 850));
            ItemList.Add(new BuyingOrder(15, 3135, "Void Staf", 1800));
            ItemList.Add(new BuyingOrder(16, 1058, "Needlessly Large Rod", 1250));
            ItemList.Add(new BuyingOrder(17, 3089, "Rabadons Deathcap", 2500));
            ItemList.Add(new BuyingOrder(18, (int) ItemId.Elixir_of_Sorcery, "Elixir of Sorcery", 500));
            //出装顺序不好，下次再改
        }

        private static void SetlistVb()
        {
            ItemList.Add(new BuyingOrder(1, 1041, "打野刀", 350));
            ItemList.Add(new BuyingOrder(2, 2031, "Refillable Potion", 150));
            ItemList.Add(new BuyingOrder(3, 3340, "Trinket", 0));
            ItemList.Add(new BuyingOrder(4, 1039, "Hunter's Talisman", 350));
            ItemList.Add(new BuyingOrder(5, 3706, "Stalker's Blade", 700));
            ItemList.Add(new BuyingOrder(6, 3751, "直接合成打野", 1100));
            ItemList.Add(new BuyingOrder(7, 1401, "Cinderhulk", 625));
            ItemList.Add(new BuyingOrder(8, 1001, "Boots of Speed", 600));
            ItemList.Add(new BuyingOrder(9, 1028, "Ruby Crystal", 400));
            ItemList.Add(new BuyingOrder(10, 1031, "Chain Vest", 800));
            ItemList.Add(new BuyingOrder(11, 3742, "Dead Man's Plate", 1800));
            ItemList.Add(new BuyingOrder(12, 3009, "Boots of Swiftness", 600));
            ItemList.Add(new BuyingOrder(13, 3211, "Spectres Cow", 1200));
            ItemList.Add(new BuyingOrder(14, 3065, "Spirit Visage", 1600));
            ItemList.Add(new BuyingOrder(15, 3052, "Jaurims Fist", 1300));
            ItemList.Add(new BuyingOrder(16, 3077, "Tiamat", 1300));
            ItemList.Add(new BuyingOrder(17, 3748, "Titanic Hydra", 1100));
            ItemList.Add(new BuyingOrder(18, (int) ItemId.Giants_Belt, "Giants Belt", 1000));
            ItemList.Add(new BuyingOrder(19, 3143, "Randuin's Omen", 1900));
            ItemList.Add(new BuyingOrder(20, (int) ItemId.Elixir_of_Iron, "Elixir of Iron", 500));
        }

        private static void SetlistYi()
        {
            ItemList.Add(new BuyingOrder(1, 1041, "Hunter's Machete", 350));
            ItemList.Add(new BuyingOrder(2, 2031, "Refillable Potion", 150));
            ItemList.Add(new BuyingOrder(3, 3340, "Trinket", 0));
            ItemList.Add(new BuyingOrder(4, 1039, "Hunter's Talisman", 350));
            ItemList.Add(new BuyingOrder(5, 3706, "Stalker's Blade", 700));
            ItemList.Add(new BuyingOrder(6, 1042, "Dagger", 350));
            ItemList.Add(new BuyingOrder(7, (int) ItemId.Recurve_Bow, "弓", 700));
            ItemList.Add(new BuyingOrder(8, 1416, "Bloodrazor", 625));
            ItemList.Add(new BuyingOrder(9, (int) ItemId.Boots_of_Speed, "Boots of Speed", 475));
            ItemList.Add(new BuyingOrder(10, 3117, "BoM", 600));
            ItemList.Add(new BuyingOrder(11, (int) ItemId.Tiamat_Melee_Only, "Tiamat", 1250));
            ItemList.Add(new BuyingOrder(12, (int) ItemId.Vampiric_Scepter, "Vampiric_Screpter", 900));
            ItemList.Add(new BuyingOrder(13, (int) ItemId.Ravenous_Hydra_Melee_Only, "Ravenous Hydra Melee Only", 1450));
            ItemList.Add(new BuyingOrder(14, (int) ItemId.Bilgewater_Cutlass, "Bilgewater Cutlass", 1650));
            ItemList.Add(new BuyingOrder(15, (int) ItemId.Blade_of_the_Ruined_King, "BOTRK", 1750));
            ItemList.Add(new BuyingOrder(16, 1037, "Pickaxe", 875));
            ItemList.Add(new BuyingOrder(17, 3133, "warhamm", 1100));
            ItemList.Add(new BuyingOrder(18, 3812, "dance", 1525));
            ItemList.Add(new BuyingOrder(19, (int) ItemId.Giants_Belt, "Giants Belt", 1000));
            ItemList.Add(new BuyingOrder(20, 3742, "Dead Mans Plate", 2600));
            ItemList.Add(new BuyingOrder(21, (int) ItemId.Elixir_of_Wrath, "Elixir of Wrath", 500));
        }

        public static void UseItemsJungle()
        {
            if (Items.HasItem((int) ItemId.Tiamat_Melee_Only) && Items.CanUseItem((int) ItemId.Tiamat_Melee_Only) &&
                Helpers.GetMobs(Program.Player.Position, 400).Count > 2)
            {
                Items.UseItem((int) ItemId.Tiamat_Melee_Only);
            }
            if (Items.HasItem((int) ItemId.Ravenous_Hydra_Melee_Only) &&
                Items.CanUseItem((int) ItemId.Ravenous_Hydra_Melee_Only) &&
                Helpers.GetMobs(Program.Player.Position, 400).Count > 2)
            {
                Items.UseItem((int) ItemId.Ravenous_Hydra_Melee_Only);
            }
            var muramana = LeagueSharp.Common.Data.ItemData.Muramana.GetItem().Id;
            if (Items.HasItem(muramana) && Items.CanUseItem(muramana) && Program.Player.HasBuff("Muramana"))
            {
                Items.UseItem(muramana);
            }
        }

        public static void UseItemsCombo(Obj_AI_Base target, bool use)
        {
            if (Items.HasItem((int) ItemId.Tiamat_Melee_Only) && Items.CanUseItem((int) ItemId.Tiamat_Melee_Only) &&
                target.Distance(Program.Player) < 400)
            {
                Items.UseItem((int) ItemId.Tiamat_Melee_Only);
            }
            if (Items.HasItem((int) ItemId.Ravenous_Hydra_Melee_Only) &&
                Items.CanUseItem((int) ItemId.Ravenous_Hydra_Melee_Only) && target.Distance(Program.Player) < 400)
            {
                Items.UseItem((int) ItemId.Ravenous_Hydra_Melee_Only);
            }
            if (!use)
            {
                return;
            }
            if (Items.HasItem((int) ItemId.Bilgewater_Cutlass) && Items.CanUseItem((int) ItemId.Bilgewater_Cutlass) &&
                (target.Distance(Program.Player) > Orbwalking.GetRealAutoAttackRange(target) ||
                 (target.HealthPercent < 35 && Program.Player.HealthPercent < 35)))
            {
                Items.UseItem((int) ItemId.Bilgewater_Cutlass, target);
            }
            if (Items.HasItem((int) ItemId.Blade_of_the_Ruined_King) &&
                Items.CanUseItem((int) ItemId.Blade_of_the_Ruined_King) &&
                (target.Distance(Program.Player) > Orbwalking.GetRealAutoAttackRange(target) ||
                 (target.HealthPercent < 35 && Program.Player.HealthPercent < 35)))
            {
                Items.UseItem((int) ItemId.Blade_of_the_Ruined_King, target);
            }
            var muramana = LeagueSharp.Common.Data.ItemData.Muramana.GetItem().Id;
            if (!Items.HasItem(muramana) || !Items.CanUseItem(muramana))
            {
                return;
            }
            if (!Program.Player.HasBuff("Muramana") && Program.Player.Mana > 250)
            {
                Items.UseItem(muramana);
            }
            else if (Program.Player.HasBuff("Muramana") && Program.Player.Mana < 260)
            {
                Items.UseItem(muramana);
            }
        }
    }

    internal class BuyingOrder
    {
        public int Index;
        public int ItemId;
        public string Name;
        public int Price;

        public BuyingOrder(int idx, int itemid, string name, int price)
        {
            this.Index = idx;
            this.ItemId = itemid;
            this.Name = name;
            this.Price = price;
        }
    }

    internal enum BuildType
    {
        As,
        Ad,
        Asmana,
        Noc,
        Eve,
        Apadt,
        Vb,
        Manwang,
        Yi
    }
}
