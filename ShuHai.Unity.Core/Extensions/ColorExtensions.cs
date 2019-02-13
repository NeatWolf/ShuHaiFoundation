using UnityEngine;

namespace ShuHai.Unity
{
    public static class ColorExtensions
    {
        /// <summary>
        ///     Copy <paramref name="self" /> and set <paramref name="channel" /> to <paramref name="value" />,
        ///     then return the copied color.
        /// </summary>
        /// <param name="self">Color to copy.</param>
        /// <param name="channel">Channel to set.</param>
        /// <param name="value">Channel value to set.</param>
        /// <returns>
        ///     Color value with given <paramref name="channel" /> set to <paramref name="value" />
        /// </returns>
        /// <remarks>
        ///     This method is not named "SetChannel" since Color is a value type, <paramref name="self" /> is
        ///     passed by value, and setting its field by extension method is actually setting the field of
        ///     the copied Color value of <paramref name="self" />. In order to get this method work, it requires
        ///     the caller use its return value assign to which meant to be changed.
        /// </remarks>
        public static Color Channelled(this Color self, RGBAChannels channel, float value)
        {
            self[(int)channel] = value;
            return self;
        }

        public static float GetChannel(this Color self, RGBAChannels channel) { return self[(int)channel]; }
    }
}