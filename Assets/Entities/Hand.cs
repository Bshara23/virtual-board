using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;




public class Hand
{

    public HG hg;


    public Hand(string json)
    {
        

        this.hg = JsonConvert.DeserializeObject<HG>(json);
    }


    public string print()
    {
       
        var reSerializedJson = JsonConvert.SerializeObject(this.hg);
        return reSerializedJson;
    }

    public Hand()
    {
        this.hg = new HG();
        this.hg.left = new XY[21];
        for (int i = 0; i < 21; i++)
        {
            XY xy = new XY();
            xy.x = 1;
            xy.y = 2;
            this.hg.left[i] = xy;

        }
        this.hg.right = new XY[21];
        for (int i = 0; i < 21; i++)
        {
            XY xy = new XY();
            xy.x = 1;
            xy.y = 2;
            this.hg.right[i] = xy;
        }
        var reSerializedJson = JsonConvert.SerializeObject(this.hg);
        this.hg = JsonConvert.DeserializeObject<HG>(reSerializedJson);

    }


    override public string ToString()
    {

        string str = "";
        foreach (XY xy in hg.right)
        {
            str += xy.x + " : " + xy.y;
        }
        foreach (XY xy in hg.left)
        {
            str += xy.x + " : " + xy.y;
        }
        return str;
    }

    [System.Serializable]
    public struct HG
    {
        public IList<XY> left { get; set; }
        public IList<XY> right { get; set; }

        public string leftGesture;
        public string rightGesture;
   
    }

  
    [System.Serializable]
    public struct XY
    {
        public float x;
        public float y;
    }
  
    /*
    private void processString(string str)
    {
        char[] delimiterChars = { ',', ']', '[', ' ' };
        string[] words = str.Split(delimiterChars);

        int i = 0;
        int j = 0;
        int k = 0;
        int c = 0;

        left = new float[21, 2];
        right = new float[21, 2];

        foreach (string w in words)
        {
            if (w != "")
            {
                c += 1;
                float x = float.Parse(w, CultureInfo.InvariantCulture);

                if (i == 0)
                {
                    left[j, k] = x;
                }
                else if (i == 1)
                {
                    right[j, k] = x;
                }

                k = c % 2;
                j = (c / 2) % 21;
                i = c / (21 * 2);

            }
        }

    }
    */
}
