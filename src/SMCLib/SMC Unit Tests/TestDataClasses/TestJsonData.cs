using SMC_Core;

namespace SMCUnitTests
{
    class StubJsonData : DataEntry
    {
        public int test1Param;
        public string test2Param;
        public bool test3Param;
        public StubJsonData(int timestamp, int id, int test1Param, string test2Param, bool test3Param) : base(timestamp, id)
        {
            this.test1Param = test1Param;
            this.test2Param = test2Param;
            this.test3Param = test3Param;
        }
    }
}
