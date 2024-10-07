namespace EventSwirl.Domain.Enums
{
    /// <summary>
    /// Возрастное ограничение
    /// </summary>
    public enum AgeLimitation
    {
        /// <summary>
        /// 0+
        /// </summary>
        NoLimitation = 0,

        /// <summary>
        /// 6+
        /// </summary>
        Kids = 1,

        /// <summary>
        /// 12+
        /// </summary>
        Teens = 2,

        /// <summary>
        /// 16+
        /// </summary>
        PreAdult = 3,

        /// <summary>
        /// 18+
        /// </summary>
        Adult = 4
    }
}
