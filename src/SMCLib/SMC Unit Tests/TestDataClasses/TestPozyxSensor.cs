using SMC_Core;

namespace SMCUnitTests
{
    public class StubPozyxSensor : PozyxSensor
    {
        public StubPozyxSensor(int id, Stream<PozyxData> stream) : base(id, stream)
        {
        }

        public static PozyxData Interpolate(PozyxData lhs, PozyxData rhs, float interpolationRate)
        {
            return interpolate(lhs, rhs, interpolationRate);
        }
    }
}
