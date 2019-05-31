using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SMG.OldEzScreenshot
{
    public class EzSS_Tablets : EzSS_Devices
    {
        public override void SetMockups()
        {
            mockup.mockups.Add(EzSS_Mockup.Categories.tablet, new List<string>()
            {
                "apple-ipadPro"
            });
        }

        public override void SetMockupsColors()
        {
            mockup.mockupsColors.Add("apple-ipadPro", new List<string>() { "spaceGray" });
        }

        public override void SetMockupsMultiOriented()
        {
            mockup.mockupsMultiOriented.Add("apple-ipadPro", true);
        }

        public override void SetMockupsPreviewSizes()
        {
            mockup.mockupsPreviewSizes.Add("apple-ipadPro", new Vector2(150f, 200f));
        }

        public override void SetMockupsSizes()
        {
            mockup.mockupsTextureSize.Add("apple-ipadPro", new Vector2(2236f, 2926f));
        }

        public override void SetMockupsAspects()
        {
            mockup.mockupsAspects.Add("apple-ipadPro", "4:3");
        }

        public override void SetMockupsScreenSizes()
        {
            mockup.mockupsScreenSize.Add("apple-ipadPro", new Vector2(93.21f, 95.25f));
        }

        public override void SetMockupsScreenOffsets()
        {

        }
    }
}