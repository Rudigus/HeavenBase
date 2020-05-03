using MapleLib.WzLib;
using System.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeavenBase
{
    class ItemWz
    {
        private readonly WzDirectory ConsumeDirectory;
        private WzImage ConsumeImage;

        public ItemWz(WzFile itemWZ)
        {
            itemWZ.ParseWzFile();
            ConsumeDirectory = itemWZ.WzDirectory.GetDirectoryByName("Consume");
            ConsumeImage = ConsumeDirectory.GetImageByName("0286.img");
        }

        // Consume/0287.img/0{CardID}/info/grade
        public string GetRarity(int cardID)
        {
            WzImage image = ConsumeDirectory.GetImageByName("0287.img");
            int familiarRarityID = image.GetFromPath($@"0{cardID}/info/grade")?.GetInt() ?? -1;
            string familiarRarity = "";
            switch (familiarRarityID)
            {
                case 0:
                    familiarRarity = "Common";
                    break;
                case 1:
                    familiarRarity = "Uncommon";
                    break;
                case 2:
                    familiarRarity = "Rare";
                    break;
                case 3:
                    familiarRarity = "Legendary";
                    break;
            }
            return familiarRarity;
        }

        // Consume/0287.img/0{CardID}/info/icon
        public Bitmap GetCardImage(int cardID)
        {
            Bitmap cardImage = null;
            WzImage image = ConsumeDirectory.GetImageByName("0287.img");
            if (image?.GetFromPath($@"0{cardID.ToString()}/info/icon/_inlink") == null)
            {
                WzImageProperty property = image?.GetFromPath($@"0{cardID.ToString()}/info/icon");
                if (property != null)
                {
                    cardImage = property.GetBitmap();
                }
            }
            else
            {
                string inlink = image?.GetFromPath($@"0{cardID.ToString()}/info/icon/_inlink").GetString();
                cardImage = image?.GetFromPath($@"{inlink}").GetBitmap();
            }

            return cardImage;
        }

        public string GetPassiveEffectTarget(int passiveEffectID)
        {
            string passiveEffectTarget = "";
            if (ConsumeImage.GetFromPath($@"0{passiveEffectID}/spec/familiarPassiveSkillTarget") == null)
            {
                passiveEffectTarget = "Individual";
            } else
            {
                int passiveEffectTargetID = ConsumeImage.GetFromPath($@"0{passiveEffectID}/spec/familiarPassiveSkillTarget").GetInt();
                switch(passiveEffectTargetID)
                {
                    case 1:
                        passiveEffectTarget = "Party";
                        break;
                    case 2:
                        passiveEffectTarget = "Nearby";
                        break;
                }
            }
                return passiveEffectTarget;
        }

        // Consume/0286.img/0{PassiveEffectID}/spec/
        public string GetPassiveEffectBonus(int passiveEffectID)
        {
            string passiveEffectBonus = "";
            if (ConsumeImage.GetFromPath($@"0{passiveEffectID}/spec/hpR") != null)
                passiveEffectBonus += $@"HP Recovery: +{ConsumeImage.GetFromPath($@"0{passiveEffectID}/spec/hpR").GetInt()}%, ";
            if (ConsumeImage.GetFromPath($@"0{passiveEffectID}/spec/mpR") != null)
                passiveEffectBonus += $@"MP Recovery: +{ConsumeImage.GetFromPath($@"0{passiveEffectID}/spec/mpR").GetInt()}%, ";
            if (ConsumeImage.GetFromPath($@"0{passiveEffectID}/spec/speed") != null)
                passiveEffectBonus += $@"Speed: +{ConsumeImage.GetFromPath($@"0{passiveEffectID}/spec/speed").GetInt()}, ";
            if (ConsumeImage.GetFromPath($@"0{passiveEffectID}/spec/jump") != null)
                passiveEffectBonus += $@"Jump: +{ConsumeImage.GetFromPath($@"0{passiveEffectID}/spec/jump").GetInt()}, ";
            if (ConsumeImage.GetFromPath($@"0{passiveEffectID}/spec/str") != null)
                passiveEffectBonus += $@"STR: +{ConsumeImage.GetFromPath($@"0{passiveEffectID}/spec/str").GetInt()}, ";
            if (ConsumeImage.GetFromPath($@"0{passiveEffectID}/spec/dex") != null)
                passiveEffectBonus += $@"DEX: +{ConsumeImage.GetFromPath($@"0{passiveEffectID}/spec/dex").GetInt()}, ";
            if (ConsumeImage.GetFromPath($@"0{passiveEffectID}/spec/int") != null)
                passiveEffectBonus += $@"INT: +{ConsumeImage.GetFromPath($@"0{passiveEffectID}/spec/int").GetInt()}, ";
            if (ConsumeImage.GetFromPath($@"0{passiveEffectID}/spec/luk") != null)
                passiveEffectBonus += $@"LUK: +{ConsumeImage.GetFromPath($@"0{passiveEffectID}/spec/luk").GetInt()}, ";
            if (ConsumeImage.GetFromPath($@"0{passiveEffectID}/spec/pdd") != null)
                passiveEffectBonus += $@"Weapon DEF: +{ConsumeImage.GetFromPath($@"0{passiveEffectID}/spec/pdd").GetInt()}, ";
            if (ConsumeImage.GetFromPath($@"0{passiveEffectID}/spec/mdd") != null)
                passiveEffectBonus += $@"Magic DEF: +{ConsumeImage.GetFromPath($@"0{passiveEffectID}/spec/mdd").GetInt()}, ";
            if (ConsumeImage.GetFromPath($@"0{passiveEffectID}/spec/mesoupbyitem") != null)
                passiveEffectBonus += $@"Meso Drop Rate: +{ConsumeImage.GetFromPath($@"0{passiveEffectID}/spec/prob").GetInt()}%, ";
            if (ConsumeImage.GetFromPath($@"0{passiveEffectID}/spec/itemupbyitem") != null)
                passiveEffectBonus += $@"Item Drop Rate: +{ConsumeImage.GetFromPath($@"0{passiveEffectID}/spec/prob").GetInt()}%, ";
            if (ConsumeImage.GetFromPath($@"0{passiveEffectID}/spec/reward/meso") != null)
                passiveEffectBonus += $@"Meso: +{ConsumeImage.GetFromPath($@"0{passiveEffectID}/spec/reward/meso").GetInt()}, ";
            if (ConsumeImage.GetFromPath($@"0{passiveEffectID}/spec/familiar/decFatigue") != null)
                passiveEffectBonus += $@"Familiar Vitality: +{ConsumeImage.GetFromPath($@"0{passiveEffectID}/spec/familiar/decFatigue").GetInt()}, ";
            if (ConsumeImage.GetFromPath($@"0{passiveEffectID}/spec/incFatigue") != null)
                passiveEffectBonus += $@"Familiar Vitality: -{ConsumeImage.GetFromPath($@"0{passiveEffectID}/spec/incFatigue").GetInt()}, ";
            if (ConsumeImage.GetFromPath($@"0{passiveEffectID}/spec/charColor") != null)
                passiveEffectBonus += $@"Character Color: #{ConsumeImage.GetFromPath($@"0{passiveEffectID}/spec/charColor").GetString()}, ";
            if (ConsumeImage.GetFromPath($@"0{passiveEffectID}/spec/thaw") != null)
                passiveEffectBonus += $@"Thaw: {ConsumeImage.GetFromPath($@"0{passiveEffectID}/spec/thaw").GetInt()}, ";
            if (ConsumeImage.GetFromPath($@"0{passiveEffectID}/spec/preventslip") != null)
                passiveEffectBonus += $@"Prevent Slip: True, ";
            if (ConsumeImage.GetFromPath($@"0{passiveEffectID}/spec/interval") != null)
                passiveEffectBonus += $@"Interval: {ConsumeImage.GetFromPath($@"0{passiveEffectID}/spec/interval").GetInt() / 1000}s, ";
            if (passiveEffectBonus.Length >= 2)
            {
                passiveEffectBonus = passiveEffectBonus.Remove(passiveEffectBonus.Length - 2) + ".";
            }
            return passiveEffectBonus;
        }
    }
}
