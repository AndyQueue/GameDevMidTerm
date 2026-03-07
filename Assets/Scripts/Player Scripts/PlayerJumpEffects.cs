using UnityEngine;

[RequireComponent(typeof(PlayerAnimation))]
public class PlayerJumpEffects : MonoBehaviour
{
    [SerializeField] private ParticleSystem jumpParticles;
    private void Awake()
    {
        if (jumpParticles == null)
        {
            jumpParticles = GetComponentInChildren<ParticleSystem>();
        }
    }

    public void PlayJumpEffects()
    {
        if (jumpParticles != null)
        {
            jumpParticles.Play();
        }
    }
}
