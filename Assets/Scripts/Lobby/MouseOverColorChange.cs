using UnityEngine;

public class MouseOverColorChange : MonoBehaviour
{
    private Color originalColor;
    public Color hoverColor = Color.red; 

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            originalColor = renderer.material.color;
        }
    }

    void OnMouseEnter()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = hoverColor;
        }
    }

    void OnMouseExit()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = originalColor;
        }
    }
}
