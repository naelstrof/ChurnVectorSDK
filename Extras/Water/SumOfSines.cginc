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

float SumOfSines(float2 uv, float amplitude, int detail, out float2 derivative, float t, float sharpness) {
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
        float waveComputed = (sin(dot(dir,uv+float2(prevDivx,prevDivy))*frequency + t*phase)+1)/2;
        sum += 2*amplitude * pow(waveComputed, sharpness);
        prevDivx = sharpness * amplitude * frequency * dir.x * pow(waveComputed, sharpness-1) * cos(dot(dir,uv+float2(prevDivx,prevDivy))*frequency + t*phase);
        prevDivy = sharpness * amplitude * frequency * dir.y * pow(waveComputed, sharpness-1) * cos(dot(dir,uv+float2(prevDivx,prevDivy))*frequency + t*phase);
        divx += prevDivx;
        divy += prevDivy;
        amplitude *= 0.82;
        frequency *= 1.12;
    }
    derivative = float2(divx,divy);
    return sum;
}

float SumOfGerstner(float tiling, float amplitude, int detail, inout float3 position, out float3 normal, out float3 tangent, out float3 binormal, float t, float steepnessNormalized) {
    float3 positionSum = 0;
    float3 normalSum = 0;
    float3 binormalSum = 0;
    float3 tangentSum = 0;

    float frequency = 0.5;
    for(int i=0;i<detail;i++) {
        const float ang = hash11(i);
        const float2 dir = float2(sin(ang),cos(ang));
        const float speed = 1;
        const float wavelength = 2/frequency;
        const float phase = speed*2/wavelength;
        const float steepness = steepnessNormalized/(frequency*amplitude*detail);
        
        const float cosineComp = cos(frequency*dot(dir, position.xz*tiling) + phase*t);
        const float sineComp = sin(frequency*dot(dir,position.xz*tiling) + phase*t);
        
        float2 waveComputed = steepness * amplitude * dir * cosineComp;
        float heightComputed = amplitude*sineComp;
        positionSum += float3(waveComputed.xy, heightComputed);
        
        float binormalXComputed = steepness*dir.x*dir.x*frequency*amplitude*sineComp;
        float binormalYComputed = steepness*dir.x*dir.y*frequency*amplitude*sineComp;
        float binormalZComputed = dir.y*frequency*amplitude*cosineComp;
        binormalSum += float3(binormalXComputed, binormalYComputed, binormalZComputed);

        float tangentXComputed = steepness*dir.x*dir.y*frequency*amplitude*sineComp;
        float tangentYComputed = steepness*dir.y*dir.y*frequency*amplitude*sineComp;
        float tangentZComputed = dir.y*frequency*amplitude*cosineComp;
        tangentSum += float3(tangentXComputed, tangentYComputed, tangentZComputed);
        
        float2 normalComputed = dir * frequency * amplitude * cosineComp;
        float normalHeightComputed = steepness * frequency * amplitude * sineComp;
        normalSum += float3(normalComputed.x, normalComputed.y, normalHeightComputed);
        
        amplitude *= 0.82;
        frequency *= 1.12;
    }
    
    position = float3(positionSum.x/tiling,positionSum.z+position.y,positionSum.y/tiling);
    normal = float3(-normalSum.x, 1-normalSum.z, -normalSum.y);
    binormal = float3(1-binormalSum.x, binormalSum.z, -binormalSum.y);
    tangent = float3(-tangentSum.x, tangentSum.z, 1-tangentSum.y);
    return positionSum.z;
}