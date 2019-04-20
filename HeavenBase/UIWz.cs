using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapleLib.WzLib;
using System.Drawing;

namespace HeavenBase
{
    class UIWz
    {
        private readonly WzImage FamiliarCardImage;

        public UIWz(WzFile UIWZ)
        {
            UIWZ.ParseWzFile();
            FamiliarCardImage = UIWZ.WzDirectory.GetImageByName("FamiliarCard.img");
        }

        // FamiliarCard.img/{FamiliarID}/normal/0
        public Bitmap GetCardImage(int familiarID)
        {
            Bitmap cardImage = null;
            if (FamiliarCardImage?.GetFromPath($@"{familiarID.ToString()}/normal/0/_inlink") == null)
            {
                cardImage = FamiliarCardImage?.GetFromPath($@"{familiarID.ToString()}/normal/0").GetBitmap();
            }
            else
            {
                string inlink = FamiliarCardImage?.GetFromPath($@"{familiarID.ToString()}/normal/0/_inlink").GetString();
                cardImage = FamiliarCardImage?.GetFromPath($@"{inlink}").GetBitmap();
            }
            
            return cardImage;
        }
    }
}
