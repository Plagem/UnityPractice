using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Flight : Unit
{
    private void Start()
    {
        SettingData settingData = new SettingData();
        StateUpdate(settingData);
    }

    public override string[] InforUnitState()
    {
        string[] state = new string[4];

        state[0] = unitName;
        state[1] = level.ToString();
        state[2] = HP.ToString();
        state[3] = attackPower.ToString();

        return state;
    }

    public override void StateUpdate(SettingData settingData)
    {
        level += settingData.flightLevel;
        HP += settingData.flightHP;
        attackPower += settingData.flightAttackPower;
    }
}
