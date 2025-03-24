using Door;
using Items;
using Player;
using Zenject;

namespace Game
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<PlayerControls>().AsSingle().NonLazy();
            Container.Bind<PlayerController>().FromComponentInHierarchy().AsSingle();
            Container.Bind<DoorController>().FromComponentInHierarchy().AsSingle();
            Container.Bind<ItemPickupController>().FromComponentInHierarchy().AsSingle();
        }
    }
}