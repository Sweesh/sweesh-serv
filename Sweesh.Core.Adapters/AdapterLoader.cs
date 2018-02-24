namespace Sweesh.Core.Adapters
{
    using Configuration;

    public static class AdapterLoader
    {
        /// <summary>
        /// This is required to for StructureMap to load adapters.
        /// If the library is no directly used by the web instance, .net will remove it.
        /// </summary>
        /// <returns>The adapters.</returns>
        /// <param name="setup">Setup.</param>
        public static Setup LoadAdapters(this Setup setup)
        {
            return setup;
        }
    }
}
