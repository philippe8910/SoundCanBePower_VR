using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;
using UnityEngine.Rendering.Universal; // 或者 UnityEngine.Rendering.HighDefinition，根据你的项目选择

public class PostProcessingController : MonoBehaviour
{
    public Volume volume;

    private LensDistortion lensDistortion;
    private ChromaticAberration chromaticAberration;

    void Start()
    {
        // 获取Lens Distortion和Chromatic Aberration效果的设置
        volume.profile.TryGet(out lensDistortion);
        volume.profile.TryGet(out chromaticAberration);

        // 初始化Lens Distortion和Chromatic Aberration效果的强度为0
        lensDistortion.intensity.value = 0f;
        chromaticAberration.intensity.value = 0f;
    }

    public void SetLensDistortion(float intensity, float duration)
    {
        // 使用DOTween动画设置Lens Distortion效果的强度
        DOTween.To(() => lensDistortion.intensity.value, x => lensDistortion.intensity.value = x, intensity, duration);
    }

    public void SetChromaticAberration(float intensity, float duration)
    {
        // 使用DOTween动画设置Chromatic Aberration效果的强度
        DOTween.To(() => chromaticAberration.intensity.value, x => chromaticAberration.intensity.value = x, intensity, duration);
    }

    [ContextMenu("FadeOut")]
    public void FadeIn()
    {
        // 使用DOTween动画设置Lens Distortion和Chromatic Aberration效果的强度为0
        DOTween.To(() => lensDistortion.intensity.value, x => lensDistortion.intensity.value = x, -1f, 0.3f).onComplete += () => 
        DOTween.To(() => lensDistortion.intensity.value, x => lensDistortion.intensity.value = x, 0f, 0.3f);

        DOTween.To(() => chromaticAberration.intensity.value, x => chromaticAberration.intensity.value = x, 0f, 1f).onComplete += () =>
        DOTween.To(() => chromaticAberration.intensity.value, x => chromaticAberration.intensity.value = x, 1f, 1f);
    }

    [ContextMenu("FadeIn")]
    public void FadeOut()
    {
        // 使用DOTween动画设置Lens Distortion和Chromatic Aberration效果的强度为1
        DOTween.To(() => lensDistortion.intensity.value, x => lensDistortion.intensity.value = x, 1f, 1f);
        DOTween.To(() => chromaticAberration.intensity.value, x => chromaticAberration.intensity.value = x, 1f, 1f);
    }


}
