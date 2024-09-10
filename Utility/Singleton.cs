namespace CodeHelper.Utility
{
    /// <summary>
    /// Singleton Model Base Class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> where T : class, new()
    {
        private static T? _instance;

        private static readonly object syslock = new object();

        /// <summary>
        /// Get T Instance
        /// </summary>
        /// <returns></returns>
        public static T GetInstance()
        {
            if (_instance == null)
            {
                lock (syslock)
                {
                    if (_instance == null)
                    {
                        _instance = new T();
                    }
                }
            }
            return _instance;
        }
    }
}
