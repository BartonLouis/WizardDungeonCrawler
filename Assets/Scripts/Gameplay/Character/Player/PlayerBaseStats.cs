using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace Gameplay.Player.Stats {
    [CreateAssetMenu(menuName = "Data/Character/CharacterStats")]
    public class PlayerBaseStats : ScriptableObject {
        [SerializeField] List<PrimaryStatValue> stats;

        public int this[PrimaryStatTag statType] {
            get {
                var stat = stats.FirstOrDefault(s => s.stat == statType);
                return stat != null ? stat.value : 0;
            }
        }

        void OnValidate() {
            foreach (var stat in EnumUtils.GetValues<PrimaryStatTag>()) {
                if (stat == PrimaryStatTag.None) continue;
                if (!stats.Exists(s => s.stat == stat)) {
                    stats.Add(new PrimaryStatValue(stat));
                }
            }
            for (int i = stats.Count() - 1; i >= 0; i--) {
                if (EnumUtils.MoreThanOneFlag(stats[i].stat)) {
                    stats.RemoveAt(i);
                }
            }
            stats = stats.Distinct(new PrimaryStatValue.PrimaryStatValueComparer()).ToList();
            stats = stats.OrderBy(s => (int)s.stat).ToList();
        }
    }
}