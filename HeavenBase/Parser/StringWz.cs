using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapleLib.WzLib;

namespace HeavenBase
{
    class StringWz
    {
        #region Familiar
        private readonly WzImage FamiliarImage;
        private readonly WzImage MobImage;
        private readonly WzImage ConsumeImage;

        // Familiar.img/skill/{SkillID}/name
        public string GetSkillName(int skillID)
        {
            WzImageProperty property = FamiliarImage?.GetFromPath($@"skill/{skillID}/name");
            if (property != null)
            {
                string skillName = property.GetString();
                return skillName;
            }
            else
            {
                return "";
            }
        }

        // Familiar.img/skill/{SkillID}/desc
        public string GetSkillDesc(int skillID)
        {
            WzImageProperty property = FamiliarImage?.GetFromPath($@"skill/{skillID}/desc");
            if (property != null)
            {
                string skillDesc = property.GetString();
                return skillDesc;
            }
            else
            {
                return "";
            }
        }

        // Mob.img/{MobID}/name
        public string GetMobName(int mobID)
        {
            WzImageProperty property = MobImage?.GetFromPath($@"{mobID.ToString()}/name");
            if (property != null)
            {
                string mobName = property.GetString();
                return mobName;
            }
            else
            {
                return "";
            }
        }

        // Consume.img/{CardID}/name
        public string GetCardName(int cardID)
        {
            WzImageProperty property = ConsumeImage?.GetFromPath($@"{cardID}/name");
            if (property != null)
            {
                string cardName = property.GetString();
                return cardName;
            }
            else
            {
                return "";
            }
        }

        // Consume.img/{PassiveEffectID}/desc
        public string GetPassiveEffect(int passiveEffectID)
        {
            WzImageProperty property = ConsumeImage?.GetFromPath($@"{passiveEffectID}/desc");
            if (property != null)
            {
                string passiveEffect = property.GetString();
                return passiveEffect;
            }
            else
            {
                return "";
            }
        }
        #endregion

        #region Equip
        private readonly WzImage EqpImage;

        // Eqp.img/Eqp/Weapon/{WeaponID}/name
        public string GetEquipName(int weaponID, string category)
        {
            if (EqpImage?.GetFromPath($@"Eqp/{category}/{weaponID}/name") == null)
                return "";
            string weaponName = EqpImage?.GetFromPath($@"Eqp/{category}/{weaponID}/name").GetString();
            return weaponName;
        }
        #endregion

        public StringWz(WzFile StringWZ, bool isFamiliar)
        {
            StringWZ.ParseWzFile();
            if (isFamiliar)
            {
                FamiliarImage = StringWZ.WzDirectory.GetImageByName("Familiar.img");
                MobImage = StringWZ.WzDirectory.GetImageByName("Mob.img");
                ConsumeImage = StringWZ.WzDirectory.GetImageByName("Consume.img");
            }
            else
            {
                EqpImage = StringWZ.WzDirectory.GetImageByName("Eqp.img");
            }
        }
    }
}
