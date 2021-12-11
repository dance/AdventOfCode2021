using static day11_dumbo_octopus.Consts;

namespace day11_dumbo_octopus
{
    public static class Extensions
    {
        public static bool Flashed(this int value) => (value & FLASHED_BIT) > 0;
    }
}