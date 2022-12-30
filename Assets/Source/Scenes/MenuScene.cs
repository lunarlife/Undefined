using Events.UI;
using Events.UI.Mouse;
using GameEngine;
using GameEngine.Scenes;
using Utils.AsyncOperations;
using Utils.Events;

namespace Scenes
{

    public class MenuScene : SceneLoader
    {
        public override string SceneName => "Menu";


        public override void OnLoading(AsyncOperationInfo<SceneLoadingState> info)
        {
            this.RegisterListener();

            /*var rect = new Rect(20, 20, 200, 50);
            var gameText = new Text(fontSize: new FontSize(36), rect: Rect.Zero, bind: new UIBind
            {
                Side = Side.Center,
                IsExpandable = true
            }, content: "game");
            var game = new Button(text: gameText, rect: rect, bind: new UIBind {Side = Side.TopLeft});
            EventManager.RegisterEvent<ButtonClickEvent>(game, OnGameStart);
            
            _ = new DebugView();
            _ = new CameraControl();
            _ = new ChatManager();
            _ = new EscapeWindow();*/

            /*var input = new InputField(new Rect(0, 0, 400, 50),bind: new UIBind
            {
                Bind = Side.Center,
            });*/

            /*_ = new ContextMenu(new Rect(600,0, 100, 100), new ContextMenuItem[]
            {
                new ContextMenuItem()
            });#1#*/
            //var text = new Text(new Rect(0, 50, 50, 50), new FontSize(20), content: "scroll test");
            //var scroll = new ScrollerView(new Rect(400,50, 100, 500), new Rect(0,0, 100, 100),false, true, 0, 4f);
            //scroll.AddElement(text);
        }
        
        [EventHandler]
        private void OnKeyUp(UIMouseUpEvent e)
        {
            Undefined.Logger.Info("up");
        }

        [EventHandler]
        private void OnKeyHolding(UIMouseHoldingEvent e)
        {
            Undefined.Logger.Info("holding");
        }
    
        [EventHandler]
        private void OnKeyDown(UIMouseDownEvent e)
        {
            Undefined.Logger.Info("down");
        }
        [EventHandler]
        private void OnKeyExit(UIMouseExitEvent e)
        {
            Undefined.Logger.Info("exit");
        }
        [EventHandler]
        private void OnKeyEnter(UIMouseEnterEvent e)
        {
            Undefined.Logger.Info("enter");
        }
        private void OnGameStart(ButtonClickEvent button)
        {
            OldScene.LoadScene<GameScene>();
        }
    }
}