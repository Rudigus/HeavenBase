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

namespace HeavenBase
{
    class FamiliarProperties
    {
        public static List<Familiar> LoadCollectionData(string chosenPath)
        {
            List<Familiar> familiars = new List<Familiar>();
            WzFile characterFile = new WzFile(PathIO.Combine(chosenPath, "Character.wz"), WzMapleVersion.CLASSIC);
            WzFile etcFile = new WzFile(PathIO.Combine(chosenPath, "Etc.wz"), WzMapleVersion.CLASSIC);
            WzFile stringFile = new WzFile(PathIO.Combine(chosenPath, "String.wz"), WzMapleVersion.CLASSIC);
            WzFile uiFile = new WzFile(PathIO.Combine(chosenPath, "UI.wz"), WzMapleVersion.CLASSIC);
            CharacterWz character = new CharacterWz(characterFile);
            EtcWz etc = new EtcWz(etcFile);
            StringWz stringM = new StringWz(stringFile);
            UIWz ui = new UIWz(uiFile);
            int loopNumber = character.GetFamiliarQuantity();
            for (int i = 0; i < loopNumber; i++)
            {
                int familiarID = character.GetFamiliarID(i);
                character.SetFamiliarImage(familiarID);
                int mobID = etc.GetMobID(familiarID);
                int skillID = character.GetSkillID();
                int passiveEffectID = etc.GetPassiveEffectID(familiarID);
                int cardID = etc.GetCardID(familiarID);
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

                });
            }
            characterFile.Dispose();
            etcFile.Dispose();
            stringFile.Dispose();
            uiFile.Dispose();
            characterFile = null;
            etcFile = null;
            stringFile = null;
            uiFile = null;

            return familiars;
        }

        public static bool PathIsValid(string path)
        {
            WzFile characterFile = new WzFile(PathIO.Combine(path, "Character.wz"), WzMapleVersion.CLASSIC);
            try {
                characterFile.ParseWzFile();
            } catch {
                characterFile.Dispose();
                characterFile = null;
                return false;
            }
            characterFile.Dispose();
            characterFile = null;
            return true;
        }
    }
}
