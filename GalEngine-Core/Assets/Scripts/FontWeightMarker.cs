using UnityEngine;

public enum FontWeightType { Bold, Light, Medium } // 定义你需要的样式

public class FontWeightMarker : MonoBehaviour
{
    public FontWeightType weight = FontWeightType.Medium;
}
