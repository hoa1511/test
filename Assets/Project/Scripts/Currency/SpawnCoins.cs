using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class SpawnCoins : MonoBehaviour, ISpawnItem, ICollectable, IPlaySound
{
    [SerializeField] private AudioClip coinSFX;
    private AudioSource audioSource;

    private bool canMoveToGamObject = false;
    private int coinReward = 50;

    private Transform player;
    private Vector3 positionToJumpIn;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player").transform;   
    }
       private void Update()
    {
        JumpToPlayer();
    }

    public void InitializeItem(Vector3 positionToJumpOut)
    {
        float randomTime = Random.Range(2f, 3f);
        Vector3 itemPickupScale = new Vector3(25,25,25);

        transform.localScale = new Vector3(0,0,0);
        if(this != null)
        {
            transform.DOJump(positionToJumpOut,Random.Range(0.1f, 2),1,Random.Range(0.3f,0.5f));
            transform.DOScale(itemPickupScale,0.2f).OnComplete(() => {
                transform.DOScale(itemPickupScale, 0.5f).OnComplete(() => {
                    canMoveToGamObject = true;
                });
            }); 
        }
    }

    private void JumpToPlayer()
    {
        if(canMoveToGamObject == true)
        {
            GetComponent<Rigidbody>().useGravity = false; 
            positionToJumpIn = player.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, positionToJumpIn, 10 * Time.unscaledDeltaTime);
        }
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Player"))
        {
            PlaySound(coinSFX);
            HandleCollectItem();
            //gameObject.SetActive(false);
            transform.GetChild(0).gameObject.SetActive(false);
            GetComponent<Collider>().enabled = false;
            //Destroy(gameObject, 1);
        }
    }

    public void HandleCollectItem()
    {
        //Bank.Instance.Deposit(coinReward);
        Bank.Instance.UpdateEarnBalance(coinReward);
        Bank.Instance.UpdateNumberOfCoinCollect();
    }

    public void PlaySound(AudioClip sound)
    {
        audioSource.PlayOneShot(sound);
    }
}
