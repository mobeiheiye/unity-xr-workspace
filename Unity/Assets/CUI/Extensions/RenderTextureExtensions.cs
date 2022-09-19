using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CUI.Extensions
{
    public static class RenderTextureExtensions
    {
        public static Texture2D ToTexture2D(this RenderTexture self)
        {
            if (!self)
            {
                return null;
            }
            Texture2D t2d = new Texture2D(self.width, self.height);
            RenderTexture.active = self;
            t2d.ReadPixels(new Rect(0, 0, self.width, self.height), 0, 0);
            t2d.Apply();
            RenderTexture.active = null;
            return t2d;
        }
    }

}