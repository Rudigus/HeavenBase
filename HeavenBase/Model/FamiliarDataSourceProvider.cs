using System;
using System.Collections.Generic;
using MapleLib.WzLib;
using PathIO = System.IO.Path;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace HeavenBase
{
    class FamiliarDataSourceProvider
    {
        #region DataGridLoader
        /// <summary>
        /// Gets all the info to be shown by DataGrid
        /// </summary>
        /// <param name="chosenPath"></param>
        /// <returns></returns>
        public static List<Familiar> LoadCollectionData(string chosenPath)
        {
            List<Familiar> familiars = new List<Familiar>();
            WzFile characterFile = new WzFile(PathIO.Combine(chosenPath, "Character.wz"), WzMapleVersion.CLASSIC);
            WzFile etcFile = new WzFile(PathIO.Combine(chosenPath, "Etc.wz"), WzMapleVersion.CLASSIC);
            WzFile stringFile = new WzFile(PathIO.Combine(chosenPath, "String.wz"), WzMapleVersion.CLASSIC);
            WzFile uiFile = new WzFile(PathIO.Combine(chosenPath, "UI.wz"), WzMapleVersion.CLASSIC);
            WzFile mobFile = new WzFile(PathIO.Combine(chosenPath, "Mob.wz"), WzMapleVersion.CLASSIC);
            WzFile mob2File = new WzFile(PathIO.Combine(chosenPath, "Mob2.wz"), WzMapleVersion.CLASSIC);
            WzFile skill001File = new WzFile(PathIO.Combine(chosenPath, "Skill001.wz"), WzMapleVersion.CLASSIC);
            WzFile itemFile = new WzFile(PathIO.Combine(chosenPath, "Item.wz"), WzMapleVersion.CLASSIC);
            CharacterWz character = new CharacterWz(characterFile);
            EtcWz etc = new EtcWz(etcFile);
            StringWz stringM = new StringWz(stringFile, true);
            UIWz ui = new UIWz(uiFile);
            MobWz mob = new MobWz(mobFile);
            MobWz mob2 = new MobWz(mob2File);
            Skill001Wz skill001 = new Skill001Wz(skill001File);
            ItemWz item = new ItemWz(itemFile);
            int loopNumber = character.GetFamiliarQuantity();
            for (int i = 0; i < loopNumber; i++)
            {
                int familiarID = character.GetFamiliarID(i);
                character.SetFamiliarImage(familiarID);
                int mobID = etc.GetMobID(familiarID);
                if (mobID == -1)
                {
                    mobID = character.GetMobID();
                }
                mob.SetMobImage(mobID, mobFile);
                mob2.SetMobImage(mobID, mob2File);
                MobWz realMob;
                if (mob.MobImage != null)
                {
                    realMob = mob;
                }
                else
                {
                    realMob = mob2;
                }
                int skillID = character.GetSkillID();
                int passiveEffectID = etc.GetPassiveEffectID(familiarID);
                int cardID = etc.GetCardID(familiarID);
                if (cardID == -1)
                {
                    cardID = character.GetCardID();
                }
                int level = character.GetLevel(); // DON'T CHANGE THE ORDER, OR IT'LL BE MESSED UP
                if (level == 0)
                {
                    level = realMob.GetLevel();
                }
                int att = character.GetATT(); // SAME AS LEVEL, DON'T CHANGE ORDER
                if (att == 0)
                {
                    att = realMob.GetATT();
                }
                Bitmap mobImage = realMob.GetMobImage();
                BitmapSource finalCardImage = CreateBitmapSourceFromGdiBitmap(item.GetCardImage(cardID));
                BitmapSource finalMobImage = CreateBitmapSourceFromGdiBitmap(mobImage);
                int hasCardImage = 1;
                int hasMobImage = 1;
                if (finalCardImage == null || finalCardImage.Height <= 1)
                    hasCardImage = 0;
                if (finalMobImage == null)
                    hasMobImage = 0;

                familiars.Add(new Familiar()
                {
                    FamiliarID = familiarID,
                    MobID = mobID,
                    MobName = stringM.GetMobName(mobID),
                    SkillID = skillID,
                    SkillName = stringM.GetSkillName(skillID),
                    SkillDescription = stringM.GetSkillDesc(skillID),
                    PassiveEffectID = passiveEffectID,
                    PassiveEffect = stringM.GetPassiveEffect(passiveEffectID),
                    Range = character.GetRange(),
                    Rarity = character.GetRarity(),
                    CardID = cardID,
                    CardName = stringM.GetCardName(cardID),
                    SkillCategory = skill001.GetSkillCategory(skillID),
                    Level = level,
                    ATT = att,
                    PassiveEffectTarget = item.GetPassiveEffectTarget(passiveEffectID),
                    PassiveEffectBonus = item.GetPassiveEffectBonus(passiveEffectID),
                    CardImage = finalCardImage,
                    MobImage = finalMobImage,
                    HasCardImage = hasCardImage,
                    HasMobImage = hasMobImage,
                });
            }
            characterFile.Dispose();
            etcFile.Dispose();
            stringFile.Dispose();
            uiFile.Dispose();
            mobFile.Dispose();
            mob2File.Dispose();
            skill001File.Dispose();
            itemFile.Dispose();

            return familiars;
        }
        #endregion

        public static List<Equip> LoadEquipData(string chosenPath, string category)
        {
            List<Equip> equips = new List<Equip>();
            WzFile characterFile = new WzFile(PathIO.Combine(chosenPath, "Character.wz"), WzMapleVersion.CLASSIC);
            WzFile stringFile = new WzFile(PathIO.Combine(chosenPath, "String.wz"), WzMapleVersion.CLASSIC);
            CharacterWz character = new CharacterWz(characterFile, category);
            StringWz stringM = new StringWz(stringFile, false);
            int loopNumber = character.GetEquipQuantity();
            for (int i = 0; i < loopNumber; i++)
            {
                int equipID = character.GetEquipID(i);
                string equipName = stringM.GetEquipName(equipID, category);
                string equipClassification = character.GetEquipClassification();
                BitmapSource equipImage = CreateBitmapSourceFromGdiBitmap(character.GetEquipImage(characterFile));
                int hasEquipImage = 1;
                if (equipImage.Height <= 1)
                    hasEquipImage = 0;
                if (equipClassification == "Cash")
                {
                    equips.Add(new Equip()
                    {
                        /* Missing Int Properties don't make the program crash, like Level. 
                         * But I'll put it anyway for organization sake.
                         */
                        EquipID = equipID,
                        EquipName = equipName,
                        EquipImage = equipImage,
                        HasEquipImage = hasEquipImage,
                        EquipLevel = 0,
                        EquipClassification = equipClassification,
                        EquipType = "",
                        RequiredStats = "",
                        RequiredJob = "",
                        TotalUpgradeCount = 0,
                        EquipStats = "",
                    });
                    continue;
                }

                equips.Add(new Equip()
                {
                    EquipID = equipID,
                    EquipName = equipName,
                    EquipImage = equipImage,
                    HasEquipImage = hasEquipImage,
                    EquipLevel = character.GetEquipLevel(),
                    EquipClassification = equipClassification,
                    EquipType = character.GetEquipType(equipID),
                    RequiredStats = character.GetRequiredStats(),
                    RequiredJob = character.GetRequiredJob(equipID),
                    TotalUpgradeCount = character.GetTotalUpgradeCount(),
                    EquipStats = character.GetEquipStats(),
                });
            }
            characterFile.Dispose();
            stringFile.Dispose();

            return equips;
        }

        #region PathValidator
        /// <summary>
        /// Checks if the given path is the root of .wz files (at least for Character.wz)
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool PathIsValid(string path)
        {
            WzFile characterFile = new WzFile(PathIO.Combine(path, "Character.wz"), WzMapleVersion.CLASSIC);
            try
            {
                try
                {
                    try
                    {
                        characterFile.ParseWzFile();
                    }
                    catch (NotSupportedException)
                    {
                        return false;
                    }
                    
                }
                catch (FileNotFoundException)
                {
                    return false;
                }
            }
            catch (DirectoryNotFoundException)
            {
                return false;
            }
            finally
            {
                characterFile.Dispose();
                characterFile = null;
            }
            return true;
        }
        #endregion

        public static BitmapSource CreateBitmapSourceFromGdiBitmap(Bitmap bitmap)
        {
            if (bitmap == null)
            {
                return null;
                //throw new ArgumentNullException("bitmap");
            }

            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            var bitmapData = bitmap.LockBits(
                rect,
                ImageLockMode.ReadWrite,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            try
            {
                var size = (rect.Width * rect.Height) * 4;

                return BitmapSource.Create(
                    bitmap.Width,
                    bitmap.Height,
                    bitmap.HorizontalResolution,
                    bitmap.VerticalResolution,
                    PixelFormats.Bgra32,
                    null,
                    bitmapData.Scan0,
                    size,
                    bitmapData.Stride);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }
    }
}
