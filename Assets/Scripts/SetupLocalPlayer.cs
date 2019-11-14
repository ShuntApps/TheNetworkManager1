using UnityEngine;
using System.Collections;
using Mirror;
using UnityEngine.UI;


public class SetupLocalPlayer : NetworkBehaviour {

	public Text namePrefab;
	public Text nameLabel;
	public Transform namePos;
	string textboxname = "";
	string colourboxname = "";
    public Slider healthPrefab;
    public Slider healthBar;
    public InputField nameEntry;
    public Dropdown colourSelect;

	[SyncVar (hook = "OnChangeName")]
	public string pName = "player";

	[SyncVar (hook = "OnChangeColour")]
	public Color pColour = Color.black;

    [SyncVar (hook = "OnChangeHealth")]
    public int healthValue=100;

    void OnChangeHealth(int n)
    {
        healthValue = 0;
        healthBar.value = healthValue;
    }

    void OnChangeName (string n)
    {
		pName = n;
		nameLabel.text = pName;
    }

    void OnChangeColour (Color newColour)
    {
		pColour = newColour;
		Renderer[] rends = GetComponentsInChildren<Renderer>( );

        foreach( Renderer r in rends )
        {
         	if(r.gameObject.name == "BODY")
            	r.material.SetColor("_Color", pColour);
            Debug.Log(r.material.color);
        }
    }



	[Command]
	public void CmdChangeName(string newName)
	{
		pName = newName;
		nameLabel.text = pName;
	}

	[Command]
	public void CmdChangeColour(Color newColour)
	{
		pColour = newColour;
		Renderer[] rends = GetComponentsInChildren<Renderer>( );

        foreach( Renderer r in rends )
        {
         	if(r.gameObject.name == "BODY")
            	r.material.SetColor("_Color", pColour);
        }
	}

    [Command]
    public void CmdChangeHealth(int amount)
    {
        healthValue = healthValue + amount;
        healthBar.value = healthValue;
    }

    /**
	void OnGUI()
	{
		if(isLocalPlayer)
		{
			textboxname = GUI.TextField (new Rect (25, 15, 100, 25), textboxname);
			if(GUI.Button(new Rect(130,15,35,25),"Set"))
				CmdChangeName(textboxname);

			colourboxname = GUI.TextField (new Rect (170, 15, 100, 25), colourboxname);
			if(GUI.Button(new Rect(275,15,35,25),"Set"))
				CmdChangeColour(colourboxname);
		}

	}*/

   public void setPlayerDetails()
    {
        if(isLocalPlayer)
        {
            Color newColour = Color.black ;
            if(colourSelect.value==0)
            {
                newColour = Color.red;
            }
            else if (colourSelect.value == 1)
            {
                newColour = Color.blue;
            }
            else if (colourSelect.value == 2)
            {
                newColour = Color.green;
            }
            else if (colourSelect.value == 3)
            {
                newColour = Color.magenta;
            }
            else if (colourSelect.value == 4)
            {
                newColour = Color.grey;
            }

            CmdChangeName(nameEntry.text);
            CmdChangeColour(newColour);
            showHideGUI();
        }
    }

    public void showHideGUI()
    {
        nameEntry.gameObject.SetActive(!nameEntry.gameObject.activeInHierarchy);
        colourSelect.gameObject.SetActive(!colourSelect.gameObject.activeInHierarchy);
        GameObject.Find("NetworkManager").GetComponent<NetworkManagerHUD>().showGUI = !GameObject.Find("NetworkManager").GetComponent<NetworkManagerHUD>().showGUI;
    }

	// Use this for initialization
	void Start () 
	{
		if(isLocalPlayer)
		{
			GetComponent<PlayerController>().enabled = true;
			CameraFollow360.player = this.gameObject.transform;
            colourSelect = GameObject.Find("ColourDropdown").GetComponent<Dropdown>();
            nameEntry = GameObject.Find("Name").GetComponent<InputField>();
            colourSelect.GetComponentInChildren<Button>().onClick.AddListener(setPlayerDetails);
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                showHideGUI();
            }
        }
		else
		{
			GetComponent<PlayerController>().enabled = false;
		}
        GetComponent<Rigidbody>().isKinematic = !isLocalPlayer;
        GameObject canvas = GameObject.FindWithTag("MainCanvas");
		nameLabel = Instantiate(namePrefab, Vector3.zero, Quaternion.identity) as Text;
		nameLabel.transform.SetParent(canvas.transform);

        healthBar = Instantiate(healthPrefab, Vector3.zero, Quaternion.identity) as Slider;
        healthBar.transform.SetParent(canvas.transform);
	}

	public void OnDestroy()
	{
		if(nameLabel != null)
			Destroy(nameLabel.gameObject);

        if (healthBar != null)
            Destroy(healthBar.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(isLocalPlayer&& collision.gameObject.tag == "bullet")
        {
            CmdChangeHealth(-5);
        }
    }



    void Update()
	{
        //determine if the object is inside the camera's viewing volume
        if (nameLabel != null)
        {
            Vector3 screenPoint = Camera.main.WorldToViewportPoint(this.transform.position);
            bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 &&
                            screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
            //if it is on screen draw its label attached to is name position
            if (onScreen)
            {
                Vector3 nameLabelPos = Camera.main.WorldToScreenPoint(namePos.position);
                nameLabel.transform.position = nameLabelPos;
                healthBar.transform.position = nameLabelPos + new Vector3(0, 15, 0);
            }
            else
            {//otherwise draw it WAY off the screen 
                nameLabel.transform.position = new Vector3(-1000, -1000, 0);
                healthBar.transform.position = new Vector3(-1000, -1000, 0);
            }
        }
	}
}
