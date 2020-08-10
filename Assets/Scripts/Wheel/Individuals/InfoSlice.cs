using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRWheel.Individuals
{
    public class InfoSlice : WheelButton
    {
        protected override void OnInit()
        {
            _type = WheelButtonType.Information;
        }
    }
}