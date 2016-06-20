using LeagueSharp.Common;

namespace AutoJungle.Data
{
    using System.Collections.Generic;

    internal class AutoLeveler
    {
        public static AutoLevel AutoLevel;
        public int[] LevelingOrder = { 0, 1, 2, 0, 0, 3, 0, 1, 0, 1, 3, 1, 1, 2, 2, 3, 2, 2 };

        public AutoLeveler(IEnumerable<int> tree)
        {
            AutoLevel = new AutoLevel(tree);
            AutoLevel.Enable();
        }
    }
}