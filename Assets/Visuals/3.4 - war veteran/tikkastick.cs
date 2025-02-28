using System.Collections;
using UnityEngine;

public class tikkastick : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator me;
    public Animator other;
    public GameObject corpse;
    public GameObject fireeffect;
    public GameObject grillingeffect;
    public GameObject grillingeffect2;
    public GameObject[] seekhs;
    public GameObject Jody;
    public AudioSource MyPlayer;
    public AudioSource OtherPlayer;
    public AudioClip[] MyClips;
    public AudioClip[] OtherClips;
    void Start()
    {
        MyPlayer.clip = MyClips[0];
        MyPlayer.Play();
        seekhs = GameObject.FindGameObjectsWithTag("seekh");
    }

    // Update is called once per frame
    void Update()
    {
        if (me.GetBool("Roll") == false)
        {
            AnimatorStateInfo stateinfo = other.GetCurrentAnimatorStateInfo(0);
            if (stateinfo.IsName("turnover sticks"))
            {
                me.SetBool("Roll", true);
                StartCoroutine(Flashback());
            }
        }
        
    }

    IEnumerator Flashback()
    {
        yield return new WaitForSeconds(2f);
        OtherPlayer.clip = OtherClips[0];
        OtherPlayer.Play();        

        yield return new WaitForSeconds(1f);
        Vector3 temp = new Vector3(0, 0, 0);
        for (int i = 0; i < seekhs.Length; i++)
        {
            if (seekhs[i].gameObject.name == "seekh")
            {
                temp = transform.position;
                Vector3 v = new Vector3(0, 1000, 0);
                transform.position = v;
            }
            else
                seekhs[i].SetActive(false);
        }
        grillingeffect.SetActive(false);
        grillingeffect2.SetActive(false);

        corpse.SetActive(true);
        fireeffect.SetActive(true);

        MyPlayer.clip = MyClips[1];
        MyPlayer.Play();
        OtherPlayer.clip = OtherClips[1];
        OtherPlayer.Play();

        yield return new WaitForSeconds(5f);
        Jody.SetActive(true);

        yield return new WaitForSeconds(1f);
        corpse.SetActive(false);
        fireeffect.SetActive(false);

        for (int i = 0; i < seekhs.Length; i++)
        {
            if (seekhs[i].gameObject.name == "seekh")
                transform.position = temp;
            else
                seekhs[i].SetActive(true);
        }
        grillingeffect.SetActive(true);
        grillingeffect2.SetActive(true);
        MyPlayer.clip = MyClips[0];
        MyPlayer.Play();
    }
}
