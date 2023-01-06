using System;
using Networking;
using Networking.DataConvert;
using UECS;
using UndefinedNetworking.GameEngine;
using UndefinedNetworking.GameEngine.Scenes.UI.Components;
using UndefinedNetworking.Packets.Components;

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
            foreach (var result in _changedUIs)
            {
                var ui = result.Get1();
                if (ui is not IClientChangeHandler) continue;
                Undefined.SendPackets(new UIComponentUpdatePacket(ui));
            }
        }

        public bool IsValidConvertor(Type type)
        {
            return typeof(INetworkComponent).IsAssignableFrom(type);
        }

        public byte[] Serialize(object o)
        {
            var serialize =
                DataConverter.Serialize(o, switcher: ServerDataAttribute.DataId, converterUsing: ConvertType.ExcludeCurrent);
            var componentId = Component.GetComponentId(o.GetType());
            var view = (o as UIComponent)?.TargetView;
            return DataConverter.Combine(DataConverter.Serialize(view?.Identifier), DataConverter.Serialize(componentId), serialize);
        }

        public object Deserialize(byte[] data, Type type)
        {
            ushort index = 0;
            var viewId = DataConverter.Deserialize<uint>(data, ref index);
            var cType = Component.GetComponentType(DataConverter.Deserialize<ushort>(data, ref index));
            if (!Undefined.Player.ActiveScene.TryGetView(viewId, out var view)) Undefined.LeaveFromServer();
            var result = view.GetComponent(cType) ?? view.AddComponent(cType);
            DataConverter.DeserializeInject(data, result, ref index, ClientDataAttribute.DataId);
            return result;
        }
    }
}