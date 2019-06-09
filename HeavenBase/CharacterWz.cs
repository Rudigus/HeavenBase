using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapleLib.WzLib;
using MapleLib.WzLib.WzProperties;

namespace HeavenBase
{
    class CharacterWz
    {
        private readonly WzDirectory FamiliarDirectory;
        private WzImage FamiliarImage;
        private readonly WzDirectory WeaponDirectory;
        private WzImage WeaponImage;

        public CharacterWz(WzFile characterWZ)
        {
            characterWZ.ParseWzFile();
            FamiliarDirectory = characterWZ.WzDirectory.GetDirectoryByName("Familiar");
        }

        public CharacterWz(WzFile characterWZ, string category)
        {
            characterWZ.ParseWzFile();
            WeaponDirectory = characterWZ.WzDirectory.GetDirectoryByName(category);
        }

        public int GetFamiliarQuantity()
        {
            return FamiliarDirectory.GetChildImages().Count;
        }

        public int GetWeaponQuantity()
        {
            return WeaponDirectory.GetChildImages().Count;
        }

        public int GetFamiliarID(int familiarNumber)
        {
            List<WzImage> familiarImages = FamiliarDirectory.GetChildImages();
            string familiarImageName = familiarImages[familiarNumber].Name;
            familiarImageName = familiarImageName.Substring(0, familiarImageName.Length - 4);
            int familiarID = Convert.ToInt32(familiarImageName);
            return familiarID;
        }

        public int GetWeaponID(int weaponNumber)
        {
            List<WzImage> weaponImages = WeaponDirectory.GetChildImages();
            WeaponImage = weaponImages[weaponNumber];
            string weaponImageName = WeaponImage.Name;
            weaponImageName = weaponImageName.Substring(1, weaponImageName.Length - 5);
            int weaponID = Convert.ToInt32(weaponImageName);
            return weaponID;
        }

        // Familiar/{FamiliarID}.img/info/skill/id
        public int GetSkillID()
        {
            int skillID = FamiliarImage.GetFromPath($@"info/skill/id").GetInt();
            return skillID;
        }

        // Familiar/{FamiliarID}.img/info/grade
        public string GetRarity()
        {
            if (FamiliarImage.GetFromPath($@"info/grade") == null)
                return "";
            int familiarRarityID = FamiliarImage.GetFromPath($@"info/grade").GetInt();
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

        // Familiar/{FamiliarID}.img/info/range
        public int GetRange()
        {
            int range = FamiliarImage.GetFromPath($@"info/range").GetInt();
            return range;
        }

        public void SetFamiliarImage(int familiarID)
        {
            FamiliarImage = FamiliarDirectory.GetImageByName($@"{familiarID}.img");
        }

        // Familiar/{FamiliarID}.img/info/level
        public int GetLevel()
        {
            if (FamiliarImage.GetFromPath($@"info/level") == null)
                return 0;
            int familiarLevel = FamiliarImage.GetFromPath($@"info/level").GetInt();
            return familiarLevel;
        }

        // Familiar/{FamiliarID}.img/info/pad
        public int GetATT()
        {
            if (FamiliarImage.GetFromPath($@"info/pad") == null)
                return 0;
            int att = FamiliarImage.GetFromPath($@"info/pad").GetInt();
            return att;
        }

        public Bitmap GetWeaponImage(WzFile characterWZ)
        {
            if (WeaponImage == null)
                return null;
            Bitmap weaponImage = null;

            if (WeaponImage?.GetFromPath($@"info/icon/_outlink") == null && WeaponImage?.GetFromPath($@"info/icon/_inlink") == null)
            {
                weaponImage = WeaponImage?.GetFromPath($@"info/icon").GetBitmap();
            }
            else if (WeaponImage?.GetFromPath($@"info/icon/_outlink") != null)
            {
                string outlink = WeaponImage?.GetFromPath($@"info/icon/_outlink").GetString();
                string treatedlink = outlink.Substring(outlink.IndexOf("/") + 1);
                string directoryName = treatedlink.Substring(0, treatedlink.IndexOf("/"));
                treatedlink = treatedlink.Substring(treatedlink.IndexOf("/") + 1);
                int newWeaponID = Convert.ToInt32(treatedlink.Substring(0, treatedlink.IndexOf(".")));
                string remainderlink = treatedlink.Substring(treatedlink.IndexOf("/") + 1);
                WzDirectory directory = characterWZ.WzDirectory.GetDirectoryByName(directoryName);
                WzImage newWeaponImage = directory.GetImageByName($@"0{newWeaponID}.img");
                weaponImage = newWeaponImage?.GetFromPath($@"{remainderlink}").GetBitmap();
            }
            else
            {
                string inlink = WeaponImage?.GetFromPath($@"info/icon/_inlink").GetString();
                weaponImage = WeaponImage?.GetFromPath($@"{inlink}").GetBitmap();
            }

            return weaponImage;
        }
    }
}
