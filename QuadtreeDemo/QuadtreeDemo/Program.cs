namespace QuadtreeDemo
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (QuadtreeDemoGame game = new QuadtreeDemoGame())
            {
                game.Run();
            }
        }
    }
#endif
}

