namespace day11_dumbo_octopus
{
    public static class Consts
    {
        public const int DIM = 10;
        public const int Steps = 100;
        public const int EnergyLevelToFlash = 9;
        public const int FLASHED_BIT = 1 << 7;
        public const int CLEAR_FLASHED_BIT = ~FLASHED_BIT;
    }
}