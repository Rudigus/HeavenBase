using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapleLib.WzLib;

namespace HeavenBase
{
    class EtcWz
    {
        private readonly WzImage FamiliarInfoImage;

        public EtcWz(WzFile EtcWZ)
        {
            EtcWZ.ParseWzFile();
            FamiliarInfoImage = EtcWZ.WzDirectory.GetImageByName("FamiliarInfo.img");
        }

        // FamiliarInfo.img/{FamiliarID}/mob
        public int GetMobID(int familiarID)
        {
            int mobID = FamiliarInfoImage.GetFromPath($@"{familiarID.ToString()}/mob").GetInt();
            return mobID;
        }

        // FamiliarInfo.img/{FamiliarID}/passive
        public int GetPassiveEffectID(int familiarID)
        {
            int passiveEffectID = FamiliarInfoImage.GetFromPath($@"{familiarID}/passive").GetInt();
            return passiveEffectID;
        }

        // FamiliarInfo.img/{FamiliarID}/consume
        public int GetCardID(int familiarID)
        {
            int cardID = FamiliarInfoImage.GetFromPath($@"{familiarID}/consume").GetInt();
            return cardID;
        }
    }
}
