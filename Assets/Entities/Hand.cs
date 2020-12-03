using System.Collections;
using System.Collections.Generic;
using System.Globalization;


public class Hand
{

    public float[,] left { get; set; }
    public float[,] right { get; set; }

    public bool isLeft { get; set; }
    public bool isRight { get; set; }
    public int leftIndex { get; set; }
    public int rightIndex { get; set; }


    public Hand(string str)
    {
        processString(str);
    }

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
}
