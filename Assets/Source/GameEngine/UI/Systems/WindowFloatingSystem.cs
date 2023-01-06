using UECS;
using UndefinedNetworking.GameEngine.Scenes.UI.Components.WindowComponents;

namespace GameEngine.UI.Systems
{
    public class WindowFloatingSystem : IAsyncSystem
    {
        [AutoInject] private Filter<WindowFloatingComponent> _windowFloatingComponents;


        public void Init()
        {
        }

        public void Update()
        {
        }
    }
}