﻿/**
 * Author:    Vinh Vu Thanh
 * This class is a part of Universal Resolver project that can be downloaded free at 
 * https://github.com/game-libgdx-unity/UnityEngine.IoC
 * (c) Copyright by MrThanhVinh168@gmail.com
 **/

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using App.Scripts.Boards;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityIoC;
using Debug = UnityEngine.Debug;

[ProcessingOrder(1)]
public class MapGenerator : MonoBehaviour
{

    [SerializeField] private GridLayoutGroup gridLayout;
    [SerializeField] private Button btnRestart;

    [Singleton] private IGameBoard gameBoard;
    [Singleton] private IGameSolver gameSolver;
    [Singleton] private GameSetting gameSetting;
    [Singleton] private Observable<GameStatus> gameStatus;

    private void Start()
    {
        //setup game status, when it get changes
        gameStatus.Subscribe(status =>
            {
                print("Game status: " + status.ToString());
                if (status == GameStatus.Completed)
                {
                    //show the button to reset the game
                    btnRestart.gameObject.SetActive(true);
                }
                else
                {
                    //hide UI elements/objects as default when game's starting
                    btnRestart.gameObject.SetActive(false);
                }
            })
            .AddTo(gameObject);

        //setup button restart
        if (btnRestart)
        {
            btnRestart.onClick.AddListener(() => { StartCoroutine(RestartRoutine()); });
        }

        //setup the layout
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = gameSetting.Width;

        //setup for CellView
        Context.OnViewResolved<CellView>()
            .Subscribe(v => { v.transform.SetParent(transform.GetChild(0)); })
            .AddTo(this);
        
        //setup a new game
        Setup();
    }

    private void Setup()
    {
        //modify the state of gameStatus
        gameStatus.Value = GameStatus.InProgress;

        //build the board
        gameBoard.Build();

        //solve the game
        StartCoroutine(gameSolver.Solve(1f));
    }

    private IEnumerator RestartRoutine()
    {
        //this is not really necessary
        yield return null;

        //delete all cells which are resolved by the Context
        //This also wiemove all associated Views with the data cell objects.
        Context.DeleteAll<CellData>();

        //setup a new game
        Setup();
    }
}