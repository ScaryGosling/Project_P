//Author: Emil Dahl
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Proj.Utility.Generic
{
    public class GenericTimer : MonoBehaviour
    {
        private float absoluteStartTime = 0;
        public float timeControl { get { return absoluteStartTime; } set { absoluteStartTime = value; } }
        public bool timeTaskCompleted;

        void Update()
        {
            while (absoluteStartTime > 0)
            {
                absoluteStartTime -= Time.deltaTime;
                if (absoluteStartTime <= 0)
                    timeTaskCompleted = true;
            }
        }


    }

}
