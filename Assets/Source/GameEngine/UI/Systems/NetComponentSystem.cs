using System;
using System.Linq;
using GameEngine.Scenes;
using Networking;
using Networking.DataConvert;
using UECS;
using UndefinedNetworking.GameEngine;
using UndefinedNetworking.GameEngine.UI.Components;

namespace GameEngine.UI.Systems
{

    public class NetComponentSystem : IAsyncSystem, IDynamicDataConverter
    {
        [ChangeHandler(true)] private Filter<UINetworkComponent> _changedUIs;
        [AutoInject(true)] private Filter<UINetworkComponent> _uis;

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
                DataConverter.Serialize(o, ServerDataAttribute.DataId, converterUsing: ConvertType.ExcludeCurrent);
            return DataConverter.Combine(DataConverter.Serialize(nc.Identifier), serialize);
        }

        public object Deserialize(byte[] data, Type type)
        {
            ushort index = 0; 
            var cType = Component.GetComponentType(DataConverter.Deserialize<ushort>(data, ref index));
            var identifier = DataConverter.Deserialize<Identifier>(data, ref index);
            if (!Undefined.Player.ActiveScene.TryGetView(identifier, out var view)) Undefined.LeaveFromServer();
            var result = view.GetComponent(cType) ?? view.AddComponent(cType);
            DataConverter.DeserializeInject(data, result, ref index, ClientDataAttribute.DataId);
            return result;
        }
    }
}