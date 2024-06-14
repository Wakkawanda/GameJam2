using Scripts;
using Zenject;

namespace DI
{
    public class PlayerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Wallet wallet = new Wallet();
            Container.BindInstance(wallet);
        }
    }
}