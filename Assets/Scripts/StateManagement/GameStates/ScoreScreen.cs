using UnityEngine;
using UnityEngine.UI;

// Controls the score screen.
public class ScoreScreen : IGameState {

	// Prefab for a single score entry.
	[SerializeField] private RectTransform scoreEntry;

	// Sprite representing a full heart.
	[SerializeField] private Sprite fullHeartSprite;

	// Initialize this screen.
	public override void OnInitializeState () {
		base.OnInitializeState ();
		SetScores (new int[] { -1, -1, 2, 1, -1, 0, 0, 1 });
	}

	// Set the scores on this screen.
	public void SetScores (int[] scores) {
		RectTransform container = transform.Find ("GUI Canvas/Scores Scroll").GetComponent<ScrollRect> ().content;
		container.DestroyAllChildren ();
		for (int i = 0; i < scores.Length; i++) {
			int score = scores [i];
			if (score >= 0) {
				Bird bird = GameController.GetBird (i);
				RectTransform entry = Instantiate (scoreEntry);
				entry.SetParent (container);
				entry.localScale = Vector3.one;
				entry.Find ("Icon").GetComponent<Image> ().sprite = bird.headSprite;
				entry.Find ("Name").GetComponent<Text> ().text = bird.name;
				for (int j = 0; j < score; j++) {
					entry.Find ("Heart " + j).GetComponent<Image> ().sprite = fullHeartSprite;
				}
			}
		}
	}
}
