using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MB
{
    [Serializable]
    public class PirateBuilder
    {
        public PirateCombine combine = new PirateCombine();
        public CharRuntime character = null;
        public CamRuntime camView = null;

        private bool[] m_optionApplies = InitApplies();

        #region Part prop
        public bool characterApply { get => this.GetApply(0); set => this.SetApply(0, value); }
        public bool productBgApply { get => this.GetApply(1); set => this.SetApply(1, value); }
        public bool detailBgApply { get => this.GetApply(2); set => this.SetApply(2, value); }
        public bool leftHandApply { get => this.GetApply(3); set => this.SetApply(3, value); }
        public bool rightHandApply { get => this.GetApply(4); set => this.SetApply(4, value); }
        #endregion

        #region Data handle
        private static bool[] InitApplies()
        {
            var result = new bool[5];
            result[0] = true;
            result[1] = false;
            result[2] = false;
            result[3] = true;
            result[4] = true;
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
            if (this.productBgApply) 
                yield return this.ApplyProductBg();
            if (this.detailBgApply) 
                yield return this.ApplyDetailBg();
            if (this.characterApply)
            {
                yield return this.ApplyBody();
                yield return this.ApplyFace();
                yield return this.ApplyHair();
                yield return this.ApplyEaring();
                yield return this.ApplyBeard();
                yield return this.ApplyEyewear();
                yield return this.ApplyHat();
                yield return this.ApplyEnergy();
                yield return this.ApplyAura();
            }
            if (this.leftHandApply)
            {
                yield return this.ApplyLeftHands();
            }
            if (this.rightHandApply)
            {
                yield return this.ApplyRightHands();
            }
        }
        #endregion

        #region Parts threading
        private IEnumerator ApplyBody()
        {
            bool updatePart = true;
            bool updateSkin = true;
            var body = this.character.nowBody;

            if (body != null)
            {
                updatePart = (body.partName != this.combine.bodyName);
                updateSkin = (body.bodySkinName != this.combine.bodySkinName);
            }

            if (updatePart)
            {
                // Stop now
                if (body != null)
                {
                    body.StopAttach(this.character);
                }
                // Start new
                var bodyNull = PirateConfig.GetBodyNull(this.combine);
                var bodyPath = PirateConfig.GetBodyPath(this.combine);
                if (!bodyNull)
                {
                    yield return CharBody.LoadPart(bodyPath, (loaded) => {
                        if (loaded != null)
                        {
                            body = loaded.Clone();
                            body.StartAttach(this.character);
                        }
                    });
                }
            }
            if (updateSkin && body != null)
            {
                // Start new
                var skinNull = PirateConfig.GetBodySkinNull(this.combine);
                var skinPath = PirateConfig.GetBodySkinPath(this.combine);
                if (!skinNull)
                {
                    yield return CharBody.LoadBodySkin(skinPath, (loaded) => {
                        body.bodySkin = loaded;
                    });
                }
                else
                {
                    body.bodySkin = null;
                }
            }
        }

        private IEnumerator ApplyFace()
        {
            bool update = true;
            var face = this.character.nowFace;

            if (face != null)
            {
                update = (face.partName != this.combine.faceName);
            }

            if (update)
            {
                // Stop now
                if (face != null)
                {
                    face.StopAttach(this.character);
                }
                // Start new
                var faceNull = PirateConfig.GetFaceNull(this.combine);
                var facePath = PirateConfig.GetFacePath(this.combine);
                if (!faceNull)
                {
                    yield return CharFace.Load(facePath, (loaded) => {
                        if (loaded != null)
                        {
                            face = loaded.Clone();
                            face.StartAttach(this.character);
                        }
                    });
                }
            }
        }

        private IEnumerator ApplyHair()
        {
            bool update = true;
            var hair = this.character.nowHair;

            if (hair != null)
            {
                update = (hair.partName != this.combine.hairName);
            }

            if (update)
            {
                // Stop now
                if (hair != null)
                {
                    hair.StopAttach(this.character);
                }
                // Start new
                var hairNull = PirateConfig.GetHairNull(this.combine);
                var hairPath = PirateConfig.GetHairPath(this.combine);
                if (!hairNull)
                {
                    yield return CharHair.Load(hairPath, (loaded) => {
                        if (loaded != null)
                        {
                            hair = loaded.Clone();
                            hair.StartAttach(this.character);
                        }
                    });
                }
            }
        }

        private IEnumerator ApplyEaring()
        {
            bool update = true;
            var earing = this.character.nowEaring;

            if (earing != null)
            {
                update = (earing.partName != this.combine.earingName);
            }

            if (update)
            {
                // Stop now
                if (earing != null)
                {
                    earing.StopAttach(this.character);
                }
                // Start new
                var earingNull = PirateConfig.GetEaringNull(this.combine);
                var earingPath = PirateConfig.GetEaringPath(this.combine);
                if (!earingNull)
                {
                    yield return CharEaring.Load(earingPath, (loaded) => {
                        if (loaded != null)
                        {
                            earing = loaded.Clone();
                            earing.StartAttach(this.character);
                        }
                    });
                }
            }
        }

        private IEnumerator ApplyBeard()
        {
            bool update = true;
            var beard = this.character.nowBeard;

            if (beard != null)
            {
                update = (beard.partName != this.combine.beardName);
            }

            if (update)
            {
                // Stop now
                if (beard != null)
                {
                    beard.StopAttach(this.character);
                }
                // Start new
                var beardNull = PirateConfig.GetBeardNull(this.combine);
                var beardPath = PirateConfig.GetBeardPath(this.combine);
                if (!beardNull)
                {
                    yield return CharBeard.Load(beardPath, (loaded) => {
                        if (loaded != null)
                        {
                            beard = loaded.Clone();
                            beard.StartAttach(this.character);
                        }
                    });
                }
            }
        }

        private IEnumerator ApplyEyewear()
        {
            bool update = true;
            var eyewear = this.character.nowEyewear;
            if (eyewear != null)
            {
                update = (eyewear.partName != this.combine.eyewearName);
            }

            if (update)
            {
                // Stop now
                if (eyewear != null)
                {
                    eyewear.StopAttach(this.character);
                }
                // Start new
                var eyewearNull = PirateConfig.GetEyewearNull(this.combine);
                var eyewearPath = PirateConfig.GetEyewearPath(this.combine);
                if (!eyewearNull)
                {
                    yield return CharEyewear.Load(eyewearPath, (loaded) => {
                        if (loaded != null)
                        {
                            eyewear = loaded.Clone();
                            eyewear.StartAttach(this.character);
                        }
                    });
                }
            }
        }

        private IEnumerator ApplyHat()
        {
            bool update = true;
            var hat = this.character.nowHat;

            if (hat != null)
            {
                update = (hat.partName != this.combine.hatName);
            }

            if (update)
            {
                // Stop now
                if (hat != null)
                {
                    hat.StopAttach(this.character);
                }
                // Start new
                var hatNull = PirateConfig.GetHatNull(this.combine);
                var hatPath = PirateConfig.GetHatPath(this.combine);
                if (!hatNull)
                {
                    yield return CharHat.Load(hatPath, (loaded) => {
                        if (loaded != null)
                        {
                            hat = loaded.Clone();
                            hat.StartAttach(this.character);
                        }
                    });
                }
            }
        }

        private IEnumerator ApplyEnergy()
        {
            bool update = true;
            var body = this.character.nowBody;

            if (body != null)
            {
                update = (body.energySkinName != this.combine.enegrySkinName);
            }

            if (update)
            {
                if (update && body != null)
                {
                    // Start new
                    var enegrySkinNull = PirateConfig.GetEnegryNull(this.combine);
                    var enegrySkinPath = PirateConfig.GetEnergyPath(this.combine);
                    if (!enegrySkinNull)
                    {
                        yield return CharBody.LoadEnergySkin(enegrySkinPath, (loaded) =>
                        {
                            body.energySkin = loaded;
                        });
                    }
                    else
                    {
                        body.energySkin = null;
                    }
                }
            }
        }

        private IEnumerator ApplyAura()
        {
            bool update = true;
            var aura = this.character.nowAura;

            if (aura != null)
            {
                update = (aura.partName != this.combine.auraName);
            }

            if (update)
            {
                // Stop now
                if (aura != null)
                {
                    aura.StopAttach(this.character);
                }
                // Start new
                var auraNull = PirateConfig.GetAruaNull(this.combine);
                var auraPath = PirateConfig.GetAuraPath(this.combine);
                if (!auraNull)
                {
                    yield return CharAura.Load(auraPath, (loaded) => {
                        if (loaded != null)
                        {
                            aura = loaded.Clone();
                            aura.StartAttach(this.character);
                        }
                    });
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
                var backgroundNull = PirateConfig.GetBgNull(this.combine);
                var backgroundPath = PirateConfig.GetProductBgPath(this.combine);
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
                var backgroundNull = PirateConfig.GetBgNull(this.combine);
                var backgroundPath = PirateConfig.GetDetailBgPath(this.combine);
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

        private IEnumerator ApplyLeftHands()
        {
            yield return this.ApplyItems(this.combine.leftHands, this.character.nowLeftHandItems, CharPartSide.LeftHand);
        }

        private IEnumerator ApplyRightHands()
        {
            yield return this.ApplyItems(this.combine.rightHands, this.character.nowRightHandItems, CharPartSide.RightHand);
        }

        private IEnumerator ApplyItems(List<string> itemValues, List<CharItem> nowCharItems, CharPartSide partSide)
        {
            var charItems = new List<CharItem>(nowCharItems);

            // Stop now
            foreach (var item in charItems)
            {
                if (!itemValues.Contains(item.partName))
                {
                    item.StopAttach(this.character, partSide);
                }
            }

            // Start new
            foreach (var itemValue in itemValues)
            {
                var have = nowCharItems.Exists((item) => item.partName == itemValue);

                if (!have)
                {
                    var itemPah = ItemConfig.GetItemPath(itemValue);
                    yield return CharItem.Load(itemPah, (loaded) =>
                    {
                        if (loaded != null)
                        {
                            var item = loaded.Clone();
                            item.StartAttach(this.character, partSide);
                        }
                    });
                }
                
            }
        }
        #endregion
    }
}