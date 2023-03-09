using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MB
{
    [Serializable]
    public class ShipBuilder
    {
        public ShipCombine combine = new ShipCombine();
        public ShipRuntime ship = null;
        public CamRuntime camView = null;

        private bool[] m_optionApplies = InitApplies();

        #region Part prop
        public bool shipApply { get => this.GetApply(0); set => this.SetApply(0, value); }
        public bool productBgApply { get => this.GetApply(1); set => this.SetApply(1, value); }
        public bool detailBgApply { get => this.GetApply(2); set => this.SetApply(2, value); }
        #endregion

        #region Data handle
        private static bool[] InitApplies()
        {
            var result = new bool[3];
            result[0] = true;
            result[1] = false;
            result[2] = false;
            return result;
        }

        private bool GetApply(int index)
        {
            return this.m_optionApplies[index];
        }

        private void SetApply(int index, bool value)
        {
            this.m_optionApplies[index] = value;
        }
        #endregion

        #region Main threading
        public IEnumerator Apply()
        {
            if (this.shipApply)
            {
                yield return this.ApplyBody();
            }
            if (this.productBgApply)
                yield return this.ApplyProductBg();
            if (this.detailBgApply)
                yield return this.ApplyDetailBg();
        }
        #endregion

        #region Parts threading
        private IEnumerator ApplyBody()
        {
            bool updatePart = true;
            bool updateSkin = true;
            bool updateSail = true;
            var body = this.ship.nowBody;

            if (body != null)
            {
                updatePart = (body.partName != this.combine.bodyName);
                updateSkin = (body.bodySkinName != this.combine.bodySkinName);
                updateSail = (body.sailSkinName != this.combine.sailSkinName);
            }

            // Body part
            if (updatePart)
            {
                if (body != null)
                {
                    body.StopAttach(this.ship);
                }
                
                var bodyNull = ShipConfig.GetBodyNull(this.combine);
                var bodyPath = ShipConfig.GetBodyPath(this.combine);
                if (!bodyNull)
                {
                    yield return ShipBody.LoadPart(bodyPath, (loaded) => {
                        if (loaded != null)
                        {
                            body = loaded.Clone();
                            body.StartAttach(this.ship);
                        }
                    });
                }
            }

            // Skin
            if (updateSkin && body != null)
            {
                var skinNull = ShipConfig.GetBodySkinNull(this.combine);
                var skinPath = ShipConfig.GetBodySkinPath(this.combine);
                if (!skinNull)
                {
                    yield return ShipBody.LoadMaterial(skinPath, (loaded) => {
                        body.bodySkin = loaded;
                    });
                }
                else
                {
                    body.bodySkin = null;
                }
            }

            // Sail
            if (updateSail && body != null)
            {
                var sailNull = ShipConfig.GetSailSkinNull(this.combine);
                var sailSkinPath = ShipConfig.GetSailSkinPath(this.combine);
                if (!sailNull)
                {
                    yield return ShipBody.LoadMaterial(sailSkinPath, (loaded) => {
                        body.sailSkin = loaded;
                    });
                }
                else
                {
                    body.sailSkin = null;
                }
            }
        }

        private IEnumerator ApplyProductBg()
        {
            bool update = true;
            var background = this.camView.nowBackground;

            if (background != null)
            {
                update = (background.partName != this.combine.productBgName);
            }

            if (update)
            {
                // Stop now
                if (background != null)
                {
                    background.StopAttach(this.camView);
                }
                // Start new
                var backgroundNull = ShipConfig.GetBgNull(this.combine);
                var backgroundPath = ShipConfig.GetProductBgPath(this.combine);
                if (!backgroundNull)
                {
                    yield return CamBackground.Load(backgroundPath, (loaded) =>
                    {
                        if (loaded != null)
                        {
                            background = loaded.Clone();
                            background.StartAttach(this.camView);
                        }
                    });
                }
            }
        }

        private IEnumerator ApplyDetailBg()
        {
            bool update = true;
            var background = this.camView.nowBackground;

            if (background != null)
            {
                update = (background.partName != this.combine.detailBgName);
            }

            if (update)
            {
                // Stop now
                if (background != null)
                {
                    background.StopAttach(this.camView);
                }
                // Start new
                var backgroundNull = ShipConfig.GetBgNull(this.combine);
                var backgroundPath = ShipConfig.GetDetailBgPath(this.combine);
                if (!backgroundNull)
                {
                    yield return CamBackground.Load(backgroundPath, (loaded) =>
                    {
                        if (loaded != null)
                        {
                            background = loaded.Clone();
                            background.StartAttach(this.camView);
                        }
                    });
                }
            }
        }
        #endregion
    }
}