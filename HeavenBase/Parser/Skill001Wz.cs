using MapleLib.WzLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeavenBase
{
    class Skill001Wz
    {
        private WzImage FamiliarImage;

        public Skill001Wz(WzFile skill001WZ)
        {
            skill001WZ.ParseWzFile();
            FamiliarImage = skill001WZ.WzDirectory.GetImageByName("FamiliarSkill.img");
        }

        // FamiliarSkill.img/{skillID}/
        public string GetSkillCategory(int skillID)
        {
            string skillCategory = "";
            if (FamiliarImage.GetFromPath($@"{skillID}/knockback") != null)
                skillCategory = "Knockback";
            else if (FamiliarImage.GetFromPath($@"{skillID}/stun") != null)
                skillCategory = "Stun";
            else if (FamiliarImage.GetFromPath($@"{skillID}/slow") != null)
                skillCategory = "Slow";
            else if (FamiliarImage.GetFromPath($@"{skillID}/poison") != null)
                skillCategory = "Poison";
            else if (FamiliarImage.GetFromPath($@"{skillID}/attract") != null)
                skillCategory = "Attract";
            else
                skillCategory = "";
            return skillCategory;
        }
    }
}
