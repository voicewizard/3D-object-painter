using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Painter painter;

    [SerializeField]
    private Button prevObjectButton;
    [SerializeField]
    private Button nextObjectButton;

    [SerializeField]
    private Button resetRotationButton;
    [SerializeField]
    private Button resetPaintingButton;

    [SerializeField]
    private Button savePaintingButton;
    [SerializeField]
    private Button loadPaintingButton;

    [SerializeField]
    private Slider brushSizeSlider;
    [SerializeField]
    private TextMeshProUGUI brushSizeTitle;

    [SerializeField]
    private Slider brushHueSlider;
    [SerializeField]
    private Image brushHueSliderHandleImage;

    private void OnEnable()
    {
        prevObjectButton.onClick.AddListener(OnPrevObjectButtonClicked);
        nextObjectButton.onClick.AddListener(OnNextObjectButtonClicked);
        resetRotationButton.onClick.AddListener(OnResetRotationButtonClicked);
        resetPaintingButton.onClick.AddListener(OnResetPaintingButtonClicked);
        savePaintingButton.onClick.AddListener(OnSavePaintingButtonClicked);
        loadPaintingButton.onClick.AddListener(OnLoadPaintingButtonClicked);
        brushSizeSlider.onValueChanged.AddListener(OnBrushSizeSliderValueChanged);
        brushHueSlider.onValueChanged.AddListener(OnBrushHueSliderValueChanged);
    }

    private void OnDisable()
    {
        prevObjectButton.onClick.RemoveListener(OnPrevObjectButtonClicked);
        nextObjectButton.onClick.RemoveListener(OnNextObjectButtonClicked);
        resetRotationButton.onClick.RemoveListener(OnResetRotationButtonClicked);
        resetPaintingButton.onClick.RemoveListener(OnResetPaintingButtonClicked);
        savePaintingButton.onClick.RemoveListener(OnSavePaintingButtonClicked);
        loadPaintingButton.onClick.RemoveListener(OnLoadPaintingButtonClicked);
        brushSizeSlider.onValueChanged.RemoveListener(OnBrushSizeSliderValueChanged);
        brushHueSlider.onValueChanged.RemoveListener(OnBrushHueSliderValueChanged);
    }

    private void OnNextObjectButtonClicked()
    {
        painter.NextObject();
    }

    private void OnPrevObjectButtonClicked()
    {
        painter.PrevObject();
    }

    private void OnResetRotationButtonClicked()
    {
        painter.ResetRotation();
    }

    private void OnResetPaintingButtonClicked()
    {
        painter.ResetPainting();
    }

    private void OnSavePaintingButtonClicked()
    {
        painter.SaveTexture();
    }

    private void OnLoadPaintingButtonClicked()
    {
        painter.LoadTexture();
    }

    private void OnBrushHueSliderValueChanged(float value)
    {
        Color color = Color.HSVToRGB(value, 1, 1);
        painter.SetBrushColor(color);

        brushHueSliderHandleImage.color = color;
    }

    private void OnBrushSizeSliderValueChanged(float value)
    {
        int brushSize = (int) value;
        painter.SetBrushSize(brushSize);

        brushSizeTitle.text = $"Brush size: {(int) value}";
    }
}
