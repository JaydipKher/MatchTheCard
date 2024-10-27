using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHandler : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;

    private void Awake() {
        StopParticle();
    }
    private void OnEnable()
    {
        GameActionManager.Instance.onLevelComplete+=StartParticle;
        GameActionManager.Instance.onLevelReset+=StopParticle;
    }
    private void OnDisable()
    {
        if(GameActionManager.Instance==null) return;
        GameActionManager.Instance.onLevelComplete-=StartParticle;
        GameActionManager.Instance.onLevelReset-=StopParticle;
    }
    public void StartParticle()
    {
        particle.Play();
    }
    public void StopParticle()
    {
        particle.Stop();
    }
}
