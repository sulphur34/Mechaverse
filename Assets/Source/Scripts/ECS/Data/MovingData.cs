using System;

namespace Data
{
    [Serializable]
    public struct MovingData
    {
        public float maxSpeedForward;
        public float maxSpeedBackward;
        public float maxSpeedRight;
        public float maxSpeedLeft;
        public float accelerationForward;
        public float accelerationBackward;
        public float accelerationSide;
        public float inertiaDamping;
    }
}