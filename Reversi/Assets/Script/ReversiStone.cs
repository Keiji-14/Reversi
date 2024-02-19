using UnityEngine;
using UnityEngine.UI;

namespace Reversi
{
    /// <summary>
    /// オセロの石
    /// </summary>
    public class ReversiStone : MonoBehaviour
    {
        #region SerializeField
        [SerializeField] private Sprite blackSprite;
        [SerializeField] private Sprite whiteSprite;
        [SerializeField] private Image stoneImg;
        #endregion

        #region PublicMethod
        /// <summary>
        /// 初期化
        /// </summary>
        public void Init(StoneType stoneType)
        {
            stoneImg.sprite = stoneType == StoneType.Black ? blackSprite : whiteSprite;
        }

        public void Flip()
        {
            stoneImg.sprite = stoneImg.sprite == blackSprite ? whiteSprite : blackSprite;
        }
        #endregion
    }
}