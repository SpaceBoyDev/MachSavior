void LightStep_half(half3 LightValue, half Threshold, half3 LitColor, half3 ShadowColor, out half3 Out)
{
    Out = LitColor;
    half light = length(LightValue);
    if (light <= Threshold)
    {
        Out = ShadowColor;
    }
}

void SmoothStep_half(half3 LightValue, half LowThreshold, half HighThreshold, half3 LitColor, half3 ShadowColor,
                     out half3 Out)
{
    Out = LitColor;
    half light = length(LightValue);
    if (light <= LowThreshold)
    {
        Out = ShadowColor;
    }
    else if (light <= HighThreshold)
    {
        half step = (light - LowThreshold) / (HighThreshold - LowThreshold);
        Out = lerp(ShadowColor, LitColor, step);
    }
}
