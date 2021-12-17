using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using TMPro;
public class MenuUI : MonoBehaviour 
{
	private GameObject manager_obj;
	private bool activated;
	public GameObject[] inputFields;
	private void Start()
    {
		manager_obj = GameObject.FindGameObjectWithTag("GameManager");
		activated = false;
	}
    public void PlayButton ()
	{
		if (!activated)
		{
			GameObject[] plateaux = GameObject.FindGameObjectsWithTag("MenuSecondState");
			foreach (var plateau in plateaux)
			{
				plateau.GetComponent<PlateauxMovementScipt>().activate();
			}
			activated = true;
			GameObject.Find("PlayButton").GetComponent<MainButtonsScript>().desactivate();
			GameObject.Find("SkillsButton").GetComponent<MainButtonsScript>().desactivate();
			GameObject.Find("QuitButton").GetComponent<MainButtonsScript>().desactivate();
		} else
        {
			GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
			foreach(var inputField in inputFields)
            {
				string text = inputField.GetComponent<TMP_InputField>().text;
				if (text.Any(x => char.IsLetterOrDigit(x)))
				{
					switch (inputField.transform.parent.name)
					{
						case "Noir":
							gameManager.players.Add(new Player(1, text)); break;
						case "Rouge":
							gameManager.players.Add(new Player(2, text)); break;
						case "Vert":
							gameManager.players.Add(new Player(3, text)); break;
						case "Bleu":
							gameManager.players.Add(new Player(4, text)); break;
					}
				}
            }
            if (gameManager.players.Count > 0)
            {
				SceneManager.LoadScene("Level1");
            }
            else
            {
				GameObject.Find("WarningMessage").GetComponent<MessageScript>().activate();
            }
        }
	}

	public void QuitButton ()
	{
		Application.Quit();			
	}

	public void SkillsButton()
	{
		SceneManager.LoadScene("Skills");       
	}

    private void Update()
    {

    }

}
