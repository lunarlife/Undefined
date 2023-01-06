using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Debugger;
using Events.GameEngine;
using Events.Networking.PlayerEvents;
using Events.Tick;
using GameEngine.Components;
using GameEngine.Exceptions;
using GameEngine.GameObjects.Core;
using GameEngine.GameSettings;
using GameEngine.Resources;
using GameEngine.Scenes;
using GameEngine.UI.Systems;
using Networking;
using Networking.DataConvert;
using Networking.Loggers;
using Networking.Packets;
using UECS;
using UndefinedNetworking;
using UndefinedNetworking.Events.GameEngine;
using UndefinedNetworking.GameEngine;
using UndefinedNetworking.GameEngine.Input;
using UndefinedNetworking.GameEngine.Scenes;
using UndefinedNetworking.GameEngine.Scenes.UI;
using UndefinedNetworking.Gameplay.Chat;
using UndefinedNetworking.Packets.Components;
using UndefinedNetworking.Packets.Player;
using UndefinedNetworking.Packets.Server;
using UndefinedNetworking.Packets.Server.Resources;
using UndefinedNetworking.Packets.UI;
using UndefinedNetworking.Packets.World;
using UnityEngine;
using Utils;
using Utils.Dots;
using Utils.Enums;
using Utils.Events;
using Logger = Networking.Loggers.Logger;

namespace GameEngine
{
    public static class Undefined
    {
        public const int FixedTicksPerSecond = 60;

        private static int _mainThreadId;
        private static Server _server;
        private static RuntimePacketer _packeter;
        private static string _nickname;
        private static Stopwatch _connectionTimer;
        private static readonly Dictionary<Identifier, NetPlayer> VisiblePlayers = new();

        private static DownloadingResource _downloadingResource;
        public static NetPlayer MyPlayer { get; private set; }
        public static bool IsConnected { get; private set; }

        public static Logger Logger { get; } = new MainClientLogger();

        public static bool IsSynchronously => Environment.CurrentManagedThreadId == _mainThreadId;
        public static bool IsStarted { get; private set; }
        public static IUIView Canvas { get; private set; }
        public static OldScene CurrentScene => OldScene.CurrentScene;
        public static IUIView Camera { get; private set; }
        public static Dot2Int MouseScreenPositionUnscaled => EngineEventsInvoker.MouseScreenPositionUnscaled;
        public static Dot2Int MouseScreenPosition => EngineEventsInvoker.MouseScreenPosition;
        public static float MouseScroll => EngineEventsInvoker.MouseScroll;
        public static Dot2Int MouseDeltaUnscaled => EngineEventsInvoker.MouseDeltaUnscaled;
        public static Dot2 MouseWorldPosition => EngineEventsInvoker.MouseWorldPosition;

        public static float UIScale => ((float)Settings.ResolutionUnscaled.Width / Settings.Resolution.Width +
                                        (float)Settings.ResolutionUnscaled.Height / Settings.Resolution.Height) / 2f;

        public static Player Player { get; private set; }
        public static ServerManager ServerManager { get; private set; }
        public static SystemsController Systems { get; private set; }

        public static void Startup()
        {
            IsStarted = IsStarted ? throw new EngineException("engine is started") : true;
            ServerManager = new ServerManager();
            _mainThreadId = Environment.CurrentManagedThreadId;
            var args = Environment.GetCommandLineArgs();
            if (args.Contains("-window"))
            {
                if (args.Contains("-floating"))
                {
                }
                else if (args.Contains("-floating"))
                {
                }
            }
            else
            {
                ServerManager.InternalResourcesManager.LoadAll();
                NetworkData.LoadNetworkData();
                EventManager.RegisterStaticEvents(typeof(Undefined));
                ObjectCore.CreateGameObjectsPoolInstances(50);
                ObjectCore.CreateUIPoolInstances(500);
                RegisterAllSystems();
                Settings.ResolutionUnscaled = Settings.MinResolution;
                CallSynchronously(new ActionCallback(
                    () => { Player = new GameObject("PLAYER").AddComponent<Player>(); }, () =>
                    {
                        Player.LoadScene(SceneType.XY);
                        Camera = Player.ActiveScene.OpenView(new ViewParameters());
                        var camera = Camera.AddComponent<CameraComponent>();
                        camera.Orthographic = true;
                        camera.OrthographicSize = 10;
                        camera.FarClipPlane = 40;
                        camera.NearClipPlane = .3f;
                        Canvas = Player.ActiveScene.OpenView(new ViewParameters
                        {
                            OriginalRect = Settings.ResolutionUnscaled
                        });
                        var canvas = Canvas.AddComponent<CanvasComponent>();
                        canvas.PlaneDistance = 10;
                        canvas.Camera = camera;
                        canvas.RenderMode = RenderMode.ScreenSpaceCamera;
                        Canvas.AddComponents<CanvasScalerComponent, GraphicRaycasterComponent>();
                        LoadMenuScene();
                    }));
            }
        }

        private static void Tests()
        {
            _ = new DebugView();
        }


        private static async void LoadMenuScene()
        {
            //var (loader, info) = OldScene.LoadScene<MenuScene>();
            //info.Wait();
            var result = await Connect(IPAddress.Parse("127.0.0.1"), 2402, "lunarlifekekw");
            Tests();
            ULogger.ShowInfo("result: " + result);
        }

        private static void RegisterAllSystems()
        {
            Systems = new SystemsController();
            Systems.Register(new CameraSystem());
            Systems.Register(new CanvasSystem());
            Systems.Register(new TextSystem());
            Systems.Register(new MouseHandlersSystem());
            Systems.Register(new WindowFloatingSystem());
            Systems.Register(new RectTransformSystem());
            Systems.Register(new ScrollSystem());
            Systems.Register(new RectMaskSystem());
            Systems.Register(new ImageSystem());
            Systems.Register(new InputFieldSystem());
            var netSystem = new NetComponentSystem();
            DataConverter.AddDynamicConverter(netSystem);
            Systems.Register(netSystem);
        }

        public static async Task<ConnectionResult> Connect(IPAddress address, int port, string nickname)
        {
            if (IsConnected) throw new ServerException("client is already connected");
            _nickname = nickname;
            return await Task.Run(() =>
            {
                Logger.Info("Connecting...");
                _connectionTimer = Stopwatch.StartNew();
                _server = new Server();
                try
                {
                    _server.Connect(address, port);
                }
                catch (Exception)
                {
                    return ConnectionResult.ServerNotFounded;
                }

                IsConnected = true;
                _packeter = new RuntimePacketer(_server, Priority.Normal)
                {
                    IsReading = true,
                    IsSending = true
                };
                RuntimePacketer.IsSenderWorking = true;
                RuntimePacketer.IsThreadPoolWorking = true;
                _packeter.Receive += PacketerOnReceive;
                _packeter.UnhandledException +=
                    exception => Logger.Error(exception.Message + "\n" + exception.StackTrace);
                _packeter.SendPacket(new ClientInfoPacket(null, ServerManager.ServerVersion, nickname));
                return ConnectionResult.Connected;
            });
        }

        private static void PacketerOnReceive(Packet packet)
        {
            if (!IsConnected) return;
            switch (packet)
            {
                case ResourceDownloadStartPacket resourceDownloadStartPacket:
                    _downloadingResource =
                        new DownloadingResource(resourceDownloadStartPacket.FilePathInResourcesFolder,
                            resourceDownloadStartPacket.Id);
                    _downloadingResource.Start(resourceDownloadStartPacket.TotalLength);
                    break;
                case ResourcesPacket rp:
                    if (_downloadingResource is null) break;
                    _downloadingResource.Write(rp.Data);
                    if (_downloadingResource.Downloaded == _downloadingResource.TotalSize)
                    {
                        _downloadingResource.Stop();
                        ((ServerResourcesManager)ServerManager.ResourcesManager).AddResource(_downloadingResource.Path);
                        _downloadingResource = null;
                        SendPackets(new ResourceDownloadCompletePacket());
                    }

                    break;
                case ServerInfoPacket sip:
                    RuntimePacketer.Tick = sip.Tick;
                    MyPlayer = new NetPlayer(sip.Identifier, _nickname);
                    VisiblePlayers.Add(MyPlayer.Identifier, MyPlayer);
                    //_isConnected = true;
                    EventManager.CallEvent(new PlayerConnectedEvent(MyPlayer));
                    var chatTypes = new Enum<ChatType>();
                    //var commands = new Enum<ClientCommand>();
                    /*if(sip.Chats is not null)
                        foreach (var chat in sip.Chats)
                            chatTypes.AddMember(chat.Name,
                                new ClientChatType(chat.DisplayName, chat.CanUseCommands, chat.CanWriteMessages));*/
                    /*if(sip.Commands is not null)
                        foreach (var cmd in sip.Commands)
                            commands.AddMember(cmd.Prefix, new ClientCommand(cmd.Prefix, cmd.Description, cmd.ParametersTitles));
                    EventManager.CallEvent(new ServerInitializeEvent(chatTypes, commands));*/
                    _connectionTimer.Stop();
                    Logger.Info($"Connected! {_connectionTimer.Elapsed:g}");
                    IsConnected = true;
                    break;
                case PlayerDisconnectPacket pdp:
                    if (MyPlayer is null || pdp.Identifier == MyPlayer.Identifier)
                    {
                        DisconnectLocal(pdp.Cause, pdp.Message);
                        break;
                    }

                    var p = VisiblePlayers[pdp.Identifier];
                    EventManager.CallEvent(new PlayerDisconnectEvent(p, pdp.Cause, pdp.Message));
                    Logger.Info($"{p.Nickname} disconnected");
                    VisiblePlayers.Remove(pdp.Identifier);
                    MyPlayer = null;
                    break;
                case PlayerConnectPacket pcp:
                    var player = new NetPlayer(pcp.Identitifer, pcp.Nickname);
                    VisiblePlayers.Add(pcp.Identitifer, player);
                    Logger.Info($"{player.Nickname} connected");
                    EventManager.CallEvent(new PlayerConnectedEvent(player));
                    break;
                case WorldPacket wp:
                    //CallSynchronously(() => World.LoadWorld(wp));
                    break;
                /*case ColonyInitializePacket cip:
                    //VisiblePlayers[cip.Owner].CreateColony(cip);
                    break;*/
                case ChatPacket cp:
                    //EventManager.CallEvent(new ChatMessageReceivedEvent(new ChatMessage(cp.Nickname,cp.Message, ClientChat.Chats[cp.ChatTypeID], cp.Color)));
                    break;
                case ClientPingPacket:
                    SendPackets(new ClientPingPacket());
                    break;
                case UIViewOpenPacket viewOpenPacket:
                    Console.WriteLine("open");
                    var open = (NetworkUIView)((Scene)Player.ActiveScene).OpenNetworkView(viewOpenPacket.Identifier);
                    break;
                case UIViewClosePacket viewClosePacket:
                    if (!Player.ActiveScene.TryGetView(viewClosePacket.Identifier, out var view))
                    {
                        DisconnectServer(DisconnectCause.InvalidPacket, "");
                        break;
                    }

                    Player.ActiveScene.CloseView(view);
                    break;
                case UIComponentUpdatePacket updatePacket:
                    break;
            }
        }


        private static void DisconnectLocal(DisconnectCause cause, string message)
        {
            IsConnected = false;
            Logger.Info($"Disconnected! Cause: {cause} Message: {message}");
            _packeter.IsSending = false;
            _packeter.IsReading = false;
            RuntimePacketer.IsSenderWorking = false;
            RuntimePacketer.IsThreadPoolWorking = false;
            _server.Close();
            _packeter = null;
            _server = null;
        }

        private static void DisconnectServer(DisconnectCause cause, string message)
        {
            _packeter?.SendPacketNow(new PlayerDisconnectPacket(null, DisconnectCause.Leave, null));
            DisconnectLocal(cause, message);
        }

        public static void LeaveFromServer()
        {
            DisconnectServer(DisconnectCause.Leave, "");
        }

        [EventHandler]
        private static void OnEngineStop(EngineStopEvent e)
        {
            DisconnectServer(DisconnectCause.Leave, "Player disconnect");
        }

        [EventHandler]
        private static void OnAsyncTickEvent(TickEvent e)
        {
            Systems.UpdateAsync();
        }

        [EventHandler]
        private static void OnTickEvent(SyncTickEvent e)
        {
            Systems.UpdateSync();
        }

        private static void SendPacketsNow(params Packet[] packets)
        {
            _packeter.SendPacketNow(packets);
        }

        public static void SendPackets(params Packet[] packets)
        {
            _packeter.SendPacket(packets);
        }
        //public static bool RaycastUISynchronously(UIElement obj, Dot2Int position, bool onlyFirst = false) => Canvas.RaycastSynchronously(obj, position, onlyFirst);
        //public static void RaycastUIAsync(UIElement obj, Dot2Int position, bool onlyFirst, Action<bool> callback) => Canvas.RaycastAsync(obj, position, onlyFirst, callback);

        public static void CallSynchronously(Action action)
        {
            EngineEventsInvoker.CallSync(action);
        }

        public static void CallSynchronously(ActionCallback action)
        {
            EngineEventsInvoker.CallSync(action);
        }

        public static bool IsPressed(params KeyboardKey[] keys)
        {
            return EngineEventsInvoker.IsPressedAll(ClickState.All, keys);
        }

        public static bool IsPressed(KeyboardKey[] keys, ClickState states)
        {
            return EngineEventsInvoker.IsPressedAll(states, keys);
        }

        public static bool IsPressedAny(params KeyboardKey[] keys)
        {
            return EngineEventsInvoker.IsPressedAny(ClickState.All, keys);
        }

        public static bool IsPressedAny(KeyboardKey[] keys, ClickState states)
        {
            return EngineEventsInvoker.IsPressedAny(states, keys);
        }

        public static bool IsPressed(MouseKey keys, ClickState states = ClickState.All)
        {
            return EngineEventsInvoker.IsPressedAll(states, keys);
        }

        public static bool IsPressedAny(MouseKey keys, ClickState states = ClickState.All)
        {
            return EngineEventsInvoker.IsPressedAny(states, keys);
        }
    }
}