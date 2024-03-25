using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class GameController : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] Button[] btns;
    [SerializeField] GameObject[] shapes;

    [Header("Atributes")]
    [SerializeField] GameObject myTrailRender;
    [SerializeField] private float totalTime = 0f;
    public float timerTrail = 0;
    [SerializeField] private int myid = 0;
    private Color _colorShape = Color.white;
    private GameObject ToMove;
    private bool isMoving = false;
    [Header("Bools")]
    [SerializeField] private bool allowIncrease= false;
    [SerializeField] private bool canCreateShape = false;
    [SerializeField] public bool isTouch = false;
    private void Start()
    {
        for (int i = 0; i < btns.Length; i++)
        {
            int id = i;
           btns[i].onClick.AddListener(() => OnPressGetID(id));
        }
    }
    private void Update()
    {
        
        totalTime += Time.deltaTime;
        timerTrail += (1 * Time.deltaTime);

        if (isMoving)
        {
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Touchscreen.current.primaryTouch.position.ReadValue()); ;
            ToMove.transform.position = touchPosition;
        }

        if (timerTrail < 0.23f)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Touchscreen.current.primaryTouch.position.ReadValue());
            myTrailRender.transform.position = pos;
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector3.forward);
            if (hit.collider != null && hit.collider.gameObject.tag == "Shape")
            {
                Destroy(hit.collider.gameObject);
            }
        }
    }
  
    public void OnPressGetID(int id)
    {
        canCreateShape = true;
        switch (id)
        {
            case 0:
                myid = id;
                break;
            case 1:
                myid = id;
                break;
            case 2:
                myid = id;
                break;
            case 3:
                _colorShape = btns[id].GetComponent<Image>().color;
                break;
            case 4:
                _colorShape = btns[id].GetComponent<Image>().color;
                break;
            case 5:
                _colorShape = btns[id].GetComponent<Image>().color;
                break;
        }
    }
    public void OnTouch(InputAction.CallbackContext context)
    {
        CrearUnObjeto(context);
        EliminarUnObejto(context);
        MoverSiTocasUnObjeto(context);
        EliminarVariosObjetos(context);
        switch (context.phase)
        {
            case InputActionPhase.Waiting:
                Debug.Log("Waiting");
                break;
            case InputActionPhase.Disabled:
                Debug.Log("Disabled");
                break;
            case InputActionPhase.Started:
                
                allowIncrease = true;
                //Debug.Log("Started");
                break;
            case InputActionPhase.Performed:
                //Debug.Log(string.Format("Performed - {0}",totalTime));
                //Debug.Log("Performed");
                break;
            case InputActionPhase.Canceled:
                allowIncrease = false;
                //Debug.Log("Canceled");
                break;
        }
    }

    void CrearUnObjeto(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            //Vector2 touchPosition = (Vector2)context.ReadValue<Vector2>() ;
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            if (touchPosition != Vector2.zero)
            {
                Vector2 touchPositionInWorld = Camera.main.ScreenToWorldPoint(touchPosition);
                RaycastHit2D hit = Physics2D.Raycast(touchPositionInWorld, Vector3.forward);

                if (hit.collider == null && canCreateShape)
                {
                    GameObject tmp = Instantiate(shapes[myid], touchPositionInWorld, Quaternion.identity);
                    tmp.GetComponent<SpriteRenderer>().color = _colorShape;
                }
            }
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            canCreateShape = false;
        }
    }
    void EliminarUnObejto(InputAction.CallbackContext context)
    {
        if (totalTime > 0.8f)
        {
            isTouch = false;
        }
        if (context.phase == InputActionPhase.Started && !canCreateShape)
        {
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            Vector3 touchPositionInWorld = Camera.main.ScreenToWorldPoint(touchPosition);
            RaycastHit2D hit = Physics2D.Raycast(touchPositionInWorld, Vector3.forward);
            Debug.DrawRay(touchPositionInWorld, Vector3.forward * 1000000f, Color.red);
            if (hit.collider != null && hit.collider.gameObject.tag == "Shape") 
            { 
                Debug.Log(hit.collider.gameObject.name);
                if (isTouch)
                {
                    if (totalTime > -0.1f && totalTime < 0.8f)
                    {
                        Destroy(hit.collider.gameObject);
                        isTouch = false;
                    }
                }
                else
                {
                    totalTime = 0;
                    isTouch = true;
                }
            }
        }
    }

    void MoverSiTocasUnObjeto(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Touchscreen.current.primaryTouch.position.ReadValue());
            RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector3.forward);
            if (hit.collider != null && hit.collider.gameObject.tag == "Shape")
            {
                ToMove = hit.collider.gameObject;
                isMoving = true;
            }

        }

        if (context.phase == InputActionPhase.Canceled)
        {
            isMoving = false;
        }     
    }
    void EliminarVariosObjetos(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {

            
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Touchscreen.current.primaryTouch.position.ReadValue());
            RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector3.forward);
            if (hit.collider == null)
            {
                timerTrail = 0;
                Vector2 pos = Camera.main.ScreenToWorldPoint(Touchscreen.current.primaryTouch.position.ReadValue());
                myTrailRender.transform.position = pos;
                myTrailRender.SetActive(true);
                myTrailRender.GetComponent<TrailRenderer>().startColor = _colorShape;
            }

        }

        if (context.phase == InputActionPhase.Canceled)
        {
            myTrailRender.SetActive(false);
        }
       
    }
}
