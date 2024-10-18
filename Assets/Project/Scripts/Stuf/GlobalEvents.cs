
namespace Project.Scripts
{
    public class GlobalEvents
    {
        private static SavingWrapper wrapper;
        private static PlayerExperience exp;
        public GlobalEvents(SavingWrapper wr, PlayerExperience experience)
        {
            wrapper = wr;
            exp = experience;
        }

        public static void InteractionFieldFinishFilling()
        {
            wrapper.Save();
        }

        public static void AddExp(int amount)
        {
            exp.AddExp(amount);
            wrapper.SaveWallet();
        }
    }
}