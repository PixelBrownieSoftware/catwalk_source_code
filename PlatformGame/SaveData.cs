using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace BrownieEngine
{
    [System.Serializable]
    public struct SaveData
    {
        public float time;
    }
    

    public class SaveLoad
    {

        public SaveData loadDat()
        {

            FileStream fs = new FileStream("saveFile.meow", FileMode.Open);
            BinaryFormatter binarySav = new BinaryFormatter();

            SaveData savdat = (SaveData)binarySav.Deserialize(fs);
            //bestTimes = savdat.bestTimes;
            fs.Close();

            return savdat;

        }

        public void saveDat(int list, float time)
        {

            BinaryFormatter binarySav = new BinaryFormatter();
            SaveData savedat;
            FileStream fs = new FileStream("saveFile.meow", FileMode.Create);
            savedat = new SaveData();
            savedat.time = time;
            binarySav.Serialize(fs, savedat);
            fs.Close();


        }

        
    }
}
