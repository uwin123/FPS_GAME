using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    [SerializeField] private float fireRate;                                        //연사 속도 
    [SerializeField] private float fireTimer;                                       //총알과 총알 사이의 발사 간격 설정 
    [SerializeField] private Transform firePoint;                                   //발사되는 지점. 
    [SerializeField] private float range;                                           //사정거리. 
    [SerializeField] private GameObject[] fxFactory;                                //이펙트 팩토리.
    [SerializeField] private GameObject bombFactory;                                //폭탄 프리팹
    [SerializeField] private GameObject bomFirePoint;                               //폭탄 발사 위치 
    [SerializeField] private float power = 20.0f;                                   //폭탄 뎀지 
    [SerializeField] private float bombRangePower;                                  //폭탄 사거리 힘

    AudioSource mp3;
    public AudioClip[] clips;
    Camera curCam;

    void Start()
    {
        curCam = Camera.main;
        mp3 = GetComponent<AudioSource>();
    }

    void Update()
    {
        Fire();
    }

    void Fire()
    {
        if(Input.GetMouseButton(0))
        {
            if(fireTimer < fireRate)
            {
                fireTimer += Time.deltaTime;
                return;
            }
            Ray ray = new Ray(curCam.transform.position, curCam.transform.forward);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit ,range))
            {
                Vector3 dest = curCam.transform.forward * range;
                Debug.DrawRay(firePoint.position, dest, Color.green);
                if(hit.transform.name =="Enemy")
                {
                    hit.transform.GetComponent<Enemy>().DAMAGE(10);
                    GameObject fx = Instantiate(fxFactory[0]);
                    fx.transform.position = hit.point;
                }
            }
            fireTimer = 0.0f;
            mp3.clip = clips[0];
            mp3.Play();
        }
        //마우스 오른쪽 버튼 클릭시 수류탄 던지기 
        if (Input.GetMouseButtonDown(1))
        {
            //수류탄 생성
            GameObject bomb = Instantiate(bombFactory);
            bomb.transform.position = bomFirePoint.transform.position;
            //폭탄은 플레이어가 던지기 때문에
            //폭탄의 리지드바디를 이용해서 던지면 된다.
            Rigidbody rb = bomb.GetComponent<Rigidbody>();

            //전방으로 물리적인 힘을 가한다.
            //rb.AddForce(Camera.main.transform.forward * power, ForceMode.Impulse);

            //45도 각도로 발사
            Vector3 dir = Camera.main.transform.forward + (Camera.main.transform.up * bombRangePower);
            dir.Normalize();
            rb.AddForce(dir * power, ForceMode.Impulse);
        }
    }
    public void ChangeCamera(Camera p_camera)
    {
        curCam = p_camera;
    }
}
