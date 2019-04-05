using System;
using System.Collections.Generic;
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

        public CharacterWz(WzFile characterWZ)
        {
            characterWZ.ParseWzFile();
            FamiliarDirectory = characterWZ.WzDirectory.GetDirectoryByName("Familiar");
        }

        public int GetFamiliarQuantity()
        {
            return FamiliarDirectory.GetChildImages().Count;
        }

        public int GetFamiliarID(int familiarNumber)
        {
            List<WzImage> familiarImages = FamiliarDirectory.GetChildImages();
            string familiarImageName = familiarImages[familiarNumber].Name;
            familiarImageName = familiarImageName.Substring(0, familiarImageName.Length - 4);
            int familiarID = Convert.ToInt32(familiarImageName);
            return familiarID;
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
    }
}
