namespace WuschelFangen
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            using (var game = new WuschelFangenGame())
            {
                game.Run();
            }
        }
    }
#endif
}

