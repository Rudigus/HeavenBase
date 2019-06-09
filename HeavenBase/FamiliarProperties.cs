using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapleLib;
using MapleLib.WzLib;
using MapleLib.WzLib.WzProperties;
using PathIO = System.IO.Path;
using System.IO;
using System.Drawing;
using System.Windows.Controls;
using System.Globalization;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;
using System.Windows.Data;
using System.Windows.Media;

namespace HeavenBase
{
    class FamiliarProperties
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
            StringWz stringM = new StringWz(stringFile);
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
                BitmapSource finalCardImage = CreateBitmapSourceFromGdiBitmap(ui.GetCardImage(familiarID));
                BitmapSource finalMobImage = CreateBitmapSourceFromGdiBitmap(mobImage);
                int hasCardImage = 1;
                int hasMobImage = 1;
                if(finalCardImage.Height <= 1)
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
            characterFile = null;
            etcFile = null;
            stringFile = null;
            uiFile = null;
            mobFile = null;
            mob2File = null;
            skill001File = null;
            itemFile = null;

            return familiars;
        }
        #endregion

        public static List<Weapon> LoadWeaponData(string chosenPath, string category)
        {
            List<Weapon> weapons = new List<Weapon>();
            WzFile characterFile = new WzFile(PathIO.Combine(chosenPath, "Character.wz"), WzMapleVersion.CLASSIC);
            WzFile stringFile = new WzFile(PathIO.Combine(chosenPath, "String.wz"), WzMapleVersion.CLASSIC);
            CharacterWz character = new CharacterWz(characterFile, category);
            StringWz stringM = new StringWz(stringFile);
            int loopNumber = character.GetWeaponQuantity();
            for (int i = 0; i < loopNumber; i++)
            {
                int weaponID = character.GetWeaponID(i);
                string weaponName = stringM.GetWeaponName(weaponID, category);
                BitmapSource weaponImage = CreateBitmapSourceFromGdiBitmap(character.GetWeaponImage(characterFile));
                int hasWeaponImage = 1;
                if (weaponImage.Height <= 1)
                    hasWeaponImage = 0;

                weapons.Add(new Weapon()
                {
                    WeaponID = weaponID,
                    WeaponName = weaponName,
                    WeaponImage = weaponImage,
                    HasWeaponImage = hasWeaponImage,
                });
            }
            characterFile.Dispose();
            characterFile = null;
            stringFile.Dispose();
            stringFile = null;

            return weapons;
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
