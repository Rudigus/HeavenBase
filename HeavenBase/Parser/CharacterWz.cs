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
        #region Familiar
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
            WzImageProperty property = FamiliarImage.GetFromPath($@"info/skill/id");
            if (property != null)
            {
                int skillID = property.GetInt();
                return skillID;
            }
            else
            {
                return -1;
            }
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

        public int GetMobID()
        {
            WzImageProperty property = FamiliarImage.GetFromPath($@"info/MobID");
            if (property != null)
            {
                int mobID = property.GetInt();
                return mobID;
            }
            else
            {
                return -1;
            }
        }

        public int GetCardID()
        {
            WzImageProperty property = FamiliarImage.GetFromPath($@"info/monsterCardID");
            if (property != null)
            {
                int cardID = property.GetInt();
                return cardID;
            }
            else
            {
                return -1;
            }
        }
        #endregion

        #region Equip
        private readonly WzDirectory EquipDirectory;
        private WzImage EquipImage;

        public CharacterWz(WzFile characterWZ, string category)
        {
            characterWZ.ParseWzFile();
            EquipDirectory = characterWZ.WzDirectory.GetDirectoryByName(category);
        }

        public int GetEquipQuantity()
        {
            return EquipDirectory.GetChildImages().Count;
        }

        public int GetEquipID(int weaponNumber)
        {
            List<WzImage> weaponImages = EquipDirectory.GetChildImages();
            EquipImage = weaponImages[weaponNumber];
            string weaponImageName = EquipImage.Name;
            weaponImageName = weaponImageName.Substring(1, weaponImageName.Length - 5);
            int weaponID = Convert.ToInt32(weaponImageName);
            return weaponID;
        }

        // {category}/{EquipImage}/info/icon
        public Bitmap GetEquipImage(WzFile characterWZ)
        {
            if (EquipImage == null)
                return null;
            Bitmap weaponImage = null;

            if (EquipImage?.GetFromPath($@"info/icon/_outlink") == null && EquipImage?.GetFromPath($@"info/icon/_inlink") == null)
            {
                weaponImage = EquipImage?.GetFromPath($@"info/icon").GetBitmap();
            }
            else if (EquipImage?.GetFromPath($@"info/icon/_outlink") != null)
            {
                string outlink = EquipImage?.GetFromPath($@"info/icon/_outlink").GetString();
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
                string inlink = EquipImage?.GetFromPath($@"info/icon/_inlink").GetString();
                weaponImage = EquipImage?.GetFromPath($@"{inlink}").GetBitmap();
            }

            return weaponImage;
        }

        // {category}/{EquipImage}/info/cash
        public string GetEquipClassification()
        {
            string equipClassification = "";
            int equipClassificationID;
            if (EquipImage.GetFromPath($@"info/cash") == null)
            {
                return "Normal";
            }
            if (EquipImage.GetFromPath($@"info/cash").GetType() != typeof(WzIntProperty))
            {
                equipClassificationID = Convert.ToInt32(EquipImage.GetFromPath($@"info/cash").GetString());
            }
            else
            {
                equipClassificationID = EquipImage.GetFromPath($@"info/cash").GetInt();
            }
            switch (equipClassificationID)
            {
                case 0:
                    equipClassification = "Normal";
                    break;
                case 1:
                    equipClassification = "Cash";
                    break;
            }
            return equipClassification;
        }

        // {category}/{EquipImage}/info/reqLevel
        public int GetEquipLevel()
        {
            int level = 0;
            if (EquipImage.GetFromPath($@"info/reqLevel") == null)
            {
                return 0;
            }
            if (EquipImage.GetFromPath($@"info/reqLevel").GetType() == typeof(WzIntProperty))
            {
                level = EquipImage.GetFromPath($@"info/reqLevel").GetInt();
            }
            else
            {
                level = Convert.ToInt32(EquipImage.GetFromPath($@"info/reqLevel").GetString());
            }
            return level;
        }

        // {category}/{EquipImage}/info/req{stat}
        public string GetRequiredStats()
        {
            string reqStats = "";
            if (EquipImage?.GetFromPath($@"info/reqDEX") != null)
                if (EquipImage?.GetFromPath($@"info/reqDEX").GetType() == typeof(WzIntProperty))
                {
                    reqStats += $"DEX: {EquipImage?.GetFromPath($@"info/reqDEX").GetInt()}, ";
                }
                else
                {
                    reqStats += $"DEX: {Convert.ToInt32(EquipImage?.GetFromPath($@"info/reqDEX").GetString())}, ";
                }
            else
                reqStats += $"DEX: 0, ";
            if (EquipImage?.GetFromPath($@"info/reqINT") != null)
                if (EquipImage?.GetFromPath($@"info/reqINT").GetType() == typeof(WzIntProperty))
                {
                    reqStats += $"INT: {EquipImage?.GetFromPath($@"info/reqINT").GetInt()}, ";
                }
                else
                {
                    reqStats += $"INT: {Convert.ToInt32(EquipImage?.GetFromPath($@"info/reqINT").GetString())}, ";
                }
            else
                reqStats += $"INT: 0, ";
            if (EquipImage?.GetFromPath($@"info/reqLUK") != null)
            {
                if(EquipImage?.GetFromPath($@"info/reqLUK").GetType() == typeof(WzIntProperty))
                {
                    reqStats += $"LUK: {EquipImage?.GetFromPath($@"info/reqLUK").GetInt()}, ";
                }
                else
                {
                    reqStats += $"LUK: {Convert.ToInt32(EquipImage?.GetFromPath($@"info/reqLUK").GetString())}, ";
                }
            }
            else
                reqStats += $"LUK: 0, ";
            if (EquipImage?.GetFromPath($@"info/reqSTR") != null)
                if (EquipImage?.GetFromPath($@"info/reqSTR").GetType() == typeof(WzIntProperty))
                {
                    reqStats += $"STR: {EquipImage?.GetFromPath($@"info/reqSTR").GetInt()}, ";
                }
                else
                {
                    reqStats += $"STR: {Convert.ToInt32(EquipImage?.GetFromPath($@"info/reqSTR").GetString())}, ";
                }
            else
                reqStats += $"STR: 0, ";
            reqStats = reqStats.Remove(reqStats.Length - 2) + ".";
            return reqStats;
        }

        // {category}/{EquipImage}/info/reqJob
        public string GetRequiredJob(int equipID)
        {
            string reqJob = "";
            if(EquipImage.GetFromPath($@"info/reqJob") == null)
            {
                reqJob = "Any";
                return reqJob;
            }
            int reqJobID = EquipImage.GetFromPath($@"info/reqJob").GetInt();
            switch(reqJobID)
            {
                case -1:
                    reqJob = "Beginner";
                    break;
                case 0:
                    reqJob = "Any";
                    return reqJob;
                case 1:
                    reqJob = "Warrior";
                    break;
                case 2:
                    reqJob = "Magician";
                    break;
                case 3:
                    reqJob = "Warrior, Magician";
                    break;
                case 4:
                    reqJob = "Bowman";
                    break;
                case 8:
                    reqJob = "Thief";
                    break;
                case 9:
                    reqJob = "Warrior, Thief";
                    break;
                case 13:
                    reqJob = "Warrior, Bowman, Thief";
                    break;
                case 16:
                    reqJob = "Pirate";
                    break;
                case 24:
                    reqJob = "Thief, Pirate";
                    break;
            }
            if(EquipDirectory.Name == "Weapon")
            {
                /* Still need to list the special jobs for every secondary in the game. Too lazy to do now.
                 */
                equipID /= 1000;
                switch (equipID)
                {
                    // Shining Rods
                    case 1212:
                        reqJob += " (Luminous Only)";
                        break;
                    // Soul Shooters
                    case 1222:
                        reqJob += " (Angelic Buster Only)";
                        break;
                    // Desperados
                    case 1232:
                        reqJob += " (Demon Avenger Only)";
                        break;
                    // Whip Blades
                    case 1242:
                        reqJob += " (Xenon Only)";
                        break;
                    // Scepters
                    case 1252:
                        reqJob += " (Beast Tamers Only)";
                        break;
                    // Psy-limiters
                    case 1262:
                        reqJob += " (Kinesis Only)";
                        break;
                    // Chains - All thiefs can use
                    case 1272:
                        reqJob += " (Cadena Primarily)";
                        break;
                    // Lucent Gauntlets
                    case 1282:
                        reqJob += " (Illium Only)";
                        break;
                    // Kataras
                    case 1342:
                        reqJob += " (Dual Blades Only)";
                        break;
                    // Canes
                    case 1362:
                        reqJob += " (Phantom Primarily)";
                        break;
                    // Dual Bowguns - All bowmen can use
                    case 1522:
                        reqJob += " (Mercedes Primarily)";
                        break;
                    // Hand Cannons - All pirates can use
                    case 1532:
                        reqJob += " (Cannoneers Primarily)";
                        break;
                    // Katanas
                    case 1542:
                        reqJob += " (Hayato Only)";
                        break;
                    case 1552:
                        reqJob += " (Kanna Only)";
                        break;
                    // Arm Cannons
                    case 1582:
                        reqJob += " (Blasters Only)";
                        break;
                    // Ancient Bows
                    case 1592:
                        reqJob += " (Pathfinders Only)";
                        break;
                }
            }
            // A new main job or new combination of current jobs
            if(reqJob == "")
            {
                reqJob = "Not Implemented";
            }
            return reqJob;
        }

        public string GetEquipType(int equipID)
        {
            string equipType = "";
            if (EquipDirectory.Name == "Weapon")
            {
                equipID /= 1000;
                switch(equipID)
                {
                    case 1212:
                        equipType = "Shining Rod";
                        break;
                    case 1222:
                        equipType = "Soul Shooter";
                        break;
                    case 1232:
                        equipType = "Desperado";
                        break;
                    case 1242:
                        equipType = "Whip Blade";
                        break;
                    case 1252:
                        equipType = "Scepter";
                        break;
                    case 1262:
                        equipType = "Psy-limiter";
                        break;
                    case 1272:
                        equipType = "Chain";
                        break;
                    case 1282:
                        equipType = "Lucent Gauntlet";
                        break;
                    case 1302:
                        equipType = "One-handed Sword";
                        break;
                    case 1312:
                        equipType = "One-handed Axe";
                        break;
                    case 1322:
                        equipType = "One-handed Mace";
                        break;
                    case 1332:
                        equipType = "Dagger";
                        break;
                    case 1342:
                        equipType = "Katara";
                        break;
                    /* Too lazy to list every different secondary and their respective jobs. I hope they overflow.
                     * Maximizer Ball doesn't have any ID space left, so I don't know how it will pan out in the future.
                     */
                    case 1352:
                        equipType = "Secondary";
                        break;
                    case 1353:
                        equipType = "Secondary";
                        break;
                    case 1362:
                        equipType = "Cane";
                        break;
                    case 1372:
                        equipType = "Wand";
                        break;
                    case 1382:
                        equipType = "Staff";
                        break;
                    case 1402:
                        equipType = "Two-handed Sword";
                        break;
                    case 1412:
                        equipType = "Two-handed Axe";
                        break;
                    case 1422:
                        equipType = "Two-handed Mace";
                        break;
                    case 1432:
                        equipType = "Spear";
                        break;
                    case 1442:
                        equipType = "Polearm";
                        break;
                    case 1452:
                        equipType = "Bow";
                        break;
                    case 1462:
                        equipType = "Crossbow";
                        break;
                    case 1472:
                        equipType = "Claw";
                        break;
                    case 1482:
                        equipType = "Knuckle";
                        break;
                    case 1492:
                        equipType = "Gun";
                        break;
                    case 1522:
                        equipType = "Dual Bowguns";
                        break;
                    case 1532:
                        equipType = "Hand Cannon";
                        break;
                    case 1542:
                        equipType = "Katana";
                        break;
                    case 1552:
                        equipType = "Fan";
                        break;
                    case 1582:
                        equipType = "Arm Cannon";
                        break;
                    case 1592:
                        equipType = "Ancient Bow";
                        break;
                }
            }
            else
            {
                if(EquipImage.GetFromPath($@"info/islot") != null)
                {
                    string islot = EquipImage.GetFromPath($@"info/islot").GetString();
                    switch(islot)
                    {
                        // Still need to do Medals, Emblems and others
                        case "Cp":
                            equipType = "Hat";
                            break;
                        case "Af":
                            equipType = "Face Accessory";
                            break;
                        case "Ay":
                            equipType = "Eye Accessory";
                            break;
                        case "Ae":
                            equipType = "Earrings";
                            break;
                        case "Ma":
                            equipType = "Top";
                            break;
                        case "Pn":
                            equipType = "Bottom";
                            break;
                        case "So":
                            equipType = "Shoes";
                            break;
                        case "Gv":
                            equipType = "Gloves";
                            break;
                        case "Sr":
                            equipType = "Cape";
                            break;
                        case "Si":
                            equipType = "Shield";
                            break;
                        case "Ri":
                            equipType = "Ring";
                            break;
                        case "Pe":
                            equipType = "Pendant";
                            break;
                    }
                }
            }
            return equipType;
        }

        public int GetTotalUpgradeCount()
        {
            if (EquipImage.GetFromPath($@"info/tuc") == null)
                return 0;
            int totalUpgradeCount = EquipImage.GetFromPath($@"info/tuc").GetInt();
            return totalUpgradeCount;
        }

        public string GetEquipStats()
        {
            string equipStats = "";
            if (EquipImage.GetFromPath($@"info/incSTR") != null)
                equipStats += $"STR: +{EquipImage.GetFromPath($@"info/incSTR")}\n";
            if (EquipImage.GetFromPath($@"info/incDEX") != null)
                equipStats += $"DEX: +{EquipImage.GetFromPath($@"info/incDEX")}\n";
            if (EquipImage.GetFromPath($@"info/incINT") != null)
                equipStats += $"INT: +{EquipImage.GetFromPath($@"info/incINT")}\n";
            if (EquipImage.GetFromPath($@"info/incLUK") != null)
                equipStats += $"LUK: +{EquipImage.GetFromPath($@"info/incLUK")}\n";
            if (EquipImage.GetFromPath($@"info/incMHP") != null)
                equipStats += $"MaxHP: +{EquipImage.GetFromPath($@"info/incMHP")}\n";
            if (EquipImage.GetFromPath($@"info/incMMP") != null)
                equipStats += $"MaxMP: +{EquipImage.GetFromPath($@"info/incMMP")}\n";
            if (EquipImage.GetFromPath($@"info/incPDD") != null)
                equipStats += $"Defense: +{EquipImage.GetFromPath($@"info/incPDD")}\n";
            return equipStats;
        }
        #endregion
    }
}
