using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.EventSystems;

public class LevelButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private int levelNumber; //1-based index for level number

    private SpotLightManager spotLight;

    public void Start()
    {

        spotLight = GameObject.FindGameObjectWithTag("SpotLight").GetComponent<SpotLightManager>();
        if (spotLight == null)
        {
            Debug.LogWarning("LevelButton: No SpotLightManager with tag 'SpotLight' found in the scene.");
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Hover start");
        spotLight.enableSpotLight();
        spotLight.moveSpotLight(levelNumber);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Hover end");
        spotLight.disableSpotLight();

    }

}
