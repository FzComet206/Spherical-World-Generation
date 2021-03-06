// UNITY_SHADER_NO_UPGRADE
#ifndef BIOMEBOUNDARIES_INCLUDED 
#define BIOMEBOUNDARIES_INCLUDED


void ParseBounds_float(
    float t, float h, float t1, float t2, float t3, float t4, float t5, float t6,
    float h8, float h9, float h10, float h11, float h12, float h13,
    float4 pole, float4 tundra, float4 taiga, float4 plains, float4 desert,
    float4 rocky, float4 savanna, float4 forest, float4 swamp, float4 rainForest,
    out float4 i)
{
    if (t < t1 && h < h13) i = pole;
    else if (t < t2 && h < h12) i = tundra;
    else if (t < t3 && h8 <= h && h < h11) i = taiga;
    else if (t < t4 && h < h8) i = plains;
    else if (h < h8) i = desert;
    else if (t < t5 && h8 <= h && h < h9) i = rocky;
    else if (h8 <= h && h < h9) i = savanna;
    else if (h < h10) i = forest;
    else if (t < t6) i = swamp;
    else i = rainForest;
}

void CalculateUV_float(float3 pos, out float2 uv)
{
    float latitude = asin(pos.y);
    float longitude = atan2(pos.x, -pos.z);
    uv = float2((longitude + 3.14159265) / (2. * 3.14159265), (latitude + 3.14159265 / 2.) / 3.14159265);
}

#endif // BIOMEBOUNDARIES_INCLUDED
