using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace HeavenBase
{
    class Weapon
    {
        public int WeaponID { get; set; }
        public string WeaponName { get; set; }
        public BitmapSource WeaponImage { get; set; }
        public int HasWeaponImage { get; set; }
    }
}
