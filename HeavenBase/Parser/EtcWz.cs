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
        private readonly WzImage familiarInfoImage;

        public EtcWz(WzFile EtcWZ)
        {
            EtcWZ.ParseWzFile();
            familiarInfoImage = EtcWZ.WzDirectory.GetImageByName("FamiliarInfo.img");
        }

        // FamiliarInfo.img/{FamiliarID}/mob
        public int GetMobID(int familiarID)
        {
            WzImageProperty property = familiarInfoImage.GetFromPath($@"{familiarID.ToString()}/mob");
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

        // FamiliarInfo.img/{FamiliarID}/passive
        public int GetPassiveEffectID(int familiarID)
        {
            WzImageProperty property = familiarInfoImage.GetFromPath($@"{familiarID}/passive");
            if (property != null)
            {
                int passiveEffectID = property.GetInt();
                return passiveEffectID;
            }
            else
            {
                return -1;
            }
        }

        // FamiliarInfo.img/{FamiliarID}/consume
        public int GetCardID(int familiarID)
        {
            WzImageProperty property = familiarInfoImage.GetFromPath($@"{familiarID}/consume");
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
    }
}
