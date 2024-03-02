using UnityEngine;
using UnityEngine.UI;

namespace Reversi
{

    /// <summary>
    /// 石の種類
    /// </summary>
    public enum StoneType
    {
        Black,
        White,
        UnSetStone,
    }

    /// <summary>
    /// オセロの石
    /// </summary>
    public class ReversiStone : MonoBehaviour
    {
        #region SerializeField
        [SerializeField] private Sprite blackSprite;
        [SerializeField] private Sprite whiteSprite;
        /// <summary>オセロ石の画像</summary>
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

        /// <summary>
        /// スプライトを切り替える処理
        /// </summary>
        public void Flip()
        {
            stoneImg.sprite = stoneImg.sprite == blackSprite ? whiteSprite : blackSprite;
        }
        #endregion
    }
}