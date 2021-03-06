﻿using WarnerEngine.Lib.Helpers;

namespace WarnerEngine.Lib.UI
{
    public struct UIIconInfo
    {
        public static readonly UIIconInfo Empty = new UIIconInfo(null, Index2.Zero, 0, 0);

        public readonly string IconTextureKey;
        public readonly Index2 IconIndex;
        public readonly int IconWidth;
        public readonly int IconHeight;

        public UIIconInfo(string IconTextureKey, Index2 IconIndex, int IconWidth, int IconHeight)
        {
            this.IconTextureKey = IconTextureKey;
            this.IconIndex = IconIndex;
            this.IconWidth = IconWidth;
            this.IconHeight = IconHeight;
        }
    }
}
