using MapleLib.WzLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeavenBase
{
    class MobWz
    {
        public WzImage MobImage;

        public MobWz(WzFile mobWZ)
        {
            mobWZ.ParseWzFile();
        }

        // {mobID}.img/info/level
        public int GetLevel()
        {
            if (MobImage == null)
                return 0;
            int level = MobImage.GetFromPath($@"info/level").GetInt();
            return level;
        }

        public int GetATT()
        {
            if (MobImage == null)
                return 0;
            int att = MobImage.GetFromPath($@"info/PADamage").GetInt();
            return att;
        }

        public void SetMobImage(int mobID, WzFile mobWZ)
        {
            if (mobWZ.WzDirectory.GetImageByName($@"{mobID}.img") != null)
            {
                MobImage = mobWZ.WzDirectory.GetImageByName($@"{mobID}.img");
            }
            else
            {
                MobImage = mobWZ.WzDirectory.GetImageByName($@"0{mobID}.img");
            }
        }

        public WzImage GetMobImage()
        {
            return MobImage;
        }
    }
}
