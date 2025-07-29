
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Text (UGUI) 组件扩展类
/// 功能
/// 实现文字过长省略号显示
/// </summary>
public static class TextExtensionUtils
{
    

    /// <summary>
    ///  Unity 2019 版本以上
    /// </summary>
    /// <param name="textComponent"></param>
    /// <param name="value"></param>
    public static void SetTextWithEllipsis(Text textComponent, string value, int characterVisibleCount)
    {

        string updatedText = value;
        if (value.Length > characterVisibleCount)
        {
            updatedText = value.Substring(0, characterVisibleCount - 1);
            updatedText += "…";
        }
        textComponent.text = updatedText;
    }
}