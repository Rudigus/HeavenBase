using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace HeavenBase
{
    public class Familiar
    {
        public int FamiliarID { get; set; } // CharacterWz - GetFamiliarID()
        public string MobName { get; set; } // StringWz - GetMobName()
        public string CardName { get; set; } // StringWz - GetCardName()
        public string Rarity { get; set; } // CharacterWz - GetRarity()
        public string SkillName { get; set; } // StringWz - GetSkillName()
        public string SkillDescription { get; set; } // StringWz - GetSkillDesc()
        public int Range { get; set; } // CharacterWz - GetRange()
        public string PassiveEffect { get; set; } // StringWz - GetPassiveEffect()
        public int MobID { get; set; } // EtcWz - GetMobID()
        public int CardID { get; set; } // EtcWz - GetCardID()
        public int SkillID { get; set; } // CharacterWz - GetSkillID()
        public int PassiveEffectID { get; set; } // EtcWz - GetPassiveEffectID()

        public Bitmap CardImage { get; set; } // UIWz - GetCardImage() - Can be found at ItemWz Consume too
        public Bitmap MobImage { get; set; }
        public int Level { get; set; }
        public int Att { get; set; }
        public int PassiveEffectBonus { get; set; }
        public string SkillCategory { get; set; } // Check for CompressedIntProperty to get the category
    }
}
