using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCursor : MonoBehaviour
{

    public static PlayerCursor instance;
    [SerializeField] private Texture2D[] cursorSprites;

    private Vector2 normalHotspot = Vector2.zero;
    private Vector2 crosshairHotspot = new Vector2(16, 16);

    [SerializeField] private GameObject clickParticle;

    private void Awake()
    {
        instance = this;
    }
    public enum Cursors 
    { 
        Default,
        ZoomIn,
        ZoomOut,
    }

    private void Start()
    {
        ChangeSprite(Cursors.Default);
    }

    private void Update()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));
        transform.position = worldPos;

        if (Mouse.current.leftButton.wasPressedThisFrame && clickParticle != null)
        {
            HelperFunctions.SpawnParticleSystem(clickParticle, 1f, transform.position);
        }
    }


    public void ChangeSprite(Cursors cursorImage)
    {
        switch (cursorImage) 
        {
            case Cursors.Default: Cursor.SetCursor(cursorSprites[0], normalHotspot, CursorMode.Auto); break;
            case Cursors.ZoomIn: Cursor.SetCursor(cursorSprites[1], normalHotspot, CursorMode.Auto); break;
            case Cursors.ZoomOut: Cursor.SetCursor(cursorSprites[2], normalHotspot, CursorMode.Auto); break;
            default: Debug.LogWarning("No Sprite Found"); break;
        }

    }

    public IEnumerator DefaultCursorCooldown(float durationUntilChange)
    {
        yield return new WaitForSeconds(durationUntilChange);
        ChangeSprite(Cursors.Default);
    }

}


