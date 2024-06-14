using Zenject;

namespace DI
{
    public class InputInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            PlayerInput playerInput = new();
            playerInput.Enable();
            Container.BindInstance(playerInput);
        }
    }
}