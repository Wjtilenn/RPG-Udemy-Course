using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("屏幕抖动")]
    private CinemachineImpulseSource screenShake;
    [SerializeField] private float shakeMultiplier;
    [SerializeField] private Vector3 shakePower;

    [Header("闪光特效")]
    [SerializeField] private float flashDuration;
    [SerializeField] private Material hitMat;
    private Material originalMat;

    [Header("debuff颜色")]
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] chillColor;
    [SerializeField] private Color[] shockColor;

    [Header("粒子效果")]
    [SerializeField] private ParticleSystem igniteFX;
    [SerializeField] private ParticleSystem chillFX;
    [SerializeField] private ParticleSystem shockFX;

    [Header("受击效果")]
    [SerializeField] private GameObject hitFX;

    [Header("接剑灰尘效果")]
    [SerializeField] private ParticleSystem dustFX;

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        screenShake = GetComponent<CinemachineImpulseSource>();
        originalMat = sr.material;
    }

    public void ScreenShake()
    {
        screenShake.m_DefaultVelocity = new Vector3(shakePower.x * PlayerManager.instance.player.facingDir, shakePower.y) * shakeMultiplier;
        screenShake.GenerateImpulse();
    }

    public void MakeTransprent(bool _transparent)
    {
        if (_transparent)
        {
            sr.color = Color.clear;
        }
        else
        {
            sr.color = Color.white;
        }
    }

    private IEnumerator FlashFX()
    {
        sr.material = hitMat;
        Color currentColor = sr.color;
        sr.color = Color.white;

        yield return new WaitForSeconds(flashDuration);

        sr.color = currentColor;
        sr.material = originalMat;
    }

    private void RedColorBlink()
    {
        if(sr.color != Color.white)
        {
            sr.color = Color.white;
        }
        else
        {
            sr.color = Color.red;
        }
    }

    private void CancelColorChange()
    {
        CancelInvoke();
        sr.color = Color.white;
        
        igniteFX.Stop();
        chillFX.Stop();
        shockFX.Stop();
    }

    public void IgniteFXFor(float _seconds)
    {

        igniteFX.Play();

        InvokeRepeating("IgnitColorFX", 0, 0.3f);
        Invoke("CancelColorChange", _seconds);
    }

    private void IgnitColorFX()
    {
        if(sr.color != igniteColor[0])
        {
            sr.color = igniteColor[0];
        }
        else
        {
            sr.color = igniteColor[1];
        }
    }

    public void ChillFXFor(float _seconds)
    {
        chillFX.Play();
        InvokeRepeating("ChillColorFX", 0, 0.3f);
        Invoke("CancelColorChange", _seconds);
    }

    private void ChillColorFX()
    {
        if (sr.color != chillColor[0])
        {
            sr.color = chillColor[0];
        }
        else
        {
            sr.color = chillColor[1];
        }
    }

    public void ShockFXFor(float _seconds)
    {
        shockFX.Play();
        
        InvokeRepeating("ShockColorFX", 0, 0.3f);
        Invoke("CancelColorChange", _seconds);
    }

    private void ShockColorFX()
    {
        if (sr.color != shockColor[0])
        {
            sr.color = shockColor[0];
        }
        else
        {
            sr.color = shockColor[1];
        }
    }

    public void CreateHitFX(Transform _target)
    {
        float zRation = Random.Range(-90, 90);
        float xPosition = Random.Range(-0.5f, 0.5f);
        float yPosition = Random.Range(-0.5f, 0.5f);
        GameObject newHitFX = Instantiate(hitFX, _target.position + new Vector3(xPosition, yPosition), Quaternion.identity);
        newHitFX.transform.Rotate(new Vector3(0, 0, zRation));


        Destroy(newHitFX, 0.5f);
    }

    public void PlayDustFX()
    {
        if(dustFX != null)
        {
            dustFX.Play();
        }
    }
}
