namespace CBRE.Settings
{
    public class General
    {
        public static bool EnableDiscordPresence { get; set; }

        static General()
        {
            EnableDiscordPresence = true;
        }
    }
}
