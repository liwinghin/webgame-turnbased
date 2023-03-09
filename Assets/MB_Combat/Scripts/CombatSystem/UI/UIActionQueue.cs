using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MB.Game
{
    public class UIActionQueue : MonoBehaviour
    {
        [SerializeField] private Transform containers = null;
        [SerializeField] private UICharacterIcon iconPrefab = null;
        [SerializeField] private List<UICharacterIcon> iconList = new List<UICharacterIcon>();
        [SerializeField] private RectTransform iconStartPos = null;
        [SerializeField] private RectTransform iconEndPos = null;
        [SerializeField] private Vector2[] iconPos = new Vector2[10];
        [SerializeField] private Sprite[] teamImg;
        //[SerializeField] private Color playerIconColor = Color.white;
        //[SerializeField] private Color enemyIconColor = Color.white;

        [SerializeField] private float moveTime = 0.3f;
        [SerializeField] public bool isDeleting = false;

        public void Start()
        {
            SetTransformPoint();
        }

        public void SetTransformPoint()
        {
            for (int i = 0; i < iconPos.Length; i++)
            {
                iconPos[i] = iconEndPos.anchoredPosition - new Vector2(110 * i, 0);
            }
        }

        public void SetIconDisable(CharacterUnit character)
        {
            if (isDeleting) { return; }
            isDeleting = true;
            StartCoroutine(FadeOutIcon(character));
        }

        public void DestroyAllIcon()
        {
            StopAllCoroutines();
            foreach (UICharacterIcon c in iconList)
            {
                Destroy(c.gameObject);
            }
        }

        public void ResetIconList()
        {
            iconList.Clear();
        }

        public void AddCharacterIcon(List<CharacterUnit> characters)
        {
            ResetIconList();

            foreach (CharacterUnit c in characters)
            {
                UICharacterIcon newIcon = Instantiate(iconPrefab, transform.localPosition, Quaternion.identity);
                Sprite img = (c.mineFormation.teamType == "Player") ? teamImg[0] : teamImg[1];
                newIcon.transform.SetParent(containers);
                newIcon.transform.localScale = Vector3.one;
                newIcon.GetComponent<RectTransform>().anchoredPosition = iconStartPos.anchoredPosition;
                newIcon.ApplyCharacterInfo(c,img);
                iconList.Add(newIcon);
            }
            SetIconQueue();
        }

        public void SetIconQueue()
        {
            StartCoroutine(MoveIcon());
        }

        private IEnumerator FadeOutIcon(CharacterUnit character)
        {
            UICharacterIcon selectedIcon = null;
            int index = -1;
            //know which index
            foreach (UICharacterIcon icon in iconList)
            {
                if (icon.characterUnit == character)
                {
                    index = iconList.IndexOf(icon);
                    selectedIcon = icon;
                }
            }
            selectedIcon.DestroyIcon();
            RectTransform selectedIconPos = selectedIcon.GetComponent<RectTransform>();
            yield return MoveTo(selectedIconPos, selectedIconPos.anchoredPosition + new Vector2(0, 100), 0.2f);
            iconList.RemoveAt(index);
            yield return MoveIcon();
            isDeleting = false;
        }

        private IEnumerator MoveIcon()
        {
            for (int i = 0; i < iconList.Count; i++)
            {
                RectTransform selectedconPos = iconList[i].GetComponent<RectTransform>();
                StartCoroutine(MoveTo(selectedconPos, iconPos[i], moveTime));
                yield return new WaitForEndOfFrame();
            }
        }

        private IEnumerator MoveTo(RectTransform obj, Vector2 targetPos, float seconds)
        {
            float elapsedTime = 0;
            Vector3 start = obj.anchoredPosition;
            while (elapsedTime < seconds)
            {
                obj.anchoredPosition = Vector3.Lerp(start, targetPos, (elapsedTime / seconds));
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            obj.anchoredPosition = targetPos;
        }

        public void ShowDebug(bool active)
        {
            foreach (UICharacterIcon c in iconList)
            {
                c.ShowDebugInfo(active);
            }
        }
    }
}
