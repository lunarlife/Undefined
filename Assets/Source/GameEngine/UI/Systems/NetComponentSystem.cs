using System;
using Networking.DataConvert;
using UECS;
using UndefinedNetworking.GameEngine;
using UndefinedNetworking.GameEngine.UI.Components;

namespace GameEngine.UI.Systems
{

    public class NetComponentSystem : IAsyncSystem, IDynamicDataConverter
    {
        [ChangeHandler] private Filter<UINetworkComponent> _changedUIs;
        [AutoInject] private Filter<UINetworkComponent> _uis;

        public void Init()
        {
        }

        public void Update()
        { 
            /*foreach (var result in _ui)
            {
                var ui = result.Get1();
                var component = (INetworkComponent)ui;
                
                if (ui.TargetView.Viewer is not PlayerComponent player) continue;
                player.Client.SendPacket(new ComponentUpdatePacket(component));
            }*/
        }
        
        public bool IsValidConvertor(Type type) => typeof(INetworkComponent).IsAssignableFrom(type);

        public byte[] Serialize(object o)
        {
            var nc = (INetworkComponent)o;
            var serialize =
                DataConverter.Serialize(o, ServerDataAttribute.DataId, converterUsing: ConverterUsing.ExcludeCurrent);
            return DataConverter.Combine(DataConverter.Serialize(nc.Identifier), serialize);
        }

        public object Deserialize(byte[] data, Type type)
        {
            ushort index = 0; 
            var cType = Component.GetComponentType(DataConverter.Deserialize<ushort>(data, ref index));
            return DataConverter.Deserialize(data, cType, ref index, ClientDataAttribute.DataId, converterUsing: ConverterUsing.ExcludeCurrent);
        }
    }
}