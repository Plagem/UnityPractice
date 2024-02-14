using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject stateWindow;
    [SerializeField] private Text textLv;
    [SerializeField] private Text textName;
    [SerializeField] private Text textHp;
    [SerializeField] private Text textAttackPower;
    [SerializeField] private Button closeButton;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            InfoUnitState();
        }
    }

    private void Init()
    {
        stateWindow.SetActive(false);
    }

    private void InfoUnitState()
    {
        RaycastHit outHit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(ray, out outHit);

        if(outHit.collider.CompareTag("Soldier"))
        {
            ShowStateInUI(outHit.collider.gameObject.GetComponent<Soldier>().InforUnitState());
        }
        else if(outHit.collider.CompareTag("Flight"))
        {
            ShowStateInUI(outHit.collider.gameObject.GetComponent<Flight>().InforUnitState());
        }
    }

    private void ShowStateInUI(string[] state)
    {
        textName.text = "Name : " + state[0];
        textLv.text = "Lv : " + state[1];
        textHp.text = "HP : " + state[2];
        textAttackPower.text = "Attack : " +  state[3];

        stateWindow.SetActive(true);
    }
}
