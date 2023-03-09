float Remap(float value, float fromLow, float fromHigh, float toLow, float toHigh)
{
    return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
}

uint XXHash32(uint value)
{
    const uint PRIME32_2 = 2246822519;
    const uint PRIME32_3 = 3266489917;
    const uint PRIME32_4 = 668265263;
    const uint PRIME32_5 = 374761393;

    uint h32 = value + PRIME32_5;
    h32 = PRIME32_4 * ((h32 << 17) | (h32 >> 15));
    h32 = PRIME32_2 * (h32 ^ (h32 >> 15));
    h32 = PRIME32_3 * (h32 ^ (h32 >> 13));
    return h32 ^ (h32 >> 16);
}

float Range(uint hash, float minValue, float maxValue)
{
    return minValue + (maxValue - minValue) * (float(XXHash32(hash)) / 4294967295.0);
}

int Range(uint hash, int minValue, int maxValue)
{
    return minValue + int((maxValue - minValue) * (float(XXHash32(hash)) / 4294967295.0));
}

float Range(uint hash)
{
    return float(XXHash32(hash)) / 4294967295.0;
}

//Implementation from https://www.shadertoy.com/view/XdXGW8
float2 Grad(int2 value)
{
    int number = XXHash32(value.x + value.y * 11111) & 7;
    float2 gradient = float2(number & 1, number >> 1) * 2.0 - 1.0;
    return (number >= 6) ? float2(0.0, gradient.x) : (number >= 4) ? float2(gradient.x, 0.0) : gradient;
}

//Implementation from https://www.shadertoy.com/view/XdXGW8
float PerlinNoise(float x, float y)
{
    int2 value = int2(floor(x), floor(y));
    float2 fraction = frac(float2(x, y));
    float2 u = fraction * fraction * fraction * (fraction * (fraction * 6.0 - 15.0) + 10.0);

    return lerp(lerp(dot(Grad(value), fraction), 
                     dot(Grad(value + int2(1.0, 0.0)), fraction - float2(1.0, 0.0)), u.x),
                lerp(dot(Grad(value + int2(0.0, 1.0)), fraction - float2(0.0, 1.0)), 
                     dot(Grad(value + int2(1.0, 1.0)), fraction - float2(1.0, 1.0)), u.x), u.y);
}