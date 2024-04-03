uint murmurHash11(uint src) {
    const uint M = 0x5bd1e995u;
    uint h = 1190494759u;
    src *= M; src ^= src>>24u; src *= M;
    h *= M; h ^= src;
    h ^= h>>13u; h *= M; h ^= h>>15u;
    return h;
}

float hash11(float src) {
    uint h = murmurHash11(uint(src));
    return float(h & 0x007fffffu | 0x3f800000u) - 1.0;
}

float SumOfSines(float2 uv, float amplitude, int detail, out float2 derivative, float t) {
    float sum = 0;
    float frequency = 1;
    float divx = 0;
    float divy = 0;
    float prevDivx = 0;
    float prevDivy = 0;
    for(int i=0;i<detail;i++) {
        const float ang = hash11(i);
        const float2 dir = float2(sin(ang),cos(ang));
        const float speed = 1;
        float wavelength = 2/frequency;
        const float phase = speed*2/wavelength;
        sum += amplitude * sin(dot(dir,uv+float2(prevDivx,prevDivy))*frequency + t*phase);
        prevDivx = amplitude * frequency * dir.x * cos(dot(dir,uv+float2(prevDivx,prevDivy))*frequency + t*phase);
        prevDivy = amplitude * frequency * dir.y * cos(dot(dir,uv+float2(prevDivx,prevDivy))*frequency + t*phase);
        divx += prevDivx;
        divy += prevDivy;
        amplitude *= 0.82;
        frequency *= 1.12;
    }
    derivative = float2(divx,divy);
    return sum;
}