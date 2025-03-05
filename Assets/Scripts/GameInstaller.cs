using Player;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<PlayerControls>().AsSingle().NonLazy();
        Container.Bind<PlayerController>().FromComponentInHierarchy().AsSingle();
        //Container.Bind<ItemPickup>().FromComponentInHierarchy().AsSingle();
    }
}

