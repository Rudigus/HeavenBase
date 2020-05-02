using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapleLib.WzLib;
using System.Drawing;
using System.Drawing.Design;

namespace HeavenBase
{
    class UIWz
    {
        private readonly WzImage familiarCardImage;

        public UIWz(WzFile UIWZ)
        {
            UIWZ.ParseWzFile();
            familiarCardImage = UIWZ.WzDirectory.GetImageByName("FamiliarCard.img");
        }

        // FamiliarCard.img/{FamiliarID}/normal/0
        public Bitmap GetCardImage(int familiarID)
        {
            Bitmap cardImage = null;
            if (familiarCardImage?.GetFromPath($@"{familiarID.ToString()}/normal/0/_inlink") == null)
            {
                WzImageProperty property = familiarCardImage?.GetFromPath($@"{familiarID.ToString()}/normal/0");
                if (property != null) 
                {
                    cardImage = property.GetBitmap();
                }
            }
            else
            {
                string inlink = familiarCardImage?.GetFromPath($@"{familiarID.ToString()}/normal/0/_inlink").GetString();
                cardImage = familiarCardImage?.GetFromPath($@"{inlink}").GetBitmap();
            }
            
            return cardImage;
        }
    }
}
