using System;
using SharpDX;

namespace AutoJungle.Data
{
    internal class Camps
    {
        public static readonly string[] BigMobs =
        {
            "SRU_Blue", "SRU_Gromp", "SRU_Murkwolf", "SRU_Razorbeak", "SRU_Red",
            "SRU_Krug", "SRU_Dragon", "SRU_BaronSpawn", "Sru_Crab"
        };

        #region Camps

        public static MonsterInfo Baron = new MonsterInfo
        {
            Id = "Baron",
            Position = new Vector3(4910f, 10268f, -71.24f),
            Name = "SRU_BaronSpawn",
            Index = 100,
            RespawnTime = 420
        };

        public static MonsterInfo Dragon = new MonsterInfo
        {
            Id = "Dragon",
            Position = new Vector3(9836f, 4408f, -71.24f),
            Name = "SRU_Dragon",
            RespawnTime = 360
        };

        public static MonsterInfo TopCrab = new MonsterInfo
        {
            Id = "top_crab",
            Position = new Vector3(4266f, 9634f, -67.87f),
            Name = "Sru_Crab",
            RespawnTime = 180
        };

        public static MonsterInfo BlueMid = new MonsterInfo
        {
            Id = "blue_MID",
            Position = new Vector3(5294.531f, 5537.924f, 50.46155f),
            Name = "noneuses",
            RespawnTime = 0
        };

        public static MonsterInfo PurpleMid = new MonsterInfo
        {
            Id = "purple_MID",
            Position = new Vector3(9443.35f, 9339.06f, 53.30994f),
            Name = "noneuses",
            RespawnTime = 0
        };

        public static MonsterInfo DownCrab = new MonsterInfo
        {
            Id = "down_crab",
            Position = new Vector3(10524f, 5116f, -62.81f),
            Name = "Sru_Crab",
            RespawnTime = 180
        };

        public static MonsterInfo BteamRazorbeak = new MonsterInfo
        {
            Id = "bteam_Razorbeak",
            Position = new Vector3(6974f, 5460f, 54f),
            Name = "SRU_Razorbeak",
            RespawnTime = 100
        };

        public static MonsterInfo BteamRed = new MonsterInfo
        {
            Id = "bteam_Red",
            Position = new Vector3(7796f, 4028f, 54f),
            Name = "SRU_Red",
            RespawnTime = 300
        };

        public static MonsterInfo BteamKrug = new MonsterInfo
        {
            Id = "bteam_Krug",
            Position = new Vector3(8394f, 2750f, 50f),
            Name = "SRU_Krug",
            RespawnTime = 100
        };

        public static MonsterInfo BteamBlue = new MonsterInfo
        {
            Id = "bteam_Blue",
            Position = new Vector3(3832f, 7996f, 52f),
            Name = "SRU_Blue",
            RespawnTime = 300
        };

        public static MonsterInfo BteamGromp = new MonsterInfo
        {
            Id = "bteam_Gromp",
            Position = new Vector3(2112f, 8372f, 51.7f),
            Name = "SRU_Gromp",
            RespawnTime = 100
        };

        public static MonsterInfo BteamWolf = new MonsterInfo
        {
            Id = "bteam_Wolf",
            Position = new Vector3(3844f, 6474f, 52.46f),
            Name = "SRU_Murkwolf",
            RespawnTime = 100
        };

        public static MonsterInfo PteamRazorbeak = new MonsterInfo
        {
            Id = "pteam_Razorbeak",
            Position = new Vector3(7856f, 9492f, 52.33f),
            Name = "SRU_Razorbeak",
            RespawnTime = 100
        };

        public static MonsterInfo PteamRed = new MonsterInfo
        {
            Id = "pteam_Red",
            Position = new Vector3(7124f, 10856f, 56.34f),
            Name = "SRU_Red",
            RespawnTime = 300
        };

        public static MonsterInfo PteamKrug = new MonsterInfo
        {
            Id = "pteam_Krug",
            Position = new Vector3(6495f, 12227f, 56.47f),
            Name = "SRU_Krug",
            RespawnTime = 100
        };

        public static MonsterInfo PteamBlue = new MonsterInfo
        {
            Id = "pteam_Blue",
            Position = new Vector3(10850f, 6938f, 51.72f),
            Name = "SRU_Blue",
            RespawnTime = 300
        };

        public static MonsterInfo PteamGromp = new MonsterInfo
        {
            Id = "pteam_Gromp",
            Position = new Vector3(12766f, 6464f, 51.66f),
            Name = "SRU_Gromp",
            RespawnTime = 100
        };

        public static MonsterInfo PteamWolf = new MonsterInfo
        {
            Id = "pteam_Wolf",
            Position = new Vector3(10958f, 8286f, 62.46f),
            Name = "SRU_Murkwolf",
            RespawnTime = 100
        };

        #endregion
    }

    public class MonsterInfo
    {
        public Vector3 Position;
        public string Id;
        public string Name;
        public int Team;
        public int Index;
        public int RespawnTime;
        public float TimeAtDead;

        public MonsterInfo(MonsterInfo baseInfo, int index)
        {
            this.Position = baseInfo.Position;
            this.Id = baseInfo.Id;
            this.Name = baseInfo.Name;
            this.Team = baseInfo.Team;
            this.Index = index;
            this.RespawnTime = baseInfo.RespawnTime;
            this.TimeAtDead = 0f;
        }

        public bool IsAlive(int time = 0)
        {
            return (Environment.TickCount - this.TimeAtDead) / 1000 > this.RespawnTime - time;
        }

        public MonsterInfo() {}
    }
}