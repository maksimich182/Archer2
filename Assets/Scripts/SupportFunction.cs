using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    class SupportFunction
    {
        static public IEnumerator wait(float waitTime)
        {
            float counter = 0;

            while (counter < waitTime)
            {
                counter += Time.deltaTime;
                yield return null;
            }
        }
    }
}
