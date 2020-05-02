using System;
using System.Drawing;
//using System.Windows.Media.Imaging;

namespace HeavenBase
{
    [Serializable]
    class Equip
    {
        public int EquipID { get; set; } // CharacterWz - GetEquipID()
        public string EquipName { get; set; } // StringWz - GetEquipName()
        public Bitmap EquipImage { get; set; } // CharacterWz - GetEquipImage()
        public int HasEquipImage { get; set; }
        public int EquipLevel { get; set; } // CharacterWz - GetEquipLevel()
        public string EquipClassification { get; set; } // CharacterWz - GetEquipClassification()
        public string EquipType { get; set; } // CharacterWz - GetEquipType()
        public string RequiredStats { get; set; } // CharacterWz - GetRequiredStats()
        public string RequiredJob { get; set; } // CharacterWz - GetRequiredJob()
        public int TotalUpgradeCount { get; set; } // CharacterWz - GetTotalUpgradeCount()
        //public bool isWearable { get; set; }
        public string EquipStats { get; set; } // CharacterWz - GetEquipStats()
    }
}
