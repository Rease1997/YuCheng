using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class InputRemoveEmoji : MonoBehaviour
{
    /// <summary>
    /// 需要屏蔽Emoji的输入框
    /// </summary>
    private InputField m_inputField;

    private void Awake()
    {
        m_inputField = gameObject.GetComponent<InputField>();

        if (m_inputField == null)
        {
            Debug.LogError("[m_inputField] can't find:" + this.gameObject.name);
        }
        else
        {
            m_inputField.onValidateInput += (_text, _index, _addedChar) =>
            {
                // Filter out the Unicode categories you want (example below),
                // keep in mind that, while this can filter out Emojis,
                // it can also filter out chars you might want to keep
                // (e.g. Asiatic languages, which I don't need to care about right now),
                // so this is not a perfect solution.
                var _unicodeCategory = char.GetUnicodeCategory(_addedChar);

                switch (_unicodeCategory)
                {
                    case UnicodeCategory.OtherSymbol:
                    case UnicodeCategory.Surrogate:
                    case UnicodeCategory.ModifierSymbol:
                    case UnicodeCategory.NonSpacingMark:
                        {
                            return char.MinValue;
                        }
                    default:
                        {
                            return _addedChar;
                        }
                }
            };
        }
    }
}
