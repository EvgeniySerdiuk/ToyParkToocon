using UnityEngine;

namespace Project.Scripts.Arch.EventBus
{
    public readonly struct EventOperatorData
    {
        public readonly float CameraDelayOnFieldActivation;
        public readonly float CameraDelayBeforeStartMoving;
        public readonly FieldByuUpgrade BuyField;
        public readonly Vector3 NextTargetPosition;

        public EventOperatorData(float cameraDelayOnFieldActivation, float cameraDelayBeforeStartMoving,
            FieldByuUpgrade buyField,  Vector3 nextTargetPosition)
        {
            CameraDelayOnFieldActivation = cameraDelayOnFieldActivation;
            CameraDelayBeforeStartMoving = cameraDelayBeforeStartMoving;
            BuyField = buyField;
            NextTargetPosition = nextTargetPosition;
        }
    }
}