using System.Collections;
using UnityEngine;

public class ParticlePooledObjects : PooledObject
{
    [SerializeField] private ParticleSystem m_PlayedParticle;
    private Coroutine m_ParticleIsAliveCoroutine;
    public override void OnObjectSpawn()
    {
        m_PlayedParticle.Play();
        base.OnObjectSpawn();
        StartParticleIsAliveCoroutine();
    }
    public override void OnObjectDeactive()
    {
        m_PlayedParticle.Stop();
        base.OnObjectDeactive();
    }
    private void StartParticleIsAliveCoroutine()
    {
        if (m_ParticleIsAliveCoroutine != null)
        {
            StopCoroutine(m_ParticleIsAliveCoroutine);
        }

        m_ParticleIsAliveCoroutine = StartCoroutine(ParticleIsAlive());
    }
    private IEnumerator ParticleIsAlive()
    {
        yield return new WaitUntil(() => (!m_PlayedParticle.IsAlive()));
        OnObjectDeactive();
    }
}
