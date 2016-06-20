using System;
using System.Collections.Generic;
using System.Linq;

using LeagueSharp;
using LeagueSharp.Common;

using SharpDX;

namespace AutoJungle.Data
{
    internal class GameInfo
    {
        public bool WaitOnFountain { get; set; }
        public Vector3 MoveTo { get; set; }
        public Vector3 LastCheckPoint { get; set; }
        public Obj_AI_Base Target { get; set; }
        public bool ShouldRecall { get; set; }
        public float DamageTaken { get; set; }
        public int DamageCount { get; set; }
        public bool AttackedByTurret { get; set; }
        public Champdata Champdata { get; set; }
        public int NextItemPrice { get; set; }
        public bool Fighting { get; set; }
        public List<MonsterInfo> MonsterList = new List<MonsterInfo>();
        public int CurrentMonster { get; set; }
        public State GameState;
        public Vector3 LastClick;
        public int MinionsAround;
        public Obj_AI_Base SmiteableMob;
        public Vector3 SpawnPoint;
        public Vector3 SpawnPointEnemy;
        public int Afk;
        public IEnumerable<Vector3> AllyStructures = new List<Vector3>();
        public IEnumerable<Vector3> EnemyStructures = new List<Vector3>();
        public Vector3 ClosestWardPos = Vector3.Zero;
        public const int ChampionRange = 1300;
        public bool GroupWithoutTarget;

        public GameInfo()
        {
            this.NextItemPrice = 350;
            if (ObjectManager.Player.Team == GameObjectTeam.Chaos)
            {
                this.SpawnPoint = new Vector3(14232f, 14354, 171.97f);
                this.SpawnPointEnemy = new Vector3(415.33f, 453.38f, 182.66f);
            }
            else
            {
                this.SpawnPoint = new Vector3(415.33f, 453.38f, 182.66f);
                this.SpawnPointEnemy = new Vector3(14232f, 14354, 171.97f);
            }
            this.GameState = State.Positioning;
            this.SetMonsterList();
            this.CurrentMonster = 1;

            var last =
                this.MonsterList.OrderBy(temp => temp.Position.Distance(ObjectManager.Player.Position)).FirstOrDefault();
            if (!ObjectManager.Player.InFountain() && last != null && ObjectManager.Player.Level > 1)
            {
                this.CurrentMonster = last.Index;
            }
            else
            {
                this.CurrentMonster = 1;
            }
        }

        public bool IsUnderAttack()
        {
            return this.DamageTaken > 0f;
        }

        public bool CanBuyItem()
        {
            if (this.GameState != State.Positioning ||
                (ObjectManager.Player.HasBuff("ElixirOfWrath") ||
                    ObjectManager.Player.HasBuff("ElixirOfIron") ||
                     ObjectManager.Player.HasBuff("ElixirOfSorcery")))
            {
                return false;
            }

            var current =
                ItemHandler.ItemList.Where(i => Items.HasItem(i.ItemId))
                    .OrderByDescending(i => i.Index)
                    .FirstOrDefault();
            var orderedList =
                ItemHandler.ItemList.Where(
                    i => current != null && (!Items.HasItem(i.ItemId) && i.Index > current.Index)).OrderBy(i => i.Index);
            var nextItem = orderedList.FirstOrDefault(i => current != null && i.Index == current.Index + 1);
            if (nextItem != null)
            {
                this.NextItemPrice = nextItem.Price;
            }
            if (nextItem == null || !(nextItem.Price < ObjectManager.Player.Gold))
            {
                return false;
            }
            if (Program.Debug)
            {
                Console.WriteLine(@"Can buy: " + nextItem.Price);
            }
            return true;
        }

        public int EnemiesAround => this.Champdata.Hero.CountEnemiesInRange(ChampionRange);
        public int AlliesAround => this.Champdata.Hero.CountAlliesInRange(ChampionRange);
        public bool InDanger => this.EnemiesAround > this.AlliesAround + 1 ||
                       this.Champdata.Hero.HealthPercent < Program.Menu.Item("HealtToBack").GetValue<Slider>().Value;
        private void SetMonsterList()
        {
            if (ObjectManager.Player.Team == GameObjectTeam.Chaos)
            {
                this.MonsterList.Add(new MonsterInfo(Camps.PteamGromp, 1));
                this.MonsterList.Add(new MonsterInfo(Camps.PteamBlue, 2));
                this.MonsterList.Add(new MonsterInfo(Camps.PteamWolf, 3));
                this.MonsterList.Add(new MonsterInfo(Camps.PteamRazorbeak, 4));
                this.MonsterList.Add(new MonsterInfo(Camps.PteamRed, 5));
                this.MonsterList.Add(new MonsterInfo(Camps.PteamKrug, 6));
                this.MonsterList.Add(new MonsterInfo(Camps.BteamGromp, 7));
                this.MonsterList.Add(new MonsterInfo(Camps.BteamBlue, 8));
                this.MonsterList.Add(new MonsterInfo(Camps.BteamWolf, 9));
                this.MonsterList.Add(new MonsterInfo(Camps.TopCrab, 10));
                this.MonsterList.Add(new MonsterInfo(Camps.PurpleMid, 11));
                this.MonsterList.Add(new MonsterInfo(Camps.DownCrab, 12));
                this.MonsterList.Add(new MonsterInfo(Camps.Dragon, 13));
                this.MonsterList.Add(new MonsterInfo(Camps.BteamRazorbeak, 14));
                this.MonsterList.Add(new MonsterInfo(Camps.BteamRed, 15));
                this.MonsterList.Add(new MonsterInfo(Camps.BteamKrug, 16));
            }
            else
            {
                this.MonsterList.Add(new MonsterInfo(Camps.BteamKrug, 1));
                this.MonsterList.Add(new MonsterInfo(Camps.BteamRed, 2));
                this.MonsterList.Add(new MonsterInfo(Camps.BteamRazorbeak, 3));
                this.MonsterList.Add(new MonsterInfo(Camps.BteamWolf, 4));
                this.MonsterList.Add(new MonsterInfo(Camps.BteamBlue, 5));
                this.MonsterList.Add(new MonsterInfo(Camps.BteamGromp, 6));
                this.MonsterList.Add(new MonsterInfo(Camps.PteamRazorbeak, 7));
                this.MonsterList.Add(new MonsterInfo(Camps.PteamRed, 8));
                this.MonsterList.Add(new MonsterInfo(Camps.PteamKrug, 9));
                this.MonsterList.Add(new MonsterInfo(Camps.TopCrab, 10));
                this.MonsterList.Add(new MonsterInfo(Camps.BlueMid, 11));
                this.MonsterList.Add(new MonsterInfo(Camps.DownCrab, 12));
                this.MonsterList.Add(new MonsterInfo(Camps.Dragon, 13));
                this.MonsterList.Add(new MonsterInfo(Camps.PteamGromp, 14));
                this.MonsterList.Add(new MonsterInfo(Camps.PteamBlue, 15));
                this.MonsterList.Add(new MonsterInfo(Camps.PteamWolf, 16));
            }
        }

        public void Show()
        {
            var result =
                string.Format(
                    "WaitOnFountain: {0}\n" + "MoveTo: {1}\n" + "CheckPoint: {2}\n" + "Target: {3}\n" +
                    "ShouldRecall: {4}\n" + "IsUnderAttack: {5}\n" + "DamageTaken: {6}\n" + "AttackedByTurret: {7}\n" +
                    "NextItemPrice: {8}\n" + "CurrentMonster: {9}\n" + "GameState: {10}\n" + "MinionsAround: {11}\n" +
                    "SmiteableMob: {12}\n" + "InDanger: {13}\n" + "Afk: {14}\n" + "DamageCount: {15}\n", this.WaitOnFountain,
                    this.MoveTo.ToString(), this.LastCheckPoint.ToString(), this.Target == null ? "null" : this.Target.Name, this.ShouldRecall,
                    this.IsUnderAttack(), this.DamageTaken, this.AttackedByTurret, this.NextItemPrice, this.CurrentMonster, this.GameState,
                    this.MinionsAround, this.SmiteableMob == null ? "null" : this.SmiteableMob.Name, this.InDanger, this.Afk, this.DamageCount);
            Console.WriteLine(result);
        }
    }

    internal enum State
    {
        Defending,
        Pushing,
        Grouping,
        Warding,
        Jungling,
        Ganking,
        LaneClear,
        Positioning,
        FightIng,
        Objective,
        Retreat,
        Null
    }
}