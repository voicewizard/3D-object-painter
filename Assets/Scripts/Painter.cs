using System.IO;
using UnityEditor;
using UnityEngine;

public class Painter : MonoBehaviour
{
    [SerializeField]
    private GameObject objectToPaint;
    
    [SerializeField]
    private int horizontalRotationSpeed = 1;
    [SerializeField]
    private int verticalRotationSpeed = 1;

    [SerializeField]
    private int brushSize = 5;
    [SerializeField]
    private Color brushColor = Color.black;

    private Texture2D paintingTex;

    private void Start()
    {
        SetupObjectToPaint(objectToPaint);
    }
    
    private void Update()
    {
        if (paintingTex == null)
        {
            return;
        }

        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100))
            {
                if (hit.transform != objectToPaint.transform)
                {
                    return;
                }

                int pixelX = Mathf.FloorToInt(hit.textureCoord.x * paintingTex.width);
                int pixelY = Mathf.FloorToInt(hit.textureCoord.y * paintingTex.height);

                DrawCircle(paintingTex, brushColor, pixelX, pixelY, brushSize);
            }
        }

        if (Input.GetMouseButton(1))
        {
            float h = horizontalRotationSpeed * Input.GetAxis("Mouse X");
            float v = verticalRotationSpeed * Input.GetAxis("Mouse Y");
            objectToPaint.transform.RotateAround(objectToPaint.transform.position, Vector3.right, v);
            objectToPaint.transform.RotateAround(objectToPaint.transform.position, Vector3.up, -h);
        }
    }

    private void SetupObjectToPaint(GameObject objectToPaint)
    {
        if (this.objectToPaint != null)
        {
            ResetRotation();
            this.objectToPaint.SetActive(false);
        }

        if (objectToPaint != null)
        {
            this.objectToPaint = objectToPaint;
            objectToPaint.SetActive(true);
            ResetPainting();
        }
    }

    public void SaveTexture()
    {
        byte[] bytes = paintingTex.EncodeToPNG();
        var dirPath = Application.persistentDataPath + "/SaveImages/";

        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }

        var fileName = objectToPaint.name;
        var filePath = dirPath + fileName + ".png";

#if UNITY_EDITOR
        filePath = EditorUtility.SaveFilePanel("Choose a folder for saving the texture", dirPath, fileName, "png");
#endif
        File.WriteAllBytes(filePath, bytes);
    }

    public void LoadTexture()
    {
        byte[] fileData;

        var dirPath = Application.persistentDataPath + "/SaveImages/";
        var fileName = objectToPaint.name;
        var filePath = dirPath + fileName + ".png";

#if UNITY_EDITOR
        filePath = EditorUtility.OpenFilePanel("Load texture", dirPath, "png");
#endif

        if (!File.Exists(filePath))
        {
            return;
        }

        fileData = File.ReadAllBytes(filePath);
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(fileData);

        Renderer renderer = objectToPaint.GetComponent<Renderer>();

        Destroy(paintingTex);
        paintingTex = tex;

        renderer.materials[1].mainTexture = paintingTex;
    }


    private void DrawCircle(Texture2D tex, Color color, int x, int y, int radius = 3)
    {
        float rSquared = radius * radius;

        for (int u = x - radius; u < x + radius + 1; u++)
            for (int v = y - radius; v < y + radius + 1; v++)
                if ((x - u) * (x - u) + (y - v) * (y - v) < rSquared)
                    tex.SetPixel(u, v, color);
        tex.Apply();
    }

    public void PrevObject()
    {
        int currentObjectIndex = objectToPaint.transform.GetSiblingIndex();
        int objectsCount = objectToPaint.transform.parent.childCount;

        if (currentObjectIndex != 0)
        {
            SetupObjectToPaint(objectToPaint.transform.parent.GetChild(currentObjectIndex - 1).gameObject);
        }
        else
        {
            SetupObjectToPaint(objectToPaint.transform.parent.GetChild(objectsCount - 1).gameObject);
        }
    }

    public void NextObject()
    {
        int currentObjectIndex = objectToPaint.transform.GetSiblingIndex();
        int objectsCount = objectToPaint.transform.parent.childCount;

        if (currentObjectIndex != objectsCount - 1)
        {
            SetupObjectToPaint(objectToPaint.transform.parent.GetChild(currentObjectIndex + 1).gameObject);
        }
        else
        {
            SetupObjectToPaint(objectToPaint.transform.parent.GetChild(0).gameObject);
        }
    }

    public void ResetRotation()
    {
        objectToPaint.transform.rotation = Quaternion.identity;
    }

    public void ResetPainting()
    {
        if (paintingTex != null)
        {
            Destroy(paintingTex);
        }

        Renderer renderer = objectToPaint.GetComponent<Renderer>();
        Texture2D mainTex = renderer.materials[0].mainTexture as Texture2D;

        paintingTex = new Texture2D(mainTex.width, mainTex.height, TextureFormat.RGBA32, false)
        {
            alphaIsTransparency = true
        };

        Color32 transparent = new Color(0, 0, 0, 0);
        Color32[] colors = new Color32[mainTex.height * mainTex.width];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = transparent;
        }

        paintingTex.SetPixels32(colors, 0);
        paintingTex.Apply();

        renderer.materials[1].mainTexture = paintingTex;
    }

    public void SetBrushSize(int size)
    {
        brushSize = size;
    }

    public void SetBrushColor(Color color)
    {
        brushColor = color;
    }
}
