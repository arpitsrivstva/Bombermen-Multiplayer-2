using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
   public static int RoundToInt(float f)
   {
       return Mathf.RoundToInt(f);
   }

   public static int RandomEven(int min, int maxExclusive)
   {
       int value = Random.Range(min, maxExclusive + 1);

       while(value % 2 != 0)
       {
           value = Random.Range(min, maxExclusive + 1);
       }
       return value;
   }

   public static int RandomOdd(int min, int maxExclusive)
   {
       int value = Random.Range(min, maxExclusive + 1);

       while(value % 2 == 0)
       {
           value = Random.Range(min, maxExclusive + 1);
       }
       return value;
}

}