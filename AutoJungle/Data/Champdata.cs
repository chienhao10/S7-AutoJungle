using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;

namespace AutoJungle.Data
{
    internal class Champdata
    {
        public Obj_AI_Hero Hero;
        public BuildType Type;

        public Func<bool> JungleClear;
        public Func<bool> Combo;
        public Spell R;
        public static Spell Q;
        public Spell W;
        public static Spell E;
        public AutoLeveler Autolvl;

        public Champdata()
        {
            switch (ObjectManager.Player.ChampionName)
            {
                case "MasterYi":
                    this.Hero = ObjectManager.Player;
                    this.Type = BuildType.Yi;

                    Q = new Spell(SpellSlot.Q, 600);
                    Q.SetTargetted(0.5f, float.MaxValue);
                    this.W = new Spell(SpellSlot.W);
                    E = new Spell(SpellSlot.E);
                    this.R = new Spell(SpellSlot.R);

                    this.Autolvl = new AutoLeveler(new [] { 0, 2, 1, 0, 0, 3, 0, 2, 0, 2, 3, 2, 2, 1, 1, 3, 1, 1 });

                    this.JungleClear = this.MasteryiJungleClear;
                    this.Combo = this.MasteryiCombo;
                    Console.WriteLine(@"Masteryi loaded");
                    break;

                case "Warwick":
                    this.Hero = ObjectManager.Player;
                    this.Type = BuildType.As;

                    Q = new Spell(SpellSlot.Q, 400, TargetSelector.DamageType.Magical);
                    Q.SetTargetted(0.5f, float.MaxValue);
                    this.W = new Spell(SpellSlot.W, 1250);
                    E = new Spell(SpellSlot.E);
                    this.R = new Spell(SpellSlot.R, 700, TargetSelector.DamageType.Magical);
                    this.R.SetTargetted(0.5f, float.MaxValue);

                    this.Autolvl = new AutoLeveler(new [] { 0, 1, 2, 0, 0, 3, 0, 1, 0, 1, 3, 1, 1, 2, 2, 3, 2, 2 });

                    this.JungleClear = this.WarwickJungleClear;
                    this.Combo = this.WarwickCombo;

                    Console.WriteLine(@"Warwick loaded");
                    break;

                case "Shyvana":
                    this.Hero = ObjectManager.Player;
                    this.Type = BuildType.As;

                    Q = new Spell(SpellSlot.Q);
                    this.W = new Spell(SpellSlot.W, 350f);
                    E = new Spell(SpellSlot.E, 925f);
                    E.SetSkillshot(0.25f, 60f, 1500, false, SkillshotType.SkillshotLine);
                    this.R = new Spell(SpellSlot.R, 1000f);
                    this.R.SetSkillshot(0.25f, 150f, 1500, false, SkillshotType.SkillshotLine);

                    this.Autolvl = new AutoLeveler(new [] { 1, 2, 0, 1, 1, 3, 1, 0, 1, 0, 3, 0, 0, 2, 2, 3, 2, 2 });

                    this.JungleClear = this.ShyvanaJungleClear;
                    this.Combo = this.ShyvanaCombo;

                    Console.WriteLine(@"Shyvana loaded");
                    break;

                case "SkarnerNOTWORKINGYET":
                    this.Hero = ObjectManager.Player;
                    this.Type = BuildType.As;

                    Q = new Spell(SpellSlot.Q, 325);
                    this.W = new Spell(SpellSlot.W);
                    E = new Spell(SpellSlot.E, 985);
                    E.SetSkillshot(0.5f, 60, 1200, false, SkillshotType.SkillshotLine);
                    this.R = new Spell(SpellSlot.R, 325);

                    this.Autolvl = new AutoLeveler(new [] { 0, 1, 2, 0, 0, 3, 0, 2, 0, 2, 3, 2, 2, 1, 1, 3, 1, 1 });

                    this.JungleClear = this.SkarnerJungleClear;
                    this.Combo = this.SkarnerCombo;

                    Console.WriteLine(@"Skarner loaded");
                    break;
                case "Jax":
                    this.Hero = ObjectManager.Player;
                    this.Type = BuildType.Asmana;

                    Q = new Spell(SpellSlot.Q, 680f);
                    Q.SetTargetted(0.50f, 75f);
                    this.W = new Spell(SpellSlot.W);
                    E = new Spell(SpellSlot.E);
                    this.R = new Spell(SpellSlot.R);

                    this.Autolvl = new AutoLeveler(new [] { 2, 1, 0, 0, 0, 3, 0, 1, 0, 1, 3, 1, 1, 2, 2, 3, 2, 2 });
                    this.JungleClear = this.JaxJungleClear;
                    this.Combo = this.JaxCombo;

                    Console.WriteLine(@"Jax loaded");
                    break;
                case "XinZhao":
                    this.Hero = ObjectManager.Player;
                    this.Type = BuildType.As;

                    Q = new Spell(SpellSlot.Q);
                    this.W = new Spell(SpellSlot.W);
                    E = new Spell(SpellSlot.E, 600);
                    this.R = new Spell(SpellSlot.R, 450f);

                    this.Autolvl = new AutoLeveler(new [] { 0, 1, 2, 0, 0, 3, 0, 2, 0, 2, 3, 2, 2, 1, 1, 3, 1, 1 });

                    this.JungleClear = this.XinJungleClear;
                    this.Combo = this.XinCombo;
                    Console.WriteLine(@"Xin Zhao loaded");
                    break;

                case "Nocturne":
                    this.Hero = ObjectManager.Player;
                    this.Type = BuildType.Noc;

                    Q = new Spell(SpellSlot.Q, 1150);
                    Q.SetSkillshot(0.25f, 60f, 1350, false, SkillshotType.SkillshotLine);
                    this.W = new Spell(SpellSlot.W);
                    E = new Spell(SpellSlot.E, 400, TargetSelector.DamageType.Magical);
                    E.SetTargetted(0.50f, 75f);
                    this.R = new Spell(SpellSlot.R, 4000);
                    this.R.SetTargetted(0.75f, 4000f);

                    this.Autolvl = new AutoLeveler(new [] { 0, 2, 1, 0, 0, 3, 0, 2, 0, 2, 3, 2, 2, 1, 1, 3, 1, 1 });

                    this.JungleClear = this.NocturneJungleClear;
                    this.Combo = this.NocturneCombo;
                    Console.WriteLine(@"Nocturne loaded");
                    break;

                case "Evelynn":
                    this.Hero = ObjectManager.Player;
                    this.Type = BuildType.Eve;

                    Q = new Spell(SpellSlot.Q, 500f);
                    this.W = new Spell(SpellSlot.W);
                    E = new Spell(SpellSlot.E, 225);
                    this.R = new Spell(SpellSlot.R, 650);
                    this.R.SetSkillshot(
                        this.R.Instance.SData.SpellCastTime, this.R.Instance.SData.LineWidth, this.R.Speed, false,
                        SkillshotType.SkillshotCone);

                    this.Autolvl = new AutoLeveler(new [] { 0, 2, 1, 0, 0, 3, 0, 2, 0, 2, 3, 2, 2, 1, 1, 3, 1, 1 });

                    this.JungleClear = this.EveJungleClear;
                    this.Combo = this.EveCombo;
                    Console.WriteLine(@"Evelynn loaded");
                    break;

                case "Volibear":
                    this.Hero = ObjectManager.Player;
                    this.Type = BuildType.Vb;

                    Q = new Spell(SpellSlot.Q);
                    this.W = new Spell(SpellSlot.W, 400);
                    E = new Spell(SpellSlot.E, 425);
                    this.R = new Spell(SpellSlot.R);

                    this.Autolvl = new AutoLeveler(new [] { 2, 1, 0, 1, 1, 3, 1, 0, 1, 0, 3, 0, 0, 2, 2, 3, 2, 2 });

                    this.JungleClear = this.VbJungleClear;
                    this.Combo = this.VoliCombo;
                    Console.WriteLine(@"Volibear loaded");
                    break;

                case "Tryndamere":
                    this.Hero = ObjectManager.Player;
                    this.Type = BuildType.Manwang;

                    Q = new Spell(SpellSlot.Q);
                    this.W = new Spell(SpellSlot.W, 850);
                    E = new Spell(SpellSlot.E, 600);
                    this.R = new Spell(SpellSlot.R);

                    this.Autolvl = new AutoLeveler(new [] { 0, 2, 0, 1, 0, 3, 0, 0, 2, 2, 3, 2, 2, 1, 1, 3, 1, 1 });

                    this.JungleClear = this.MwJungleClear;
                    this.Combo = this.MwCombo;
                    Console.WriteLine(@"Tryndamere loaded");
                    break;
                default:
                    Console.WriteLine(ObjectManager.Player.ChampionName + @" not supported");
                    break;
                //nidale w buff?(优先）)nunu R check | sej，结束skr，amumu？ graves！
            }
        }

        private bool MwCombo()
        {
            var targetHero = Program.GameInfo.Target;
            if (this.Hero.Spellbook.IsChanneling)
            {
                return false;
            }
            if (Program.Menu.Item("ComboSmite").GetValue<Boolean>())
            {
                Jungle.CastSmiteHero((Obj_AI_Hero) targetHero);
            }
            if (this.Hero.IsWindingUp)
            {
                return false;
            }
            if (E.IsReady() && targetHero.IsValidTarget(600))
            {
                E.Cast(targetHero);
            }
            ItemHandler.UseItemsCombo(targetHero, true);
            if (this.W.IsReady() && targetHero.IsValidTarget(850))
            {
                this.W.Cast();
            }
            if (Q.IsReady() && !this.Hero.HasBuff("UndyingRage") && this.Hero.HealthPercent < 20)
            {
                Q.Cast();
            }
            if (this.R.IsReady() && this.Hero.HealthPercent < 15 && targetHero.CountEnemiesInRange(700) >= 1)
            {
                this.R.Cast();
            }
            this.Hero.IssueOrder(GameObjectOrder.AttackUnit, targetHero);
            return false;
        }

        private bool MwJungleClear()
        {
            var targetMob = Program.GameInfo.Target;
            var structure = Helpers.CheckStructure();
            if (structure != null)
            {
                this.Hero.IssueOrder(GameObjectOrder.AttackUnit, structure);
                return false;
            }
            if (targetMob == null)
            {
                return false;
            }
            ItemHandler.UseItemsJungle();
            if (E.IsReady() && targetMob.IsValidTarget(600))
            {
                E.Cast(targetMob);
            }
            if (Q.IsReady() && this.Hero.HealthPercent < 30)
            {
                Q.Cast();
            }
            if (this.Hero.IsWindingUp)
            {
                return false;
            }
            this.Hero.IssueOrder(GameObjectOrder.AttackUnit, targetMob);
            return false;
        }

        private bool VoliCombo()
        {
            var targetHero = Program.GameInfo.Target;
            if (this.Hero.Spellbook.IsChanneling)
            {
                return false;
            }
            if (Program.Menu.Item("ComboSmite").GetValue<Boolean>())
            {
                Jungle.CastSmiteHero((Obj_AI_Hero) targetHero);
            }
            ItemHandler.UseItemsCombo(targetHero, true);
            if (this.Hero.IsWindingUp)
            {
                return false;
            }
            if (Q.IsReady() && targetHero.IsValidTarget(550))
            {
                Q.Cast();
            }
            if (E.IsReady() && targetHero.IsValidTarget(425))
            {
                E.Cast();
            }
            if (this.R.IsReady() && this.Hero.Distance(targetHero) < 400 && this.Hero.Mana > 100)
            {
                this.R.Cast();
            }
            if (this.W.IsReady() && targetHero.IsValidTarget(400))
            {
                this.W.CastOnUnit(targetHero);
            }
            this.Hero.IssueOrder(GameObjectOrder.AttackUnit, targetHero);
            return false;
        }

        private bool VbJungleClear()
        {
            var targetMob = Program.GameInfo.Target;
            var structure = Helpers.CheckStructure();
            if (structure != null)
            {
                this.Hero.IssueOrder(GameObjectOrder.AttackUnit, structure);
                return false;
            }
            if (targetMob == null)
            {
                return false;
            }
            ItemHandler.UseItemsJungle();
            if (E.IsReady() && targetMob.IsValidTarget(425) && (this.Hero.ManaPercent > 60 || this.Hero.HealthPercent < 50))
            {
                E.Cast();
            }
            if (Q.IsReady() && targetMob.IsValidTarget(550))
            {
                Q.Cast();
            }
            if (this.W.IsReady() && targetMob.IsValidTarget(400))
            {
                this.W.CastOnUnit(targetMob);
            }
            if (this.Hero.IsWindingUp)
            {
                return false;
            }
            this.Hero.IssueOrder(GameObjectOrder.AttackUnit, targetMob);
            return false;
        }

        private bool EveCombo()
        {
            var targetHero = Program.GameInfo.Target;
            if (this.Hero.Spellbook.IsChanneling)
            {
                return false;
            }
            if (Program.Menu.Item("ComboSmite").GetValue<Boolean>())
            {
                Jungle.CastSmiteHero((Obj_AI_Hero) targetHero);
            }
            if (this.Hero.IsWindingUp)
            {
                return false;
            }
            if (Q.IsReady() && Q.CanCast(targetHero))
            {
                Q.CastOnUnit(targetHero);
            }
            ItemHandler.UseItemsCombo(targetHero, true);
            if (this.W.IsReady() && targetHero.IsValidTarget(750))
            {
                this.W.Cast();
            }
            if (this.R.IsReady() && this.Hero.Distance(targetHero) < 650 && this.Hero.Mana > 100)
            {
                this.R.Cast(targetHero);
            }
            if (E.IsReady() && E.CanCast(targetHero))
            {
                E.CastOnUnit(targetHero);
            }
            this.Hero.IssueOrder(GameObjectOrder.AttackUnit, targetHero);
            return false;
        }

        private bool EveJungleClear()
        {
            var targetMob = Program.GameInfo.Target;
            var structure = Helpers.CheckStructure();
            if (structure != null)
            {
                this.Hero.IssueOrder(GameObjectOrder.AttackUnit, structure);
                return false;
            }
            if (targetMob == null)
            {
                return false;
            }
            ItemHandler.UseItemsJungle();
            if (Q.IsReady() && this.Hero.Distance(targetMob) < Q.Range &&
                (Helpers.GetMobs(this.Hero.Position, Q.Range).Count >= 2 || targetMob.MaxHealth > 700))
            {
                Q.Cast(targetMob);
            }
            if (E.IsReady() && E.CanCast(targetMob) && (this.Hero.ManaPercent > 60 || targetMob.MaxHealth > 700))
            {
                E.CastOnUnit(targetMob);
            }
            if (this.Hero.IsWindingUp)
            {
                return false;
            }
            this.Hero.IssueOrder(GameObjectOrder.AttackUnit, targetMob);
            return false;
        }

        private bool JaxCombo()
        {
            var targetHero = Program.GameInfo.Target;
            if (targetHero == null)
            {
                return false;
            }
            if (this.R.IsReady() && this.Hero.Distance(targetHero) < 300 && this.Hero.Mana > 250)
            {
                this.R.Cast();
            }
            if (this.W.IsReady() && targetHero.IsValidTarget(300))
            {
                this.W.Cast();
            }
            ItemHandler.UseItemsCombo(targetHero, !Q.IsReady());
            if (this.Hero.IsWindingUp)
            {
                return false;
            }
            if (Q.IsReady() && Q.CanCast(targetHero) &&
                (targetHero.Distance(this.Hero) > Orbwalking.GetRealAutoAttackRange(targetHero) || this.Hero.HealthPercent < 40))
            {
                Q.CastOnUnit(targetHero);
            }
            this.Hero.IssueOrder(GameObjectOrder.AttackUnit, targetHero);
            return false;
        }

        private bool XinJungleClear()
        {
            var targetMob = Program.GameInfo.Target;
            var structure = Helpers.CheckStructure();
            if (structure != null)
            {
                this.Hero.IssueOrder(GameObjectOrder.AttackUnit, structure);
                return false;
            }
            if (targetMob == null)
            {
                return false;
            }
            if (this.W.IsReady() && targetMob.IsValidTarget(300) && (this.Hero.ManaPercent > 60 || this.Hero.HealthPercent < 50))
            {
                this.W.Cast();
            }
            ItemHandler.UseItemsJungle();
            if (Q.IsReady() && targetMob.IsValidTarget(300))
            {
                Q.Cast();
            }
            if (E.IsReady() && E.CanCast(targetMob) && (this.Hero.ManaPercent > 60 || this.Hero.HealthPercent < 50))
            {
                E.CastOnUnit(targetMob);
            }
            if (this.Hero.IsWindingUp)
            {
                return false;
            }
            this.Hero.IssueOrder(GameObjectOrder.AttackUnit, targetMob);
            return false;
        }

        private bool XinCombo()
        {
            var targetHero = Program.GameInfo.Target;
            if (targetHero == null)
            {
                return false;
            }
            if (this.R.IsReady() && this.Hero.Distance(targetHero) < this.R.Range && targetHero.HasBuff("xenzhaointimidate") &&
                targetHero.Health > this.R.GetDamage(targetHero) + this.Hero.GetAutoAttackDamage(targetHero, true) * 4)
            {
                this.R.Cast();
            }
            if (this.W.IsReady() && targetHero.IsValidTarget(300))
            {
                this.W.Cast();
            }
            ItemHandler.UseItemsCombo(targetHero, !E.IsReady());
            if (this.Hero.IsWindingUp)
            {
                return false;
            }
            if (Q.IsReady() && targetHero.Distance(this.Hero) < Orbwalking.GetRealAutoAttackRange(targetHero) + 50)
            {
                Q.Cast();
            }
            if (E.IsReady() && E.CanCast(targetHero) &&
                (this.Hero.HealthPercent < 40 || targetHero.Distance(this.Hero) > Orbwalking.GetRealAutoAttackRange(targetHero) ||
                 Prediction.GetPrediction(targetHero, 1f).UnitPosition.UnderTurret(true)))
            {
                E.CastOnUnit(targetHero);
            }
            this.Hero.IssueOrder(GameObjectOrder.AttackUnit, targetHero);
            return false;
        }

        private bool JaxJungleClear()
        {
            var targetMob = Program.GameInfo.Target;
            var structure = Helpers.CheckStructure();
            if (structure != null)
            {
                this.Hero.IssueOrder(GameObjectOrder.AttackUnit, structure);
                return false;
            }
            if (targetMob == null)
            {
                return false;
            }
            if (this.W.IsReady() && targetMob.IsValidTarget(300))
            {
                this.W.Cast();
            }
            ItemHandler.UseItemsJungle();
            if (Q.IsReady() && Q.CanCast(targetMob) && (this.Hero.ManaPercent > 60 || this.Hero.HealthPercent < 50))
            {
                Q.CastOnUnit(targetMob);
            }
            if (this.Hero.IsWindingUp)
            {
                return false;
            }
            this.Hero.IssueOrder(GameObjectOrder.AttackUnit, targetMob);
            return false;
        }

        private bool SkarnerCombo()
        {
            var targetHero = Program.GameInfo.Target;
            var rActive = this.Hero.HasBuff("skarnerimpalevo");
            if (this.W.IsReady() && targetHero != null && this.Hero.Distance(targetHero) < 700)
            {
                this.W.Cast();
            }
            ItemHandler.UseItemsCombo(targetHero, !E.IsReady());
            if (Q.IsReady() && ((targetHero != null && Q.CanCast(targetHero)) || rActive))
            {
                Q.Cast();
            }
            if (this.Hero.IsWindingUp)
            {
                return false;
            }
            if (E.IsReady() && !rActive && targetHero != null && E.CanCast(targetHero) &&
                this.Hero.Distance(targetHero) < 700)
            {
                E.CastIfHitchanceEquals(targetHero, HitChance.High);
            }
            if (this.R.IsReady() && targetHero != null && this.R.CanCast(targetHero) && !targetHero.HasBuff("SkarnerImpale"))
            {
                this.R.CastOnUnit(targetHero);
            }
            if (rActive)
            {
                var allyTower =
                    Program.GameInfo.AllyStructures.OrderBy(a => a.Distance(this.Hero.Position)).FirstOrDefault();
                if (allyTower.Distance(this.Hero.Position) < 2000 &&
                    allyTower.Distance(this.Hero.Position) > 300)
                {
                    Console.WriteLine(2);
                    Console.WriteLine(allyTower.Distance(this.Hero.Position));
                    this.Hero.IssueOrder(GameObjectOrder.MoveTo, allyTower.Extend(Program.GameInfo.SpawnPoint, 300));
                    Program.Pos = allyTower.Extend(Program.GameInfo.SpawnPoint, 300);
                    return false;
                }
                var ally =
                    HeroManager.Allies.Where(a => a.Distance(this.Hero.Position) < 1500)
                        .OrderBy(a => a.Distance(this.Hero))
                        .FirstOrDefault();
                if (ally != null && ally.Distance(this.Hero) > 300)
                {
                    this.Hero.IssueOrder(GameObjectOrder.MoveTo, ally.Position);
                    Console.WriteLine(1);
                    Program.Pos = ally.Position;
                    return false;
                }
                var enemyTower =
                    Program.GameInfo.EnemyStructures.OrderBy(a => a.Distance(this.Hero.Position)).FirstOrDefault();
                if (enemyTower.Distance(this.Hero.Position) < 2000 &&
                    enemyTower.Distance(this.Hero.Position) > 300)
                {
                    Console.WriteLine(3);
                    if (targetHero != null)
                    {
                        Program.Pos = targetHero.Position.Extend(enemyTower, 2500);
                    }
                    this.Hero.IssueOrder(GameObjectOrder.MoveTo, this.Hero.Position.Extend(enemyTower, 2500));
                    return false;
                }
            }
            else if (targetHero != null)
            {
                this.Hero.IssueOrder(GameObjectOrder.AttackUnit, targetHero);
            }
            return false;
        }

        private bool SkarnerJungleClear()
        {
            var targetMob = Program.GameInfo.Target;
            var structure = Helpers.CheckStructure();
            if (structure != null)
            {
                this.Hero.IssueOrder(GameObjectOrder.AttackUnit, structure);
                return false;
            }
            if (targetMob == null)
            {
                return false;
            }
            if (this.W.IsReady() && this.Hero.Distance(targetMob) < Q.Range &&
                (Helpers.GetMobs(this.Hero.Position, this.W.Range).Count >= 2 ||
                 targetMob.Health > this.Hero.GetAutoAttackDamage(targetMob, true) * 5))
            {
                this.W.Cast();
            }
            ItemHandler.UseItemsJungle();
            if (Q.IsReady() && Q.CanCast(targetMob))
            {
                Q.Cast();
            }
            if (this.Hero.IsWindingUp)
            {
                return false;
            }
            if (E.IsReady() && E.CanCast(targetMob))
            {
                var pred = E.GetLineFarmLocation(Helpers.GetMobs(this.Hero.Position, E.Range));
                if (pred.MinionsHit >= 2 || targetMob.Health > this.Hero.GetAutoAttackDamage(targetMob, true) * 5)
                {
                    E.CastIfHitchanceEquals(targetMob, HitChance.VeryHigh);
                }
            }
            this.Hero.IssueOrder(GameObjectOrder.AttackUnit, targetMob);
            return false;
        }

        private bool ShyvanaCombo()
        {
            var targetHero = Program.GameInfo.Target;
            if (this.Hero.Spellbook.IsChanneling)
            {
                return false;
            }
            if (this.W.IsReady() && this.Hero.Distance(targetHero) < this.W.Range + 100)
            {
                this.W.Cast();
            }
            ItemHandler.UseItemsCombo(targetHero, true);
            if (this.Hero.IsWindingUp)
            {
                return false;
            }
            if (Q.IsReady() && Orbwalking.GetRealAutoAttackRange(targetHero) > this.Hero.Distance(targetHero))
            {
                Q.Cast();
            }
            if (E.IsReady() && E.CanCast(targetHero))
            {
                E.Cast(targetHero);
            }
            if (this.R.IsReady() && this.Hero.Mana.Equals(100) &&
                targetHero.CountEnemiesInRange(GameInfo.ChampionRange) <=
                targetHero.CountAlliesInRange(GameInfo.ChampionRange) &&
                !this.Hero.Position.Extend(targetHero.Position, GameInfo.ChampionRange).UnderTurret(true))
            {
                this.R.CastIfHitchanceEquals(targetHero, HitChance.VeryHigh);
            }

            this.Hero.IssueOrder(GameObjectOrder.AttackUnit, targetHero);
            return false;
        }

        private bool ShyvanaJungleClear()
        {
            var targetMob = Program.GameInfo.Target;
            if (this.Hero.Spellbook.IsChanneling)
            {
                return false;
            }
            var structure = Helpers.CheckStructure();
            if (structure != null)
            {
                this.Hero.IssueOrder(GameObjectOrder.AttackUnit, structure);
                return false;
            }
            if (targetMob == null)
            {
                return false;
            }
            if (this.W.IsReady() && this.Hero.Distance(targetMob) < this.W.Range &&
                (Helpers.GetMobs(this.Hero.Position, this.W.Range).Count >= 2 ||
                 targetMob.Health > this.W.GetDamage(targetMob) * 7 + this.Hero.GetAutoAttackDamage(targetMob, true) * 2))
            {
                this.W.Cast();
            }
            ItemHandler.UseItemsJungle();
            if (this.Hero.IsWindingUp)
            {
                return false;
            }
            if (Q.IsReady())
            {
                Q.Cast();
                this.Hero.IssueOrder(GameObjectOrder.AutoAttack, targetMob);
            }
            if (E.IsReady() && E.CanCast(targetMob))
            {
                E.Cast(targetMob);
            }
            this.Hero.IssueOrder(GameObjectOrder.AttackUnit, targetMob);
            return false;
        }

        private bool WarwickCombo()
        {
            var targetHero = Program.GameInfo.Target;
            if (this.Hero.Spellbook.IsChanneling)
            {
                return false;
            }
            if (Program.Menu.Item("ComboSmite").GetValue<Boolean>())
            {
                Jungle.CastSmiteHero((Obj_AI_Hero) targetHero);
            }
            if (this.Hero.IsWindingUp)
            {
                return false;
            }
            if (Q.IsReady() && Q.CanCast(targetHero))
            {
                Q.CastOnUnit(targetHero);
            }
            if (this.W.IsReady() && this.Hero.Distance(targetHero) < 300)
            {
                if (this.Hero.Mana > Q.ManaCost + this.W.ManaCost || this.Hero.HealthPercent > 70)
                {
                    this.W.Cast();
                }
            }
            if (this.R.IsReady() && this.R.CanCast(targetHero) && !targetHero.MagicImmune)
            {
                this.R.CastOnUnit(targetHero);
            }
            if (E.IsReady() && this.Hero.Spellbook.GetSpell(SpellSlot.E).ToggleState == 1 && this.Hero.Distance(targetHero) < 1000)
            {
                E.Cast();
            }
            ItemHandler.UseItemsCombo(targetHero, !this.R.IsReady());
            this.Hero.IssueOrder(GameObjectOrder.AttackUnit, targetHero);
            return false;
        }

        private bool WarwickJungleClear()
        {
            var targetMob = Program.GameInfo.Target;
            if (this.Hero.Spellbook.IsChanneling)
            {
                return false;
            }
            if (this.Hero.IsWindingUp)
            {
                return false;
            }
            var structure = Helpers.CheckStructure();
            if (structure != null)
            {
                this.Hero.IssueOrder(GameObjectOrder.AttackUnit, structure);
                return false;
            }
            if (targetMob == null)
            {
                return false;
            }
            if (Q.IsReady() && Q.CanCast(targetMob) &&
                (this.Hero.ManaPercent > 50 || this.Hero.MaxHealth - this.Hero.Health > Q.GetDamage(targetMob) * 0.8f))
            {
                Q.CastOnUnit(targetMob);
            }
            if (this.W.IsReady() && this.Hero.Distance(targetMob) < 300 &&
                (Program.GameInfo.SmiteableMob != null || Program.GameInfo.MinionsAround > 3))
            {
                if (this.Hero.Mana > Q.ManaCost + this.W.ManaCost || this.Hero.HealthPercent > 70)
                {
                    this.W.Cast();
                }
            }
            if (E.IsReady() && this.Hero.Spellbook.GetSpell(SpellSlot.E).ToggleState != 1 && this.Hero.Distance(targetMob) < 500)
            {
                E.Cast();
            }
            ItemHandler.UseItemsJungle();
            this.Hero.IssueOrder(GameObjectOrder.AttackUnit, targetMob);
            return false;
        }

        private bool MasteryiJungleClear()
        {
            var targetMob = Program.GameInfo.Target;
            if (this.Hero.Spellbook.IsChanneling)
            {
                return false;
            }
            if (E.IsReady() && this.Hero.IsWindingUp)
            {
                E.Cast();
            }
            var structure = Helpers.CheckStructure();
            if (structure != null)
            {
                this.Hero.IssueOrder(GameObjectOrder.AttackUnit, structure);
                return false;
            }
            if (targetMob == null)
            {
                return false;
            }
            if (this.R.IsReady() && this.Hero.Position.Distance(this.Hero.Position) < 300 &&
                Jungle.Bosses.Any(n => targetMob.Name.Contains(n)))
            {
                this.R.Cast();
            }
            if (this.Hero.IsWindingUp)
            {
                return false;
            }
            if (Q.IsReady() && Q.CanCast(targetMob) && targetMob.Health < targetMob.MaxHealth)
            {
                Q.CastOnUnit(targetMob);
            }
            if (this.W.IsReady() && this.Hero.HealthPercent < 50)
            {
                this.W.Cast();
            }
            ItemHandler.UseItemsJungle();
            this.Hero.IssueOrder(GameObjectOrder.AttackUnit, targetMob);
            return false;
        }

        private bool MasteryiCombo()
        {
            var targetHero = Program.GameInfo.Target;
            if (this.Hero.Spellbook.IsChanneling &&
                targetHero.Health > Program.Player.GetAutoAttackDamage(targetHero, true) * 2)
            {
                return false;
            }
            if (Program.Menu.Item("ComboSmite").GetValue<Boolean>())
            {
                Jungle.CastSmiteHero((Obj_AI_Hero) targetHero);
            }
            if (E.IsReady() && this.Hero.IsWindingUp)
            {
                E.Cast();
            }
            if (this.R.IsReady() && this.Hero.Distance(targetHero) < 600)
            {
                this.R.Cast();
            }
            if (this.Hero.IsWindingUp)
            {
                return false;
            }
            if (Q.IsReady())
            {
                Q.CastOnUnit(targetHero);
            }
            if (this.W.IsReady() && this.Hero.HealthPercent < 25 || Program.GameInfo.DamageTaken >= this.Hero.Health / 3)
            {
                this.W.Cast();
            }
            ItemHandler.UseItemsCombo(targetHero, !Q.IsReady());
            this.Hero.IssueOrder(GameObjectOrder.AttackUnit, targetHero);
            return false;
        }

        private bool NocturneCombo()
        {
            var targetHero = Program.GameInfo.Target;
            if (this.Hero.Spellbook.IsChanneling)
            {
                return false;
            }
            if (Program.Menu.Item("ComboSmite").GetValue<Boolean>())
            {
                Jungle.CastSmiteHero((Obj_AI_Hero) targetHero);
            }
            if (this.Hero.IsWindingUp)
            {
                return false;
            }
            /* check under tower? r active 1sec delay move to target
                        if (R.IsReady() && Hero.Distance(targetHero) < 1300 &&
                            (targetHero.Distance(Hero) > Orbwalking.GetRealAutoAttackRange(targetHero) &&
                            targetHero.UnderTurret(true))
            {
                R.CastOnUnit(targetHero);
            }
            */
            if (this.R.IsReady() && this.Hero.Distance(targetHero) < 900)
            {
                this.R.CastOnUnit(targetHero);
            }
            ItemHandler.UseItemsCombo(targetHero, true);
            if (Q.IsReady() && Q.CanCast(targetHero))
            {
                Q.Cast(targetHero);
            }
            if (this.W.IsReady() && targetHero.IsValidTarget(300))
            {
                this.W.Cast();
            }
            if (E.IsReady() && E.CanCast(targetHero))
            {
                E.CastOnUnit(targetHero);
            }
            this.Hero.IssueOrder(GameObjectOrder.AttackUnit, targetHero);
            return false;
        }

        private bool NocturneJungleClear()
        {
            var targetMob = Program.GameInfo.Target;
            var structure = Helpers.CheckStructure();
            if (structure != null)
            {
                this.Hero.IssueOrder(GameObjectOrder.AttackUnit, structure);
                return false;
            }
            if (targetMob == null)
            {
                return false;
            }
            ItemHandler.UseItemsJungle();
            if (Q.IsReady() && targetMob.IsValidTarget(400))
            {
                Q.Cast(targetMob);
            }
            if (E.IsReady() && E.CanCast(targetMob) && (this.Hero.ManaPercent > 60 || this.Hero.HealthPercent < 50))
            {
                E.CastOnUnit(targetMob);
            }
            if (this.Hero.IsWindingUp)
            {
                return false;
            }
            this.Hero.IssueOrder(GameObjectOrder.AttackUnit, targetMob);
            return false;
        }
    }
}