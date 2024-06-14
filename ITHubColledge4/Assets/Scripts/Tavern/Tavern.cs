using Zenject;

namespace weed
{
    public class Barman : MonoInstaller
    {
	    // public Player player;
        public float price;
        void Start() 
        {
            // not sure what could be here; maybe edit price based on the player's variable of how far they've fooled around

        }

        public override void CheckIfEnough()
        {
            /*
            if (player.money >= price) 
            {
                player.money -= price;
                player.PlayAnimation("drinking"); // just a shitty placeholder on what it should theoretically do
                
            }

            waitForInputAndSendToHell();
            */
        }

        public IEnumerator waitForInputAndSendToHell() 
        {
            while (Input.GetKeyDown(KeyCode.E))
            {
                yield return null;
            }
            SceneManager.LoadScene("Game");
        }
    }
}