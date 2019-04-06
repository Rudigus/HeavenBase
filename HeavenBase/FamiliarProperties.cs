﻿using System;
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
            CharacterWz character = new CharacterWz(characterFile);
            EtcWz etc = new EtcWz(etcFile);
            StringWz stringM = new StringWz(stringFile);
            UIWz ui = new UIWz(uiFile);
            MobWz mob = new MobWz(mobFile);
            MobWz mob2 = new MobWz(mob2File);
            int loopNumber = character.GetFamiliarQuantity();
            for (int i = 0; i < loopNumber; i++)
            {
                int familiarID = character.GetFamiliarID(i);
                character.SetFamiliarImage(familiarID);
                int mobID = etc.GetMobID(familiarID);
                int skillID = character.GetSkillID();
                int passiveEffectID = etc.GetPassiveEffectID(familiarID);
                int cardID = etc.GetCardID(familiarID);
                int level = character.GetLevel();
                if (level == 0) {
                    mob.SetMobImage(mobID, mobFile);
                    level = mob.GetLevel();
                }
                if (level == 0) {
                    mob2.SetMobImage(mobID, mob2File);
                    level = mob2.GetLevel();
                }
                familiars.Add(new Familiar()
                {
                    FamiliarID = familiarID,
                    MobID = mobID,
                    MobName = stringM.GetMobName(mobID),
                    CardImage = ui.GetCardImage(familiarID),
                    SkillID = skillID,
                    SkillName = stringM.GetSkillName(skillID),
                    SkillDescription = stringM.GetSkillDesc(skillID),
                    PassiveEffectID = passiveEffectID,
                    PassiveEffect = stringM.GetPassiveEffect(passiveEffectID),
                    Range = character.GetRange(),
                    Rarity = character.GetRarity(),
                    CardID = cardID,
                    CardName = stringM.GetCardName(cardID),
                    SkillCategory = "",
                    Level = level,

                });
            }
            characterFile.Dispose();
            etcFile.Dispose();
            stringFile.Dispose();
            uiFile.Dispose();
            mobFile.Dispose();
            mob2File.Dispose();
            characterFile = null;
            etcFile = null;
            stringFile = null;
            uiFile = null;
            mobFile = null;
            mob2File = null;

            return familiars;
        }
        #endregion

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
                    characterFile.ParseWzFile();
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

    }
}
