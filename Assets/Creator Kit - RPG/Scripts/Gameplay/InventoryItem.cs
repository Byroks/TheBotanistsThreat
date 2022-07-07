using RPGM.Core;
using RPGM.Gameplay;
using RPGM.UI;
using UnityEngine;


namespace RPGM.Gameplay
{
    /// <summary>
    /// Marks a gameObject as a collectable item.
    /// </summary>
    [ExecuteInEditMode]
    [RequireComponent(typeof(SpriteRenderer), typeof(CircleCollider2D))]
    public class InventoryItem : MonoBehaviour
    {
        public int count = 1;
        public Sprite sprite;
        AudioSource audioSource;

        GameModel model = Schedule.GetModel<GameModel>();

        void awake(){
            audioSource = GetComponent<AudioSource>();
        }

        void Reset()
        {
            GetComponent<CircleCollider2D>().isTrigger = true;
        }

        void OnEnable()
        {
            GetComponent<SpriteRenderer>().sprite = sprite;
        }

        public void OnCollisionEnter2D(Collision2D collider)
        {
            if(collider.gameObject.tag == "Player"){
                gameObject.SetActive(false);
                model.AddInventoryItem(this);
                if(count != 0)
                    MessageBar.Show($"{name} x {count}");
                UserInterfaceAudio.OnCollect();
            }
        }
    }
}