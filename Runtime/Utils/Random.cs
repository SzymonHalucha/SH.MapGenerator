namespace SH.MapGenerator.Utils
{
    public class Random
    {
        private class SplitMix64
        {
            private ulong state;

            public SplitMix64(ulong seed)
            {
                state = seed;
            }

            public ulong Next()
            {
                state += 0x9E3779B97F4A7C15;

                ulong result = state;
                result = (result ^ (result >> 30)) * 0xBF58476D1CE4E5B9;
                result = (result ^ (result >> 27)) * 0x94D049BB133111EB;
                return result ^ (result >> 31);
            }
        }

        private ulong[] state = new ulong[4];

        public Random()
        {
            ulong seed = (ulong)System.DateTime.Now.Ticks;
            SplitMix64 generator = new SplitMix64(seed);
            state[0] = generator.Next();
            state[1] = generator.Next();
            state[2] = generator.Next();
            state[3] = generator.Next();
        }

        public Random(int seed)
        {
            SplitMix64 generator = new SplitMix64((ulong)(seed + int.MaxValue + 1));
            state[0] = generator.Next();
            state[1] = generator.Next();
            state[2] = generator.Next();
            state[3] = generator.Next();
        }

        public void Seed(int value)
        {
            SplitMix64 generator = new SplitMix64((ulong)(value + int.MaxValue + 1));
            state[0] = generator.Next();
            state[1] = generator.Next();
            state[2] = generator.Next();
            state[3] = generator.Next();
        }

        public ulong Next()
        {
            ulong result = (state[1] * 5ul << 7) | (state[1] * 5ul >> (64 - 7)) * 9ul;
            ulong t = state[1] << 17;

            state[2] ^= state[0];
            state[3] ^= state[1];
            state[1] ^= state[2];
            state[0] ^= state[3];

            state[2] ^= t;
            state[3] = (state[3] << 45) | (state[3] >> (64 - 45));

            return result;
        }

        public float Range()
        {
            ulong result = state[0] + state[3];
            ulong t = state[1] << 17;

            state[2] ^= state[0];
            state[3] ^= state[1];
            state[1] ^= state[2];
            state[0] ^= state[3];

            state[2] ^= t;
            state[3] = (state[3] << 45) | (state[3] >> (64 - 45));

            return (float)result / ulong.MaxValue;
        }

        public int Range(int max)
        {
            return (int)(Next() % (ulong)max);
        }

        public int Range(int min, int max)
        {
            return (int)(Next() % (ulong)(max - min)) + min;
        }

        public float Range(float max)
        {
            return Range() * max;
        }

        public float Range(float min, float max)
        {
            return Range() * (max - min) + min;
        }

        public int Weighted(float[] weights)
        {
            float weightSum = 0;
            foreach (float weight in weights)
                weightSum += weight;

            float selected = Range(weightSum);
            float weightCurrent = 0;
            float weightPrevious = 0;

            for (int i = 0; i < weights.Length; i++)
            {
                weightCurrent += weights[i];

                if (selected <= weightCurrent && selected >= weightPrevious)
                    return i;

                weightPrevious += weights[i];
            }

            return 0;
        }

        public int Weighted(int[] weights)
        {
            int weightSum = 0;
            foreach (int weight in weights)
                weightSum += weight;

            int selected = Range(weightSum);
            int weightCurrent = 0;
            int weightPrevious = 0;

            for (int i = 0; i < weights.Length; i++)
            {
                weightCurrent += weights[i];

                if (selected <= weightCurrent && selected >= weightPrevious)
                    return i;

                weightPrevious += weights[i];
            }

            return 0;
        }
    }
}