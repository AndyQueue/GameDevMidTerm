using UnityEngine;

[RequireComponent(typeof(PlayerAnimation))]
public class PlayerJumpEffects : MonoBehaviour
{
    [SerializeField] private ParticleSystem jumpParticles;
    private void Awake()
    {
        jumpParticles = GetComponentInChildren<ParticleSystem>();
    }

    public void PlayJumpEffects()
    {
        jumpParticles.Play();
    }
}
