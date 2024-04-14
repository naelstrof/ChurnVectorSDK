#ifndef PI
#define PI 3.141592653589793238462643383279502884197
#endif

float3 mod2D289a( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
float2 mod2D289a( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
float3 permutea( float3 x ) { return mod2D289a( ( ( x * 34.0 ) + 1.0 ) * x ); }
float snoisea( float2 v ) {
	v += float2(1,1);
	const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
	float2 i = floor( v + dot( v, C.yy ) );
	float2 x0 = v - i + dot( i, C.xx );
	float2 i1;
	i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
	float4 x12 = x0.xyxy + C.xxzz;
	x12.xy -= i1;
	i = mod2D289a( i );
	float3 p = permutea( permutea( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
	float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
	m = m * m;
	m = m * m;
	float3 x = 2.0 * frac( p * C.www ) - 1.0;
	float3 h = abs( x ) - 0.5;
	float3 ox = floor( x + 0.5 );
	float3 a0 = x - ox;
	m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
	float3 g;
	g.x = a0.x * x0.x + h.x * x0.y;
	g.yz = a0.yz * x12.xz + h.yz * x12.yw;
	return 130.0 * dot( m, g );
}

float SumOfSines(float2 uv, float amplitude, int detail, out float2 derivative, float t, float sharpness) {
    float sum = 0;
    float frequency = 1;
    float divx = 0;
    float divy = 0;
    float prevDivx = 0;
    float prevDivy = 0;
    for(int i=0;i<detail;i++) {
        const float ang = saturate(snoisea(float2(i*234,i*182)));
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

    float frequency = 1;
    float currentSteepness = min(2,detail);
    for(int i=0;i<detail;i++) {
        const float ang = i*3.37;
        const float2 dir = float2(sin(ang*2*PI),cos(ang*2*PI));
        const float speed = 1+(saturate(snoisea(float2(i*619,i*491)))*0.2-0.1);
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
        frequency *= 1.17;
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

    float frequency = 1;
    float currentSteepness = min(2,detail);
    for(int i=0;i<detail;i++) {
        float x = saturate(snoisea(float2(i*97,i*19)));
        float y = saturate(snoisea(float2(i*47,i*67)));
        const float2 circleOffset = (float2(x,y)*2-float2(1,1))*spawnExtents;
        const float2 circlePos = (spawnPosition+circleOffset);
        const float2 dir = normalize(circlePos-position.xz);
        
        const float speed = 1+(saturate(snoisea(float2(i*619,i*491)))*0.2-0.1);
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
        frequency *= 1.17;
    }
    
    position = position+float3(positionSum.x/tiling,positionSum.z,positionSum.y/tiling);
    normal = float3(-normalSum.x, 1-normalSum.z, -normalSum.y);
    return positionSum.z;
}
