using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRWheel.Individuals
{
    public class SearchSlice : WheelButton
    {
        protected override void OnInit()
        {
            _type = WheelButtonType.Search;
        }
    }
}