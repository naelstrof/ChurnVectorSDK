#ifndef PI
#define PI 3.141592653589793238462643383279502884197
#endif

inline float noise_randomValue (float2 uv) { return frac(sin(dot(uv, float2(12.9898, 78.233)))*43758.5453); }

float SumOfSines(float2 uv, float amplitude, int detail, out float2 derivative, float t, float sharpness) {
    float sum = 0;
    float frequency = 1;
    float divx = 0;
    float divy = 0;
    float prevDivx = 0;
    float prevDivy = 0;
    for(int i=0;i<detail;i++) {
        const float ang = noise_randomValue(float2(i*234,i*182));
        const float2 dir = float2(sin(ang*2*PI),cos(ang*2*PI));
        const float speed = 1;
        const float wavelength = 2*PI/frequency;
        const float phase = speed*2*PI/wavelength;
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

float SumOfGerstnerDirectional(float tiling, float amplitude, int detail, inout float3 position, out float3 normal, float t, float steepnessNormalized) {
    float3 positionSum = 0;
    float3 normalSum = 0;

    float frequency = 0.5;
    float currentSteepness = min(2,detail);
    for(int i=0;i<detail;i++) {
        const float ang = noise_randomValue(float2(i*234,i*182));
        const float2 dir = float2(sin(ang*2*PI),cos(ang*2*PI));
        const float speed = 1+(noise_randomValue(i*123)-0.5)*0.1f;
        const float wavelength = 2*PI/frequency;
        const float phase = speed*2*PI/wavelength;
        const float steepness = steepnessNormalized/(frequency*amplitude*currentSteepness);
        currentSteepness *= ((float)(detail-1)/(float)detail)*2;
        
        const float cosineComp = cos(frequency*dot(dir, position.xz*tiling) + phase*t);
        const float sineComp = sin(frequency*dot(dir,position.xz*tiling) + phase*t);
        
        float2 waveComputed = steepness * amplitude * dir * cosineComp;
        float heightComputed = amplitude*sineComp;
        positionSum += float3(waveComputed.xy, heightComputed);
        
        //float binormalXComputed = steepness*dir.x*dir.x*frequency*amplitude*sineComp;
        //float binormalYComputed = steepness*dir.x*dir.y*frequency*amplitude*sineComp;
        //float binormalZComputed = dir.y*frequency*amplitude*cosineComp;
        //binormalSum += float3(binormalXComputed, binormalYComputed, binormalZComputed);

        //float tangentXComputed = steepness*dir.x*dir.y*frequency*amplitude*sineComp;
        //float tangentYComputed = steepness*dir.y*dir.y*frequency*amplitude*sineComp;
        //float tangentZComputed = dir.y*frequency*amplitude*cosineComp;
        //tangentSum += float3(tangentXComputed, tangentYComputed, tangentZComputed);
        
        float2 normalComputed = dir * frequency * amplitude * cosineComp;
        float normalHeightComputed = steepness * frequency * amplitude * sineComp;
        normalSum += float3(normalComputed.x, normalComputed.y, normalHeightComputed);
        
        amplitude *= 0.82;
        frequency *= 1.12;
    }
    
    position = position+float3(positionSum.x/tiling,positionSum.z,positionSum.y/tiling);
    normal = float3(-normalSum.x, 1-normalSum.z, -normalSum.y);
    //binormal = float3(1-binormalSum.x, binormalSum.z, -binormalSum.y);
    //tangent = float3(-tangentSum.x, tangentSum.z, 1-tangentSum.y);
    return positionSum.z;
}

float SumOfGerstnerCircular(float2 spawnExtents, float2 spawnPosition, float tiling, float amplitude, int detail, inout float3 position, out float3 normal, float t, float steepnessNormalized) {
    float3 positionSum = 0;
    float3 normalSum = 0;

    float frequency = 0.5;
    float currentSteepness = min(2,detail);
    for(int i=0;i<detail;i++) {
        float x = noise_randomValue(float2(i*2545,i*1428));
        float y = noise_randomValue(float2(i*1234,i*3294));
        const float2 circleOffset = (float2(x,y)*2-float2(1,1))*spawnExtents;
        const float2 circlePos = (spawnPosition+circleOffset);
        const float2 dir = normalize(circlePos-position.xz);
        
        const float speed = 1;
        const float wavelength = 2*PI/frequency;
        const float phase = speed*2*PI/wavelength;
        const float steepness = steepnessNormalized/(frequency*amplitude*currentSteepness);
        currentSteepness *= ((float)(detail-1)/(float)detail)*2;
        
        const float cosineComp = cos(frequency*dot(dir, (position.xz-circlePos)*tiling) + phase*t);
        const float sineComp = sin(frequency*dot(dir, (position.xz-circlePos)*tiling) + phase*t);
        
        float2 waveComputed = steepness * amplitude * dir * cosineComp;
        float heightComputed = amplitude*sineComp;
        positionSum += float3(waveComputed.xy, heightComputed);
        
        float2 normalComputed = dir * frequency * amplitude * cosineComp;
        float normalHeightComputed = steepness * frequency * amplitude * sineComp;
        normalSum += float3(normalComputed.x, normalComputed.y, normalHeightComputed);
        
        amplitude *= 0.82;
        frequency *= 1.12;
    }
    
    position = position+float3(positionSum.x/tiling,positionSum.z,positionSum.y/tiling);
    normal = float3(-normalSum.x, 1-normalSum.z, -normalSum.y);
    return positionSum.z;
}
