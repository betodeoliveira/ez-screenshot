using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SMG.EzScreenshot
{
    public class EzSS_Watches : EzSS_Devices
    {
        public override void SetMockups()
        {
            mockup.mockups.Add(EzSS_Mockup.Categories.watch, new List<string>()
            {
                "apple-watch4"
            });
        }

        public override void SetMockupsColors()
        {
            mockup.mockupsColors.Add("apple-watch4", new List<string>() { "black", "roseGold", "white" });
        }

        public override void SetMockupsMultiOriented()
        {
            mockup.mockupsMultiOriented.Add("apple-watch4", false);
        }

        public override void SetMockupsPreviewSizes()
        {
            mockup.mockupsPreviewSizes.Add("apple-watch4", new Vector2(100f, 150f));
        }

        public override void SetMockupsSizes()
        {
            mockup.mockupsTextureSize.Add("apple-watch4", new Vector2(556, 944));
        }

        public override void SetMockupsAspects()
        {
            mockup.mockupsAspects.Add("apple-watch4", "13:16");
        }

        public override void SetMockupsScreenSizes()
        {
            mockup.mockupsScreenSize.Add("apple-watch4", new Vector2(82.83f, 58.41f));
        }

        public override void SetMockupsScreenOffsets()
        {
            mockup.mockupsScreenOffset.Add("apple-watch4", new Vector2(-13f, 0f));
        }
    }
}