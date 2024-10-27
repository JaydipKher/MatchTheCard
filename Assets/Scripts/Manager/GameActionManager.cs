using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameActionManager : Singleton<GameActionManager>
{
   public Action<int> generateLevel;
   public Action levelGenerated;
   public Action onLevelReset;
   public Action onLevelComplete;
   public Action<Card> onShowComplete;
   public Action showLoading;
   public Action showLoadingComplete;
   public Action hideLoading;
   public Action hideLoadingComplete;
   public Action<bool> UpdateScrore;
   public Action<Action> displayScore;
}
