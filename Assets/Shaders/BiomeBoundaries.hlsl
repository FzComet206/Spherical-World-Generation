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

void ParseBounds2_float(
    float t, float h, float t1, float t2, float t3, float t4, float t5, float t6,
    float h8, float h9, float h10, float h11, float h12, float h13,
    float4 pole, float4 tundra, float4 taiga, float4 plains, float4 desert,
    float4 rocky, float4 savanna, float4 forest, float4 swamp, float4 rainForest,
    out float4 i)
{
    i =   (t < t1 && h < h13) * pole
        + (t1 <= t < t2 && h < h12) * tundra
        + (t2 <= t < t3 && h8 <= h < h11) * taiga
        + (t2 <= t < t4 && h < h8) * plains
        + (t4 <= t && h < h8) * desert
        + (t3 <= t < t5 && h8 <= h && h < h9) * rocky
        + (t5 <= t && h8 <= h && h < h9) * savanna
        + (t3 <= t && h9 <= h < h10) * forest
        + (t3 <= t < t6 && h10 <= h) * swamp
        + (t6 <= t && h10 <= h) * rainForest;
}

#endif // BIOMEBOUNDARIES_INCLUDED
