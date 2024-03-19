using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameManager : MonoBehaviour
{
    [SerializeField] private MonitorManager monitorManager;
    [SerializeField] private ScObPatient[] possiblePatients;
    private int _playerPoints;
}
