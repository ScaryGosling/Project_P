using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectP.Utility.Generic
{
    public class GenericTimer : MonoBehaviour
    {
        private float absoluteTime = 0f;
        public float setAbsoluteTime { get { return absoluteTime;  } set { absoluteTime = value;  } }
        //public bool timerTaskCompleted { get; set { } }

        void Update()
        {
            while(absoluteTime > 0)
            {
                //if (absoluteTime <= 0)
                    //timerTaskCompleted = true;
            }
        }
    }
}
