using System.Collections.Generic;
using System.Numerics;


namespace SMC_Core
{
    //TODO: If json deserialize does not work, create custom deserializer class for pozyx data
    public class PozyxData : DataEntry
    {
        public float score;
        public List<Vector3> accelerometer;
        public Vector3 coordinates;
        public float hardwareTimestamp;
        public float speed; // note: not in class diagram
        /// <summary>
        /// Constructor for Pozyx positional data
        /// </summary>
        /// <param name="timestamp">Timestamp at which the data was recieved</param>
        /// <param name="id">Sensor ID</param>
        /// <param name="score">Certainty score (presumably of a neural network) of the coordinates</param>
        /// <param name="accelerometer">A list of accerometer data (in milli G)</param>
        /// <param name="coordinates">Coordinates of the tag</param>
        /// <param name="hardwareTimestamp">Timestamp at which the sample was captured</param>
        /// <param name="speed">The speed of the tag</param>
        public PozyxData(long timestamp, int id, float score, List<Vector3> accelerometer, Vector3 coordinates, float hardwareTimestamp, float speed)
            : base(timestamp, id)
        {
            this.score = score;
            this.accelerometer = accelerometer;
            this.coordinates = coordinates;
            this.hardwareTimestamp = hardwareTimestamp;
            this.speed = speed;
        }
        /// <summary>
        /// Provides a string representation of a Pozyx data entry
        /// </summary>
        /// <returns>A string representation of a Pozyx data entry</returns>
        public override string ToString()
        {
            // doesn't show accelerometer data
            return base.ToString() + $" ID: { id } score: { score } position: { coordinates } hardwareTimestamp: { hardwareTimestamp } speed: { speed }";
        }

        public override bool Equals(object obj)
        {
            return obj is PozyxData data && this == data;
        }

        public static bool operator ==(PozyxData lhs, PozyxData rhs)
        {
            return !(lhs != rhs);
        }

        public static bool operator !=(PozyxData lhs, PozyxData rhs)
        {
            // needed for if one side is null, otherwise throws exception
            if (ReferenceEquals(rhs, null) || ReferenceEquals(lhs, null))
                return true;

            return lhs.hardwareTimestamp != rhs.hardwareTimestamp ||
                    lhs.id != rhs.id ||
                    lhs.score != rhs.score ||
                    // TODO?: lhs.accelerometer != rhs.accelerometer ||
                    lhs.coordinates != rhs.coordinates ||
                    lhs.speed != rhs.speed;
        }
    }
}