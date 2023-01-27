using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Popup : MonoBehaviour
{
	[SerializeField] Button _button;
	[SerializeField] Text _buttonText;
	[SerializeField] Text _popupText;

	public void Init(Transform canvas, TileObject tile) 
	{
		_buttonText.text = "OK";

		if(!tile.data.occupied)
		{
			_popupText.text="Cette case est vide.\nUn espace naturel s'y développera si rien n'y est construit.";
		}
		else
		{
			if(tile.data.obstacleType==Tile.ObstacleType.Nature)
			{
				_popupText.text="<b>"+tile.data.natureOccupierRef.data.name+"</b>"+"\n\n";
				if(tile.canEvolve)
				{
					_popupText.text+="Absorbe "+ tile.data.natureOccupierRef.data.carbonAbsorption.ToString() + " tonnes de CO2 par an pendant encore "+(tile.evolveAge-tile.age).ToString()+" ans.\n";
				}
				else
				{
					_popupText.text+="Est surchargée en carbone et ne peut plus en stocker.\n";
				}

				_popupText.text+="A stocké "+ tile.carbonStocks.ToString() +" tonnes de CO2.";
			}
			else
			{
				_popupText.text="<b>"+tile.data.buildingOccupierRef.data.name+"</b>"+"\n\n";
				_popupText.text+="Nourrit "+ tile.data.buildingOccupierRef.data.fedPeople.ToString() +" personnes.\n";
				_popupText.text+="Émet "+ tile.data.buildingOccupierRef.data.carbonEmission.ToString() +" tonnes de CO2 par an.\n";

				if(tile.data.buildingOccupierRef.data.ressourcesNeeded.Count>0)
				{
					_popupText.text+="\nA besoin de : ";
				}

				for(int i=0; i<tile.data.buildingOccupierRef.data.ressourcesNeeded.Count; ++i)
				{
					_popupText.text+="\n"+tile.data.buildingOccupierRef.data.ressourcesNeeded[i].y.ToString()+" hectares de "+BuildingsDatabase.Instance.buildingsDatabase[tile.data.buildingOccupierRef.data.ressourcesNeeded[i].x].name;
				}
			}
		}


		transform.SetParent(canvas);
		transform.localScale = Vector3.one;
		GetComponent<RectTransform>().offsetMin = Vector2.zero;
		GetComponent<RectTransform>().offsetMax = Vector2.zero;

		_button.onClick.AddListener(() => {
			GameObject.Destroy(this.gameObject);
		});
	}
}
