using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerBehaviour : MonoBehaviour
{
    public GameObject Player;
    public Rigidbody rb;
    public float defaultSpeed = 5f;
    public float speed = 5f;
    public float swimmingSpeed = 2.5f;
    public Vector3 movementInput;
    private Vector3 movementVelocity;
    public Vector3 playerDirection;
    public Animator animator;
    public CameraFollow cameraFollow;
    public bool isSwimming;
    public bool ShowWeaponHoldAnimation;
    public bool canEquipWeapon;
    public bool canUnequipWeapon;
    public Equipment equippedWeapon;
    public Equipment heldWeapon;
    public EquipmentManager EquipmentManager;
    public Transform PlayerHand;
    public Transform PlayerBack;
    public GameObject HandWeapon;
    public GameObject BackWeapon;

    public bool canMove = true;
    //public float InteractableRadius;
    //public LayerMask InteractableLayer;

    // Start is called before the first frame update
    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        rb = Player.GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        cameraFollow = FindObjectOfType<CameraFollow>();
        EquipmentManager = FindObjectOfType<EquipmentManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            playerDirection = GetInput();
            playerDirection = GetKeyboardInput();

            playerDirection = RotateWithView();

            Move();

            transform.LookAt(transform.position + new Vector3(playerDirection.x, 0, playerDirection.z));
        
            animator.SetFloat("Speed", playerDirection.sqrMagnitude);
            animator.SetBool("Swimming", isSwimming);
            animator.SetBool("ShowWeaponHoldAnimation", ShowWeaponHoldAnimation);
        }

        if (PlayerBack.transform.childCount != 0)
        {
            BackWeapon = PlayerBack.GetChild(0).gameObject;
            ShowWeaponHoldAnimation = false;
        }

        if (PlayerHand.transform.childCount != 0)
        {
            HandWeapon = PlayerHand.GetChild(0).gameObject;
            ShowWeaponHoldAnimation = true;
        }
        
        //Debug.Log(playerDirection.sqrMagnitude);

        if (Input.GetButtonUp("Triangle") && canEquipWeapon)
        {
            //Debug.Log("equipped weapon");
            EquipWeapon();
        }

        if (EquipmentManager.instance.CurrentEquipment[4] != null)
        {
            //Debug.Log("Player has weapon equipped.");
            equippedWeapon = EquipmentManager.instance.CurrentEquipment[4];
        }
        else
        {
            if (BackWeapon)
            {
                Destroy(BackWeapon.gameObject);
            }

            if (HandWeapon)
            {
                Destroy(HandWeapon.gameObject);
            }

            equippedWeapon = null;
            heldWeapon = null;
            HandWeapon = null;
            BackWeapon = null;
            ShowWeaponHoldAnimation = false;
            canUnequipWeapon = false;
        }

        if (heldWeapon && canUnequipWeapon)
        {         
            Debug.Log("Player is holding a weapon.");
            if (Input.GetButtonUp("Triangle"))
            {
                UnequipWeapon();
            }
        }
        else canUnequipWeapon = false;

        if (isSwimming && heldWeapon)
        {
            UnequipWeapon();
        }

        //TODO: FIND INTERACTABLES WITH A SPHERECAST OR SIMILAR INSTEAD OF TRIGGERS.
        /*RaycastHit hit;
        Ray ray;

        if (Physics.SphereCast(transform.position, InteractableRadius, out hit, InteractableLayer))
        {
            Debug.Log("Near interactable");
        }*/
    }

    public void EquipWeapon()
    {
        StartCoroutine(Equip());
    }

    public void UnequipWeapon()
    {
        StartCoroutine(Unequip());
    }

    IEnumerator Equip()
    {
        canEquipWeapon = false;
        
        if (equippedWeapon && !heldWeapon && PlayerBack.childCount == 0) //if we aren't holding a weapon and don't have one on our back
        {
            //Equip the weapon to our hand
            
            GameObject newWeapon = Instantiate(equippedWeapon.ItemObject, PlayerHand.position, Quaternion.identity);
            newWeapon.transform.SetParent(PlayerHand);
            newWeapon.transform.localScale = new Vector3(1,1,1);
            newWeapon.transform.localRotation = Quaternion.Euler(newWeapon.transform.eulerAngles.x, newWeapon.transform.eulerAngles.y, 0);
            PlayerHand.gameObject.SetActive(true);
            heldWeapon = equippedWeapon;
        }
        else if (!heldWeapon && PlayerBack.childCount != 0) //if we aren't holding a weapon but do have one on our back
        {
            //Re-equip the weapon to our hand
            ShowWeaponHoldAnimation = false;
            BackWeapon = PlayerBack.GetChild(0).gameObject;
            BackWeapon.transform.SetParent(PlayerHand);
            BackWeapon.transform.position = PlayerHand.transform.position;
            BackWeapon.transform.localScale = new Vector3(1,1,1);
            BackWeapon.transform.localRotation = Quaternion.Euler(0, 0, 0);
            heldWeapon = equippedWeapon;
            BackWeapon = null;
        }
        yield return new WaitForSeconds(0.5f);
        
        canUnequipWeapon = true;
        canEquipWeapon = true;
    }

    IEnumerator Unequip()
    {
        canUnequipWeapon = false;
        
        if (heldWeapon)
        {
            HandWeapon = PlayerHand.GetChild(0).gameObject;
            HandWeapon.transform.SetParent(PlayerBack);
            HandWeapon.transform.position = PlayerBack.transform.position;
            HandWeapon.transform.localScale = new Vector3(1,1,1);
            HandWeapon.transform.localRotation = Quaternion.Euler(0, 0, 0);
            HandWeapon = null;
        }

        heldWeapon = null;
        yield return new WaitForSeconds(0.5f);
        canEquipWeapon = true;
    }
    
    private void Move()
    {
        rb.velocity = new Vector3(playerDirection.x * speed,
        rb.velocity.y,
        playerDirection.z * speed);
    }

    private Vector3 GetInput()
    {
        Vector3 dir = Vector3.zero;

        dir.x = Input.GetAxis("LHorizontal");
        dir.y = 0;
        dir.z = Input.GetAxis("LVertical");

        if (dir.magnitude > 1)
        {
            dir.Normalize();
        }

        return dir;
    }
    
    private Vector3 GetKeyboardInput()
    {
        Vector3 dir = Vector3.zero;

        dir.x = Input.GetAxis("Horizontal");
        dir.y = 0;
        dir.z = Input.GetAxis("Vertical");

        if (dir.magnitude > 1)
        {
            dir.Normalize();
        }

        return dir;
    }
    
    private Vector3 RotateWithView()
    {
        Vector3 dir = cameraFollow.cameraTransform.TransformDirection(playerDirection);
        dir.Set(dir.x, 0, dir.z);
        return dir.normalized * playerDirection.magnitude;
    }
}
