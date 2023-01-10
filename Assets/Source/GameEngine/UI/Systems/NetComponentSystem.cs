using UECS;
using UndefinedNetworking.GameEngine.Components;
using UndefinedNetworking.GameEngine.Scenes.UI.Components;
using UndefinedNetworking.Packets.Components;

namespace GameEngine.UI.Systems
{

    public class NetComponentSystem : IAsyncSystem
    {
        [ChangeHandler] private Filter<IComponent<UINetworkComponentData>> _changedUIs;

        public void Init()
        {

        }

        public void Update()
        {
            foreach (var result in _changedUIs)
            {
                var component = result.Get1();
                if(component.DataTypeIs(typeof(IClientChangeHandler)))
                    Undefined.SendPackets(new UIComponentUpdatePacket(component));
            }
        }
    }
}