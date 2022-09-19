using UnityEngine;
using UnityEngine.UI;

namespace CUI.UI
{
    /// <summary>
    /// UGUI.Text Extension
    /// </summary>
    public static class TextExtension
    {
        /// <summary>
        /// set "..." behind the label when it can't hold all the text
        /// </summary>
        /// <param name="self"></param>
        /// <param name="value">txt value</param>
        public static void SetTextWithEllipsis(this Text self, string value)
        {
            float uiScale = self.GetComponentInParent<Canvas>().scaleFactor;
            float containerWidth = self.rectTransform.rect.size.x;

            var generator = new TextGenerator();
            var settings = self.GetGenerationSettings(self.rectTransform.rect.size);
            float contentWidth = generator.GetPreferredWidth(value, settings) / uiScale;

            if (contentWidth <= containerWidth)
            {
                self.text = value;
            }
            else
            {
                string finalvalue = value;
                while (contentWidth / 3 > containerWidth && finalvalue.Length > 1)
                {
                    //Debug.Log(finalvalue);
                    finalvalue = value.Substring(0, finalvalue.Length / 2 - 1);
                    contentWidth = generator.GetPreferredWidth(finalvalue + "...", settings) / uiScale;
                }
                while (contentWidth > containerWidth && finalvalue.Length > 1)
                {
                    //Debug.Log(finalvalue);
                    finalvalue = value.Substring(0, finalvalue.Length - 1);
                    contentWidth = generator.GetPreferredWidth(finalvalue + "...", settings) / uiScale;
                }
                self.text = finalvalue + "...";
            }
        }
    }
}