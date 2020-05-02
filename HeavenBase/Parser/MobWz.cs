using MapleLib.WzLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeavenBase
{
    class MobWz
    {
        public WzImage MobImage;
        public WzDirectory MobDirectory;

        public MobWz(WzFile mobWZ)
        {
            mobWZ.ParseWzFile();
            MobDirectory = mobWZ.WzDirectory;
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

        // {mobID}.img/stand/0 DON'T CONFUSE THE DIFFERENT MOBIMAGES
        public Bitmap GetMobImage()
        {
            if (MobImage == null)
                return null;
            Bitmap mobImage = null;
            string thumbnailPath;

            if (MobImage?.GetFromPath($@"info/thumbnail") != null)
            {
                mobImage = MobImage?.GetFromPath($@"info/thumbnail").GetBitmap();
                return mobImage;
            }
            else if (MobImage?.GetFromPath($@"stand/0") != null)
                thumbnailPath = "stand";
            else if (MobImage?.GetFromPath($@"fly/0") != null)
                thumbnailPath = "fly";
            else
                return null;

            if (MobImage?.GetFromPath($@"{thumbnailPath}/0/_outlink") == null && MobImage?.GetFromPath($@"{thumbnailPath}/0/_inlink") == null)
            {
                mobImage = MobImage?.GetFromPath($@"{thumbnailPath}/0").GetBitmap();
            }
            else if (MobImage?.GetFromPath($@"{thumbnailPath}/0/_outlink") != null)
            {
                string outlink = MobImage?.GetFromPath($@"{thumbnailPath}/0/_outlink").GetString();
                string treatedlink = outlink.Substring(outlink.IndexOf("/") + 1);
                int newMobID = Convert.ToInt32(treatedlink.Substring(0, treatedlink.IndexOf(".")));
                string remainderlink = treatedlink.Substring(treatedlink.IndexOf("/") + 1);
                WzImage newMobImage = MobDirectory.GetImageByName($@"{newMobID}.img");
                mobImage = newMobImage?.GetFromPath($@"{remainderlink}").GetBitmap();
            }
            else
            {
                string inlink = MobImage?.GetFromPath($@"{thumbnailPath}/0/_inlink").GetString();
                mobImage = MobImage?.GetFromPath($@"{inlink}").GetBitmap();
            }

            return mobImage;
        }
    }
}
