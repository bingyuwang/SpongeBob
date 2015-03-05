public static class GameManager{
	public delegate void GameEvent();

	public static event GameEvent GameStart,GameOver;
	public static bool isStart;
	public static void TriggerGameStart(){
		if (GameStart != null) {
			GameStart();		
			isStart = true;
		}
	}

	public static void TriggerGameOver(){
		if (GameOver != null) {
			GameOver();	
			isStart = false;
		}
	}
	
}
