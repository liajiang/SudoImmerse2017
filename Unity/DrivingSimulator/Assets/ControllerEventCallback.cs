namespace Backend
{
	using UnityEngine;
	using VRTK;

	public class ControllerEventCallbacks : MonoBehaviour
	{
		private VRTK_ControllerEvents events;

		private void Start()
		{
			events = GetComponent<VRTK_ControllerEvents> ();
			if (events == null)
			{
				Debug.Log("Cannot get VRTK controller events");
				return;
			}
				
			events.TouchpadPressed += new ControllerInteractionEventHandler(DoTouchpadPressed);
			events.TouchpadReleased += new ControllerInteractionEventHandler(DoTouchpadReleased);

			events.TriggerAxisChanged += new ControllerInteractionEventHandler(DoTouchpadAxisChanged);


		}
			
		private void DoTouchpadPressed(object sender, ControllerInteractionEventArgs e)
		{
			// emit grab
		}

		private void DoTouchpadReleased(object sender, ControllerInteractionEventArgs e)
		{
			// emit ungrab
		}
			
		private void DoTouchpadAxisChanged(object sender, ControllerInteractionEventArgs e)
		{
			// emit throttle/break
		}


	}
}
