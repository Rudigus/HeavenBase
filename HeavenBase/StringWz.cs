using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapleLib.WzLib;

namespace HeavenBase
{
    class StringWz
    {
        private readonly WzImage FamiliarImage;
        private readonly WzImage MobImage;
        private readonly WzImage ConsumeImage;
        private readonly WzImage EqpImage;

        public StringWz(WzFile StringWZ)
        {
            StringWZ.ParseWzFile();
            FamiliarImage = StringWZ.WzDirectory.GetImageByName("Familiar.img");
            MobImage = StringWZ.WzDirectory.GetImageByName("Mob.img");
            ConsumeImage = StringWZ.WzDirectory.GetImageByName("Consume.img");
            EqpImage = StringWZ.WzDirectory.GetImageByName("Eqp.img");
        }

        // Familiar.img/skill/{SkillID}/name
        public string GetSkillName(int skillID)
        {
            string skillName = FamiliarImage?.GetFromPath($@"skill/{skillID}/name").GetString();
            return skillName;
        }

        // Familiar.img/skill/{SkillID}/desc
        public string GetSkillDesc(int skillID)
        {
            string skillDesc = FamiliarImage?.GetFromPath($@"skill/{skillID}/desc").GetString();
            return skillDesc;
        }

        // Mob.img/{MobID}/name
        public string GetMobName(int mobID)
        {
            string mobName = MobImage?.GetFromPath($@"{mobID.ToString()}/name").GetString();
            return mobName;
        }

        // Consume.img/{CardID}/name
        public string GetCardName(int cardID)
        {
            string cardName = ConsumeImage?.GetFromPath($@"{cardID}/name").GetString();
            return cardName;
        }

        // Eqp.img/Eqp/Weapon/{WeaponID}/name
        public string GetWeaponName(int weaponID, string category)
        {
            if (EqpImage?.GetFromPath($@"Eqp/{category}/{weaponID}/name") == null)
                return "";
            string weaponName = EqpImage?.GetFromPath($@"Eqp/{category}/{weaponID}/name").GetString();
            return weaponName;
        }

        // Consume.img/{PassiveEffectID}/desc
        public string GetPassiveEffect(int passiveEffectID)
        {
            string passiveEffect = ConsumeImage?.GetFromPath($@"{passiveEffectID}/desc").GetString();
            return passiveEffect;
        }
    }
}
