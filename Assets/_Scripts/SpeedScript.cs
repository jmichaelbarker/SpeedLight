using UnityEngine;
using UnityEngine.UI;
using System.Collections;


//Ahmed Mahmoud 16/06/16

/*This script will update the speedometer value in the bottom right corner*/
namespace UnityStandardAssets.Vehicles.Car
{
	public class SpeedScript : MonoBehaviour
	{

		public Text speedTxt;
		private CarController carcontroller;

		void Start ()
		{

			carcontroller = GetComponent <CarController> ();
		}

		void Update ()
		{
			speedTxt.text = carcontroller.CurrentSpeed.ToString("F0") + "KM/H";
			if (carcontroller.CurrentSpeed >= 260) {
				speedTxt.color = Color.red;
			} else {
				speedTxt.color = Color.white;
			}
		}
	}
}