using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading;
using AutoJungle.Data;
using System.IO;
using LeagueSharp;
using System.Text;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;
using System.Resources;

namespace AutoJungle
{
    internal class Program
    {
        public static GameInfo GameInfo = new GameInfo();

        public static Menu Menu;

        public static float UpdateLimiter, ResetTimer, GameStateChanging;

        public static readonly Obj_AI_Hero Player = ObjectManager.Player;

        public static Random Random = new Random(Environment.TickCount);

        public static ItemHandler ItemHandler;

        public static Vector3 Pos;

        public static ResourceManager ResourceM;
        public static string Culture;
        public static String[] Languages = new String[] { "English", "Chinese (Simplified)", "Chinese (Traditional)" };
        public static String[] LanguagesShort = new String[] { "en", "cn", "tw" };
        public static string FileName, Path;

        #region Main

        private static void Game_OnGameUpdate(EventArgs args)
        {
            if (GameInfo.SmiteableMob != null)
            {
                Jungle.CastSmite(GameInfo.SmiteableMob);
            }
            CastHighPrioritySpells();
            if (ShouldSkipUpdate())
            {
                return;
            }
            SetGameInfo();
            if (GameInfo.WaitOnFountain)
            {
                return;
            }
            //Checking Afk
            if (CheckAfk())
            {
                return;
            }
            if (HighPriorityPositioning())
            {
                MoveToPos();
                return;
            }

            //Check the camp, maybe its cleared
            CheckCamp();
            if (Debug)
            {
                /* Console.WriteLine("Items: ");
                foreach (var i in player.InventoryItems)
                {
                    Console.WriteLine("\t Name: {0}, ID: {1}({2})", i.IData.TranslatedDisplayName, i.Id, (int) i.Id);
                }*/
                GameInfo.Show();
                /*
                foreach (var v in _GameInfo.MonsterList)
                {
                    Console.WriteLine(
                        v.name + ": " + v.IsAlive() + " Next: " + ((Environment.TickCount - v.TimeAtDead) / 1000));
                }*/
            }
            //Shopping
            if (Shopping())
            {
                return;
            }

            //Recalling
            if (RecallHander())
            {
                return;
            }
            if (Menu.Item("UseTrinket").GetValue<bool>())
            {
                PlaceWard();
            }
            MoveToPos();

            CastSpells();
        }

        private static void CastHighPrioritySpells()
        {
            var target = GameInfo.Target;
            switch (ObjectManager.Player.ChampionName)
            {
                case "Jax":
                    var eActive = Player.HasBuff("JaxCounterStrike");
                    switch (GameInfo.GameState)
                    {
                        case State.Jungling:
                        case State.LaneClear:
                            var targetMob = GameInfo.Target;
                            if (Champdata.E.IsReady() && targetMob.IsValidTarget(350) &&
                                (Player.ManaPercent > 40 || Player.HealthPercent < 60 || Player.Level == 1) && !eActive &&
                                GameInfo.DamageCount >= 2 || GameInfo.DamageTaken > Player.Health * 0.2f)
                            {
                                Champdata.E.Cast();
                            }
                            return;
                        case State.FightIng:
                            if (Champdata.E.IsReady() &&
                                ((Champdata.Q.CanCast(target) && !eActive) || (target.IsValidTarget(350)) ||
                                 ((GameInfo.DamageCount >= 2 || GameInfo.DamageTaken > Player.Health * 0.2f) || !eActive)))
                            {
                                Champdata.E.Cast();
                            }
                            break;
                    }
                    break;
            }
        }

        private static bool HighPriorityPositioning()
        {
            if (Player.ChampionName != "Skarner")
            {
                return false;
            }
            var capturablePoints =
                ObjectManager.Get<Obj_AI_Base>()
                    .Where(o => o.Distance(Player) < 700 && !o.IsAlly && o.Name == "SkarnerPassiveCrystal")
                    .OrderBy(o => o.Distance(Player))
                    .FirstOrDefault();
            if (capturablePoints == null)
            {
                return false;
            }
            GameInfo.MoveTo = capturablePoints.Position;
            GameInfo.GameState = State.Positioning;
            return true;
        }

        private static void PlaceWard()
        {
            if (GameInfo.ClosestWardPos.IsValid() && Items.CanUseItem(3340))
            {
                Items.UseItem(3340, GameInfo.ClosestWardPos);
            }
        }

        private static bool CheckAfk()
        {
            if (Player.IsMoving || Player.IsWindingUp || Player.IsRecalling() || Player.Level == 1)
            {
                GameInfo.Afk = 0;
            }
            else
            {
                GameInfo.Afk++;
            }
            if (GameInfo.Afk <= 15 || Player.InFountain())
            {
                return false;
            }
            Player.Spellbook.CastSpell(SpellSlot.Recall);
            return true;
        }

        private static void CheckCamp()
        {
            var nextMob = GetNextMob();
            if (nextMob != null && !nextMob.IsAlive())
            {
                //Console.WriteLine(nextMob.name + " skipped: " + (Environment.TickCount - nextMob.TimeAtDead / 1000f));
                GameInfo.CurrentMonster++;
                GameInfo.MoveTo = nextMob.Position;
                nextMob =
                    GameInfo.MonsterList.OrderBy(m => m.Index).FirstOrDefault(m => m.Index == GameInfo.CurrentMonster);
            }
            if (GameInfo.GameState != State.Positioning)
            {
                return;
            }
            if (Helpers.GetRealDistance(Player, GameInfo.MoveTo) < 500 && GameInfo.MinionsAround == 0 &&
                Player.Level > 1)
            {
                GameInfo.CurrentMonster++;
                if (nextMob != null)
                {
                    GameInfo.MoveTo = nextMob.Position;
                }
                //Console.WriteLine("CheckCamp - MoveTo: CurrentMonster++");
            }

            var probablySkippedMob = Helpers.GetNearest(Player.Position, 1000);
            if (probablySkippedMob == null || !(probablySkippedMob.Distance(GameInfo.MoveTo) > 200))
            {
                return;
            }
            var monster = GameInfo.MonsterList.FirstOrDefault(m => probablySkippedMob.Name.Contains(m.Name));
            if (monster != null && monster.Index < 13)
            {
                GameInfo.MoveTo = probablySkippedMob.Position;
            }
        }

        private static void SetGameInfo()
        {
            GameInfo.GroupWithoutTarget = false;
            ResetDamageTakenTimer();
            AutoLevel.Enable();
            GameInfo.WaitOnFountain = WaitOnFountain();
            GameInfo.ShouldRecall = ShouldRecall();
            GameInfo.GameState = SetGameState();
            GameInfo.MoveTo = GetMovePosition();
            GameInfo.Target = GetTarget();
            GameInfo.MinionsAround = Helpers.GetMobs(Player.Position, 700).Count;
            GameInfo.SmiteableMob = Helpers.GetNearest(Player.Position);
            GameInfo.AllyStructures = GetStructures(true, GameInfo.SpawnPointEnemy);
            GameInfo.EnemyStructures = GetStructures(false, GameInfo.SpawnPoint);
            GameInfo.ClosestWardPos = Helpers.GetClosestWard();
        }

        private static IEnumerable<Vector3> GetStructures(bool ally, Vector3 basePos)
        {
            var turrets =
                ObjectManager.Get<Obj_Turret>()
                    .Where(t => t.IsAlly == ally && t.IsValid && t.Health > 0 && t.Health < t.MaxHealth)
                    .OrderBy(t => t.Position.Distance(basePos))
                    .Select(t => t.Position);
            var inhibs =
                ObjectManager.Get<Obj_BarracksDampener>()
                    .Where(t => t.IsAlly == ally && t.IsValid && t.Health > 0 && !t.IsDead && t.Health < t.MaxHealth)
                    .OrderBy(t => t.Position.Distance(basePos))
                    .Select(t => t.Position);
            var nexus =
                ObjectManager.Get<Obj_HQ>()
                    .Where(t => t.IsAlly == ally && t.IsValid && t.Health > 0 && !t.IsDead && t.Health < t.MaxHealth)
                    .OrderBy(t => t.Position.Distance(basePos))
                    .Select(t => t.Position);

            return turrets.Concat(inhibs).Concat(nexus);
        }

        #region MainFunctions

        private static bool RecallHander()
        {
            if ((GameInfo.GameState != State.Positioning && GameInfo.GameState != State.Retreat) ||
                !GameInfo.MonsterList.Any(m => m.Position.Distance(Player.Position) < 800))
            {
                return false;
            }
            if (Helpers.GetMobs(Player.Position, 1300).Count > 0)
            {
                return false;
            }
            if (Player.InFountain() || Player.ServerPosition.Distance(GameInfo.SpawnPoint) < 1000)
            {
                return false;
            }
            if ((GameInfo.ShouldRecall && !Player.IsRecalling() && !Player.InFountain()) &&
                (GameInfo.GameState == State.Positioning ||
                 (GameInfo.GameState == State.Retreat &&
                  (GameInfo.Afk > 15 ||
                   ObjectManager.Get<Obj_AI_Base>().Count(o => o.IsEnemy && o.Distance(Player) < 2000) == 0))))
            {
                if (Player.Distance(GameInfo.SpawnPoint) > 6000)
                {
                    Player.Spellbook.CastSpell(SpellSlot.Recall);
                }
                else
                {
                    Player.IssueOrder(GameObjectOrder.MoveTo, GameInfo.SpawnPoint);
                }
                return true;
            }

            return Player.IsRecalling();
        }

        private static void CastSpells()
        {
            if (GameInfo.Target == null)
            {
                return;
            }
            switch (GameInfo.GameState)
            {
                case State.FightIng:
                    GameInfo.Champdata.Combo();
                    break;
                case State.Ganking:
                    break;
                case State.Jungling:
                    GameInfo.Champdata.JungleClear();
                    UsePotions();
                    break;
                case State.LaneClear:
                    GameInfo.Champdata.JungleClear();
                    UsePotions();
                    break;
                case State.Objective:
                    if (GameInfo.Target is Obj_AI_Hero)
                    {
                        GameInfo.Champdata.Combo();
                    }
                    else
                    {
                        GameInfo.Champdata.JungleClear();
                    }
                    break;
            }
        }

        private static void UsePotions()
        {
            if (Items.HasItem(2031) && Items.CanUseItem(2031) && Player.HealthPercent < Menu.Item("HealthToPotion").GetValue<Slider>().Value &&
                !Player.Buffs.Any(b => b.Name.Equals("ItemCrystalFlask")))
            {
                Items.UseItem(2031);
            }
        }

        private static void MoveToPos()
        {
            if ((GameInfo.GameState != State.Positioning && GameInfo.GameState != State.Ganking &&
                 GameInfo.GameState != State.Retreat && GameInfo.GameState != State.Grouping) ||
                !GameInfo.MoveTo.IsValid())
            {
                return;
            }
            if (!Helpers.CheckPath(Player.GetPath(GameInfo.MoveTo)))
            {
                GameInfo.CurrentMonster++;
                if (Debug)
                {
                    Console.WriteLine(@"MoveTo: CurrentMonster++2");
                }
            }
            if (GameInfo.GameState == State.Retreat && GameInfo.MoveTo.Distance(Player.Position) < 100)
            {
                return;
            }
            if (!GameInfo.MoveTo.IsValid()
                || (!(GameInfo.MoveTo.Distance(GameInfo.LastClick) > 150) && (Player.IsMoving || GameInfo.Afk <= 10)))
            {
                return;
            }
            if (Player.IsMoving)
            {
                var x = (int) GameInfo.MoveTo.X;
                var y = (int) GameInfo.MoveTo.Y;
                Player.IssueOrder(
                    GameObjectOrder.MoveTo,
                    new Vector3(Random.Next(x, x + 100), Random.Next(y, y + 100), GameInfo.MoveTo.Z));
            }
            else
            {
                Player.IssueOrder(GameObjectOrder.MoveTo, GameInfo.MoveTo);
            }
        }

        private static bool Shopping()
        {
            if (!Player.InFountain())
            {
                if (Debug)
                {
                    Console.WriteLine(@"Shopping: Not in shop - false");
                }
                return false;
            }
            if (ObjectManager.Player.HasBuff("ElixirOfWrath") ||
                    ObjectManager.Player.HasBuff("ElixirOfIron") ||
                     ObjectManager.Player.HasBuff("ElixirOfSorcery"))
            {
                return false;
            }
            var current =
                ItemHandler.ItemList.Where(i => Items.HasItem(i.ItemId))
                    .OrderByDescending(i => i.Index)
                    .FirstOrDefault();

            if (current != null)
            {
                var currentIndex = current.Index;
                var orderedList =
                    ItemHandler.ItemList.Where(i => !Items.HasItem(i.ItemId) && i.Index > currentIndex)
                        .OrderBy(i => i.Index);
                var itemToBuy = orderedList.FirstOrDefault();
                if (itemToBuy == null)
                {
                    if (Debug)
                    {
                        Console.WriteLine(@"Shopping: No next Item - false");
                    }
                    return false;
                }
                if (itemToBuy.Price <= Player.Gold)
                {
                    Player.BuyItem((ItemId) itemToBuy.ItemId);
                    if (itemToBuy.Index > 9 && Items.HasItem(2031))
                    {
                        Player.SellItem(Player.InventoryItems.First(i => i.Id == (ItemId) 2031).Slot);
                    }
                    var nextItem = orderedList.FirstOrDefault(i => i.Index == itemToBuy.Index + 1);
                    if (nextItem != null)
                    {
                        GameInfo.NextItemPrice = nextItem.Price;
                    }
                    if (Debug)
                    {
                        Console.WriteLine(@"Shopping: Shopping- " + itemToBuy.Name + @" - true");
                    }
                    return true;
                }
            }
            else
            {
                var firstOrDefault = ItemHandler.ItemList.FirstOrDefault(i => i.Index == 1);
                if (firstOrDefault != null)
                {
                    Player.BuyItem((ItemId) firstOrDefault.ItemId);
                }
                var nextItem = ItemHandler.ItemList.FirstOrDefault(i => i.Index == 2);
                if (nextItem != null)
                {
                    GameInfo.NextItemPrice = nextItem.Price;
                }
                return true;
            }


            if (Debug)
            {
                Console.WriteLine(@"Shopping: End - false");
            }
            return false;
        }

        private static Obj_AI_Base GetTarget()
        {
            switch (GameInfo.GameState)
            {
                case State.Objective:
                    var obj = Helpers.GetNearest(Player.Position, GameInfo.ChampionRange);
                    if (obj != null && (obj.Name.Contains("Dragon") || obj.Name.Contains("Baron")) &&
                        (HealthPrediction.GetHealthPrediction(obj, 3000) + 500 < Jungle.SmiteDamage(obj) ||
                         (GameInfo.EnemiesAround == 0 && Player.Level > 8 &&
                          MinionManager.GetMinions(
                              Player.Position, GameInfo.ChampionRange, MinionTypes.All, MinionTeam.NotAlly)
                              .Take(5)
                              .FirstOrDefault(m => m.Name.Contains("Sru_Crab") && m.Health < m.MaxHealth) == null)))
                    {
                        return obj;
                    }
                    else
                    {
                        return GameInfo.EnemiesAround > 0 ? Helpers.GetTargetEnemy() : null;
                    }
                case State.FightIng:
                    return Helpers.GetTargetEnemy();
                case State.Ganking:
                    return null;
                case State.Jungling:
                    return Helpers.GetMobs(Player.Position, 1000).OrderByDescending(m => m.MaxHealth).FirstOrDefault();
                case State.LaneClear:
                    return
                        Helpers.GetMobs(Player.Position, GameInfo.ChampionRange)
                            .Where(m => !m.UnderTurret(true))
                            .OrderByDescending(m => Player.GetAutoAttackDamage(m, true) > m.Health)
                            .ThenBy(m => m.Distance(Player))
                            .FirstOrDefault();
                case State.Pushing:
                    var enemy = Helpers.GetTargetEnemy();
                    if (enemy != null)
                    {
                        GameInfo.Target = enemy;
                        GameInfo.Champdata.Combo();
                        return enemy;
                    }
                    var enemyTurret =
                        ObjectManager.Get<Obj_AI_Turret>()
                            .FirstOrDefault(
                                t =>
                                    t.IsEnemy && !t.IsDead && t.Distance(Player) < 2000 &&
                                    Helpers.GetAllyMobs(t.Position, 500).Count > 0);
                    if (enemyTurret != null)
                    {
                        GameInfo.Champdata.JungleClear();
                        return enemyTurret;
                    }
                    var mob =
                        Helpers.GetMobs(Player.Position, GameInfo.ChampionRange)
                            .Where(
                                m => !m.UnderTurret(true))
                            .OrderByDescending(m => Player.GetAutoAttackDamage(m, true) > m.Health)
                            .ThenBy(m => m.Distance(Player))
                            .FirstOrDefault();
                    if (mob != null)
                    {
                        GameInfo.Target = mob;
                        GameInfo.Champdata.JungleClear();
                        return mob;
                    }
                    break;
                case State.Defending:
                    var enemyDef = Helpers.GetTargetEnemy();
                    if (enemyDef != null && !GameInfo.InDanger)
                    {
                        GameInfo.Target = enemyDef;
                        GameInfo.Champdata.Combo();
                        return enemyDef;
                    }
                    var mobDef =
                        Helpers.GetMobs(Player.Position, GameInfo.ChampionRange)
                            .OrderByDescending(m => m.CountEnemiesInRange(500) == 0)
                            .ThenByDescending(m => Player.GetAutoAttackDamage(m, true) > m.Health)
                            .ThenBy(m => m.CountEnemiesInRange(500))
                            .FirstOrDefault();
                    if (mobDef != null)
                    {
                        GameInfo.Target = mobDef;
                        GameInfo.Champdata.JungleClear();
                        return mobDef;
                    }
                    break;
            }

            if (Debug)
            {
                Console.WriteLine(@"GetTarget: Cant get target");
            }
            return null;
        }


        private static bool CheckObjective(Vector3 pos)
        {
            if ((pos.CountEnemiesInRange(800) > 0 || pos.CountAlliesInRange(800) > 0) && !CheckForRetreat(null, pos))
            {
                var obj = Helpers.GetNearest(pos);
                if (obj != null && obj.Health < obj.MaxHealth - 300)
                {
                    if (Player.Distance(pos) > Jungle.SmiteRange)
                    {
                        GameInfo.MoveTo = pos;
                        return true;
                    }
                }
            }
            if ((!Jungle.SmiteReady() && (Player.Level < 14 || !(Player.HealthPercent > 80))) || Player.Level < 9
                || !(Player.Distance(Camps.Dragon.Position) < GameInfo.ChampionRange))
            {
                return false;
            }
            var drake = Helpers.GetNearest(Player.Position, GameInfo.ChampionRange);
            if (drake == null || !drake.Name.Contains("Dragon"))
            {
                return false;
            }
            GameInfo.CurrentMonster = 13;
            GameInfo.MoveTo = drake.Position;
            return true;
        }

        private static bool CheckGanking()
        {
            Obj_AI_Hero gankTarget = null;
            if (Player.Level >= Menu.Item("GankLevel").GetValue<Slider>().Value &&
                ((Player.Mana > GameInfo.Champdata.R.ManaCost && Player.MaxMana > 100) || Player.MaxMana <= 100))
            {
                var heroes =
                    HeroManager.Enemies.Where(
                        e =>
                            e.Distance(Player) < Menu.Item("GankRange").GetValue<Slider>().Value && e.IsValidTarget() &&
                            !e.UnderTurret(true) && !CheckForRetreat(e, e.Position)).OrderBy(e => Player.Distance(e));
                foreach (var possibleTarget in heroes)
                {
                    var myDmg = Helpers.GetComboDmg(Player, possibleTarget);
                    if (Player.Level + 1 <= possibleTarget.Level)
                    {
                        continue;
                    }
                    if (Helpers.AlliesThere(possibleTarget.Position, 3000) + 1 <
                        possibleTarget.Position.CountEnemiesInRange(GameInfo.ChampionRange))
                    {
                        continue;
                    }
                    if (Helpers.GetComboDmg(possibleTarget, Player) > Player.Health)
                    {
                        continue;
                    }
                    var ally =
                        HeroManager.Allies.Where(a => !a.IsDead && a.Distance(possibleTarget) < 3000)
                            .OrderBy(a => a.Distance(possibleTarget))
                            .FirstOrDefault();
                    var hp = possibleTarget.Health - myDmg * Menu.Item("GankFrequency").GetValue<Slider>().Value / 100f;
                    if (ally != null)
                    {
                        hp -= Helpers.GetComboDmg(ally, possibleTarget) *
                              Menu.Item("GankFrequency").GetValue<Slider>().Value / 100;
                    }
                    if (!(hp < 0))
                    {
                        continue;
                    }
                    gankTarget = possibleTarget;
                    break;
                }
            }
            if (gankTarget == null)
            {
                return false;
            }
            var gankPosition =
                Helpers.GankPos.Where(p => p.Distance(gankTarget.Position) < 2000)
                    .OrderBy(p => Player.Distance(gankTarget.Position))
                    .FirstOrDefault();
            if (gankTarget.Distance(Player) > 2000 && gankPosition.IsValid() &&
                gankPosition.Distance(gankTarget.Position) < 2000 &&
                Player.Distance(gankTarget) > gankPosition.Distance(gankTarget.Position))
            {
                GameInfo.MoveTo = gankPosition;
                return true;
            }
            else if (gankTarget.Distance(Player) <= 2000)
            {
                GameInfo.MoveTo = gankTarget.Position;
                return true;
            }
            else if (!gankPosition.IsValid())
            {
                GameInfo.MoveTo = gankTarget.Position;
                return true;
            }
            return false;
        }

        private static State SetGameState()
        {
            var enemy = Helpers.GetTargetEnemy();
            var tempstate = State.Null;
            if (CheckForRetreat(enemy, Player.Position))
            {
                tempstate = State.Retreat;
            }
            if (tempstate == State.Null && GameInfo.EnemiesAround == 0 &&
                (CheckObjective(Camps.Baron.Position) || CheckObjective(Camps.Dragon.Position)))
            {
                tempstate = State.Objective;
            }
            if (tempstate == State.Null && GameInfo.GameState != State.Retreat && GameInfo.GameState != State.Pushing &&
                GameInfo.GameState != State.Defending &&
                ((enemy != null && !CheckForRetreat(enemy, enemy.Position) &&
                  Helpers.GetRealDistance(Player, enemy.Position) < GameInfo.ChampionRange)) ||
                Player.HasBuff("skarnerimpalevo"))
            {
                tempstate = State.FightIng;
            }
            if (tempstate == State.Null && Player.Level >= 6 && CheckForGrouping())
            {
                if (GameInfo.MoveTo.Distance(Player.Position) <= GameInfo.ChampionRange)
                {
                    if (
                        ObjectManager.Get<Obj_AI_Turret>()
                            .FirstOrDefault(t => t.Distance(GameInfo.MoveTo) < 2000 && t.IsAlly) != null &&
                        (GameInfo.GameState == State.Grouping || GameInfo.GameState == State.Defending))
                    {
                        tempstate = State.Defending;
                    }
                    else if (GameInfo.GameState != State.Grouping && GameInfo.GameState != State.Retreat &&
                             GameInfo.GameState != State.Jungling)
                    {
                        tempstate = State.Pushing;
                    }
                }
                if (tempstate == State.Null &&
                    (GameInfo.MoveTo.Distance(Player.Position) > GameInfo.ChampionRange || GameInfo.GroupWithoutTarget) &&
                    (GameInfo.GameState == State.Positioning || GameInfo.GameState == State.Grouping))
                {
                    tempstate = State.Grouping;
                }
            }
            if (tempstate == State.Null && GameInfo.EnemiesAround == 0 &&
                (GameInfo.GameState == State.Ganking || GameInfo.GameState == State.Positioning) && CheckGanking())
            {
                tempstate = State.Ganking;
            }
            if (tempstate == State.Null && GameInfo.MinionsAround > 0 &&
                (GameInfo.MonsterList.Any(m => m.Position.Distance(Player.Position) < 700) ||
                 GameInfo.SmiteableMob != null) && GameInfo.GameState != State.Retreat)
            {
                tempstate = State.Jungling;
            }
            if (tempstate == State.Null && CheckLaneClear(Player.Position))
            {
                tempstate = State.LaneClear;
            }
            if (tempstate == State.Null)
            {
                tempstate = State.Positioning;
            }
            if (tempstate == GameInfo.GameState)
            {
                return tempstate;
            }
            else if (Environment.TickCount - GameStateChanging > 1300 || GameInfo.GameState == State.Retreat ||
                     tempstate == State.FightIng)
            {
                GameStateChanging = Environment.TickCount;
                return tempstate;
            }
            else
            {
                return GameInfo.GameState;
            }
        }

        private static bool CheckLaneClear(Vector3 pos)
        {
            return (Helpers.AlliesThere(pos) == 0 || Helpers.AlliesThere(pos) >= 2 ||
                    Player.Distance(GameInfo.SpawnPoint) < 6000 || Player.Distance(GameInfo.SpawnPointEnemy) < 6000 ||
                    Player.Level >= 10) && pos.CountEnemiesInRange(GameInfo.ChampionRange) == 0 &&
                   Helpers.GetMobs(pos, GameInfo.ChampionRange).Count +
                   GameInfo.EnemyStructures.Count(p => p.Distance(pos) < GameInfo.ChampionRange) > 0 &&
                   !GameInfo.MonsterList.Any(m => m.Position.Distance(pos) < 600) && GameInfo.SmiteableMob == null &&
                   GameInfo.GameState != State.Retreat;
        }

        private static bool CheckForRetreat(Obj_AI_Base enemy, Vector3 pos)
        {
            if (GameInfo.GameState == State.Jungling)
            {
                return false;
            }
            if (enemy != null && !enemy.UnderTurret(true) && Player.Distance(enemy) < 350 && !GameInfo.AttackedByTurret)
            {
                return false;
            }
            var indanger = ((Helpers.GetHealth(true, pos) +
                             ((Player.Distance(pos) < GameInfo.ChampionRange) ? 0 : Player.Health)) * 1.3f <
                            Helpers.GetHealth(false, pos) && pos.CountEnemiesInRange(GameInfo.ChampionRange) > 1 &&
                            Helpers.AlliesThere(pos, 500) == 0) ||
                           Player.HealthPercent < Menu.Item("HealtToBack").GetValue<Slider>().Value;
            if (!indanger && !GameInfo.AttackedByTurret)
            {
                return false;
            }
            if (((enemy != null && Helpers.AlliesThere(pos, 600) > 0) && Player.HealthPercent > 25))
            {
                return false;
            }
            if (GameInfo.AttackedByTurret)
            {
                if ((enemy != null &&
                     (enemy.Health > Player.GetAutoAttackDamage(enemy, true) * 2 ||
                      enemy.Distance(Player) > Orbwalking.GetRealAutoAttackRange(enemy) + 20) || enemy == null))
                {
                    return true;
                }
            }
            return indanger;
        }

        private static bool CheckForGrouping()
        {
            //Checking grouping allies
            var ally =
                HeroManager.Allies.FirstOrDefault(
                    a => Helpers.AlliesThere(a.Position) >= 2 && a.Distance(GameInfo.SpawnPointEnemy) < 7000);
            if (ally != null && !CheckForRetreat(null, ally.Position) &&
                Helpers.CheckPath(Player.GetPath(ally.Position)))
            {
                GameInfo.MoveTo = ally.Position.Extend(Player.Position, 200);
                GameInfo.GroupWithoutTarget = true;
                if (Debug)
                {
                    Console.WriteLine(@"CheckForGrouping() - Checking grouping allies");
                }
                return true;
            }
            //Checknig base after recall
            if (Player.Distance(GameInfo.SpawnPoint) < 5000)
            {
                var mob =
                    Helpers.GetMobs(GameInfo.SpawnPoint, 5000)
                        .OrderByDescending(m => Helpers.GetMobs(m.Position, 300).Count)
                        .FirstOrDefault();
                if (mob != null && Helpers.GetMobs(mob.Position, 300).Count > 700 &&
                    Helpers.CheckPath(Player.GetPath(mob.Position)) && !CheckForRetreat(null, mob.Position))
                {
                    GameInfo.MoveTo = mob.Position;
                    if (Debug)
                    {
                        Console.WriteLine(@"CheckForGrouping() - Checknig base after recall");
                    }
                    return true;
                }
            }
            //Checknig enemy turrets
            foreach (var vector in
                GameInfo.EnemyStructures.Where(
                    s =>
                        s.Distance(Player.Position) < Menu.Item("GankRange").GetValue<Slider>().Value &&
                        CheckLaneClear(s)))
            {
                var aMinis = Helpers.GetAllyMobs(vector, GameInfo.ChampionRange);
                if (aMinis.Count <= 1)
                {
                    continue;
                }
                var eMinis =
                    Helpers.GetMobs(vector, GameInfo.ChampionRange)
                        .OrderByDescending(m => Helpers.GetMobs(m.Position, 300).Count)
                        .FirstOrDefault();
                if (eMinis != null)
                {
                    var pos = eMinis.Position;
                    if (!Helpers.CheckPath(Player.GetPath(pos)) || CheckForRetreat(null, pos))
                    {
                        continue;
                    }
                    GameInfo.MoveTo = pos;
                    if (Debug)
                    {
                        Console.WriteLine(@"CheckForGrouping() - Checknig enemy turrets 1");
                    }
                    return true;
                }
                else
                {
                    if (!Helpers.CheckPath(Player.GetPath(vector)) || CheckForRetreat(null, vector))
                    {
                        continue;
                    }
                    GameInfo.MoveTo = vector;
                    if (Debug)
                    {
                        Console.WriteLine(@"CheckForGrouping() - Checknig enemy turrets 2");
                    }
                    return true;
                }
            }
            //Checknig ally turrets
            foreach (var vector in
                GameInfo.AllyStructures.Where(
                    s => s.Distance(Player.Position) < Menu.Item("GankRange").GetValue<Slider>().Value))
            {
                var eMinis = Helpers.GetMobs(vector, GameInfo.ChampionRange);
                if (!CheckLaneClear(vector))
                {
                    continue;
                }
                if (eMinis.Count <= 3)
                {
                    continue;
                }
                var temp = eMinis.OrderByDescending(m => Helpers.GetMobs(m.Position, 300).Count).FirstOrDefault();
                if (temp != null)
                {
                    var pos = temp.Position;
                    if (!Helpers.CheckPath(Player.GetPath(pos)) || CheckForRetreat(null, pos))
                    {
                        continue;
                    }
                    GameInfo.MoveTo = pos;
                    if (Debug)
                    {
                        Console.WriteLine(@"CheckForGrouping() - Checknig ally turrets 1");
                    }
                    return true;
                }
                else
                {
                    if (!Helpers.CheckPath(Player.GetPath(vector)) || CheckForRetreat(null, vector))
                    {
                        continue;
                    }
                    GameInfo.MoveTo = vector;
                    if (Debug)
                    {
                        Console.WriteLine(@"CheckForGrouping() - Checknig ally turrets 2");
                    }
                    return true;
                }
            }
            //follow minis
            var minis = Helpers.GetAllyMobs(Player.Position, 1000);
            if (minis.Count >= 5 && Player.Level >= 8)
            {
                var objAiBase = minis.OrderBy(m => m.Distance(GameInfo.SpawnPointEnemy)).FirstOrDefault();
                if (objAiBase != null &&
                    (objAiBase.CountAlliesInRange(GameInfo.ChampionRange) == 0 ||
                     objAiBase.CountAlliesInRange(GameInfo.ChampionRange) >= 2 || Player.Level >= 10) &&
                    Helpers.GetMobs(objAiBase.Position, 1000).Count == 0)
                {
                    GameInfo.MoveTo = objAiBase.Position.Extend(GameInfo.SpawnPoint, 100);
                    GameInfo.GroupWithoutTarget = true;
                    if (Debug)
                    {
                        Console.WriteLine(@"CheckForGrouping() - follow minis");
                    }
                    return true;
                }
            }
            //Checking free enemy minionwaves
            if (Player.Level > 8)
            {
                var miniwaves =
                    Helpers.GetMobs(Player.Position, Menu.Item("GankRange").GetValue<Slider>().Value)
                        .Where(m => Helpers.GetMobs(m.Position, 1200).Count > 6 && CheckLaneClear(m.Position))
                        .OrderByDescending(m => m.Distance(GameInfo.SpawnPoint) < 7000)
                        .ThenByDescending(m => m.Distance(Player) < 2000)
                        .ThenByDescending(m => Helpers.GetMobs(m.Position, 1200).Count);
                foreach (var miniwave in
                    miniwaves.Where(miniwave => Helpers.GetMobs(miniwave.Position, 1200).Count >= 6)
                        .Where(
                            miniwave =>
                                !CheckForRetreat(null, miniwave.Position) &&
                                Helpers.CheckPath(Player.GetPath(miniwave.Position))))
                {
                    GameInfo.MoveTo = miniwave.Position.Extend(Player.Position, 200);
                    if (Debug)
                    {
                        Console.WriteLine(@"CheckForGrouping() - Checking free enemy minionwavess");
                    }
                    return true;
                }
            }
            //Checking ally mobs, pushing
            if (Player.Level <= 8)
            {
                return false;
            }
            var miniWave =
                ObjectManager.Get<Obj_AI_Minion>()
                    .Where(
                        m =>
                        m.Distance(GameInfo.SpawnPointEnemy) < 7000 &&
                        Helpers.GetAllyMobs(m.Position, 1200).Count >= 7)
                    .OrderByDescending(m => m.Distance(GameInfo.SpawnPointEnemy) < 7000)
                    .ThenBy(m => m.Distance(Player))
                    .FirstOrDefault();
            if (miniWave == null || !Helpers.CheckPath(Player.GetPath(miniWave.Position))
                || CheckForRetreat(null, miniWave.Position) || !CheckLaneClear(miniWave.Position))
            {
                return false;
            }
            GameInfo.MoveTo = miniWave.Position.Extend(Player.Position, 200);
            return true;
        }

        private static Vector3 GetMovePosition()
        {
            switch (GameInfo.GameState)
            {
                case State.Retreat:
                    var enemyTurret =
                        ObjectManager.Get<Obj_AI_Turret>()
                            .FirstOrDefault(t => t.IsEnemy && !t.IsDead && t.Distance(Player) < 2000);
                    var allyTurret =
                        ObjectManager.Get<Obj_AI_Turret>()
                            .OrderBy(t => t.Distance(Player))
                            .FirstOrDefault(
                                t =>
                                    t.IsAlly && !t.IsDead && t.Distance(Player) < 4000 &&
                                    t.CountEnemiesInRange(1200) == 0);
                    if (GameInfo.AttackedByTurret && enemyTurret != null)
                    {
                        if (allyTurret != null)
                        {
                            return allyTurret.Position;
                        }
                        var nextPost = Prediction.GetPrediction(Player, 1);
                        return !nextPost.UnitPosition.UnderTurret(true) ? nextPost.CastPosition : GameInfo.SpawnPoint;
                    }
                    if (allyTurret != null && Player.Distance(GameInfo.SpawnPoint) > Player.Distance(allyTurret))
                    {
                        return allyTurret.Position.Extend(GameInfo.SpawnPoint, 300);
                    }
                    return GameInfo.SpawnPoint;
                case State.Objective:
                    return GameInfo.MoveTo;
                case State.Grouping:
                    return GameInfo.MoveTo;
                case State.Defending:
                    return Vector3.Zero;
                case State.Pushing:
                    return Vector3.Zero;
                case State.Warding:
                    return GameInfo.MoveTo;
                case State.FightIng:
                    return Vector3.Zero;
                case State.Ganking:
                    return GameInfo.MoveTo;
                case State.Jungling:
                    return Vector3.Zero;
                case State.LaneClear:
                    return Vector3.Zero;
                default:
                    var nextMob = GetNextMob();
                    if (nextMob != null)
                    {
                        return nextMob.Position;
                    }
                    var firstOrDefault = GameInfo.MonsterList.FirstOrDefault(m => m.Index == 1);
                    if (firstOrDefault != null)
                    {
                        return firstOrDefault.Position;
                    }
                    break;
            }

            if (Debug)
            {
                Console.WriteLine(@"GetMovePosition: Can't get Position");
            }
            return Vector3.Zero;
        }

        private static MonsterInfo GetNextMob()
        {
            MonsterInfo nextMob;
            if (!Menu.Item("EnemyJungle").GetValue<Boolean>())
            {
                if (Player.Team == GameObjectTeam.Chaos)
                {
                    nextMob =
                        GameInfo.MonsterList.OrderBy(m => m.Index)
                            .FirstOrDefault(m => m.Index == GameInfo.CurrentMonster && !m.Id.Contains("bteam"));
                }
                else
                {
                    nextMob =
                        GameInfo.MonsterList.OrderBy(m => m.Index)
                            .FirstOrDefault(m => m.Index == GameInfo.CurrentMonster && !m.Id.Contains("pteam"));
                }
            }
            else
            {
                nextMob =
                    GameInfo.MonsterList.OrderBy(m => m.Index).FirstOrDefault(m => m.Index == GameInfo.CurrentMonster);
            }
            return nextMob;
        }

        private static void ResetDamageTakenTimer()
        {
            if (Environment.TickCount - ResetTimer > 1200)
            {
                ResetTimer = Environment.TickCount;
                GameInfo.DamageTaken = 0f;
                GameInfo.DamageCount = 0;
            }
            if (GameInfo.CurrentMonster == 13 && Player.Level <= 9)
            {
                GameInfo.CurrentMonster++;
            }
            if (GameInfo.CurrentMonster > 16)
            {
                GameInfo.CurrentMonster = 1;
            }
        }

        private static bool ShouldRecall()
        {
            if (Player.HealthPercent <= Menu.Item("HealtToBack").GetValue<Slider>().Value)
            {
                if (Debug)
                {
                    Console.WriteLine(@"ShouldRecall: Low Health - true");
                }
                return true;
            }
            if (GameInfo.CanBuyItem())
            {
                if (Debug)
                {
                    Console.WriteLine(@"ShouldRecall: Can buy item - true");
                }
                return true;
            }
            if (Helpers.GetMobs(GameInfo.SpawnPoint, 5000).Count > 6)
            {
                if (Debug)
                {
                    Console.WriteLine(@"ShouldRecall: Def base - true");
                }
                return true;
            }
            if (GameInfo.GameState == State.Retreat && Player.CountEnemiesInRange(GameInfo.ChampionRange) == 0)
            {
                if (Debug)
                {
                    Console.WriteLine(@"ShouldRecall: After retreat - true");
                }
                return true;
            }
            if (Debug)
            {
                Console.WriteLine(@"ShouldRecall: End - false");
            }
            return false;
        }

        private static bool WaitOnFountain()
        {
            if (!Player.InFountain())
            {
                return false;
            }
            if (Player.InFountain() && Player.IsRecalling())
            {
                return false;
            }
            if (!(Player.HealthPercent < 90) && (!(Player.ManaPercent < 90) || !(Player.MaxMana > 100)))
            {
                return false;
            }
            if (Player.IsMoving)
            {
                Player.IssueOrder(GameObjectOrder.HoldPosition, Player.Position);
            }
            return true;
        }

        private static bool ShouldSkipUpdate()
        {
            if (!Menu.Item("Enabled").GetValue<Boolean>())
            {
                return true;
            }
            if (Environment.TickCount - UpdateLimiter <= 400)
            {
                return true;
            }
            if (Player.IsDead)
            {
                return true;
            }
            if (Player.IsRecalling() && !Player.InFountain())
            {
                return true;
            }
            UpdateLimiter = Environment.TickCount - Random.Next(0, 100);
            return false;
        }

        public static bool Debug => Menu.Item("debug").GetValue<KeyBind>().Active;

        #endregion

        #endregion

        #region Events

        private static void GameProcessSpell(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            var target = args.Target as Obj_AI_Hero;
            if (target == null)
            {
                return;
            }
            if (!target.IsMe || !sender.IsValid || sender.IsDead || !sender.IsEnemy || !target.IsValid)
            {
                return;
            }
            if (Orbwalking.IsAutoAttack(args.SData.Name))
            {
                GameInfo.DamageTaken += (float) sender.GetAutoAttackDamage(Player, true);
                GameInfo.DamageCount++;
            }
            if (!(sender is Obj_AI_Turret) || GameInfo.AttackedByTurret)
            {
                return;
            }
            GameInfo.AttackedByTurret = true;
            Utility.DelayAction.Add(2000, () => GameInfo.AttackedByTurret = false);
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (Debug)
            {
                if (Pos.IsValid())
                {
                    Render.Circle.DrawCircle(Pos, 50, Color.Crimson, 7);
                }

                foreach (var m in Helpers.Mod)
                {
                    Render.Circle.DrawCircle(m, 50, Color.Crimson, 7);
                }

                if (GameInfo.LastClick.IsValid())
                {
                    Render.Circle.DrawCircle(GameInfo.LastClick, 70, Color.Blue, 7);
                }
                if (GameInfo.MoveTo.IsValid())
                {
                    Render.Circle.DrawCircle(GameInfo.MoveTo, 77, Color.BlueViolet, 7);
                }
                foreach (var e in GameInfo.EnemyStructures)
                {
                    Render.Circle.DrawCircle(e, 300, Color.Red, 7);
                }
                foreach (var a in GameInfo.AllyStructures)
                {
                    Render.Circle.DrawCircle(a, 300, Color.DarkGreen, 7);
                }
                if (GameInfo.ClosestWardPos.IsValid())
                {
                    Render.Circle.DrawCircle(GameInfo.ClosestWardPos, 70, Color.LawnGreen, 7);
                }
            }
            if (Menu.Item("State").GetValue<Boolean>())
            {
                Drawing.DrawText(150f, 200f, Color.Aqua, GameInfo.GameState.ToString());
            }
        }

        private static void Obj_AI_Base_OnNewPath(Obj_AI_Base sender, GameObjectNewPathEventArgs args)
        {
            if (sender.IsMe)
            {
                GameInfo.LastClick = args.Path.Last();
            }
        }

        private static void OnValueChanged(object sender, OnValueChangeEventArgs onValueChangeEventArgs)
        {
            try
            {
                var index = Languages.ToList().IndexOf(onValueChangeEventArgs.GetNewValue<StringList>().SelectedValue);
                File.WriteAllText(Path + FileName, LanguagesShort[index], Encoding.Default);
                Console.WriteLine(@"Changed to " + LanguagesShort[index]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        #endregion

        #region Init

        private static void Main()
        {
            CustomEvents.Game.OnGameLoad += OnGameLoad;
        }

        private static void OnGameLoad(EventArgs args)
        {
            SetCulture();
            if (Game.MapId != GameMapId.SummonersRift)
            {
                Game.PrintChat(ResourceM.GetString("MapNotSupported"));
                return;
            }
            GameInfo.Champdata = new Champdata();
            if (GameInfo.Champdata.Hero == null)
            {
                Game.PrintChat(ResourceM.GetString("ChampNotSupported"));
                return;
            }
            Jungle.SetSmiteSlot();
            if (Jungle.SmiteSlot == SpellSlot.Unknown)
            {
                Console.WriteLine(@"Items: ");
                foreach (var i in Player.InventoryItems)
                {
                    Console.WriteLine(@"	 Name: {0}, ID: {1}({2})", i.IData.TranslatedDisplayName, i.Id, (int) i.Id);
                }
                Game.PrintChat(ResourceM.GetString("NoSmite"));
                return;
            }

            ItemHandler = new ItemHandler(GameInfo.Champdata.Type);
            CreateMenu();

            Game.OnUpdate += Game_OnGameUpdate;
            Obj_AI_Base.OnProcessSpellCast += GameProcessSpell;
            Drawing.OnDraw += Drawing_OnDraw;
            Obj_AI_Base.OnNewPath += Obj_AI_Base_OnNewPath;
            Game.OnEnd += Game_OnEnd;
            GameObject.OnDelete += Obj_AI_Base_OnDelete;
        }

        private static void SetCulture()
        {
            try
            {
                Path = string.Format(@"{0}\AutoJ\", Config.AppDataDirectory);
                FileName = "Lang.txt";
                if (!Directory.Exists(Path))
                {
                    Directory.CreateDirectory(Path);
                }
                if (!File.Exists(Path + FileName))
                {
                    File.AppendAllText(Path + FileName, @"en", Encoding.Default);
                    ResourceM = new ResourceManager("AutoJungle.Resource.en", typeof(Program).Assembly);
                    Console.WriteLine(@"First start, lang is English");
                }
                else
                {
                    Culture = File.ReadLines(Path + FileName).First();
                    Console.WriteLine(Culture);
                    ResourceM = new ResourceManager("AutoJungle.Resource." + Culture, typeof(Program).Assembly);
                    Console.WriteLine(@"Lang set to " + Culture);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ResourceM = new ResourceManager("AutoJungle.Resource.en", typeof(Program).Assembly);
            }
        }

        private static void Obj_AI_Base_OnDelete(GameObject sender, EventArgs args)
        {
            if (!(sender.Position.Distance(Player.Position) < 600))
            {
                return;
            }
            var closest = GameInfo.MonsterList.FirstOrDefault(m => m.Position.Distance(sender.Position) < 600);
            if (closest == null || GameInfo.GameState != State.Jungling
                || Helpers.GetMobs(sender.Position, 600).Count(m => !m.IsDead) != 0)
            {
                return;
            }
            if (Environment.TickCount - closest.TimeAtDead > closest.RespawnTime)
            {
                closest.TimeAtDead = Environment.TickCount;
            }
        }

        private static void Game_OnEnd(GameEndEventArgs args)
        {
            if (!Menu.Item("AutoClose").GetValue<Boolean>())
            {
                return;
            }
            Console.WriteLine(@"END");
            Thread.Sleep(Random.Next(10000, 13000));
            Game.Quit();
        }

        private static void CreateMenu()
        {
            Menu = new Menu(ResourceM.GetString("AutoJungle"), "AutoJungle", true);

            var menuD = new Menu(ResourceM.GetString("dsettings"), "dsettings");
            menuD.AddItem(new MenuItem("debug", ResourceM.GetString("debug")))
                .SetValue(new KeyBind("T".ToCharArray()[0], KeyBindType.Press))
                .SetFontStyle(FontStyle.Bold, SharpDX.Color.Orange);
            menuD.AddItem(new MenuItem("State", ResourceM.GetString("State"))).SetValue(false);
            Menu.AddSubMenu(menuD);
            var menuJ = new Menu(ResourceM.GetString("jsettings"), "jsettings");
            menuJ.AddItem(
                new MenuItem("HealtToBack", ResourceM.GetString("HealtToBack")).SetValue(new Slider(35)));
            menuJ.AddItem(
                new MenuItem("HealthToPotion", ResourceM.GetString("HealthToPotion")).SetValue(new Slider(40)));
            menuJ.AddItem(new MenuItem("UseTrinket", ResourceM.GetString("UseTrinket"))).SetValue(true);
            menuJ.AddItem(new MenuItem("EnemyJungle", ResourceM.GetString("EnemyJungle"))).SetValue(true);
            Menu.AddSubMenu(menuJ);
            var menuG = new Menu(ResourceM.GetString("dsettings"), "gsettings");
            menuG.AddItem(new MenuItem("GankLevel", ResourceM.GetString("GankLevel")).SetValue(new Slider(5, 1, 18)));
            menuG.AddItem(
                new MenuItem("GankFrequency", ResourceM.GetString("GankFrequency")).SetValue(new Slider(100)));
            menuG.AddItem(
                new MenuItem("GankRange", ResourceM.GetString("GankRange")).SetValue(new Slider(7000, 0, 20000)));
            menuG.AddItem(new MenuItem("ComboSmite", ResourceM.GetString("ComboSmite"))).SetValue(true);
            Menu.AddSubMenu(menuG);
            Menu.AddItem(new MenuItem("Enabled", ResourceM.GetString("Enabled"))).SetValue(true);
            Menu.AddItem(new MenuItem("AutoClose", ResourceM.GetString("AutoClose"))).SetValue(true);
            var menuChamps = new Menu(ResourceM.GetString("supported"), "supported");
            menuChamps.AddItem(new MenuItem("supportedYi", ResourceM.GetString("supportedYi")));
            menuChamps.AddItem(new MenuItem("supportedWarwick", ResourceM.GetString("supportedWarwick")));
            menuChamps.AddItem(new MenuItem("supportedShyvana", ResourceM.GetString("supportedShyvana")));
            menuChamps.AddItem(new MenuItem("supportedJax", ResourceM.GetString("supportedJax")));
            menuChamps.AddItem(new MenuItem("supportedXinZhao", ResourceM.GetString("supportedXinZhao")));
            menuChamps.AddItem(new MenuItem("supportedNocturne", ResourceM.GetString("supportedNocturne")));
            menuChamps.AddItem(new MenuItem("supportedEvelyn", ResourceM.GetString("supportedEvelyn")));
            menuChamps.AddItem(new MenuItem("supportedVolibear", ResourceM.GetString("supportedVolibear")));
            menuChamps.AddItem(new MenuItem("supportedTryndamere", ResourceM.GetString("supportedTryndamere")));

            //menuChamps.AddItem(new MenuItem("supportedSkarner", "Skarner"));
            Menu.AddSubMenu(menuChamps);

            var menuLang = new Menu(ResourceM.GetString("lsetting"), "lsetting");
            menuLang.AddItem(
                new MenuItem("Language", ResourceM.GetString("Language")).SetValue(new StringList(Languages)));
            menuLang.AddItem(
                new MenuItem("AutoJungleInfoReload", ResourceM.GetString("AutoJungleInfoReload")).SetFontStyle(
                    FontStyle.Bold, SharpDX.Color.Red));
            Menu.AddSubMenu(menuLang);
            Menu.AddItem(
                new MenuItem(
                    "AutoJungleInfo2",
                    ResourceM.GetString("AutoJungleInfo2") +
                    Assembly.GetExecutingAssembly().GetName().Version.ToString().Replace(",", ".")).SetFontStyle(
                        FontStyle.Bold, SharpDX.Color.Orange));
            /*menu.AddItem(
                new MenuItem("AutoJungleInfo3", resourceM.GetString("AutoJungleInfo3")).SetFontStyle(
                    FontStyle.Bold, SharpDX.Color.Purple));*/

            Menu.AddToMainMenu();
            Menu.Item("Language").ValueChanged += OnValueChanged;
        }

        #endregion
    }
}