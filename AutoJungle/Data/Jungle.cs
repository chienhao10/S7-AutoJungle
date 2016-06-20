using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;

namespace AutoJungle.Data
{
    public class Jungle
    {
        public static Obj_AI_Hero Player = ObjectManager.Player;

        public static readonly string[] Bosses = { "TT_Spiderboss", "SRU_Dragon", "SRU_Baron", "SRU_RiftHerald" };
        public static SpellSlot SmiteSlot = SpellSlot.Unknown;
        public static Spell Smite;
        public static int SmiteRange = 700;

        public static double SmiteDamage(Obj_AI_Base target)
        {
            return Player.GetSummonerSpellDamage(target, Damage.SummonerSpell.Smite);
        }

        public static void CastSmite(Obj_AI_Base target)
        {
            if (!target.IsValidTarget())
            {
                return;
            }
            if (Program.GameInfo.CurrentMonster == 13 && !target.Name.Contains("Dragon"))
            {
                return;
            }
            if (SmiteSlot == SpellSlot.Unknown)
            {
                return;
            }
            var smiteReady = ObjectManager.Player.Spellbook.CanUseSpell(SmiteSlot) == SpellState.Ready;
            if (target == null)
            {
                return;
            }
            if (!Smite.CanCast(target) || !smiteReady || !(Player.Distance(target.Position) <= Smite.Range)
                || !(target.Health < target.MaxHealth))
            {
                return;
            }
            if (SmiteDamage(target) > target.Health ||
                (((target.Name.Contains("Krug") || target.Name.Contains("Gromp")) &&
                  Player.CountEnemiesInRange(1000) == 0)) ||
                (target.Name.Contains("SRU_Red") && Player.HealthPercent < 5))
            {
                Smite.Cast(target);
            }
        }

        public static void CastSmiteHero(Obj_AI_Hero target)
        {
            if (!target.IsValidTarget())
            {
                return;
            }
            if (Program.GameInfo.CurrentMonster == 13 && !target.Name.Contains("Dragon"))
            {
                return;
            }
            if (SmiteSlot == SpellSlot.Unknown)
            {
                return;
            }
            var smiteReady = ObjectManager.Player.Spellbook.CanUseSpell(SmiteSlot) == SpellState.Ready;
            if (target == null)
            {
                return;
            }
            if (Smite.CanCast(target) && smiteReady && Player.Distance(target.Position) <= Smite.Range &&
                target.Health > Helpers.GetComboDmg(Player, target) * 0.7f &&
                Player.Distance(target) < Orbwalking.GetRealAutoAttackRange(target) &&
                Program.GameInfo.SmiteableMob == null)
            {
                Smite.Cast(target);
            }
        }

        public static bool SmiteReady()
        {
            if (SmiteSlot != SpellSlot.Unknown)
            {
                return ObjectManager.Player.Spellbook.CanUseSpell(SmiteSlot) == SpellState.Ready;
            }
            return false;
        }

        //BIO
        private static readonly int[] SmiteGreen = { 3711, 1408, 1409, 1410, 1418 };
        private static readonly int[] SmiteRed = { 3715, 1412, 1413, 1414, 1419 };
        private static readonly int[] SmiteBlue = { 3706, 1400, 1401, 1402, 1416 };

        public static string Smitetype()
        {
            if (SmiteBlue.Any(id => Items.HasItem(id)))
            {
                return "s5_summonersmiteplayerganker";
            }
            if (SmiteRed.Any(id => Items.HasItem(id)))
            {
                return "s5_summonersmiteduel";
            }
            return SmiteGreen.Any(id => Items.HasItem(id)) ? "summonersmite" : "summonersmite";
        }

        public static void SetSmiteSlot()
        {
            foreach (var spell in
                ObjectManager.Player.Spellbook.Spells.Where(
                    spell => String.Equals(spell.Name, Smitetype(), StringComparison.CurrentCultureIgnoreCase)))
            {
                SmiteSlot = spell.Slot;
                Smite = new Spell(SmiteSlot, SmiteRange);
                return;
            }
        }
    }
}