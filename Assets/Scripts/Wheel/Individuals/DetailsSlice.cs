using UnityEngine;
namespace VRWheel.Individuals
{
    public class DetailsSlice : WheelButton
    {
        protected override void OnInit()
        {
            _type = WheelButtonType.Details;
        }
    }
}